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
      /// 
      /// See the OpenCV documentation for more information about the calibration: http://docs.opencv.org/3.1.0/da/d13/tutorial_aruco_calibration.html
      /// </summary>
      [Serializable]
      public class CameraParameters
      {
        // Constructors
        /// <summary>
        /// Create an empty CameraParameters. Populate it manually or using the <see cref="ImportArrays(Mat, Mat)"/> method.
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
        public int ImageHeight { get; set; }

        /// <summary>
        /// The image width during the calibration.
        /// </summary>
        public int ImageWidth { get; set; }

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
        public Mat CameraMatrix { get; set; }

        /// <summary>
        /// The camera matrix type of the calibration. Equal to <see cref="CameraMatrix.Type()"/> and automatically written when 
        /// <see cref="SaveToXmlFile(string)"/> is called.
        /// </summary>
        /// <remarks>This property is be public for the serialization.</remarks>
        public TYPE CameraMatrixType { get; set; }

        /// <summary>
        /// The camera matrix values of the calibration. Equal to the <see cref="CameraMatrix"/> content and automatically written when 
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
        /// The distorsition coefficients type of the calibration. Equal to <see cref="DistCoeffs.Type()"/> and automatically written when 
        /// <see cref="SaveToXmlFile(string)"/> is called.
        /// </summary>
        /// <remarks>This property is be public for the serialization.</remarks>
        public TYPE DistCoeffsType { get; set; }

        /// <summary>
        /// The distorsition coefficients values of the calibration. Equal to the <see cref="DistCoeffs"/> content and automatically written when 
        /// <see cref="SaveToXmlFile(string)"/> is called.
        /// </summary>
        /// <remarks>This property is be public for the serialization.</remarks>
        public double[][] DistCoeffsValues { get; set; }

        // Methods

        /// <summary>
        /// Create a new CameraParameters object from a previously saved XML file.
        /// </summary>
        /// <param name="filePath">The file path to load.</param>
        /// <returns>The new CameraParameters created from the XML file.</returns>
        public static CameraParameters LoadFromXmlFile(string filePath)
        {
          CameraParameters cameraParameters = null;

          // Load the file and deserialize it
          using (StreamReader reader = new StreamReader(filePath))
          {
            XmlSerializer serializer = new XmlSerializer(typeof(CameraParameters));
            cameraParameters = serializer.Deserialize(reader) as CameraParameters;
          }

          if (cameraParameters == null)
          {
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
          }

          return cameraParameters;
        }

        /// <summary>
        /// Save the object to a XML file.
        /// </summary>
        /// <param name="filePath">The file path where to save the object.</param>
        public void SaveToXmlFile(string filePath)
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
          using (StreamWriter writer = new StreamWriter(filePath))
          {
            XmlSerializer serializer = new XmlSerializer(typeof(CameraParameters));
            serializer.Serialize(writer, this);
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}