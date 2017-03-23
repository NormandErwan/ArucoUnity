using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    namespace Std
    {
      public class VectorPoint3f : HandleCppPtr
      {
        // Constructor & Destructor
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorPoint3f_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorPoint3f_delete(System.IntPtr vector);

        // Functions
        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorPoint3f_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe System.IntPtr* au_std_vectorPoint3f_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorPoint3f_push_back(System.IntPtr vector, System.IntPtr value);

        [DllImport("ArucoUnity")]
        static extern uint au_std_vectorPoint3f_size(System.IntPtr vector);

        public VectorPoint3f() : base(au_std_vectorPoint3f_new())
        {
        }

        public VectorPoint3f(System.IntPtr vectorPoint3fPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(vectorPoint3fPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCvPtr()
        {
          au_std_vectorPoint3f_delete(cppPtr);
        }

        public Cv.Point3f At(uint pos)
        {
          Cv.Exception exception = new Cv.Exception();
          Cv.Point3f element = new Cv.Point3f(au_std_vectorPoint3f_at(cppPtr, pos, exception.cppPtr), DeleteResponsibility.False);
          exception.Check();
          return element;
        }

        public unsafe Cv.Point3f[] Data()
        {
          System.IntPtr* dataPtr = au_std_vectorPoint3f_data(cppPtr);
          uint size = Size();

          Cv.Point3f[] data = new Cv.Point3f[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = new Cv.Point3f(dataPtr[i], DeleteResponsibility.False);
          }

          return data;
        }

        public void PushBack(Cv.Point3f value)
        {
          au_std_vectorPoint3f_push_back(cppPtr, value.cppPtr);
        }

        public uint Size()
        {
          return au_std_vectorPoint3f_size(cppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}