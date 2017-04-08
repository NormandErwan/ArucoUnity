using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Std
    {
      public class VectorMat : Utility.HandleCppPtr
      {
        // Native functions

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorMat_new();

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorMat_delete(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern System.IntPtr au_std_vectorMat_at(System.IntPtr vector, uint pos, System.IntPtr exception);

        [DllImport("ArucoUnity")]
        static extern unsafe System.IntPtr* au_std_vectorMat_data(System.IntPtr vector);

        [DllImport("ArucoUnity")]
        static extern void au_std_vectorMat_push_back(System.IntPtr vector, System.IntPtr value);

        [DllImport("ArucoUnity")]
        static extern uint au_std_vectorMat_size(System.IntPtr vector);

        // Constructors & destructor

        public VectorMat() : base(au_std_vectorMat_new())
        {
        }

        public VectorMat(System.IntPtr vectorMatPtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True)
          : base(vectorMatPtr, deleteResponsibility)
        {
        }

        protected override void DeleteCvPtr()
        {
          au_std_vectorMat_delete(cppPtr);
        }

        // Methods

        public Cv.Core.Mat At(uint pos)
        {
          Cv.Core.Exception exception = new Cv.Core.Exception();
          Cv.Core.Mat element = new Cv.Core.Mat(au_std_vectorMat_at(cppPtr, pos, exception.cppPtr), DeleteResponsibility.False);
          exception.Check();
          return element;
        }

        public unsafe Cv.Core.Mat[] Data()
        {
          System.IntPtr* dataPtr = au_std_vectorMat_data(cppPtr);
          uint size = Size();

          Cv.Core.Mat[] data = new Cv.Core.Mat[size];
          for (int i = 0; i < size; i++)
          {
            data[i] = new Cv.Core.Mat(dataPtr[i], DeleteResponsibility.False);
          }

          return data;
        }

        public void PushBack(Cv.Core.Mat value)
        {
          au_std_vectorMat_push_back(cppPtr, value.cppPtr);
        }

        public uint Size()
        {
          return au_std_vectorMat_size(cppPtr);
        }
      }
    }
  }

  /// \} aruco_unity_package
}