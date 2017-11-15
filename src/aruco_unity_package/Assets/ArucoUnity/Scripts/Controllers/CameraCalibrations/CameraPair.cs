using ArucoUnity.Cameras;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.CameraCalibrations
  {
    [Serializable]
    public class CameraPair
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The id of first camera of the stereo pair.")]
      private int cameraId1;

      [SerializeField]
      [Tooltip("The id of second camera of the stereo pair.")]
      private int cameraId2;

      // Properties

      /// <summary>
      /// The id of first camera of the stereo pair.
      /// </summary>
      public int CameraId1 { get { return cameraId1; } set { cameraId1 = value; } }

      /// <summary>
      /// The id of second camera of the stereo pair.
      /// </summary>
      public int CameraId2 { get { return cameraId2; } set { cameraId2 = value; } }

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
      }
    }
  }

  /// \} aruco_unity_package
}