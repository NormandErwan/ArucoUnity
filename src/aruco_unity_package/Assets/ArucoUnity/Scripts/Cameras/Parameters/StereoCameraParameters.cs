using ArucoUnity.Plugin;
using System;
using System.Xml.Serialization;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Parameters
  {
    /// <summary>
    /// Manages the camera parameters of a stereo camera calibration.
    /// </summary>
    [Serializable]
    public class StereoCameraParameters
    {
      // Const

      public const int CameraNumber = 2;

      // Constructors

      /// <summary>
      /// Creates an empty StereoCameraParameters.
      /// </summary>
      /// <remarks>This constructor is needed for the serialization.</remarks>
      public StereoCameraParameters()
      {
      }

      // Properties

      /// <summary>
      /// Gets or sets the stereo calibration flags used.
      /// </summary>
      public int CalibrationFlagsValue { get; set; }

      /// <summary>
      /// Gets or sets the rotation matrix between the first and the second camera coordinate systems.
      /// </summary>
      /// <remarks>When <see cref="UpdateSerializedProperties"/> is called, it's copied to the <see cref="RotationVectorValues"/> property.</remarks>
      [XmlIgnore]
      public Cv.Vec3d RotationVector { get; set; }

      /// <summary>
      /// Gets or sets the rotation matrix values between the coordinate systems of the cameras. Equals to the <see cref="RotationVector"/>
      /// content and automatically written when <see cref="CameraParameters.SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public double[] RotationVectorValues { get; set; }

      /// <summary>
      /// Gets or sets the translation vector between the coordinate systems of the cameras.
      /// </summary>
      /// <remarks>When <see cref="UpdateSerializedProperties"/> is called, it's copied to the <see cref="TranslationVectorValues"/>
      /// property.</remarks>
      [XmlIgnore]
      public Cv.Vec3d TranslationVector { get; set; }

      /// <summary>
      /// Gets or sets the translation vector values between the coordinate systems of the cameras. Equals to the <see cref="TranslationVector"/>
      /// content and automatically written when <see cref="CameraParameters.SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public double[] TranslationVectorValues { get; set; }

      /// <summary>
      /// Gets or sets the average re-projection error of the calibration.
      /// </summary>
      public double ReprojectionError { get; set; }

      /// <summary>
      /// Gets or sets the rotation matrix (rectification transform) for the two cameras of the stereo pair.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's copied to the <see cref="RotationMatricesType"/> and
      /// <see cref="RotationMatricesValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Mat[] RotationMatrices { get; set; }

      /// <summary>
      /// Gets or sets the rotation matrix type of the calibration. Equals to <see cref="RotationMatrices.Type()"/> and automatically written when
      /// <see cref="CameraParameters.SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public Cv.Type RotationMatricesType { get; set; }

      /// <summary>
      /// Gets or sets the xi parameter values of the calibration. Equals to the <see cref="RotationMatrices"/> content and automatically written
      /// when <see cref="CameraParameters.SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public double[][][] RotationMatricesValues { get; set; }

      /// <summary>
      /// Projection matrix in the new (rectified) coordinate systems for the two cameras of the stereo pair.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's copied to the <see cref="NewCameraMatricesType"/> and
      /// <see cref="NewCameraMatricesValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Mat[] NewCameraMatrices { get; set; }

      /// <summary>
      /// Gets or sets the new camera matrix type of the calibration. Equals to <see cref="NewCameraMatrices.Type()"/> and automatically written when
      /// <see cref="CameraParameters.SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>

      public Cv.Type NewCameraMatricesType { get; set; }

      /// <summary>
      /// Gets or sets the new camera matrix values of the calibration. Equals to the <see cref="NewCameraMatrices"/> content and automatically
      /// written when <see cref="CameraParameters.SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public double[][][] NewCameraMatricesValues { get; set; }

      // Methods

      /// <summary>
      /// Updates the serialized properties from the non serialized properties.
      /// </summary>
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

      /// <summary>
      /// Initializes the non serialized properties from the serialized properties.
      /// </summary>
      public void UpdateNonSerializedProperties()
      {
        RotationVector = new Cv.Vec3d(RotationVectorValues[0], RotationVectorValues[1], RotationVectorValues[2]);
        TranslationVector = new Cv.Vec3d(TranslationVectorValues[0], TranslationVectorValues[1], TranslationVectorValues[2]);
        RotationMatrices = CameraParameters.CreateProperty(RotationMatricesType, RotationMatricesValues);
        NewCameraMatrices = CameraParameters.CreateProperty(NewCameraMatricesType, NewCameraMatricesValues);
      }
    }
  }

  /// \} aruco_unity_package
}