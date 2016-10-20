
using System;
using System.IO;
using System.Xml.Serialization;

namespace ArucoUnity
{
  namespace Examples
  {
    [Serializable]
    public class CameraParameters
    {
      public DateTime CalibrationDateTime { get; set; }

      public int ImageHeight { get; set; }

      public int ImageWidth { get; set; }

      public int CalibrationFlags { get; set; }

      public float AspectRatio { get; set; }

      public double[][] CameraMatrix { get; set; }

      public double[][] DistCoeffs { get; set; }

      public double ReprojectionError { get; set; }

      public CameraParameters()
      {
      }

      public CameraParameters(Utility.Mat cameraMatrix, Utility.Mat distCoeffs)
      {
        CalibrationDateTime = DateTime.Now;
        ImportArrays(cameraMatrix, distCoeffs);
      }

      public void ImportArrays(Utility.Mat cameraMatrix, Utility.Mat distCoeffs)
      {
        int cameraMatrixRows = cameraMatrix.rows,
            cameraMatrixCols = cameraMatrix.cols;

        CameraMatrix = new double[cameraMatrixRows][];
        for (int i = 0; i < cameraMatrixRows; ++i)
        {
          CameraMatrix[i] = new double[cameraMatrixCols];
          for (int j = 0; j < cameraMatrixCols; ++j)
          {
            CameraMatrix[i][j] = cameraMatrix.AtDouble(i, j);
          }
        }

        int distCoeffsRows = distCoeffs.rows,
            distCoeffsCols = distCoeffs.cols;

        DistCoeffs = new double[distCoeffsRows][];
        for (int i = 0; i < distCoeffsRows; ++i)
        {
          DistCoeffs[i] = new double[distCoeffsCols];
          for (int j = 0; j < distCoeffsCols; ++j)
          {
            DistCoeffs[i][j] = distCoeffs.AtDouble(i, j);
          }
        }
      }

      public void SaveToXmlFile(string filePath)
      {
        StreamWriter writer = null;
        try
        {
          writer = new StreamWriter(filePath);
          XmlSerializer serializer = new XmlSerializer(typeof(CameraParameters));
          serializer.Serialize(writer, this);
        }
        finally
        {
          if (writer != null)
          {
            writer.Close();
          }
        }
      }

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