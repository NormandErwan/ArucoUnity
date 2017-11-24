using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Controllers.CameraDisplays;
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
    public abstract class ArucoCameraGenericOmnidirUndistortion<T, U> : ArucoCameraUndistortion<T, U>
      where T : ArucoCamera
      where U : ArucoCameraGenericDisplay<T>
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

      // ArucoCameraUndistortion methods

      protected override void InitializeRectification()
      {
        for (int cameraId = 0; cameraId < CameraParameters.CameraNumber; cameraId++)
        {
          float imageWidth = CameraParameters.ImageWidths[cameraId];
          float imageHeight = CameraParameters.ImageHeights[cameraId];

          // Uses the camera matrix recommended values: https://docs.opencv.org/3.3.1/dd/d12/tutorial_omnidir_calib_main.html
          if (RectificationType == RectificationTypes.Perspective)
          {
            RectifiedCameraMatrices[cameraId] = new Cv.Mat(3, 3, Cv.Type.CV_64F,
              new double[9] { imageWidth / 4, 0, imageWidth / 2, 0, imageHeight / 4, imageHeight / 2, 0, 0, 1 }).Clone();
          }
          else
          {
            RectifiedCameraMatrices[cameraId] = new Cv.Mat(3, 3, Cv.Type.CV_64F,
              new double[9] { imageWidth / 3.1415, 0, 0, 0, imageHeight / 3.1415, 0, 0, 0, 1 }).Clone();
          }
          RectificationMatrices[cameraId] = noRectificationMatrix;
        }
      }

      protected override void InitializeUndistortionMaps()
      {
        for (int cameraId = 0; cameraId < CameraParameters.CameraNumber; cameraId++)
        {
          Cv.Omnidir.InitUndistortRectifyMap(CameraParameters.CameraMatrices[cameraId], CameraParameters.DistCoeffs[cameraId],
            CameraParameters.OmnidirXis[cameraId], RectificationMatrices[cameraId], RectifiedCameraMatrices[cameraId],
            ArucoCamera.Images[cameraId].Size, Cv.Type.CV_16SC2, out UndistortionRectificationMaps[cameraId][0],
            out UndistortionRectificationMaps[cameraId][1], rectifyFlags[RectificationType]);
        }
      }
    }
  }

  /// \} aruco_unity_package
}