using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Cv
    {
      public class TermCriteria : Utility.HandleCppPtr
      {
        public enum Type
        {
          COUNT = 0,
          MAX_ITER = COUNT,
          EPS = 2
        }

        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_TermCriteria_new1();

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_TermCriteria_new2(int type, int maxCount, double epsilon);

        [DllImport("ArucoUnity")]
        static extern void au_cv_TermCriteria_delete(System.IntPtr termCriteria);

        // Variables
        [DllImport("ArucoUnity")]
        static extern double au_cv_TermCriteria_getEpsilon(System.IntPtr termCriteria);

        [DllImport("ArucoUnity")]
        static extern void au_cv_TermCriteria_setEpsilon(System.IntPtr termCriteria, double epsilon);

        [DllImport("ArucoUnity")]
        static extern int au_cv_TermCriteria_getMaxCount(System.IntPtr termCriteria);

        [DllImport("ArucoUnity")]
        static extern void au_cv_TermCriteria_setMaxCount(System.IntPtr termCriteria, int maxCount);

        [DllImport("ArucoUnity")]
        static extern int au_cv_TermCriteria_getType(System.IntPtr termCriteria);

        [DllImport("ArucoUnity")]
        static extern void au_cv_TermCriteria_setType(System.IntPtr termCriteria, int type);

        public TermCriteria() : base(au_cv_TermCriteria_new1())
        {
        }

        public TermCriteria(Type type, int maxCount, double epsilon) : base(au_cv_TermCriteria_new2((int)type, maxCount, epsilon))
        {
        }

        protected override void DeleteCvPtr()
        {
          au_cv_TermCriteria_delete(cppPtr);
        }

        public double epsilon
        {
          get { return au_cv_TermCriteria_getEpsilon(cppPtr); }
          set { au_cv_TermCriteria_setEpsilon(cppPtr, value); }
        }

        public int maxCount
        {
          get { return au_cv_TermCriteria_getMaxCount(cppPtr); }
          set { au_cv_TermCriteria_setMaxCount(cppPtr, value); }
        }

        public int type
        {
          get { return au_cv_TermCriteria_getType(cppPtr); }
          set { au_cv_TermCriteria_setType(cppPtr, value); }
        }
      }
    }
  }

  /// \} aruco_unity_package
}