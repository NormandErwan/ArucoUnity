using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    /// <summary>
    /// Manage OpenCV's camera parameters from a calibration.
    /// 
    /// See the OpenCV documentation for more information about the calibration: http://docs.opencv.org/3.1.0/da/d13/tutorial_aruco_calibration.html
    /// </summary>
    [Serializable]
    public class CameraParameters
    {
      // Constructors
      /// <summary>
      /// Create an empty CameraParameters and set <see cref="CalibrationDateTime"/> to now.
      /// </summary>
      /// <remarks>The constructor if needed for the serialization.</remarks>
      public CameraParameters()
      {
        CalibrationDateTime = DateTime.Now;
      }

      // Properties

      /// <summary>
      /// The calibration date and time.
      /// </summary>
      public DateTime CalibrationDateTime { get; set; }

      /// <summary>
      /// The image height during the calibration.
      /// </summary>
      public int ImageHeight
      {
        get { return imageHeight; }
        set
        {
          imageHeight = value;
          UpdateCameraMatrixDerivedVariables();
        }
      }

      /// <summary>
      /// The image width during the calibration.
      /// </summary>
      public int ImageWidth
      {
        get { return imageWidth; }
        set
        {
          imageWidth = value;
          UpdateCameraMatrixDerivedVariables();
        }
      }

      /// <summary>
      /// The calibration flags used.
      /// </summary>
      public int CalibrationFlags { get; set; }

      /// <summary>
      /// Non null if there is a fix image aspect ratio.
      /// </summary>
      public float FixAspectRatio { get; set; }

      /// <summary>
      /// The average re-projection error of the calibration.
      /// </summary>
      public double ReprojectionError { get; set; }

      /// <summary>
      /// The camera matrix of the calibration.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="CameraMatrixType"/> and 
      /// <see cref="CameraMatrixValues"/> properties.</remarks>
      [XmlIgnore]
      public Mat CameraMatrix
      {
        get { return cameraMatrix; }
        set
        {
          cameraMatrix = value;
          UpdateCameraMatrixDerivedVariables();
        }
      }

      /// <summary>
      /// The camera matrix type of the calibration. Equals to <see cref="CameraMatrix.Type()"/> and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public TYPE CameraMatrixType { get; set; }

      /// <summary>
      /// The camera matrix values of the calibration. Equals to the <see cref="CameraMatrix"/> content and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public double[][] CameraMatrixValues { get; set; }

      /// <summary>
      /// The distorsition coefficients of the calibration.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="DistCoeffsType"/> and 
      /// <see cref="DistCoeffsValues"/> properties.</remarks>
      [XmlIgnore]
      public Mat DistCoeffs { get; set; }

      /// <summary>
      /// The distorsition coefficients type of the calibration. Equals to <see cref="DistCoeffs.Type()"/> and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public TYPE DistCoeffsType { get; set; }

      /// <summary>
      /// The distorsition coefficients values of the calibration. Equals to the <see cref="DistCoeffs"/> content and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public double[][] DistCoeffsValues { get; set; }

      /// <summary>
      /// The camera focal length on the x-axis expressed in pixels coordinates. Equals to <see cref="CameraMatrix.AtDouble(0, 0)"/>.
      /// </summary>
      [XmlIgnore]
      public float CameraFx { get; protected set; }

      /// <summary>
      /// The camera focal length on the y-axis expressed in pixels coordinates. Equals to <see cref="CameraMatrix.AtDouble(1, 1)"/>.
      /// </summary>
      [XmlIgnore]
      public float CameraFy { get; protected set; }

      /// <summary>
      /// The camera optical center on the x-axis expressed in pixels coordinates. Equals to <see cref="CameraMatrix.AtDouble(0, 2)"/>.
      /// </summary>
      [XmlIgnore]
      public float CameraCx { get; protected set; }

      /// <summary>
      /// The camera optical center on the y-axis expressed in pixels coordinates. Equals to <see cref="CameraMatrix.AtDouble(1, 2)"/>.
      /// </summary>
      [XmlIgnore]
      public float CameraCy { get; protected set; }

      /// <summary>
      /// The camera optical center in the Unity world space.
      /// </summary>
      [XmlIgnore]
      public Vector3 OpticalCenter { get; protected set; }

      // Variables

      protected int imageHeight, imageWidth;
      protected Mat cameraMatrix;

      // Methods

      /// <summary>
      /// Create a new CameraParameters object from a previously saved camera parameters XML file.
      /// </summary>
      /// <param name="cameraParametersFilePath">The file path to load.</param>
      /// <returns>The new CameraParameters created from the XML file.</returns>
      public static CameraParameters LoadFromXmlFile(string cameraParametersFilePath)
      {
        CameraParameters cameraParameters = null;

        // Load the file and deserialize it
        using (StreamReader reader = new StreamReader(cameraParametersFilePath))
        {
          XmlSerializer serializer = new XmlSerializer(typeof(CameraParameters));
          cameraParameters = serializer.Deserialize(reader) as CameraParameters;
        }

        if (cameraParameters == null)
        {
          Debug.LogError("Unable to load the camera parameters from the '" + cameraParametersFilePath + "' file path. Can't estimate pose of the detected"
            + " markers.");
          return null;
        }

        // Update CameraMatrix
        int cameraMatrixRows = cameraParameters.CameraMatrixValues.Length,
            cameraMatrixCols = cameraParameters.CameraMatrixValues[0].Length;

        cameraParameters.CameraMatrix = new Mat();
        cameraParameters.CameraMatrix.Create(cameraMatrixRows, cameraMatrixCols, cameraParameters.CameraMatrixType);
        for (int i = 0; i < cameraMatrixRows; i++)
        {
          for (int j = 0; j < cameraMatrixCols; j++)
          {
            cameraParameters.CameraMatrix.AtDouble(i, j, cameraParameters.CameraMatrixValues[i][j]);
          }
        }
        cameraParameters.UpdateCameraMatrixDerivedVariables();

        // Update DistCoeffs
        int distCoeffsRows = cameraParameters.DistCoeffsValues.Length,
            distCoeffsCols = cameraParameters.DistCoeffsValues[0].Length;

        cameraParameters.DistCoeffs = new Mat();
        cameraParameters.DistCoeffs.Create(distCoeffsRows, distCoeffsCols, cameraParameters.DistCoeffsType);
        for (int i = 0; i < distCoeffsRows; i++)
        {
          for (int j = 0; j < distCoeffsCols; j++)
          {
            cameraParameters.DistCoeffs.AtDouble(i, j, cameraParameters.DistCoeffsValues[i][j]);
          }
        }

        return cameraParameters;
      }

      /// <summary>
      /// Save the camera parameters to a XML file.
      /// </summary>
      /// <param name="cameraParametersFilePath">The file path where to save the object.</param>
      public void SaveToXmlFile(string cameraParametersFilePath)
      {
        // Update CameraMatrixValues and CameraMatrixType
        CameraMatrixType = CameraMatrix.Type();
        int cameraMatrixRows = CameraMatrix.rows,
            cameraMatrixCols = CameraMatrix.cols;

        CameraMatrixValues = new double[cameraMatrixRows][];
        for (int i = 0; i < cameraMatrixRows; i++)
        {
          CameraMatrixValues[i] = new double[cameraMatrixCols];
          for (int j = 0; j < cameraMatrixCols; j++)
          {
            CameraMatrixValues[i][j] = CameraMatrix.AtDouble(i, j);
          }
        }

        // Update DistCoeffsValues and DistCoeffsType
        DistCoeffsType = DistCoeffs.Type();
        int distCoeffsRows = DistCoeffs.rows,
            distCoeffsCols = DistCoeffs.cols;

        DistCoeffsValues = new double[distCoeffsRows][];
        for (int i = 0; i < distCoeffsRows; i++)
        {
          DistCoeffsValues[i] = new double[distCoeffsCols];
          for (int j = 0; j < distCoeffsCols; j++)
          {
            DistCoeffsValues[i][j] = DistCoeffs.AtDouble(i, j);
          }
        }

        // Serialize the object and save it to the file
        using (StreamWriter writer = new StreamWriter(cameraParametersFilePath))
        {
          XmlSerializer serializer = new XmlSerializer(typeof(CameraParameters));
          serializer.Serialize(writer, this);
        }
      }

      protected void UpdateCameraMatrixDerivedVariables()
      {
        if (ImageWidth == 0 || ImageHeight == 0 || CameraMatrix == null || CameraMatrix.size.width != 3 || CameraMatrix.size.height != 3)
        {
          return;
        }

        // Camera parameter's focal lenghts
        CameraFy = (float)CameraMatrix.AtDouble(0, 0);
        CameraFy = (float)CameraMatrix.AtDouble(1, 1);

        // Camera parameter's optical centers
        CameraCx = (float)CameraMatrix.AtDouble(0, 2);
        CameraCy = (float)CameraMatrix.AtDouble(1, 2);

        // Optical center in the Unity world space; based on: http://stackoverflow.com/a/36580522
        // TODO: take account of FixAspectRatio
        OpticalCenter = new Vector3(CameraCx / ImageWidth, CameraCy / ImageHeight, CameraFy);
      }
    }
  }

  /// \} aruco_unity_package
}