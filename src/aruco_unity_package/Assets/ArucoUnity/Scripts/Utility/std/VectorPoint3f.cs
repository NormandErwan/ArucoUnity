using System.Runtime.InteropServices;
using ArucoUnity.Utility.cv;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    namespace std
    {
      public class VectorPoint3f : HandleCvPtr
      {
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_vectorPoint3f_new();

        [DllImport("ArucoUnity")]
        static extern void au_vectorPoint3f_delete(System.IntPtr vector);

        // Functions
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_vectorPoint3f_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe System.IntPtr* au_vectorPoint3f_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_vectorPoint3f_push_back(System.IntPtr vector, System.IntPtr value);

        [DllImport("ArucoUnity")]
        static extern uint au_vectorPoint3f_size(System.IntPtr vector);

        public VectorPoint3f() : base(au_vectorPoint3f_new())
        {
        }

        public VectorPoint3f(System.IntPtr vectorPoint3fPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(vectorPoint3fPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCvPtr()
        {
          au_vectorPoint3f_delete(cvPtr);
        }

        public Point3f At(uint pos)
        {
          Exception exception = new Exception();
          Point3f element = new Point3f(au_vectorPoint3f_at(cvPtr, pos, exception.cvPtr), DeleteResponsibility.False);
          exception.Check();
          return element;
        }

        public unsafe Point3f[] Data()
        {
          System.IntPtr* dataPtr = au_vectorPoint3f_data(cvPtr);
          uint size = Size();

          Point3f[] data = new Point3f[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = new Point3f(dataPtr[i], DeleteResponsibility.False);
          }

          return data;
        }

        public void PushBack(Point3f value)
        {
          au_vectorPoint3f_push_back(cvPtr, value.cvPtr);
        }

        public uint Size()
        {
          return au_vectorPoint3f_size(cvPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}