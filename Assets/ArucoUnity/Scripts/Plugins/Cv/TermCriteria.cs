using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Cv
  {
    public class TermCriteria : HandleCppPtr
    {
      // Enums

      public enum Type
      {
        Count = 0,
        MaxIter = Count,
        Eps = 2
      }

      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_cv_TermCriteria_new1();

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_cv_TermCriteria_new2(int type, int maxCount, double epsilon);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_TermCriteria_delete(IntPtr termCriteria);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_cv_TermCriteria_getEpsilon(IntPtr termCriteria);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_TermCriteria_setEpsilon(IntPtr termCriteria, double epsilon);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_cv_TermCriteria_getMaxCount(IntPtr termCriteria);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_TermCriteria_setMaxCount(IntPtr termCriteria, int maxCount);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_cv_TermCriteria_getType(IntPtr termCriteria);

      [DllImport("ArucoUnityPlugin")]
      static extern void au_cv_TermCriteria_setType(IntPtr termCriteria, int type);

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