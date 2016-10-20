using System.Runtime.InteropServices;

namespace ArucoUnity
{
  namespace Utility
  {
    public class Size : HandleCvPtr
    {
      // Constructor & Destructor
      [DllImport("ArucoUnity")]
      static extern System.IntPtr au_Size_new();

      [DllImport("ArucoUnity")]
      static extern void au_Size_delete(System.IntPtr size);

      // Member functions
      [DllImport("ArucoUnity")]
      static extern int au_Size_area(System.IntPtr size);

      // Variables
      [DllImport("ArucoUnity")]
      static extern int au_Size_getHeight(System.IntPtr size);

      [DllImport("ArucoUnity")]
      static extern void au_Size_setHeight(System.IntPtr size, int height);

      [DllImport("ArucoUnity")]
      static extern int au_Size_getWidth(System.IntPtr size);

      [DllImport("ArucoUnity")]
      static extern void au_Size_setWidth(System.IntPtr size, int width);

      public Size() : base(au_Size_new())
      {
      }

      public Size(System.IntPtr sizePtr, DeleteResponsibility deleteResponsibility = DeleteResponsibility.True) 
        : base(sizePtr, deleteResponsibility)
      {
      }

      protected override void DeleteCvPtr()
      {
        au_Size_delete(cvPtr);
      }

      public int Area()
      {
        return au_Size_area(cvPtr);
      }

      public int height
      {
        get { return au_Size_getHeight(cvPtr); }
        set { au_Size_setHeight(cvPtr, value); }
      }

      public int width
      {
        get { return au_Size_getWidth(cvPtr); }
        set { au_Size_setWidth(cvPtr, value); }
      }
    }
  }
}