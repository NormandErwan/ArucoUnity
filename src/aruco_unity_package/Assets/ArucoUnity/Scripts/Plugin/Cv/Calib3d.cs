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
        // Enums

        public enum CALIB
        {
          USE_INTRINSIC_GUESS = 0x00001,
          FIX_ASPECT_RATIO = 0x00002,
          FIX_PRINCIPAL_POINT = 0x00004,
          ZERO_TANGENT_DIST = 0x00008,
          FIX_FOCAL_LENGTH = 0x00010,
          FIX_K1 = 0x00020,
          FIX_K2 = 0x00040,
          FIX_K3 = 0x00080,
          FIX_K4 = 0x00800,
          FIX_K5 = 0x01000,
          FIX_K6 = 0x02000,
          RATIONAL_MODEL = 0x04000,
          THIN_PRISM_MODEL = 0x08000,
          FIX_S1_S2_S3_S4 = 0x10000,
          TILTED_MODEL = 0x40000,
          FIX_TAUX_TAUY = 0x80000,
          // only for stereo
          FIX_INTRINSIC = 0x00100,
          SAME_FOCAL_LENGTH = 0x00200,
          // for stereo rectification
          ZERO_DISPARITY = 0x00400,
          USE_LU = (1 << 17)
        };

        // Static Member Functions
        [DllImport("ArucoUnity")]
        static extern void au_cv_calib3d_Rodrigues(System.IntPtr rotationVector, out System.IntPtr rotationMatrix, System.IntPtr exception);

        public static void Rodrigues(Core.Vec3d rotationVector, out Core.Mat rotationMatrix)
        {
          Core.Exception exception = new Core.Exception();
          System.IntPtr rotationMatPtr;
          au_cv_calib3d_Rodrigues(rotationVector.cppPtr, out rotationMatPtr, exception.cppPtr);
          rotationMatrix = new Core.Mat(rotationMatPtr);
          exception.Check();
        }
      }
    }
  }

  /// \} aruco_unity_package
}