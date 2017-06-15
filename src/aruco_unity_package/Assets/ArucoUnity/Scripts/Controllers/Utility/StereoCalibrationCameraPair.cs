using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers.CalibrationFlagsControllers;
using ArucoUnity.Plugin;
using System;
using System.Linq;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.Utility
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
      private CalibrationFlagsController calibrationFlagsController;

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
      /// The flags for the stereo calibration and rectification.
      /// </summary>
      public CalibrationFlagsController CalibrationFlagsController { get { return calibrationFlagsController; } set { calibrationFlagsController = value; } }

      /// <summary>
      /// New image resolution after rectification. When null (default) or (0,0) is passed, it is set to the original imageSize. Setting it to
      /// larger value can help you preserve details in the original image, especially when there is a big radial distortion.
      /// </summary>
      public Cv.Size NewImageSize { get { return newImageSize; } set { newImageSize = value; } }

      // Variables

      CalibrationFlagsPinholeController calibrationFlagsPinholeController;
      CalibrationFlagsOmnidirController calibrationFlagsOmnidirController;
      Cv.Size newImageSize = new Cv.Size();

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
          throw new ArgumentOutOfRangeException("CameraId2", "The id of the second camera is higher than the number of the cameras.");
        }

        // Check for equality of image sizes
        if (arucoCamera.ImageTextures[CameraId1].width != arucoCamera.ImageTextures[CameraId2].width 
          || arucoCamera.ImageTextures[CameraId1].height != arucoCamera.ImageTextures[CameraId2].height)
        {
          throw new Exception("The two cameras must have the same image size.");
        }

        // Check for the calibration flags
        calibrationFlagsPinholeController = CalibrationFlagsController as CalibrationFlagsPinholeController;
        calibrationFlagsOmnidirController = CalibrationFlagsController as CalibrationFlagsOmnidirController;
        if (CalibrationFlagsController == null
          || (calibrationFlagsPinholeController == null && calibrationFlagsOmnidirController == null))
        {
          throw new ArgumentNullException("CalibrationFlagsController", "This property needs to be set to configure the calibrator.");
        }
        else if (arucoCamera.UndistortionType == UndistortionType.Pinhole && calibrationFlagsPinholeController == null)
        {
          throw new ArgumentException("CalibrationFlagsController", "The camera is set for the pinhole undistortion, but the calibration flags are"
            + " not. Use CalibrationFlagsPinholeController instead.");
        }
        else if (new[] { UndistortionType.OmnidirPerspective, UndistortionType.OmnidirCylindrical, UndistortionType.OmnidirLonglati,
          UndistortionType.OmnidirStereographic }.Contains(arucoCamera.UndistortionType) && calibrationFlagsOmnidirController == null)
        {
          throw new ArgumentException("CalibrationFlagsController", "The camera is set for an omnidir undistortion, but the calibration flags are"
            + " not. Use CalibrationFlagsOmnidirController instead.");
        }
      }

      public StereoCameraParameters Calibrate(ArucoCamera arucoCamera, CameraParameters cameraParameters, Std.VectorVectorPoint3f[] objectPoints,
        Std.VectorVectorPoint2f[] imagePoints)
      {
        // Prepare the camera parameters
        StereoCameraParameters stereoCameraParameters = new StereoCameraParameters()
        {
          CameraIds = new int[] { CameraId1, CameraId2 },
          CalibrationFlagsValue = CalibrationFlagsController.CalibrationFlagsValue
        };

        // Estimates transformation between the two cameras 
        Cv.Mat cameraMatrix1 = cameraParameters.CameraMatrices[CameraId1];
        Cv.Mat distCoeffs1 = cameraParameters.DistCoeffs[CameraId1];
        Cv.Mat xi1 = cameraParameters.OmnidirXis[CameraId1];
        Cv.Mat cameraMatrix2 = cameraParameters.CameraMatrices[CameraId2];
        Cv.Mat distCoeffs2 = cameraParameters.DistCoeffs[CameraId2];
        Cv.Mat xi2 = cameraParameters.OmnidirXis[CameraId2];
        Cv.Size imageSize = arucoCamera.Images[CameraId1].Size;
        Cv.Vec3d rvec, tvec;
        Cv.Mat essentialMatrix, fundamentalMatrix;
        if (calibrationFlagsPinholeController)
        {
          stereoCameraParameters.ReprojectionError = Cv.StereoCalibrate(objectPoints[CameraId1], imagePoints[CameraId1], imagePoints[CameraId2],
            cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, out rvec, out tvec, out essentialMatrix, out fundamentalMatrix,
            calibrationFlagsPinholeController.CalibrationFlags);
        }
        else if (calibrationFlagsOmnidirController)
        {
          Cv.Mat rvecsL, tvecsL;
          stereoCameraParameters.ReprojectionError = Cv.Omnidir.StereoCalibrate(objectPoints[CameraId1], imagePoints[CameraId1],
            imagePoints[CameraId2], imageSize, imageSize, cameraMatrix1, xi1, distCoeffs1, cameraMatrix2, xi2, distCoeffs2, out rvec, out tvec,
            out rvecsL, out tvecsL, calibrationFlagsOmnidirController.CalibrationFlags);
        }
        else
        {
          rvec = new Cv.Vec3d();
          tvec = new Cv.Vec3d();
        }

        // Computes rectification transforms
        Cv.Mat rotationMatrix1, rotationMatrix2, newCameraMatrix1, newCameraMatrix2, Q;
        if (calibrationFlagsPinholeController)
        {
          Cv.StereoRectifyFlags stereoRectifyFlags = (calibrationFlagsPinholeController.ZeroDisparity) ? Cv.StereoRectifyFlags.ZeroDisparity : 0;
          Cv.StereoRectify(cameraMatrix1, distCoeffs1, cameraMatrix2, distCoeffs2, imageSize, rvec, tvec, out rotationMatrix1,
            out rotationMatrix2, out newCameraMatrix1, out newCameraMatrix2, out Q, stereoRectifyFlags,
            calibrationFlagsPinholeController.Skew, NewImageSize);
        }
        else if (calibrationFlagsOmnidirController)
        {
          Cv.Omnidir.StereoRectify(rvec, tvec, out rotationMatrix1, out rotationMatrix2);
          newCameraMatrix1 = new Cv.Mat();
          newCameraMatrix2 = new Cv.Mat();
        }
        else
        {
          rotationMatrix1 = new Cv.Mat();
          rotationMatrix2 = new Cv.Mat();
          newCameraMatrix1 = new Cv.Mat();
          newCameraMatrix2 = new Cv.Mat();
        }

        // Save the camera parameters
        stereoCameraParameters.RotationVector = rvec;
        stereoCameraParameters.TranslationVector = tvec;
        stereoCameraParameters.RotationMatrices = new Cv.Mat[] { rotationMatrix1, rotationMatrix2 };
        stereoCameraParameters.NewCameraMatrices = new Cv.Mat[] { newCameraMatrix1, newCameraMatrix2 };

        return stereoCameraParameters;
      }
    }
  }

  /// \} aruco_unity_package
}