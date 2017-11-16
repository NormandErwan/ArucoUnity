using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;
using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraUndistortions
  {
    /// <summary>
    /// Manages the undistortion and rectification process of fisheye and omnidir cameras.
    /// 
    /// See the OpenCV's ccalib module documentation for more information:
    /// http://docs.opencv.org/3.3.0/dd/d12/tutorial_omnidir_calib_main.html
    /// </summary>
    public class OmnidirArucoCameraUndistortion : ArucoCameraUndistortion
    {
      /// <summary>
      /// The different algorithms to use for the undistortion of the images.
      /// </summary>
      public enum RectificationTypes
      {
        Perspective,
        Cylindrical,
        LongitudeLatitude,
        Stereographic
      }

      // Editor fields

      [SerializeField]
      [Tooltip("The algorithm to use for the recitification of the images.")]
      private RectificationTypes rectificationType = RectificationTypes.Perspective;

      // Properties

      /// <summary>
      /// Gets or sets the algorithm to use for the rectification of the images. See this tutorial for illustrated examples:
      /// https://docs.opencv.org/3.3.0/dd/d12/tutorial_omnidir_calib_main.html
      /// </summary>
      public RectificationTypes RectificationType { get { return rectificationType; } set { rectificationType = value; } }

      // Variables

      protected Dictionary<RectificationTypes, Cv.Omnidir.Rectifify> rectifyFlags = new Dictionary<RectificationTypes, Cv.Omnidir.Rectifify>()
      {
        { RectificationTypes.Perspective,       Cv.Omnidir.Rectifify.Perspective },
        { RectificationTypes.Cylindrical,       Cv.Omnidir.Rectifify.Cylindrical },
        { RectificationTypes.LongitudeLatitude, Cv.Omnidir.Rectifify.Longlati },
        { RectificationTypes.Stereographic,     Cv.Omnidir.Rectifify.Stereographic }
      };

      // CalibrationController methods

      protected override void ConfigureUndistortionRectification(int cameraId, Cv.Mat rectificationMatrix, Cv.Mat newCameraMatrix)
      {
        var cameraParameters = CameraParametersController.CameraParameters;
        float imageWidth = cameraParameters.ImageWidths[cameraId];
        float imageHeight = cameraParameters.ImageHeights[cameraId];

        if (RectificationType == RectificationTypes.Perspective)
        {
          // Configure the rectified camera matrix from the camera's fov for perspective rectification
          float cameraF = imageHeight / (2f * Mathf.Tan(ArucoCamera.ImageCameras[cameraId].fieldOfView * Mathf.Deg2Rad / 2f));
          newCameraMatrix = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] { cameraF, 0, imageWidth / 2, 0, cameraF, imageHeight / 2, 0, 0, 1 }).Clone();
        }
        else
        {
          // Configure the rectified camera matrix with the recommended values by this tutorial for other rectification types:
          // https://docs.opencv.org/3.3.1/dd/d12/tutorial_omnidir_calib_main.html
          newCameraMatrix = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] { imageWidth / 3.1415, 0, 0, 0, imageHeight / 3.1415, 0, 0, 0, 1 }).Clone();
        }

        // Configure the undistortion maps
        Cv.Omnidir.InitUndistortRectifyMap(cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId],
          cameraParameters.OmnidirXis[cameraId], rectificationMatrix, newCameraMatrix, ArucoCamera.Images[cameraId].Size, Cv.Type.CV_16SC2,
          out UndistortionRectificationMaps[cameraId][0], out UndistortionRectificationMaps[cameraId][1], rectifyFlags[RectificationType]);

        RectifiedCameraMatrices[cameraId] = newCameraMatrix;
      }
    }
  }

  /// \} aruco_unity_package
}