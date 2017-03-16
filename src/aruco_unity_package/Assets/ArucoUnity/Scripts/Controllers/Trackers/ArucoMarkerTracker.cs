using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using ArucoUnity.Plugin.std;
using ArucoUnity.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  public class ArucoMarkerTracker : ArucoObjectTracker
  {
    // Constants

    protected const float ESTIMATE_POSE_MARKER_LENGTH = 1f;
    
    protected readonly Color REJECTED_MARKERS_CANDIDATES_COLOR = new Color(100, 0, 255);

    // Properties

    public Dictionary<ArucoUnity.Plugin.Dictionary, int>[] DetectedMarkers { get; protected internal set; }

    /// <summary>
    /// Vector of the detected marker corners on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[] MarkerCorners { get; protected internal set; }

    /// <summary>
    /// Vector of identifiers of the detected markers on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorInt>[] MarkerIds { get; protected internal set; }

    /// <summary>
    /// Vector of the corners with not a correct identification on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[] RejectedCandidateCorners { get; protected internal set; }

    /// <summary>
    /// Vector of rotation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[] MarkerRvecs { get; protected internal set; }

    /// <summary>
    /// Vector of translation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
    /// </summary>
    public Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[] MarkerTvecs { get; protected internal set; }

    // ArucoObjectController related methods

    /// <summary>
    /// Update the properties when a new dictionary is added.
    /// </summary>
    /// <param name="dictionary">The new dictionary.</param>
    protected override void ArucoObjectController_DictionaryAdded(Dictionary dictionary)
    {
      base.ArucoObjectController_DictionaryAdded(dictionary);

      for (int cameraId = 0; cameraId < arucoTracker.ArucoCamera.CamerasNumber; cameraId++)
      {
        MarkerCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
        MarkerIds[cameraId].Add(dictionary, new VectorInt());
        RejectedCandidateCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
        MarkerRvecs[cameraId].Add(dictionary, new VectorVec3d());
        MarkerTvecs[cameraId].Add(dictionary, new VectorVec3d());
        DetectedMarkers[cameraId].Add(dictionary, 0);
      }
    }

    /// <summary>
    /// Update the properties when a dictionary is removed.
    /// </summary>
    /// <param name="dictionary">The removed dictionary.</param>
    protected override void ArucoObjectController_DictionaryRemoved(Dictionary dictionary)
    {
      base.ArucoObjectController_DictionaryRemoved(dictionary);

      for (int cameraId = 0; cameraId < arucoTracker.ArucoCamera.CamerasNumber; cameraId++)
      {
        MarkerCorners[cameraId].Remove(dictionary);
        MarkerIds[cameraId].Remove(dictionary);
        RejectedCandidateCorners[cameraId].Remove(dictionary);
        MarkerRvecs[cameraId].Remove(dictionary);
        MarkerTvecs[cameraId].Remove(dictionary);
        DetectedMarkers[cameraId].Remove(dictionary);
      }
    }

    // ArucoObjectTracker methods

    /// <summary>
    /// <see cref="ArucoObjectTracker.Activate(ArucoTracker)"/>
    /// </summary>
    public override void Activate(ArucoTracker arucoTracker)
    {
      base.Activate(arucoTracker);

      // Initialize the properties and the ArUco objects
      MarkerCorners = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[arucoTracker.ArucoCamera.CamerasNumber];
      MarkerIds = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorInt>[arucoTracker.ArucoCamera.CamerasNumber];
      RejectedCandidateCorners = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>[arucoTracker.ArucoCamera.CamerasNumber];
      MarkerRvecs = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[arucoTracker.ArucoCamera.CamerasNumber];
      MarkerTvecs = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>[arucoTracker.ArucoCamera.CamerasNumber];
      DetectedMarkers = new Dictionary<ArucoUnity.Plugin.Dictionary, int>[arucoTracker.ArucoCamera.CamerasNumber];

      for (int cameraId = 0; cameraId < arucoTracker.ArucoCamera.CamerasNumber; cameraId++)
      {
        MarkerCorners[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>();
        MarkerIds[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorInt>();
        RejectedCandidateCorners[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVectorPoint2f>();
        MarkerRvecs[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>();
        MarkerTvecs[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, VectorVec3d>();
        DetectedMarkers[cameraId] = new Dictionary<ArucoUnity.Plugin.Dictionary, int>();

        foreach (var arucoObjectDictionary in arucoTracker.ArucoObjects)
        {
          Dictionary dictionary = arucoObjectDictionary.Key;

          MarkerCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
          MarkerIds[cameraId].Add(dictionary, new VectorInt());
          RejectedCandidateCorners[cameraId].Add(dictionary, new VectorVectorPoint2f());
          MarkerRvecs[cameraId].Add(dictionary, new VectorVec3d());
          MarkerTvecs[cameraId].Add(dictionary, new VectorVec3d());
          DetectedMarkers[cameraId].Add(dictionary, 0);
        }
      }
    }

    /// <summary>
    /// <see cref="ArucoObjectTracker.Deactivate"/>
    /// </summary>
    public override void Deactivate()
    {
      base.Deactivate();

      MarkerCorners = null;
      MarkerIds = null;
      RejectedCandidateCorners = null;
      MarkerRvecs = null;
      MarkerTvecs = null;
      DetectedMarkers = null;
    }

    /// <summary>
    /// <see cref="ArucoObjectTracker.Detect(int, Dictionary)"/>
    /// </summary>
    public override void Detect(int cameraId, Dictionary dictionary)
    {
      if (!IsActivated)
      {
        return;
      }

      VectorVectorPoint2f markerCorners, rejectedCandidateCorners;
      VectorInt markerIds;

      Functions.DetectMarkers(arucoTracker.ArucoCamera.Images[cameraId], dictionary, out markerCorners, out markerIds, 
        arucoTracker.DetectorParameters, out rejectedCandidateCorners);

      DetectedMarkers[cameraId][dictionary] = (int)markerIds.Size();
      MarkerCorners[cameraId][dictionary] = markerCorners;
      MarkerIds[cameraId][dictionary] = markerIds;
      RejectedCandidateCorners[cameraId][dictionary] = rejectedCandidateCorners;
    }

    /// <summary>
    /// <see cref="ArucoObjectTracker.EstimateTranforms(int, Dictionary)"/>
    /// </summary>
    public override void EstimateTranforms(int cameraId, Dictionary dictionary)
    {
      if (!IsActivated || DetectedMarkers[cameraId][dictionary] <= 0)
      {
        return;
      }

      CameraParameters cameraParameters = arucoTracker.ArucoCamera.CameraParameters;

      VectorVec3d rvecs, tvecs;
      Functions.EstimatePoseSingleMarkers(MarkerCorners[cameraId][dictionary], ESTIMATE_POSE_MARKER_LENGTH,
        cameraParameters.CamerasMatrix[cameraId], cameraParameters.DistCoeffs[cameraId], out rvecs, out tvecs);

      MarkerRvecs[cameraId][dictionary] = rvecs;
      MarkerTvecs[cameraId][dictionary] = tvecs;
    }

    /// <summary>
    /// <see cref="ArucoObjectTracker.Draw(int, Dictionary)"/>
    /// </summary>
    public override void Draw(int cameraId, Dictionary dictionary)
    {
      if (!IsActivated)
      {
        return;
      }

      bool updatedCameraImage = false;
      Mat[] cameraImages = arucoTracker.ArucoCamera.Images;

      // Draw the detected markers
      // TODO: draw only markers in ArucoObjects list + add option to draw all the detected markers
      if (arucoTracker.DrawDetectedMarkers && DetectedMarkers[cameraId][dictionary] > 0)
      {
        Functions.DrawDetectedMarkers(cameraImages[cameraId], MarkerCorners[cameraId][dictionary], MarkerIds[cameraId][dictionary]);
        updatedCameraImage = true;
      }

      // Draw the rejected marker candidates
      if (arucoTracker.DrawRejectedCandidates && RejectedCandidateCorners[cameraId][dictionary].Size() > 0)
      {
        Functions.DrawDetectedMarkers(cameraImages[cameraId], RejectedCandidateCorners[cameraId][dictionary], REJECTED_MARKERS_CANDIDATES_COLOR);
        updatedCameraImage = true;
      }

      if (updatedCameraImage)
      {
        arucoTracker.ArucoCamera.Images = cameraImages;
      }
    }

    /// <summary>
    /// <see cref="ArucoObjectTracker.Place(int, Dictionary)"/>
    /// </summary>
    public override void Place(int cameraId, Dictionary dictionary)
    {
      if (!IsActivated)
      {
        return;
      }

      for (uint i = 0; i < DetectedMarkers[cameraId][dictionary]; i++)
      {
        ArucoObject foundArucoObject;
        int detectedMarkerHashCode = ArucoMarker.GetArucoHashCode(MarkerIds[cameraId][dictionary].At(i));
        if (arucoTracker.ArucoObjects[dictionary].TryGetValue(detectedMarkerHashCode, out foundArucoObject))
        {
          float positionFactor = foundArucoObject.MarkerSideLength / ESTIMATE_POSE_MARKER_LENGTH;
          PlaceArucoObject(foundArucoObject, MarkerRvecs[cameraId][dictionary].At(i), MarkerTvecs[cameraId][dictionary].At(i), 
            cameraId, positionFactor);
        }
      }
    }
  }

  /// \} aruco_unity_package
}