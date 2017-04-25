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
    public abstract class ArucoObjectTracker
    {
      // Properties

      public bool IsActivated { get; protected set; }

      // Variables

      protected ArucoTracker arucoTracker;

      // Constructor

      public ArucoObjectTracker()
      {
        Deactivate();
      }

      // ArucoObject related methods

      /// <summary>
      /// Before the ArUco object's properties will be updated, restore the game object's scale of this object.
      /// </summary>
      public virtual void RestoreGameObjectScale(ArucoObject arucoObject)
      {
        if (arucoObject.MarkerSideLength != 0)
        {
          arucoObject.gameObject.transform.localScale /= arucoObject.MarkerSideLength;
        }
      }

      /// <summary>
      /// Adjust the game object's scale of the ArUco object according to its MarkerSideLength property.
      /// </summary>
      public virtual void AdjustGameObjectScale(ArucoObject arucoObject)
      {
        if (arucoObject.MarkerSideLength != 0)
        {
          arucoObject.gameObject.transform.localScale *= arucoObject.MarkerSideLength;
        }
      }

      // ArucoObjectController related methods

      protected virtual void ArucoObjectController_DictionaryAdded(Aruco.Dictionary dictionary)
      {
        if (!IsActivated)
        {
          return;
        }
      }

      protected virtual void ArucoObjectController_DictionaryRemoved(Aruco.Dictionary dictionary)
      {
        if (!IsActivated)
        {
          return;
        }
      }

      // Methods

      /// <summary>
      /// Initialize the properties and the tracker.
      /// </summary>
      public virtual void Activate(ArucoTracker arucoTracker)
      {
        this.arucoTracker = arucoTracker;
        IsActivated = true;

        arucoTracker.DictionaryAdded += ArucoObjectController_DictionaryAdded;
        arucoTracker.DictionaryRemoved += ArucoObjectController_DictionaryRemoved;
      }

      /// <summary>
      /// Deactivate the tracker.
      /// </summary>
      public virtual void Deactivate()
      {
        if (arucoTracker != null)
        {
          arucoTracker.DictionaryAdded -= ArucoObjectController_DictionaryAdded;
          arucoTracker.DictionaryRemoved -= ArucoObjectController_DictionaryRemoved;
        }
        arucoTracker = null;
        IsActivated = false;
      }

      /// <summary>
      /// Detect the ArUco objects on each <see cref="ArucoCamera.Images"/>. Should be called during the OnImagesUpdated() event, after the update of 
      /// the CameraImageTexture.
      /// </summary>
      public abstract void Detect(int cameraId, Aruco.Dictionary dictionary);

      /// <summary>
      /// Estimate the gameObject's transform of each detected ArUco object. Works on the results of 
      /// <see cref="Detect(int, Dictionary)"/>.
      /// </summary>
      public abstract void EstimateTranforms(int cameraId, Aruco.Dictionary dictionary);

      /// <summary>
      /// Draw the detected ArUco objects on each <see cref="ArucoCamera.Images"/>. Works on the results of 
      /// <see cref="Detect(int, Dictionary)"/>.
      /// </summary>
      public abstract void Draw(int cameraId, Aruco.Dictionary dictionary);

      /// <summary>
      /// Place and orient the detected ArUco objects on the first camera image according to the results of 
      /// <see cref="EstimateTranforms(int, Dictionary)"/>.
      /// </summary>
      public abstract void Place(int cameraId, Aruco.Dictionary dictionary);

      /// <summary>
      /// Place and orient an ArUco object.
      /// </summary>
      protected void PlaceArucoObject(ArucoObject arucoObject, Cv.Vec3d rvec, Cv.Vec3d tvec, int cameraId, float positionFactor = 1f)
      {
        GameObject arucoGameObject = arucoObject.gameObject;
        Camera camera = arucoTracker.ArucoCamera.ImageCameras[cameraId];

        // Place and orient the object to match the marker
        Transform arucoObjectTransform = arucoGameObject.transform;
        arucoGameObject.transform.SetParent(camera.transform);
        arucoGameObject.transform.localPosition = tvec.ToPosition() * positionFactor;
        arucoGameObject.transform.localRotation = rvec.ToRotation();
        arucoGameObject.transform.SetParent(arucoObjectTransform);

        // Adjust the object position
        Vector3 cameraOpticalCenter = arucoTracker.ArucoCamera.CameraParameters.OpticalCenters[cameraId];

        Vector3 imageCenter = new Vector3(0.5f, 0.5f, arucoGameObject.transform.position.z);
        Vector3 opticalCenter = new Vector3(cameraOpticalCenter.x, cameraOpticalCenter.y, arucoGameObject.transform.position.z);
        Vector3 opticalShift = camera.ViewportToWorldPoint(opticalCenter) - camera.ViewportToWorldPoint(imageCenter);

        // TODO: fix the position shift orientation
        Vector3 positionShift = opticalShift // Take account of the optical center not in the image center
          + arucoGameObject.transform.up * arucoGameObject.transform.localScale.y / 2; // Move up the object to coincide with the marker
        arucoGameObject.transform.localPosition += positionShift;

        //print(arucoGameObject.name + " - imageCenter: " + imageCenter.ToString("F3") + "; opticalCenter: " + opticalCenter.ToString("F3")
        //  + "; positionShift: " + (arucoGameObject.transform.rotation * opticalShift).ToString("F4"));

        arucoGameObject.SetActive(true);
      }
    }
  }

  /// \} aruco_unity_package
}