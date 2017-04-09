using ArucoUnity.Cameras;
using ArucoUnity.Controllers.CalibrationFlagsControllers;
using ArucoUnity.Plugin;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    [Serializable]
    public class StereoCalibrationCameraPair
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The id of first camera of the stereo pair.")]
      private int cameraId1;

      [SerializeField]
      [Tooltip("The id of second camera of the stereo pair.")]
      private int cameraId2;

      [SerializeField]
      [Tooltip("The flags for the stereo calibration.")]
      private CalibrationFlagsBaseController calibrationFlagsController;

      [SerializeField]
      [Tooltip("If true (default), the principal points of the images have the same pixel coordinates in the rectified views.")]
      private bool rectifyZeroDisparity = true;

      [SerializeField]
      [Tooltip("Free scaling parameter between 0 and 1, or -1 (default) for default scaling: 0 to zoom the images so that only valid pixels are"
        + " visible, 1 to shift the images so that no source image pixels are lost.")]
      private double rectifyAlpha = -1;

      [SerializeField]
      [Tooltip("Sets the new focal length in range between the min focal length and the max focal length, between 0 and 1 (default: 0).")]
      private double rectifyFisheyeBalance = 0;

      [SerializeField]
      [Tooltip("Divisor for new focal length (default: 1).")]
      private double rectifyFisheyeFovScale = 1;

      // Properties

      /// <summary>
      /// The id of first camera of the stereo pair.
      /// </summary>
      public int CameraId1 { get { return cameraId1; } set { cameraId1 = value; } }

      /// <summary>
      /// The id of second camera of the stereo pair.
      /// </summary>
      public int CameraId2 { get { return cameraId2; } set { cameraId2 = value; } }

      /// <summary>
      /// The flags for the stereo calibration.
      /// </summary>
      public CalibrationFlagsBaseController CalibrationFlagsController { get { return calibrationFlagsController; } set { calibrationFlagsController = value; } }

      /// <summary>
      /// If true (default), the principal points of the images have the same pixel coordinates in the rectified views.
      /// </summary>
      public bool RectifyZeroDisparity { get { return rectifyZeroDisparity; } set { rectifyZeroDisparity = value; } }

      /// <summary>
      /// Free scaling parameter between 0 and 1, or -1 (default) for default scaling: 0 to zoom the images so that only valid pixels are visible,
      /// 1 to shift the images so that no source image pixels are lost.
      /// </summary>
      public double RectifyAlpha { get { return rectifyAlpha; } set { rectifyAlpha = value; } }

      /// <summary>
      /// New image resolution after rectification. When null (default) or (0,0) is passed, it is set to the original imageSize. Setting it to
      /// larger value can help you preserve details in the original image, especially when there is a big radial distortion.
      /// </summary>
      public Cv.Core.Size NewImageSize { get { return newImageSize; } set { newImageSize = value; } }

      /// <summary>
      /// Sets the new focal length in range between the min focal length and the max focal length, between 0 and 1 (default: 0).
      /// </summary>
      public double RectifyFisheyeBalance { get { return rectifyFisheyeBalance; } set { rectifyFisheyeBalance = value; } }

      /// <summary>
      /// Divisor for new focal length (default: 1).
      /// </summary>
      public double RectifyFisheyeFovScale { get { return rectifyFisheyeFovScale; } set { rectifyFisheyeFovScale = value; } }

      public StereoCameraParameters CameraParameters { get; set; }

      // Variables

      CalibrationFlagsController calibrationFlagsNonFisheyeController;
      CalibrationFlagsFisheyeController calibrationFlagsFisheyeController;
      Cv.Core.Size newImageSize = new Cv.Core.Size();

      // Methods

      /// <summary>
      /// Check if the properties are properly set.
      /// </summary>
      public void PropertyCheck(ArucoCamera arucoCamera)
      {
        // Check for camera ids
        if (CameraId1 >= arucoCamera.CameraNumber)
        {
          throw new ArgumentOutOfRangeException("CameraId1", "The id of the first camera is higher than the number of the cameras.");
        }
        if (CameraId2 >= arucoCamera.CameraNumber)
        {
          throw new ArgumentOutOfRangeException("CameraId2", "The id of the first camera is higher than the number of the cameras.");
        }

        // Check for equality of image sizes
        if (arucoCamera.ImageTextures[CameraId1].width != arucoCamera.ImageTextures[CameraId2].width 
          || arucoCamera.ImageTextures[CameraId1].height != arucoCamera.ImageTextures[CameraId2].height)
        {
          throw new Exception("The two cameras must have the same image size.");
        }

        // Check for calibration flags
        CalibrationFlagsController calibrationFlagsNonFisheyeController = CalibrationFlagsController as CalibrationFlagsController;
        CalibrationFlagsFisheyeController calibrationFlagsFisheyeController = CalibrationFlagsController as CalibrationFlagsFisheyeController;
        if (CalibrationFlagsController == null || calibrationFlagsNonFisheyeController == null || calibrationFlagsFisheyeController != null)
        {
          throw new ArgumentNullException("CalibrationFlagsController", "This property needs to be set to configure the calibrator if there"
            + " is some pair of cameras are set for stereo calibration.");
        }
        if (!arucoCamera.IsFisheye && calibrationFlagsNonFisheyeController == null)
        {
          throw new ArgumentException("CalibrationFlagsController", "The camera used if non fisheye, but the calibration flags are for fisheye"
            + " camera. Use CalibrationFlagsController instead.");
        }
        if (arucoCamera.IsFisheye && calibrationFlagsFisheyeController == null)
        {
          throw new ArgumentException("CalibrationFlagsController", "The camera used if fisheye, but the calibration flags are for non-fisheye"
            + " camera. Use CalibrationFlagsFisheyeController instead.");
        }
      }

      public void Calibrate(ArucoCamera arucoCamera, CameraParameters cameraParameters, Std.VectorVectorPoint3f[] objectPoints,
        Std.VectorVectorPoint2f[] imagePoints)
      {
        // Prepare data
        calibrationFlagsNonFisheyeController = CalibrationFlagsController as CalibrationFlagsController;
        calibrationFlagsFisheyeController = CalibrationFlagsController as CalibrationFlagsFisheyeController;
        Cv.Calib3d.StereoRectifyFlags stereoRectifyFlags = (RectifyZeroDisparity) ? Cv.Calib3d.StereoRectifyFlags.ZeroDisparity : 0;

        // Prepare the camera parameters
        CameraParameters = new StereoCameraParameters()
        {
          CameraIds = new int[] { CameraId1, CameraId2 },
          CalibrationFlagsValue = CalibrationFlagsController.CalibrationFlagsValue
        };

        // Estimates transformation between the two cameras 
        Cv.Core.Mat cameraMatrix1 = cameraParameters.CameraMatrices[CameraId1];
        Cv.Core.Mat distCoeffs1 = cameraParameters.DistCoeffs[CameraId1];
        Cv.Core.Mat cameraMatrix2 = cameraParameters.CameraMatrices[CameraId2];
        Cv.Core.Mat distCoeffs2 = cameraParameters.DistCoeffs[CameraId2];
        Cv.Core.Size imageSize = arucoCamera.Images[CameraId1].Size;
        Cv.Core.Mat rvec, tvec, essentialMatrix, fundamentalMatrix;
        if (!arucoCamera.IsFisheye)
        {
          CameraParameters.ReprojectionError = Cv.Calib3d.StereoCalibrate(objectPoints[CameraId1], imagePoints[CameraId1], imagePoints[CameraId2],
            cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, out rvec, out tvec, out essentialMatrix, out fundamentalMatrix,
            calibrationFlagsNonFisheyeController.CalibrationFlags);
        }
        else
        {
          CameraParameters.ReprojectionError = Cv.Calib3d.Fisheye.StereoCalibrate(objectPoints[CameraId1], imagePoints[CameraId1],
            imagePoints[CameraId2], cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, out rvec, out tvec,
            calibrationFlagsFisheyeController.CalibrationFlags);
        }

        // Computes rectification transforms
        Cv.Core.Mat rotationMatrix1, rotationMatrix2, projectionMatrix1, projectionMatrix2, Q;
        if (!arucoCamera.IsFisheye)
        {
          Cv.Calib3d.StereoRectify(cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, rvec, tvec, out rotationMatrix1,
            out rotationMatrix2, out projectionMatrix1, out projectionMatrix2, out Q, stereoRectifyFlags, RectifyAlpha, NewImageSize);
        }
        else
        {
          Cv.Calib3d.Fisheye.StereoRectify(cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, rvec, tvec, out rotationMatrix1,
            out rotationMatrix2, out projectionMatrix1, out projectionMatrix2, out Q, stereoRectifyFlags, NewImageSize, RectifyFisheyeBalance,
            RectifyFisheyeFovScale);
        }

        // Save the camera parameters
        CameraParameters.RotationMatrix = rvec;
        CameraParameters.TranslationVector = tvec;
        CameraParameters.RotationMatrices = new Cv.Core.Mat[] { rotationMatrix1, rotationMatrix2 };
        CameraParameters.ProjectionMatrices = new Cv.Core.Mat[] { projectionMatrix1, projectionMatrix2 };
      }
    }
  }

  /// \} aruco_unity_package
}