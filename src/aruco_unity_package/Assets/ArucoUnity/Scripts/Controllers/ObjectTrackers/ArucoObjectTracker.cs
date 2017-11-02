using ArucoUnity.Cameras.Parameters;
using ArucoUnity.Objects;
using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{
  /// 

  namespace Controllers.ObjectTrackers
  {
    /// <summary>
    /// Base for detecting and estimating the transform of an ArUco object.
    /// </summary>
    public abstract class ArucoObjectTracker
    {
      // Properties

      /// <summary>
      /// Is the tracker configured and activated?
      /// </summary>
      public bool IsActivated { get; protected set; }

      // Variables

      protected ArucoTracker arucoTracker;
      protected CameraParameters cameraParameters;

      // ArucoObjectsController related methods

      /// <summary>
      /// Update the properties when a new dictionary is added.
      /// </summary>
      /// <param name="dictionary">The new dictionary.</param>
      protected virtual void ArucoObjectsController_DictionaryAdded(Aruco.Dictionary dictionary)
      {
      }

      /// <summary>
      /// Update the properties when a dictionary is removed.
      /// </summary>
      /// <param name="dictionary">The removed dictionary.</param>
      protected virtual void ArucoObjectsController_DictionaryRemoved(Aruco.Dictionary dictionary)
      {
      }

      // Methods

      /// <summary>
      /// Configure and activate the tracker.
      /// </summary>
      public virtual void Activate(ArucoTracker arucoTracker)
      {
        this.arucoTracker = arucoTracker;
        cameraParameters = arucoTracker.ArucoCamera.CameraParameters;
        IsActivated = true;

        arucoTracker.DictionaryAdded += ArucoObjectsController_DictionaryAdded;
        arucoTracker.DictionaryRemoved += ArucoObjectsController_DictionaryRemoved;
      }


      /// <summary>
      /// Deactivate the tracker.
      /// </summary>
      public virtual void Deactivate()
      {
        arucoTracker.DictionaryAdded -= ArucoObjectsController_DictionaryAdded;
        arucoTracker.DictionaryRemoved -= ArucoObjectsController_DictionaryRemoved;

        IsActivated = false;
        arucoTracker = null;
        cameraParameters = null;
      }

      /// <summary>
      /// Detect the ArUco objects on the current image of a camera.
      /// </summary>
      /// <param name="cameraId">The id of the camera to use.</param>
      /// <param name="dictionary">The dictionary to use for the detection.</param>
      public virtual void Detect(int cameraId, Aruco.Dictionary dictionary)
      {
        if (IsActivated)
        {
          Detect(cameraId, dictionary, arucoTracker.ArucoCamera.Images[cameraId]);
        }
      }

      /// <summary>
      /// Detect the ArUco objects for a camera on an custom image.
      /// </summary>
      /// <param name="cameraId">The id of the camera.</param>
      /// <param name="dictionary">The dictionary to use for the detection.</param>
      /// <param name="dictionary">The image to use for the detection.</param>
      public abstract void Detect(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image);

      /// <summary>
      /// Estimate the gameObject's transform of each detected ArUco object.
      /// </summary>
      /// <param name="cameraId">The id of the camera to use.</param>
      /// <param name="dictionary">The dictionary to use.</param>
      public abstract void EstimateTransforms(int cameraId, Aruco.Dictionary dictionary);

      /// <summary>
      /// Draw the detected ArUco objects on the current image of a camera.
      /// </summary>
      /// <param name="cameraId">The id of the camera to use.</param>
      /// <param name="dictionary">The dictionary to use.</param>
      public virtual void Draw(int cameraId, Aruco.Dictionary dictionary)
      {
        if (IsActivated)
        {
          Draw(cameraId, dictionary, arucoTracker.ArucoCamera.Images[cameraId]);
        }
      }

      /// <summary>
      /// Draw the detected ArUco objects for a camera on a custom image.
      /// </summary>
      /// <param name="cameraId">The id of the camera to use.</param>
      /// <param name="dictionary">The dictionary to use.</param>
      /// <param name="image">Draw on this image.</param>
      public abstract void Draw(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image);

      /// <summary>
      /// Place and orient the detected ArUco objects relative to a camera.
      /// </summary>
      /// <param name="cameraId">The id of the camera to use.</param>
      /// <param name="dictionary">The dictionary to use.</param>
      public abstract void Place(int cameraId, Aruco.Dictionary dictionary);

      /// <summary>
      /// Update the gameObject's transform of an ArUco object.
      /// </summary>
      /// <param name="arucoObject">The ArUco object to place.</param>
      /// <param name="rvec">The estimated rotation vector of the ArUco object.</param>
      /// <param name="tvec">The estimated translation vector of the ArUco object.</param>
      /// <param name="cameraId">The id of the camera to use. The gameObject is placed and oriented relative to this camera.</param>
      /// <param name="positionFactor">Factor on the position vector.</param>
      protected void PlaceArucoObject(ArucoObject arucoObject, Cv.Vec3d rvec, Cv.Vec3d tvec, int cameraId, float positionFactor = 1f)
      {
        GameObject arucoGameObject = arucoObject.gameObject;

        arucoGameObject.transform.SetParent(arucoTracker.ArucoCamera.ImageCameras[cameraId].transform);
        arucoGameObject.transform.localPosition = tvec.ToPosition() * positionFactor;
        arucoGameObject.transform.localRotation = rvec.ToRotation();

        arucoGameObject.SetActive(true);
      }
    }
  }

  /// \} aruco_unity_package
}