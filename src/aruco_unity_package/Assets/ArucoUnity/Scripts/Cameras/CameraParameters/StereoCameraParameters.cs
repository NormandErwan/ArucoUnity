using ArucoUnity.Plugin;
using System;
using System.Xml.Serialization;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras
  {
    [Serializable]
    public class StereoCameraParameters
    {
      // Const

      public const int CameraNumber = 2;

      // Constructors

      /// <summary>
      /// Create an empty StereoCameraParameters.
      /// </summary>
      /// <remarks>This constructor is needed for the serialization.</remarks>
      public StereoCameraParameters()
      {
      }

      // Properties

      /// <summary>
      /// The ids of the two cameras of the stereo pair.
      /// </summary>
      public int[] CameraIds { get; set; }

      /// <summary>
      /// The calibration flags used.
      /// </summary>
      public int CalibrationFlagsValue { get; set; }

      /// <summary>
      /// The rotation matrix between the first and the second camera coordinate systems.
      /// </summary>
      /// <remarks>When <see cref="UpdateSerializedProperties"/> is called, it's copied to the <see cref="RotationVectorValues"/> property.</remarks>
      [XmlIgnore]
      public Cv.Vec3d RotationVector { get; set; }

      public double[] RotationVectorValues { get; set; }

      /// <summary>
      /// The translation vector between the coordinate systems of the cameras. 
      /// </summary>
      /// <remarks>When <see cref="UpdateSerializedProperties"/> is called, it's copied to the <see cref="TranslationVectorValues"/>
      /// property.</remarks>
      [XmlIgnore]
      public Cv.Vec3d TranslationVector { get; set; }

      public double[] TranslationVectorValues { get; set; }

      /// <summary>
      /// The average re-projection error of the calibration.
      /// </summary>
      public double ReprojectionError { get; set; }

      /// <summary>
      /// The rotation matrix (rectification transform) for the two cameras of the stereo pair.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's copied to the <see cref="RotationMatricesType"/> and 
      /// <see cref="RotationMatricesValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Mat[] RotationMatrices { get; set; }

      public Cv.Type RotationMatricesType { get; set; }

      public double[][][] RotationMatricesValues { get; set; }

      /// <summary>
      /// Projection matrix in the new (rectified) coordinate systems for the two cameras of the stereo pair.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's copied to the <see cref="NewCameraMatricesType"/> and 
      /// <see cref="NewCameraMatricesValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Mat[] NewCameraMatrices { get; set; }

      public Cv.Type NewCameraMatricesType { get; set; }

      public double[][][] NewCameraMatricesValues { get; set; }

      // Methods

      public void UpdateSerializedProperties()
      {
        RotationVectorValues = new double[3] { RotationVector.Get(0), RotationVector.Get(1), RotationVector.Get(2) };
        TranslationVectorValues = new double[3] { TranslationVector.Get(0), TranslationVector.Get(1), TranslationVector.Get(2) };

        RotationMatricesType = RotationMatrices[0].Type();
        RotationMatricesValues = new double[CameraNumber][][];
        CameraParameters.UpdatePropertyValues(RotationMatrices, RotationMatricesValues);

        NewCameraMatricesType = NewCameraMatrices[0].Type();
        NewCameraMatricesValues = new double[CameraNumber][][];
        CameraParameters.UpdatePropertyValues(NewCameraMatrices, NewCameraMatricesValues);
      }

      public void UpdateNonSerializedProperties()
      {
        RotationVector = new Cv.Vec3d(RotationVectorValues[0], RotationVectorValues[1], RotationVectorValues[2]);
        TranslationVector = new Cv.Vec3d(TranslationVectorValues[0], TranslationVectorValues[1], TranslationVectorValues[2]);
        RotationMatrices = CameraParameters.CreateProperty(RotationMatricesType, RotationMatricesValues);
        NewCameraMatrices = CameraParameters.CreateProperty(NewCameraMatricesType, NewCameraMatricesValues);
      }

      protected double[][] PropertyValues(Cv.Mat property)
      {
        double[][] propertyValues = new double[property.Rows][];
        for (int i = 0; i < property.Rows; i++)
        {
          propertyValues[i] = new double[property.Cols];
          for (int j = 0; j < property.Cols; j++)
          {
            propertyValues[i][j] = property.AtDouble(i, j);
          }
        }
        return propertyValues;
      }

      protected Cv.Mat CreateProperty(Cv.Type propertyType, double[][] propertyValues)
      {
        int rows = propertyValues.Length, cols = propertyValues[0].Length;
        Cv.Mat property = new Cv.Mat(rows, cols, propertyType);
        for (int i = 0; i < rows; i++)
        {
          for (int j = 0; j < cols; j++)
          {
            property.AtDouble(i, j, propertyValues[i][j]);
          }
        }
        return property;
      }
    }
  }
}