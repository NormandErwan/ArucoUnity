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
      /// <remarks>The constructor is needed for the serialization.</remarks>
      public CameraParameters()
      {
      }

      public CameraParameters(int camerasNumber)
      {
        CalibrationDateTime = DateTime.Now;
        CamerasNumber = camerasNumber;

        ImagesHeight = new int[CamerasNumber];
        ImagesWidth = new int[CamerasNumber];
        CamerasMatrix = new Cv.Core.Mat[CamerasNumber];
        CamerasMatrixValues = new double[CamerasNumber][][];
        DistCoeffs = new Cv.Core.Mat[CamerasNumber];
        DistCoeffsValues = new double[CamerasNumber][][];
        CamerasFocalLength = new Vector2[CamerasNumber];
        CamerasOpticalCenter = new Vector2[CamerasNumber];
        OpticalCenters = new Vector3[CamerasNumber];
      }

      // Properties

      /// <summary>
      /// The calibration date and time.
      /// </summary>
      public DateTime CalibrationDateTime { get; set; }

      public int CamerasNumber { get; protected set; }

      /// <summary>
      /// The image height during the calibration.
      /// </summary>
      public int[] ImagesHeight
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
      public int[] ImagesWidth
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
      public float FixAspectRatioValue { get; set; }

      /// <summary>
      /// The average re-projection error of the calibration.
      /// </summary>
      public double[] ReprojectionError { get; set; }

      /// <summary>
      /// The camera matrix of the calibration.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="CameraMatrixType"/> and 
      /// <see cref="CamerasMatrixValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Core.Mat[] CamerasMatrix
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
      public Cv.Core.TYPE CameraMatrixType { get; set; }

      /// <summary>
      /// The camera matrix values of the calibration. Equals to the <see cref="CamerasMatrix"/> content and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public double[][][] CamerasMatrixValues { get; set; }

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
      public Cv.Core.TYPE DistCoeffsType { get; set; }

      /// <summary>
      /// The distorsition coefficients values of the calibration. Equals to the <see cref="DistCoeffs"/> content and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public double[][][] DistCoeffsValues { get; set; }

      /// <summary>
      /// The camera focal length expressed in pixels coordinates. Equals to <see cref="CameraMatrix.AtDouble(0, 0)"/> on the x-axis
      /// and to to <see cref="CameraMatrix.AtDouble(1, 1)"/> on the y-axis.
      /// </summary>
      [XmlIgnore]
      public Vector2[] CamerasFocalLength { get; protected set; }

      /// <summary>
      /// The camera optical center expressed in pixels coordinates. Equals to <see cref="CameraMatrix.AtDouble(0, 2)"/> on the x-axis
      /// and to <see cref="CameraMatrix.AtDouble(1, 2)"/> on the y-axis.
      /// </summary>
      [XmlIgnore]
      public Vector2[] CamerasOpticalCenter { get; protected set; }

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
        cameraParameters.CamerasMatrix = new Cv.Core.Mat[cameraParameters.CamerasNumber];
        cameraParameters.DistCoeffs = new Cv.Core.Mat[cameraParameters.CamerasNumber];
        cameraParameters.CamerasFocalLength = new Vector2[cameraParameters.CamerasNumber];
        cameraParameters.CamerasOpticalCenter = new Vector2[cameraParameters.CamerasNumber];
        cameraParameters.OpticalCenters = new Vector3[cameraParameters.CamerasNumber];

        for (int cameraId = 0; cameraId < cameraParameters.CamerasNumber; cameraId++)
        {
          // Update CameraMatrix
          int cameraMatrixRows = cameraParameters.CamerasMatrixValues[cameraId].Length,
              cameraMatrixCols = cameraParameters.CamerasMatrixValues[cameraId][0].Length;

          cameraParameters.CamerasMatrix[cameraId] = new Cv.Core.Mat();
          cameraParameters.CamerasMatrix[cameraId].Create(cameraMatrixRows, cameraMatrixCols, cameraParameters.CameraMatrixType);
          for (int i = 0; i < cameraMatrixRows; i++)
          {
            for (int j = 0; j < cameraMatrixCols; j++)
            {
              cameraParameters.CamerasMatrix[cameraId].AtDouble(i, j, cameraParameters.CamerasMatrixValues[cameraId][i][j]);
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
        // Update CameraMatrixValues and CameraMatrixType
        CameraMatrixType = CamerasMatrix[0].Type();
        for (int cameraId = 0; cameraId < CamerasNumber; cameraId++)
        {
          int cameraMatrixRows = CamerasMatrix[cameraId].rows,
              cameraMatrixCols = CamerasMatrix[cameraId].cols;

          CamerasMatrixValues[cameraId] = new double[cameraMatrixRows][];
          for (int i = 0; i < cameraMatrixRows; i++)
          {
            CamerasMatrixValues[cameraId][i] = new double[cameraMatrixCols];
            for (int j = 0; j < cameraMatrixCols; j++)
            {
              CamerasMatrixValues[cameraId][i][j] = CamerasMatrix[cameraId].AtDouble(i, j);
            }
          }
        }

        // Update DistCoeffsValues and DistCoeffsType
        DistCoeffsType = DistCoeffs[0].Type();
        for (int cameraId = 0; cameraId < CamerasNumber; cameraId++)
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
        for (int cameraId = 0; cameraId < CamerasNumber; cameraId++)
        {
          if (ImagesWidth == null || ImagesWidth[cameraId] == 0 || ImagesHeight == null || ImagesHeight[cameraId] == 0 || CamerasMatrix == null
            || CamerasMatrix[cameraId] == null || CamerasMatrix[cameraId].size.width != 3 || CamerasMatrix[cameraId].size.height != 3)
          {
            return;
          }

          // Camera parameter's focal lenghts and optical centers
          CamerasFocalLength[cameraId] = new Vector2((float)CamerasMatrix[cameraId].AtDouble(0, 0), (float)CamerasMatrix[cameraId].AtDouble(1, 1));
          CamerasOpticalCenter[cameraId] = new Vector2((float)CamerasMatrix[cameraId].AtDouble(0, 2), (float)CamerasMatrix[cameraId].AtDouble(1, 2));

          // Optical center in the Unity world space; based on: http://stackoverflow.com/a/36580522
          // TODO: take account of FixAspectRatio
          OpticalCenters[cameraId] = new Vector3(CamerasOpticalCenter[cameraId].x / ImagesWidth[cameraId], CamerasOpticalCenter[cameraId].y / ImagesHeight[cameraId], CamerasFocalLength[cameraId].y);
        }
      }
    }
  }

  /// \} aruco_unity_package
}