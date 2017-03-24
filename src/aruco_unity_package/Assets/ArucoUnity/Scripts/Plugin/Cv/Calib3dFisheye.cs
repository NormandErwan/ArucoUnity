using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Cv
    {
      public static partial class Calib3d
      {
        public static partial class Fisheye
        {
          // Enums

          public enum Calib
          {
            UseIntrinsicGuess = 1 << 0,
            RecomputeExtrinsic = 1 << 1,
            CheckCond = 1 << 2,
            FixSkew = 1 << 3,
            FixK1 = 1 << 4,
            FixK2 = 1 << 5,
            FixK3 = 1 << 6,
            FixK4 = 1 << 7,
            FixIntrinsic = 1 << 8,
            FixPrincipalPoint = 1 << 9
          };
        }
      }
    }
  }

  /// \} aruco_unity_package
}