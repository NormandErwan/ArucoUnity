using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    namespace cv
    {
      public static class Calib3d
      {
        // Static Member Functions
        [DllImport("ArucoUnity")]
        static extern void au_calib3d_Rodrigues(System.IntPtr rotationVector, out System.IntPtr rotationMatrix, System.IntPtr exception);

        public static void Rodrigues(Vec3d rotationVector, out Mat rotationMatrix)
        {
          Exception exception = new Exception();
          System.IntPtr rotationMatPtr;
          au_calib3d_Rodrigues(rotationVector.cvPtr, out rotationMatPtr, exception.cvPtr);
          rotationMatrix = new Mat(rotationMatPtr);
          exception.Check();
        }
      }
    }
  }

  /// \} aruco_unity_package
}