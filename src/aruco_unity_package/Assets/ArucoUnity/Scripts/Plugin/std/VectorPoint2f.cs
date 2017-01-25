using System.Runtime.InteropServices;
using ArucoUnity.Plugin.cv;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    namespace std
    {
      public class VectorPoint2f : HandleCppPtr
      {
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorPoint2f_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorPoint2f_delete(System.IntPtr vector);

        // Functions
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorPoint2f_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe System.IntPtr* au_std_vectorPoint2f_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorPoint2f_push_back(System.IntPtr vector, System.IntPtr value);

        [DllImport("ArucoUnity")]
        static extern uint au_std_vectorPoint2f_size(System.IntPtr vector);

        public VectorPoint2f() : base(au_std_vectorPoint2f_new())
        {
        }

        public VectorPoint2f(System.IntPtr vectorPoint2fPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(vectorPoint2fPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCvPtr()
        {
          au_std_vectorPoint2f_delete(cppPtr);
        }

        public Point2f At(uint pos)
        {
          Exception exception = new Exception();
          Point2f element = new Point2f(au_std_vectorPoint2f_at(cppPtr, pos, exception.cppPtr), DeleteResponsibility.False);
          exception.Check();
          return element;
        }

        public unsafe Point2f[] Data()
        {
          System.IntPtr* dataPtr = au_std_vectorPoint2f_data(cppPtr);
          uint size = Size();

          Point2f[] data = new Point2f[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = new Point2f(dataPtr[i], DeleteResponsibility.False);
          }

          return data;
        }

        public void PushBack(Point2f value)
        {
          au_std_vectorPoint2f_push_back(cppPtr, value.cppPtr);
        }

        public uint Size()
        {
          return au_std_vectorPoint2f_size(cppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}