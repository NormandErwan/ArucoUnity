using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    namespace cv
    {
      public class Mat : HandleCppPtr
      {
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Mat_new1();

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Mat_new2_uchar(int rows, int cols, int type, byte[] data);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Mat_new2_double(int rows, int cols, int type, double[] data);

        [DllImport("ArucoUnity")]
        static extern void au_cv_Mat_create(System.IntPtr mat, int rows, int cols, int type);

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
        static extern void au_cv_Mat_create(System.IntPtr mat, int rows, int cols, int type, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern uint au_cv_Mat_total(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern int au_cv_Mat_type(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern uint au_cv_Mat_elemSize(System.IntPtr mat);

        // Variables
        [DllImport("ArucoUnity")]
        static extern int au_cv_Mat_getCols(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Mat_getData(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern int au_cv_Mat_getRows(System.IntPtr mat);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_cv_Mat_getSize(System.IntPtr mat);

        public Mat() : base(au_cv_Mat_new1())
        {
        }

        public Mat(int rows, int cols, TYPE type, byte[] data) : base(au_cv_Mat_new2_uchar(rows, cols, (int)type, data))
        {
        }

        public Mat(int rows, int cols, TYPE type, double[] data) : base(au_cv_Mat_new2_double(rows, cols, (int)type, data))
        {
        }

        internal Mat(System.IntPtr matPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(matPtr, deleteResponsibility)
        {
        }

        public void Create(int rows, int cols, TYPE type)
        {
          Exception exception = new Exception();
          au_cv_Mat_create(cvPtr, rows, cols, (int)type, exception.cvPtr);
          exception.Check();
        }

        protected override void DeleteCvPtr()
        {
          au_cv_Mat_delete(cvPtr);
        }

        public int AtInt(int i0, int i1)
        {
          Exception exception = new Exception();
          int value = au_cv_Mat_at_int_get(cvPtr, i0, i1, exception.cvPtr);
          exception.Check();
          return value;
        }

        public void AtInt(int i0, int i1, int value)
        {
          Exception exception = new Exception();
          au_cv_Mat_at_int_set(cvPtr, i0, i1, value, exception.cvPtr);
          exception.Check();
        }

        public double AtDouble(int i0, int i1)
        {
          Exception exception = new Exception();
          double value = au_cv_Mat_at_double_get(cvPtr, i0, i1, exception.cvPtr);
          exception.Check();
          return value;
        }

        public void AtDouble(int i0, int i1, double value)
        {
          Exception exception = new Exception();
          au_cv_Mat_at_double_set(cvPtr, i0, i1, value, exception.cvPtr);
          exception.Check();
        }

        public uint ElemSize()
        {
          return au_cv_Mat_elemSize(cvPtr);
        }

        public uint Total()
        {
          return au_cv_Mat_total(cvPtr);
        }

        public TYPE Type()
        {
          return (TYPE)au_cv_Mat_type(cvPtr);
        }

        public int cols
        {
          get { return au_cv_Mat_getCols(cvPtr); }
        }

        public System.IntPtr data
        {
          get { return au_cv_Mat_getData(cvPtr); }
        }

        public int rows
        {
          get { return au_cv_Mat_getRows(cvPtr); }
        }

        public Size size
        {
          get { return new Size(au_cv_Mat_getSize(cvPtr)); }
        }
      }
    }
  }

  /// \} aruco_unity_package
}