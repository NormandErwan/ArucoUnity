using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Plugin;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity.Cameras.Undistortions
{
  /// <summary>
  /// Manages the undistortion and rectification process for fisheye and omnidir <see cref="ArucoCamera"/>.
  /// 
  /// See the OpenCV's ccalib module documentation for more information:
  /// http://docs.opencv.org/3.4/dd/d12/tutorial_omnidir_calib_main.html
  /// </summary>
  public abstract class OmnidirCameraUndistortionGeneric<T> : ArucoCameraUndistortionGeneric<T>
    where T : ArucoCamera
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

    // Constants

    protected const float minPerspectiveFov = 1f;
    protected const float maxPerspectiveFov = 179f;

    // Editor fields

    [SerializeField]
    [Tooltip("The algorithm to use for the recitification of the images.")]
    private RectificationTypes rectificationType = RectificationTypes.Perspective;

    [SerializeField]
    [Tooltip("The desired field of view for the Unity cameras shooting the undistorted and rectified images.")]
    [Range(1f, 179f)]
    private float[] perspectiveFieldOfViews;

    // Properties

    /// <summary>
    /// Gets or sets the algorithm to use for the rectification of the images. See this tutorial for illustrated examples:
    /// https://docs.opencv.org/3.4/dd/d12/tutorial_omnidir_calib_main.html
    /// </summary>
    public RectificationTypes RectificationType { get { return rectificationType; } set { rectificationType = value; } }

    /// <summary>
    /// Gets or sets the desired field of view for the Unity cameras shooting the undistorted and rectified images.
    /// </summary>
    public float[] PerspectiveFieldOfViews { get { return perspectiveFieldOfViews; } set { perspectiveFieldOfViews = value; } }

    // Variables

    protected Dictionary<RectificationTypes, Cv.Omnidir.Rectifify> rectifyFlags = new Dictionary<RectificationTypes, Cv.Omnidir.Rectifify>()
      {
        { RectificationTypes.Perspective,       Cv.Omnidir.Rectifify.Perspective },
        { RectificationTypes.Cylindrical,       Cv.Omnidir.Rectifify.Cylindrical },
        { RectificationTypes.LongitudeLatitude, Cv.Omnidir.Rectifify.Longlati },
        { RectificationTypes.Stereographic,     Cv.Omnidir.Rectifify.Stereographic }
      };

    // MonoBehaviour methods

    /// <summary>
    /// Resizes the length of the <see cref="perspectiveFieldOfViews"/> editor field to <see cref="ArucoCamera.CameraNumber"/> if different.
    /// </summary>
    protected virtual void OnValidate()
    {
      if (ArucoCamera != null && perspectiveFieldOfViews != null && perspectiveFieldOfViews.Length != ArucoCamera.CameraNumber)
      {
        Array.Resize(ref perspectiveFieldOfViews, ArucoCamera.CameraNumber);
      }
    }

    // ConfigurableController methods

    /// <summary>
    /// Throw exception if <see cref="PerspectiveFieldOfViews"/> length is different than <see cref="ArucoCamera.CameraNumber"/>.
    /// </summary>
    protected override void Configuring()
    {
      base.Configuring();
      if (PerspectiveFieldOfViews.Length != ArucoCamera.CameraNumber)
      {
        throw new Exception("The number of cameras for the perspective desired field of view must be equal to the number of cameras in" +
          "ArucoCamera");
      }
    }

    // ArucoCameraUndistortion methods

    /// <summary>
    /// Initializes the <see cref="RectifiedCameraMatrices"/> using the <see cref="PerspectiveFieldOfViews"/> values for perspective rectification
    /// or uses the recommended values: https://docs.opencv.org/3.3.1/dd/d12/tutorial_omnidir_calib_main.html. Initializes the
    /// <see cref="RectificationMatrices"/> to identity matrix.
    /// </summary>
    protected override void InitializeRectification()
    {
      for (int cameraId = 0; cameraId < CameraParameters.CameraNumber; cameraId++)
      {
        float imageWidth = CameraParameters.ImageWidths[cameraId];
        float imageHeight = CameraParameters.ImageHeights[cameraId];

        if (RectificationType == RectificationTypes.Perspective)
        {
          float cameraFocalLength = imageHeight / (2f * Mathf.Tan(0.5f * PerspectiveFieldOfViews[cameraId] * Mathf.Deg2Rad));
          RectifiedCameraMatrices[cameraId] = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] {
              cameraFocalLength, 0, imageWidth / 2,
              0, cameraFocalLength, imageHeight / 2,
              0, 0, 1
            }).Clone();
        }
        else
        {
          // Uses the camera matrix recommended values: https://docs.opencv.org/3.3.1/dd/d12/tutorial_omnidir_calib_main.html
          RectifiedCameraMatrices[cameraId] = new Cv.Mat(3, 3, Cv.Type.CV_64F, new double[9] {
              imageWidth / 3.1415, 0, 0,
              0, imageHeight / 3.1415, 0,
              0, 0, 1
            }).Clone();
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