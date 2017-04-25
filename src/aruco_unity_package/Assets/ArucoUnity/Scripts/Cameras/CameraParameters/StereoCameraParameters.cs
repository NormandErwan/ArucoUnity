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

      public const int CAMERA_NUMBER = 2;

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
      /// <remarks>When <see cref="UpdateSerializedProperties"/> is called, it's copied to the <see cref="RotationMatrixType"/> and 
      /// <see cref="RotationMatrixValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Mat RotationMatrix { get; set; }

      public Cv.Type RotationMatrixType { get; set; }

      public double[][] RotationMatrixValues { get; set; }

      /// <summary>
      /// The translation vector between the coordinate systems of the cameras. 
      /// </summary>
      /// <remarks>When <see cref="UpdateSerializedProperties"/> is called, it's copied to the <see cref="TranslationVectorType"/> and 
      /// <see cref="TranslationVectorValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Mat TranslationVector { get; set; }

      public Cv.Type TranslationVectorType { get; set; }

      public double[][] TranslationVectorValues { get; set; }

      /// <summary>
      /// The average re-projection error of the calibration.
      /// </summary>
      public double ReprojectionError { get; set; }

      /// <summary>
      /// The rotation matrix (rectification transform) for the two cameras of the stereo pair.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's copied to the <see cref="RotationMatrixType"/> and 
      /// <see cref="RotationMatrixValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Mat[] RotationMatrices { get; set; }

      public Cv.Type RotationMatricesType { get; set; }

      public double[][][] RotationMatricesValues { get; set; }

      /// <summary>
      /// Projection matrix in the new (rectified) coordinate systems for the two cameras of the stereo pair.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's copied to the <see cref="ProjectionMatricesType"/> and 
      /// <see cref="ProjectionMatricesValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Mat[] ProjectionMatrices { get; set; }

      public Cv.Type ProjectionMatricesType { get; set; }

      public double[][][] ProjectionMatricesValues { get; set; }

      // Methods

      public void UpdateSerializedProperties()
      {
        RotationMatrixType = RotationMatrix.Type();
        RotationMatrixValues = PropertyValues(RotationMatrix);

        TranslationVectorType = TranslationVector.Type();
        TranslationVectorValues = PropertyValues(TranslationVector);

        RotationMatricesType = RotationMatrices[0].Type();
        RotationMatricesValues = new double[CAMERA_NUMBER][][];
        CameraParameters.UpdatePropertyValues(RotationMatrices, RotationMatricesValues);

        ProjectionMatricesType = ProjectionMatrices[0].Type();
        ProjectionMatricesValues = new double[CAMERA_NUMBER][][];
        CameraParameters.UpdatePropertyValues(ProjectionMatrices, ProjectionMatricesValues);
      }

      public void UpdateNonSerializedProperties()
      {
        RotationMatrix = CreateProperty(RotationMatrixType, RotationMatrixValues);
        TranslationVector = CreateProperty(TranslationVectorType, TranslationVectorValues);
        RotationMatrices = CameraParameters.CreateProperty(RotationMatricesType, RotationMatricesValues);
        ProjectionMatrices = CameraParameters.CreateProperty(ProjectionMatricesType, ProjectionMatricesValues);
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