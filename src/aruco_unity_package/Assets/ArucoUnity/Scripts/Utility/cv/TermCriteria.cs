using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    namespace cv
    {
      public class TermCriteria : HandleCppPtr
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

        public TermCriteria(int type, int maxCount, double epsilon) : base(au_cv_TermCriteria_new2(type, maxCount, epsilon))
        {
        }

        protected override void DeleteCvPtr()
        {
          au_cv_TermCriteria_delete(cvPtr);
        }

        public double epsilon
        {
          get { return au_cv_TermCriteria_getEpsilon(cvPtr); }
          set { au_cv_TermCriteria_setEpsilon(cvPtr, value); }
        }

        public int maxCount
        {
          get { return au_cv_TermCriteria_getMaxCount(cvPtr); }
          set { au_cv_TermCriteria_setMaxCount(cvPtr, value); }
        }

        public int type
        {
          get { return au_cv_TermCriteria_getType(cvPtr); }
          set { au_cv_TermCriteria_setType(cvPtr, value); }
        }
      }
    }
  }

  /// \} aruco_unity_package
}