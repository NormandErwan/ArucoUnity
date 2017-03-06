using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Utility;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{
  /// 

  public abstract class ArucoObjectTracker
  {
    // Variables

    protected ArucoTracker arucoTracker;

    // ArucoObject related methods

    /// <summary>
    /// Before the ArUco object's properties will be updated, restore the game object's scale of this object.
    /// </summary>
    public virtual void ArucoObject_PropertyUpdating(ArucoObject arucoObject)
    {
      if (arucoObject.MarkerSideLength != 0)
      {
        arucoObject.gameObject.transform.localScale /= arucoObject.MarkerSideLength;
      }
    }

    /// <summary>
    /// Adjust the game object's scale of the ArUco object according to its MarkerSideLength property.
    /// </summary>
    public virtual void ArucoObject_PropertyUpdated(ArucoObject arucoObject)
    {
      if (arucoObject.MarkerSideLength != 0)
      {
        arucoObject.gameObject.transform.localScale *= arucoObject.MarkerSideLength;
      }
    }

    // ArucoObjectController related methods

    public virtual void ArucoObjectController_DictionaryAdded(Dictionary dictionary)
    {
    }

    public virtual void ArucoObjectController_DictionaryRemoved(Dictionary dictionary)
    {
    }

    // Methods

    /// <summary>
    /// Initialize the properties and the tracking.
    /// </summary>
    public virtual void Configure(ArucoTracker arucoTracker)
    {
      this.arucoTracker = arucoTracker;
    }

    /// <summary>
    /// Detect the ArUco objects on each <see cref="ArucoCamera.Images"/>. Should be called during the OnImagesUpdated() event, after the update of 
    /// the CameraImageTexture.
    /// </summary>
    public abstract void Detect(int cameraId, Dictionary dictionary);

    /// <summary>
    /// Estimate the gameObject's transform of each detected ArUco object. Works on the results of 
    /// <see cref="Detect(int, Dictionary)"/>.
    /// </summary>
    public abstract void EstimateTranforms(int cameraId, Dictionary dictionary);

    /// <summary>
    /// Draw the detected ArUco objects on each <see cref="ArucoCamera.Images"/>. Works on the results of 
    /// <see cref="Detect(int, Dictionary)"/>.
    /// </summary>
    public abstract void Draw(int cameraId, Dictionary dictionary);

    /// <summary>
    /// Place and orient the detected ArUco objects on the first camera image according to the results of 
    /// <see cref="EstimateTranforms(int, Dictionary)"/>.
    /// </summary>
    public abstract void Place(int cameraId, Dictionary dictionary);

    /// <summary>
    /// Place and orient an ArUco object.
    /// </summary>
    protected void PlaceArucoObject(ArucoObject arucoObject, Vec3d rvec, Vec3d tvec, int cameraId, float positionFactor = 1f)
    {
      GameObject arucoGameObject = arucoObject.gameObject;

      // Place and orient the object to match the marker
      arucoGameObject.transform.position = tvec.ToPosition() * positionFactor;
      arucoGameObject.transform.rotation = rvec.ToRotation();

      // Adjust the object position
      Camera camera = arucoTracker.ArucoCamera.ImageCameras[cameraId];
      Vector3 cameraOpticalCenter = arucoTracker.ArucoCamera.CameraParameters[cameraId].OpticalCenter;

      Vector3 imageCenter = new Vector3(0.5f, 0.5f, arucoGameObject.transform.position.z);
      Vector3 opticalCenter = new Vector3(cameraOpticalCenter.x, cameraOpticalCenter.y, arucoGameObject.transform.position.z);
      Vector3 opticalShift = camera.ViewportToWorldPoint(opticalCenter) - camera.ViewportToWorldPoint(imageCenter);

      Vector3 positionShift = opticalShift // Take account of the optical center not in the image center
        + arucoGameObject.transform.up * arucoGameObject.transform.localScale.y / 2; // Move up the object to coincide with the marker
      arucoGameObject.transform.localPosition += positionShift;

      //print(arucoGameObject.name + " - imageCenter: " + imageCenter.ToString("F3") + "; opticalCenter: " + opticalCenter.ToString("F3")
      //  + "; positionShift: " + (arucoGameObject.transform.rotation * opticalShift).ToString("F4"));

      arucoGameObject.SetActive(true);
    }
  }

  /// \} aruco_unity_package
}