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

        public enum Calib
        {
          UseIntrinsicGuess = 0x00001,
          FixAspectRatio = 0x00002,
          FixPrincipalPoint = 0x00004,
          ZeroTangentDist = 0x00008,
          FixFocalLength = 0x00010,
          FixK1 = 0x00020,
          FixK2 = 0x00040,
          FixK3 = 0x00080,
          FixK4 = 0x00800,
          FixK5 = 0x01000,
          FixK6 = 0x02000,
          RationalModel = 0x04000,
          ThinPrismModel = 0x08000,
          FixS1S2S3S4 = 0x10000,
          TiltedModel = 0x40000,
          FixTauxTauy = 0x80000,
          // only for stereo
          FixIntrinsic = 0x00100,
          SameFocalLength = 0x00200,
          // for stereo rectification
          ZeroDisparity = 0x00400,
          UseLu = (1 << 17)
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