using ArucoUnity.Plugin;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers.Utility
  {
    public class CvMatUtility : MonoBehaviour
    {
      /// <summary>
      /// Returns the OpenCV type equivalent to a texture format.
      /// </summary>
      /// <param name="textureFormat">The Unity texture format.</param>
      /// <returns>The equivalent OpenCV type.</returns>
      public static Cv.Type ImageType(TextureFormat textureFormat)
      {
        Cv.Type type;
        switch (textureFormat)
        {
          case TextureFormat.RGB24:
            type = Cv.Type.CV_8UC3;
            break;
          case TextureFormat.BGRA32:
          case TextureFormat.ARGB32:
          case TextureFormat.RGBA32:
            type = Cv.Type.CV_8UC4;
            break;
          default:
            throw new ArgumentException("This type of texture is actually not supported: " + textureFormat + ".", "textureFormat");
        }
        return type;
      }
    }
  }
}
