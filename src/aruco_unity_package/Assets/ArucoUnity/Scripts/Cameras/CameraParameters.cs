using ArucoUnity.Plugin;
using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras
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
      /// <remarks>This constructor is needed for the serialization.</remarks>
      public CameraParameters()
      {
      }

      public CameraParameters(int camerasNumber)
      {
        CalibrationDateTime = DateTime.Now;
        CameraNumber = camerasNumber;

        ImageHeights = new int[CameraNumber];
        ImageWidths = new int[CameraNumber];
        CameraMatrices = new Cv.Core.Mat[CameraNumber];
        CameraMatricesValues = new double[CameraNumber][][];
        DistCoeffs = new Cv.Core.Mat[CameraNumber];
        DistCoeffsValues = new double[CameraNumber][][];
        CameraFocalLengths = new Vector2[CameraNumber];
        CameraOpticalCenters = new Vector2[CameraNumber];
        OpticalCenters = new Vector3[CameraNumber];
      }

      // Properties

      /// <summary>
      /// The calibration date and time.
      /// </summary>
      public DateTime CalibrationDateTime { get; set; }

      /// <summary>
      /// The number of the camera during the calibration.
      /// </summary>
      public int CameraNumber { get; protected set; }

      /// <summary>
      /// The image height during the calibration.
      /// </summary>
      public int[] ImageHeights
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
      public int[] ImageWidths
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
      public int CalibrationFlagsValue { get; set; }

      /// <summary>
      /// Non null if there is a fix image aspect ratio.
      /// </summary>
      public float FixAspectRatioValue { get; set; }

      /// <summary>
      /// The average re-projection error of the calibration.
      /// </summary>
      public double[] ReprojectionError { get; set; }

      /// <summary>
      /// The camera matrix of the calibration.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="CameraMatricesType"/> and 
      /// <see cref="CameraMatricesValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Core.Mat[] CameraMatrices
      {
        get { return cameraMatrix; }
        set
        {
          cameraMatrix = value;
          UpdateCameraMatrixDerivedVariables();
        }
      }

      /// <summary>
      /// The camera matrix type of the calibration. Equals to <see cref="CameraMatrices.Type()"/> and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public Cv.Core.Type CameraMatricesType { get; set; }

      /// <summary>
      /// The camera matrix values of the calibration. Equals to the <see cref="CameraMatrices"/> content and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public double[][][] CameraMatricesValues { get; set; }

      /// <summary>
      /// The distorsition coefficients of the calibration.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="DistCoeffsType"/> and 
      /// <see cref="DistCoeffsValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Core.Mat[] DistCoeffs { get; set; }

      /// <summary>
      /// The distorsition coefficients type of the calibration. Equals to <see cref="DistCoeffs.Type()"/> and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public Cv.Core.Type DistCoeffsType { get; set; }

      /// <summary>
      /// The distorsition coefficients values of the calibration. Equals to the <see cref="DistCoeffs"/> content and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public double[][][] DistCoeffsValues { get; set; }

      /// <summary>
      /// The camera focal length expressed in pixels coordinates. Equals to <see cref="CameraMatrices.AtDouble(0, 0)"/> on the x-axis
      /// and to to <see cref="CameraMatrices.AtDouble(1, 1)"/> on the y-axis.
      /// </summary>
      [XmlIgnore]
      public Vector2[] CameraFocalLengths { get; protected set; }

      /// <summary>
      /// The camera optical center expressed in pixels coordinates. Equals to <see cref="CameraMatrices.AtDouble(0, 2)"/> on the x-axis
      /// and to <see cref="CameraMatrices.AtDouble(1, 2)"/> on the y-axis.
      /// </summary>
      [XmlIgnore]
      public Vector2[] CameraOpticalCenters { get; protected set; }

      /// <summary>
      /// The camera optical center in the Unity world space.
      /// </summary>
      [XmlIgnore]
      public Vector3[] OpticalCenters { get; protected set; }

      /// <summary>
      /// The file path of the parameters.
      /// </summary>
      [XmlIgnore]
      public string FilePath { get; protected set; }

      // Variables

      protected int[] imageHeight, imageWidth;
      protected Cv.Core.Mat[] cameraMatrix;

      // Methods

      /// <summary>
      /// Create a new CameraParameters object from a previously saved camera parameters XML file.
      /// </summary>
      /// <param name="cameraParametersFilePath">The file path to load.</param>
      /// <exception cref="ArgumentException">If the camera parameters couldn't be loaded because of a wrong file path.</exception>
      /// <returns>The CameraParameters loaded from the XML file or null if the file coulnd't be loaded.</returns>
      public static CameraParameters LoadFromXmlFile(string cameraParametersFilePath)
      {
        CameraParameters cameraParameters = null;

        // Try to load the file and deserialize it
        StreamReader reader = null;
        try
        {
          reader = new StreamReader(cameraParametersFilePath);
          XmlSerializer serializer = new XmlSerializer(typeof(CameraParameters));
          cameraParameters = serializer.Deserialize(reader) as CameraParameters;
        }
        catch
        {
          throw new ArgumentException("Couldn't load the camera parameters file path '" + cameraParametersFilePath + ".", "cameraParametersFilePath");
        }
        finally
        {
          if (reader != null)
          {
            reader.Close();
          }
        }
        cameraParameters.FilePath = cameraParametersFilePath;

        // Populate non-serialized properties
        cameraParameters.CameraMatrices = new Cv.Core.Mat[cameraParameters.CameraNumber];
        cameraParameters.DistCoeffs = new Cv.Core.Mat[cameraParameters.CameraNumber];
        cameraParameters.CameraFocalLengths = new Vector2[cameraParameters.CameraNumber];
        cameraParameters.CameraOpticalCenters = new Vector2[cameraParameters.CameraNumber];
        cameraParameters.OpticalCenters = new Vector3[cameraParameters.CameraNumber];

        for (int cameraId = 0; cameraId < cameraParameters.CameraNumber; cameraId++)
        {
          // Update CameraMatrices
          int cameraMatrixRows = cameraParameters.CameraMatricesValues[cameraId].Length,
              cameraMatrixCols = cameraParameters.CameraMatricesValues[cameraId][0].Length;

          cameraParameters.CameraMatrices[cameraId] = new Cv.Core.Mat();
          cameraParameters.CameraMatrices[cameraId].Create(cameraMatrixRows, cameraMatrixCols, cameraParameters.CameraMatricesType);
          for (int i = 0; i < cameraMatrixRows; i++)
          {
            for (int j = 0; j < cameraMatrixCols; j++)
            {
              cameraParameters.CameraMatrices[cameraId].AtDouble(i, j, cameraParameters.CameraMatricesValues[cameraId][i][j]);
            }
          }

          // Update DistCoeffs
          int distCoeffsRows = cameraParameters.DistCoeffsValues[cameraId].Length,
              distCoeffsCols = cameraParameters.DistCoeffsValues[cameraId][0].Length;

          cameraParameters.DistCoeffs[cameraId] = new Cv.Core.Mat();
          cameraParameters.DistCoeffs[cameraId].Create(distCoeffsRows, distCoeffsCols, cameraParameters.DistCoeffsType);
          for (int i = 0; i < distCoeffsRows; i++)
          {
            for (int j = 0; j < distCoeffsCols; j++)
            {
              cameraParameters.DistCoeffs[cameraId].AtDouble(i, j, cameraParameters.DistCoeffsValues[cameraId][i][j]);
            }
          }
        }
        cameraParameters.UpdateCameraMatrixDerivedVariables();

        return cameraParameters;
      }

      /// <summary>
      /// Save the camera parameters to a XML file.
      /// </summary>
      /// <param name="cameraParametersFilePath">The file path where to save the object.</param>
      /// <exception cref="ArgumentException">If the camera parameters couldn't be saved because of a wrong file path.</exception>
      public void SaveToXmlFile(string cameraParametersFilePath)
      {
        // Update CameraMatricesValues and CameraMatricesType
        CameraMatricesType = CameraMatrices[0].Type();
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          int cameraMatrixRows = CameraMatrices[cameraId].rows,
              cameraMatrixCols = CameraMatrices[cameraId].cols;

          CameraMatricesValues[cameraId] = new double[cameraMatrixRows][];
          for (int i = 0; i < cameraMatrixRows; i++)
          {
            CameraMatricesValues[cameraId][i] = new double[cameraMatrixCols];
            for (int j = 0; j < cameraMatrixCols; j++)
            {
              CameraMatricesValues[cameraId][i][j] = CameraMatrices[cameraId].AtDouble(i, j);
            }
          }
        }

        // Update DistCoeffsValues and DistCoeffsType
        DistCoeffsType = DistCoeffs[0].Type();
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          int distCoeffsRows = DistCoeffs[cameraId].rows,
            distCoeffsCols = DistCoeffs[cameraId].cols;

          DistCoeffsValues[cameraId] = new double[distCoeffsRows][];
          for (int i = 0; i < distCoeffsRows; i++)
          {
            DistCoeffsValues[cameraId][i] = new double[distCoeffsCols];
            for (int j = 0; j < distCoeffsCols; j++)
            {
              DistCoeffsValues[cameraId][i][j] = DistCoeffs[cameraId].AtDouble(i, j);
            }
          }
        }

        // Try to serialize the object and save it to the file
        StreamWriter writer = null;
        try
        {
          writer = new StreamWriter(cameraParametersFilePath);
          XmlSerializer serializer = new XmlSerializer(typeof(CameraParameters));
          serializer.Serialize(writer, this);
        }
        catch
        {
          throw new ArgumentException("Couldn't save the camera parameters to the file path '" + cameraParametersFilePath + ".", "cameraParametersFilePath");
        }
        finally
        {
          if (writer != null)
          {
            writer.Close();
          }
        }
      }

      protected void UpdateCameraMatrixDerivedVariables()
      {
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          if (ImageWidths == null || ImageWidths[cameraId] == 0 || ImageHeights == null || ImageHeights[cameraId] == 0 || CameraMatrices == null
            || CameraMatrices[cameraId] == null || CameraMatrices[cameraId].size.width != 3 || CameraMatrices[cameraId].size.height != 3)
          {
            return;
          }

          // Camera parameter's focal lenghts and optical centers
          CameraFocalLengths[cameraId] = new Vector2((float)CameraMatrices[cameraId].AtDouble(0, 0), (float)CameraMatrices[cameraId].AtDouble(1, 1));
          CameraOpticalCenters[cameraId] = new Vector2((float)CameraMatrices[cameraId].AtDouble(0, 2), (float)CameraMatrices[cameraId].AtDouble(1, 2));

          // Optical center in the Unity world space; based on: http://stackoverflow.com/a/36580522
          // TODO: take account of FixAspectRatio
          OpticalCenters[cameraId] = new Vector3(CameraOpticalCenters[cameraId].x / ImageWidths[cameraId], CameraOpticalCenters[cameraId].y / ImageHeights[cameraId], CameraFocalLengths[cameraId].y);
        }
      }
    }
  }

  /// \} aruco_unity_package
}