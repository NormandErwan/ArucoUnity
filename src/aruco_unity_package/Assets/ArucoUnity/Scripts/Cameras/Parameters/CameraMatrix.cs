using ArucoUnity.Plugin;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras.Parameters
  {
    public class CameraMatrix : Cv.Mat
    {
      /// <summary>
      /// Gets the camera focal lengths, expressed in pixels units. Equals to
      /// <code>F = (AtDouble(0, 0), AtDouble(1, 1))</code>
      /// </summary>
      public Vector2 GetCameraFocalLengths()
      {
        return new Vector2((float)AtDouble(0, 0), (float)AtDouble(1, 1));
      }

      /// <summary>
      /// Gets the camera principal point, expressed in pixels units. Equals to
      /// <code>C = (AtDouble(0, 2), AtDouble(1, 2))</code>
      /// </summary>
      public Vector2 GetCameraPrincipalPoint()
      {
        return new Vector2((float)AtDouble(0, 2), (float)AtDouble(1, 2));
      }
    }
  }
}