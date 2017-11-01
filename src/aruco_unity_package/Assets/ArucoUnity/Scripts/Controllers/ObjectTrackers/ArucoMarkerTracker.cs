using ArucoUnity.Objects;
using ArucoUnity.Plugin;
using System.Collections.Generic;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.ObjectTrackers
  {
    public class ArucoMarkerTracker : ArucoObjectTracker
    {
      // Constants

      protected const float EstimatePoseMarkerLength = 1f;

      protected readonly Color RejectedMarkerCandidatesColor = new Color(100, 0, 255);

      // Properties

      public Dictionary<Aruco.Dictionary, int>[] DetectedMarkers { get; protected internal set; }

      /// <summary>
      /// Vector of the detected marker corners on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
      /// </summary>
      public Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>[] MarkerCorners { get; protected internal set; }

      /// <summary>
      /// Vector of identifiers of the detected markers on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
      /// </summary>
      public Dictionary<Aruco.Dictionary, Std.VectorInt>[] MarkerIds { get; protected internal set; }

      /// <summary>
      /// Vector of the corners with not a correct identification on each <see cref="ArucoCamera.Images"/>. Updated by <see cref="Detect"/>.
      /// </summary>
      public Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>[] RejectedCandidateCorners { get; protected internal set; }

      /// <summary>
      /// Vector of rotation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
      /// </summary>
      public Dictionary<Aruco.Dictionary, Std.VectorVec3d>[] MarkerRvecs { get; protected internal set; }

      /// <summary>
      /// Vector of translation vectors of the detected markers on each <see cref="ArucoCamera.Images"/>.
      /// </summary>
      public Dictionary<Aruco.Dictionary, Std.VectorVec3d>[] MarkerTvecs { get; protected internal set; }

      // ArucoObjectsController related methods

      protected override void ArucoObjectsController_DictionaryAdded(Aruco.Dictionary dictionary)
      {
        for (int cameraId = 0; cameraId < arucoTracker.ArucoCamera.CameraNumber; cameraId++)
        {
          MarkerCorners[cameraId].Add(dictionary, new Std.VectorVectorPoint2f());
          MarkerIds[cameraId].Add(dictionary, new Std.VectorInt());
          RejectedCandidateCorners[cameraId].Add(dictionary, new Std.VectorVectorPoint2f());
          MarkerRvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
          MarkerTvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
          DetectedMarkers[cameraId].Add(dictionary, 0);
        }
      }

      
      protected override void ArucoObjectsController_DictionaryRemoved(Aruco.Dictionary dictionary)
      {
        for (int cameraId = 0; cameraId < arucoTracker.ArucoCamera.CameraNumber; cameraId++)
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

      public override void Activate(ArucoTracker arucoTracker)
      {
        base.Activate(arucoTracker);

        // Initialize the properties and the ArUco objects
        MarkerCorners = new Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>[arucoTracker.ArucoCamera.CameraNumber];
        MarkerIds = new Dictionary<Aruco.Dictionary, Std.VectorInt>[arucoTracker.ArucoCamera.CameraNumber];
        RejectedCandidateCorners = new Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>[arucoTracker.ArucoCamera.CameraNumber];
        MarkerRvecs = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>[arucoTracker.ArucoCamera.CameraNumber];
        MarkerTvecs = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>[arucoTracker.ArucoCamera.CameraNumber];
        DetectedMarkers = new Dictionary<Aruco.Dictionary, int>[arucoTracker.ArucoCamera.CameraNumber];

        for (int cameraId = 0; cameraId < arucoTracker.ArucoCamera.CameraNumber; cameraId++)
        {
          MarkerCorners[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>();
          MarkerIds[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorInt>();
          RejectedCandidateCorners[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorVectorPoint2f>();
          MarkerRvecs[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>();
          MarkerTvecs[cameraId] = new Dictionary<Aruco.Dictionary, Std.VectorVec3d>();
          DetectedMarkers[cameraId] = new Dictionary<Aruco.Dictionary, int>();

          foreach (var arucoObjectDictionary in arucoTracker.ArucoObjects)
          {
            Aruco.Dictionary dictionary = arucoObjectDictionary.Key;

            MarkerCorners[cameraId].Add(dictionary, new Std.VectorVectorPoint2f());
            MarkerIds[cameraId].Add(dictionary, new Std.VectorInt());
            RejectedCandidateCorners[cameraId].Add(dictionary, new Std.VectorVectorPoint2f());
            MarkerRvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
            MarkerTvecs[cameraId].Add(dictionary, new Std.VectorVec3d());
            DetectedMarkers[cameraId].Add(dictionary, 0);
          }
        }
      }

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

      public override void Detect(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
      {
        Std.VectorVectorPoint2f markerCorners, rejectedCandidateCorners;
        Std.VectorInt markerIds;

        Aruco.DetectMarkers(image, dictionary, out markerCorners, out markerIds, arucoTracker.DetectorParameters, out rejectedCandidateCorners);

        DetectedMarkers[cameraId][dictionary] = (int)markerIds.Size();
        MarkerCorners[cameraId][dictionary] = markerCorners;
        MarkerIds[cameraId][dictionary] = markerIds;
        RejectedCandidateCorners[cameraId][dictionary] = rejectedCandidateCorners;
      }

      public override void EstimateTransforms(int cameraId, Aruco.Dictionary dictionary)
      {
        Std.VectorVec3d rvecs = null, tvecs = null;

        if (DetectedMarkers[cameraId][dictionary] > 0 && cameraParameters != null)
        {
          Aruco.EstimatePoseSingleMarkers(MarkerCorners[cameraId][dictionary], EstimatePoseMarkerLength, cameraParameters.CameraMatrices[cameraId],
            cameraParameters.DistCoeffs[cameraId], out rvecs, out tvecs);
        }

        MarkerRvecs[cameraId][dictionary] = rvecs;
        MarkerTvecs[cameraId][dictionary] = tvecs;
      }

      public override void Draw(int cameraId, Aruco.Dictionary dictionary, Cv.Mat image)
      {
        if (DetectedMarkers[cameraId][dictionary] > 0)
        {
          // Draw all the detected markers
          if (arucoTracker.DrawDetectedMarkers)
          {
            // TODO: draw only markers in ArucoObjects list + add option to draw all the detected markers
            Aruco.DrawDetectedMarkers(image, MarkerCorners[cameraId][dictionary], MarkerIds[cameraId][dictionary]);
          }

          // Draw axes of detected tracked markers
          if (arucoTracker.DrawAxes && cameraParameters != null && MarkerRvecs[cameraId][dictionary] != null)
          {
            for (uint i = 0; i < DetectedMarkers[cameraId][dictionary]; i++)
            {
              ArucoObject foundArucoObject;
              int detectedMarkerHashCode = ArucoMarker.GetArucoHashCode(MarkerIds[cameraId][dictionary].At(i));
              if (arucoTracker.ArucoObjects[dictionary].TryGetValue(detectedMarkerHashCode, out foundArucoObject))
              {
                Aruco.DrawAxis(image, cameraParameters.CameraMatrices[cameraId], cameraParameters.DistCoeffs[cameraId],
                  MarkerRvecs[cameraId][dictionary].At(i), MarkerTvecs[cameraId][dictionary].At(i), EstimatePoseMarkerLength);
              }
            }
          }
        }

        // Draw the rejected marker candidates
        if (arucoTracker.DrawRejectedCandidates && RejectedCandidateCorners[cameraId][dictionary].Size() > 0)
        {
          Aruco.DrawDetectedMarkers(image, RejectedCandidateCorners[cameraId][dictionary]);
        }
      }

      public override void Place(int cameraId, Aruco.Dictionary dictionary)
      {
        if (MarkerRvecs[cameraId][dictionary] != null)
        {
          for (uint i = 0; i < DetectedMarkers[cameraId][dictionary]; i++)
          {
            ArucoObject foundArucoObject;
            int detectedMarkerHashCode = ArucoMarker.GetArucoHashCode(MarkerIds[cameraId][dictionary].At(i));
            if (arucoTracker.ArucoObjects[dictionary].TryGetValue(detectedMarkerHashCode, out foundArucoObject))
            {
              float positionFactor = foundArucoObject.MarkerSideLength / EstimatePoseMarkerLength;
              PlaceArucoObject(foundArucoObject, MarkerRvecs[cameraId][dictionary].At(i), MarkerTvecs[cameraId][dictionary].At(i),
                cameraId, positionFactor);
              foundArucoObject.gameObject.transform.localScale = foundArucoObject.MarkerSideLength * Vector3.one; // TODO: move it to PlaceArucoObject?
            }
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}