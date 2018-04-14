using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Cv
    {
      public class Mat : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Mat_new1();

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Mat_new2(int rows, int cols, int type);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Mat_new3(System.IntPtr size, int type);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Mat_new8_uchar(int rows, int cols, int type, byte[] data);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Mat_new8_double(int rows, int cols, int type, double[] data);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Mat_delete(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern int au_cv_Mat_at_int_get(System.IntPtr mat, int i0, int i1, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Mat_at_int_set(System.IntPtr mat, int i0, int i1, int value, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern double au_cv_Mat_at_double_get(System.IntPtr mat, int i0, int i1, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Mat_at_double_set(System.IntPtr mat, int i0, int i1, double value, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern int au_cv_Mat_channels(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Mat_clone(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Mat_create(System.IntPtr mat, int rows, int cols, int type, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern uint au_cv_Mat_total(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern int au_cv_Mat_type(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern uint au_cv_Mat_elemSize(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern uint au_cv_Mat_elemSize1(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern int au_cv_Mat_getCols(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Mat_getData_void(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Mat_setData_void(System.IntPtr mat, System.IntPtr data);

        [DllImport("ArucoUnity")]
        static extern byte[] au_cv_Mat_getData_uchar(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Mat_setData_uchar(System.IntPtr mat, byte[] data);

        [DllImport("ArucoUnity")]
        static extern int au_cv_Mat_getRows(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Mat_getSize(System.IntPtr mat);

        // Constructors & destructor

        public Mat() : base(au_cv_Mat_new1())
        {
        }

        public Mat(int rows, int cols, Type type) : base(au_cv_Mat_new2(rows, cols, (int)type))
        {
        }

        public Mat(Size size, Type type) : base(au_cv_Mat_new3(size.CppPtr, (int)type))
        {
        }

        public Mat(int rows, int cols, Type type, byte[] data) : base(au_cv_Mat_new8_uchar(rows, cols, (int)type, data))
        {
        }

        public Mat(int rows, int cols, Type type, double[] data) : base(au_cv_Mat_new8_double(rows, cols, (int)type, data))
        {
        }

        internal Mat(System.IntPtr matPtr, Utility.DeleteResponsibility deleteResponsibility = Utility.DeleteResponsibility.True)
          : base(matPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCppPtr()
        {
          au_cv_Mat_delete(CppPtr);
        }

        // Properties

        public int Cols
        {
          get { return au_cv_Mat_getCols(CppPtr); }
        }

        public System.IntPtr DataIntPtr
        {
          get { return au_cv_Mat_getData_void(CppPtr); }
          set { au_cv_Mat_setData_void(CppPtr, value); }
        }

        public byte[] DataByte
        {
          get { return au_cv_Mat_getData_uchar(CppPtr); }
          set { au_cv_Mat_setData_uchar(CppPtr, value); }
        }

        public int Rows
        {
          get { return au_cv_Mat_getRows(CppPtr); }
        }

        public Size Size
        {
          get { return new Size(au_cv_Mat_getSize(CppPtr)); }
        }

        // Methods

        public int Channels()
        {
          return au_cv_Mat_channels(CppPtr);
        }

        public Mat Clone()
        {
          return new Mat(au_cv_Mat_clone(CppPtr));
        }

        public void Create(int rows, int cols, Type type)
        {
          Exception exception = new Exception();
          au_cv_Mat_create(CppPtr, rows, cols, (int)type, exception.CppPtr);
          exception.Check();
        }

        public int AtInt(int i0, int i1)
        {
          Exception exception = new Exception();
          int value = au_cv_Mat_at_int_get(CppPtr, i0, i1, exception.CppPtr);
          exception.Check();
          return value;
        }

        public void AtInt(int i0, int i1, int value)
        {
          Exception exception = new Exception();
          au_cv_Mat_at_int_set(CppPtr, i0, i1, value, exception.CppPtr);
          exception.Check();
        }

        public double AtDouble(int i0, int i1)
        {
          Exception exception = new Exception();
          double value = au_cv_Mat_at_double_get(CppPtr, i0, i1, exception.CppPtr);
          exception.Check();
          return value;
        }

        public void AtDouble(int i0, int i1, double value)
        {
          Exception exception = new Exception();
          au_cv_Mat_at_double_set(CppPtr, i0, i1, value, exception.CppPtr);
          exception.Check();
        }

        public uint ElemSize()
        {
          return au_cv_Mat_elemSize(CppPtr);
        }

        public uint ElemSize1()
        {
          return au_cv_Mat_elemSize1(CppPtr);
        }

        public uint Total()
        {
          return au_cv_Mat_total(CppPtr);
        }

        public Type Type()
        {
          return (Type)au_cv_Mat_type(CppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}