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
      // Constructors

      public StereoCameraParameters()
      {
      }

      // Properties

      /// <summary>
      /// The id of first camera of the stereo pair.
      /// </summary>
      public int CameraId1 { get; set; }

      /// <summary>
      /// The id of second camera of the stereo pair.
      /// </summary>
      public int CameraId2 { get; set; }

      /// <summary>
      /// The calibration flags used.
      /// </summary>
      public int CalibrationFlagsValue { get; set; }

      /// <summary>
      /// The rotation matrix between the 1st and the 2nd camera coordinate systems.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="RotationMatrixType"/> and 
      /// <see cref="RotationMatrixValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Core.Mat RotationMatrix { get; set; }

      public Cv.Core.Type RotationMatrixType { get; set; }

      public double[][] RotationMatrixValues { get; set; }

      /// <summary>
      /// The translation vector between the coordinate systems of the cameras. 
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="TranslationVectorType"/> and 
      /// <see cref="TranslationVectorValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Core.Mat TranslationVector { get; set; }

      public Cv.Core.Type TranslationVectorType { get; set; }

      public double[][] TranslationVectorValues { get; set; }

      /// <summary>
      /// The average re-projection error of the calibration.
      /// </summary>
      public double ReprojectionError { get; set; }

      /// <summary>
      /// The rotation matrix (rectification transform) for the first camera.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="RotationMatrix1Type"/> and 
      /// <see cref="RotationMatrix1Values"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Core.Mat RotationMatrix1 { get; set; }

      public Cv.Core.Type RotationMatrix1Type { get; set; }

      public double[][] RotationMatrix1Values { get; set; }

      /// <summary>
      /// The rotation matrix (rectification transform) for the second camera.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="RotationMatrix2Type"/> and 
      /// <see cref="RotationMatrix2Values"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Core.Mat RotationMatrix2 { get; set; }

      public Cv.Core.Type RotationMatrix2Type { get; set; }

      public double[][] RotationMatrix2Values { get; set; }

      /// <summary>
      /// Projection matrix in the new (rectified) coordinate systems for the first camera.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="ProjectionMatrix1Type"/> and 
      /// <see cref="ProjectionMatrix1Values"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Core.Mat ProjectionMatrix1 { get; set; }

      public Cv.Core.Type ProjectionMatrix1Type { get; set; }

      public double[][] ProjectionMatrix1Values { get; set; }

      /// <summary>
      /// Projection matrix in the new (rectified) coordinate systems for the second camera.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="ProjectionMatrix2Type"/> and 
      /// <see cref="ProjectionMatrix2Values"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Core.Mat ProjectionMatrix2 { get; set; }

      public Cv.Core.Type ProjectionMatrix2Type { get; set; }

      public double[][] ProjectionMatrix2Values { get; set; }

      // Methods

      public void UpdateSerializedProperties()
      {
        RotationMatrixType = RotationMatrix.Type();
        RotationMatrixValues = PropertyValues(RotationMatrix);

        TranslationVectorType = TranslationVector.Type();
        TranslationVectorValues = PropertyValues(TranslationVector);

        RotationMatrix1Type = RotationMatrix1.Type();
        RotationMatrix1Values = PropertyValues(RotationMatrix1);

        RotationMatrix2Type = RotationMatrix2.Type();
        RotationMatrix2Values = PropertyValues(RotationMatrix2);

        ProjectionMatrix1Type = ProjectionMatrix1.Type();
        ProjectionMatrix1Values = PropertyValues(ProjectionMatrix1);

        ProjectionMatrix2Type = ProjectionMatrix2.Type();
        ProjectionMatrix2Values = PropertyValues(ProjectionMatrix2);
      }

      public void UpdateNonSerializedProperties()
      {
        RotationMatrix = CreateProperty(RotationMatrixType, RotationMatrixValues);
        TranslationVector = CreateProperty(TranslationVectorType, TranslationVectorValues);
        RotationMatrix1 = CreateProperty(RotationMatrix1Type, RotationMatrix1Values);
        RotationMatrix2 = CreateProperty(RotationMatrix2Type, RotationMatrix2Values);
        ProjectionMatrix1 = CreateProperty(ProjectionMatrix1Type, ProjectionMatrix1Values);
        ProjectionMatrix2 = CreateProperty(ProjectionMatrix2Type, ProjectionMatrix2Values);
      }

      protected double[][] PropertyValues(Cv.Core.Mat property)
      {
        double[][] propertyValues = new double[property.rows][];
        for (int i = 0; i < property.rows; i++)
        {
          propertyValues[i] = new double[property.cols];
          for (int j = 0; j < property.cols; j++)
          {
            propertyValues[i][j] = property.AtDouble(i, j);
          }
        }
        return propertyValues;
      }

      protected Cv.Core.Mat CreateProperty(Cv.Core.Type propertyType, double[][] propertyValues)
      {
        int rows = propertyValues.Length, cols = propertyValues[0].Length;
        Cv.Core.Mat property = new Cv.Core.Mat(rows, cols, propertyType);
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