using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    namespace cv
    {
      public class Mat : HandleCppPtr
      {
        // Constructor & Destructor
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

        // Member Functions
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
        static extern void au_cv_Mat_create(System.IntPtr mat, int rows, int cols, int type, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern uint au_cv_Mat_total(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern int au_cv_Mat_type(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern uint au_cv_Mat_elemSize(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern uint au_cv_Mat_elemSize1(System.IntPtr mat);

        // Variables
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

        public Mat() : base(au_cv_Mat_new1())
        {
        }

        public Mat(int rows, int cols, TYPE type) : base(au_cv_Mat_new2(rows, cols, (int)type))
        {
        }

        public Mat(Size size, TYPE type) : base(au_cv_Mat_new3(size.cppPtr, (int)type))
        {
        }

        public Mat(int rows, int cols, TYPE type, byte[] data) : base(au_cv_Mat_new8_uchar(rows, cols, (int)type, data))
        {
        }

        public Mat(int rows, int cols, TYPE type, double[] data) : base(au_cv_Mat_new8_double(rows, cols, (int)type, data))
        {
        }

        internal Mat(System.IntPtr matPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(matPtr, deleteResponsibility)
        {
        }

        public int Channels()
        {
          return au_cv_Mat_channels(cppPtr);
        }

        public void Create(int rows, int cols, TYPE type)
        {
          Exception exception = new Exception();
          au_cv_Mat_create(cppPtr, rows, cols, (int)type, exception.cppPtr);
          exception.Check();
        }

        protected override void DeleteCvPtr()
        {
          au_cv_Mat_delete(cppPtr);
        }

        public int AtInt(int i0, int i1)
        {
          Exception exception = new Exception();
          int value = au_cv_Mat_at_int_get(cppPtr, i0, i1, exception.cppPtr);
          exception.Check();
          return value;
        }

        public void AtInt(int i0, int i1, int value)
        {
          Exception exception = new Exception();
          au_cv_Mat_at_int_set(cppPtr, i0, i1, value, exception.cppPtr);
          exception.Check();
        }

        public double AtDouble(int i0, int i1)
        {
          Exception exception = new Exception();
          double value = au_cv_Mat_at_double_get(cppPtr, i0, i1, exception.cppPtr);
          exception.Check();
          return value;
        }

        public void AtDouble(int i0, int i1, double value)
        {
          Exception exception = new Exception();
          au_cv_Mat_at_double_set(cppPtr, i0, i1, value, exception.cppPtr);
          exception.Check();
        }

        public uint ElemSize()
        {
          return au_cv_Mat_elemSize(cppPtr);
        }

        public uint ElemSize1()
        {
          return au_cv_Mat_elemSize1(cppPtr);
        }

        public uint Total()
        {
          return au_cv_Mat_total(cppPtr);
        }

        public TYPE Type()
        {
          return (TYPE)au_cv_Mat_type(cppPtr);
        }

        public int cols
        {
          get { return au_cv_Mat_getCols(cppPtr); }
        }

        public System.IntPtr dataIntPtr
        {
          get { return au_cv_Mat_getData_void(cppPtr); }
          set { au_cv_Mat_setData_void(cppPtr, value); }
        }

        public byte[] dataByte
        {
          get { return au_cv_Mat_getData_uchar(cppPtr); }
          set { au_cv_Mat_setData_uchar(cppPtr, value); }
        }

        public int rows
        {
          get { return au_cv_Mat_getRows(cppPtr); }
        }

        public Size size
        {
          get { return new Size(au_cv_Mat_getSize(cppPtr)); }
        }
      }
    }
  }

  /// \} aruco_unity_package
}