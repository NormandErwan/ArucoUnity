using UnityEngine;
using System.Runtime.InteropServices;

class Mat {
  [DllImport("aruco_unity_lib")]
  static extern System.IntPtr createMat(int i);

  [DllImport("aruco_unity_lib")]
  static extern void destroyMat(System.IntPtr mat);

  [DllImport("aruco_unity_lib")]
  static extern int displayMat(System.IntPtr mat);

  HandleRef handle;

  public Mat(int i) {
    System.IntPtr mat = createMat(i);
    handle = new HandleRef(this, mat);
  }

  ~Mat() {
    destroyMat(handle.Handle);
  }

  public int display() {
    return displayMat(handle.Handle);
  }
}

public class ArucoManager : MonoBehaviour {

  Mat mat, mat1;

  void Start() {
    mat = new Mat(0);
    mat1 = new Mat(1);
  }

  void Update() {
    if (Input.GetButton("Fire1")) {
      print(mat.display());
      print(mat1.display());
    }
  }
}
