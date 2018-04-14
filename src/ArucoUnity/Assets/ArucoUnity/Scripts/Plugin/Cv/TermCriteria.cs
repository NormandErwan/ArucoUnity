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
        // Enums

        public enum Type
        {
          Count = 0,
          MaxIter = Count,
          Eps = 2
        }

        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_TermCriteria_new1();

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_TermCriteria_new2(int type, int maxCount, double epsilon);

        [DllImport("ArucoUnity")]
        static extern void au_cv_TermCriteria_delete(System.IntPtr termCriteria);

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

        // Constructors & destructor

        public TermCriteria() : base(au_cv_TermCriteria_new1())
        {
        }

        public TermCriteria(Type type, int maxCount, double epsilon) : base(au_cv_TermCriteria_new2((int)type, maxCount, epsilon))
        {
        }

        protected override void DeleteCppPtr()
        {
          au_cv_TermCriteria_delete(CppPtr);
        }

        // Properties

        public double Epsilon
        {
          get { return au_cv_TermCriteria_getEpsilon(CppPtr); }
          set { au_cv_TermCriteria_setEpsilon(CppPtr, value); }
        }

        public int MaxCount
        {
          get { return au_cv_TermCriteria_getMaxCount(CppPtr); }
          set { au_cv_TermCriteria_setMaxCount(CppPtr, value); }
        }

        public int TypeValue
        {
          get { return au_cv_TermCriteria_getType(CppPtr); }
          set { au_cv_TermCriteria_setType(CppPtr, value); }
        }
      }
    }
  }

  /// \} aruco_unity_package
}