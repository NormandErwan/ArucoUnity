using UnityEngine;
using System.Runtime.InteropServices;

public class ArucoManager : MonoBehaviour {

  [DllImport("aruco_unity_lib")]
  private static extern int opencvAbs(int n);

  [DllImport("aruco_unity_lib")]
  private static extern void createMarkers();

  void Start () {
    createMarkers();
    Debug.Log(opencvAbs(-2));
  }
}
