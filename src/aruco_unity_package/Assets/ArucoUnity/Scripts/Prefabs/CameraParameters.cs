
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

      public TYPE CameraMatrixType { get; set; }

      public double[][] CameraMatrix { get; set; }

      public TYPE DistCoeffsType { get; set; }

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

      public void ExportArrays(out Utility.Mat cameraMatrix, out Utility.Mat distCoeffs)
      {
        int cameraMatrixRows = CameraMatrix.Length,
            cameraMatrixCols = CameraMatrix[0].Length;

        cameraMatrix = new Utility.Mat();
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

        distCoeffs = new Utility.Mat();
        distCoeffs.Create(distCoeffsRows, distCoeffsCols, DistCoeffsType);
        for (int i = 0; i < distCoeffsRows; i++)
        {
          for (int j = 0; j < distCoeffsCols; j++)
          {
            distCoeffs.AtDouble(i, j, DistCoeffs[i][j]);
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