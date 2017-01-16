using System;
using System.IO;
using System.Xml.Serialization;
using ArucoUnity.Utility.cv;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
    namespace Utility
    {
      /// <summary>
      /// Manage OpenCV's camera parameters from a calibration.
      /// </summary>
      [Serializable]
      public class CameraParameters
      {
        // Constructors

        /// <summary>
        /// Create an empty CameraParameters. Populate it manually or using the <see cref="ImportArrays(Mat, Mat)"/> method.
        /// </summary>
        public CameraParameters()
        {
        }

        /// <summary>
        /// Create a CameraParameters from a OpenCV calibration.
        /// </summary>
        /// <param name="cameraMatrix">The camera matrix of the OpenCV calibration.</param>
        /// <param name="distCoeffs">The distorsition coefficients of the OpenCV calibration.</param>
        public CameraParameters(Mat cameraMatrix, Mat distCoeffs)
        {
          ImportArrays(cameraMatrix, distCoeffs);
        }

        // Properties

        public DateTime CalibrationDateTime { get; set; }

        public int ImageHeight { get; set; }

        public int ImageWidth { get; set; }

        public int CalibrationFlags { get; set; }

        public float AspectRatio { get; set; }

        public TYPE CameraMatrixType { get; set; }

        public double[][] CameraMatrix { get; set; }

        public TYPE DistCoeffsType { get; set; }

        public double[][] DistCoeffs { get; set; }

        public double ReprojectionError { get; set; }

        // Methods

        /// <summary>
        /// Populate the camera parameters from a OpenCV calibration.
        /// </summary>
        /// <param name="cameraMatrix">The camera matrix of the OpenCV calibration.</param>
        /// <param name="distCoeffs">The distorsition coefficients of the OpenCV calibration.</param>
        public void ImportArrays(Mat cameraMatrix, Mat distCoeffs)
        {
          CalibrationDateTime = DateTime.Now;

          CameraMatrixType = cameraMatrix.Type();
          int cameraMatrixRows = cameraMatrix.rows,
              cameraMatrixCols = cameraMatrix.cols;

          CameraMatrix = new double[cameraMatrixRows][];
          for (int i = 0; i < cameraMatrixRows; i++)
          {
            CameraMatrix[i] = new double[cameraMatrixCols];
            for (int j = 0; j < cameraMatrixCols; j++)
            {
              CameraMatrix[i][j] = cameraMatrix.AtDouble(i, j);
            }
          }

          DistCoeffsType = distCoeffs.Type();
          int distCoeffsRows = distCoeffs.rows,
              distCoeffsCols = distCoeffs.cols;

          DistCoeffs = new double[distCoeffsRows][];
          for (int i = 0; i < distCoeffsRows; i++)
          {
            DistCoeffs[i] = new double[distCoeffsCols];
            for (int j = 0; j < distCoeffsCols; j++)
            {
              DistCoeffs[i][j] = distCoeffs.AtDouble(i, j);
            }
          }
        }

        /// <summary>
        /// Return the camera matrix and the distorsion coefficients from the camera parameters.
        /// </summary>
        /// <param name="cameraMatrix"></param>
        /// <param name="distCoeffs"></param>
        public void ExportArrays(out Mat cameraMatrix, out Mat distCoeffs)
        {
          int cameraMatrixRows = CameraMatrix.Length,
              cameraMatrixCols = CameraMatrix[0].Length;

          cameraMatrix = new Mat();
          cameraMatrix.Create(cameraMatrixRows, cameraMatrixCols, CameraMatrixType);
          for (int i = 0; i < cameraMatrixRows; i++)
          {
            for (int j = 0; j < cameraMatrixCols; j++)
            {
              cameraMatrix.AtDouble(i, j, CameraMatrix[i][j]);
            }
          }

          int distCoeffsRows = DistCoeffs.Length,
              distCoeffsCols = DistCoeffs[0].Length;

          distCoeffs = new Mat();
          distCoeffs.Create(distCoeffsRows, distCoeffsCols, DistCoeffsType);
          for (int i = 0; i < distCoeffsRows; i++)
          {
            for (int j = 0; j < distCoeffsCols; j++)
            {
              distCoeffs.AtDouble(i, j, DistCoeffs[i][j]);
            }
          }
        }

        /// <summary>
        /// Save the object to a XML file.
        /// </summary>
        /// <param name="filePath">The file path where to save the object.</param>
        public void SaveToXmlFile(string filePath)
        {
          StreamWriter writer = null;
          try
          {
            writer = new StreamWriter(filePath);
            XmlSerializer serializer = new XmlSerializer(typeof(CameraParameters));
            serializer.Serialize(writer, this);
          }
          catch
          {
          }
          finally
          {
            if (writer != null)
            {
              writer.Close();
            }
          }
        }

        /// <summary>
        /// Create a new CameraParameters object from a previously saved XML file.
        /// </summary>
        /// <param name="filePath">The file path to load.</param>
        /// <returns>The new CameraParameters created from the XML file.</returns>
        public static CameraParameters LoadFromXmlFile(string filePath)
        {
          CameraParameters cameraParameters = null;
          StreamReader reader = null;
          try
          {
            reader = new StreamReader(filePath);
            XmlSerializer serializer = new XmlSerializer(typeof(CameraParameters));
            cameraParameters = (CameraParameters)serializer.Deserialize(reader);
          }
          catch
          {
          }
          finally
          {
            if (reader != null)
            {
              reader.Close();
            }
          }
          return cameraParameters;
        }
      }
    }
  }

  /// \} aruco_unity_package
}