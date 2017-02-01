using ArucoUnity.Plugin;
using ArucoUnity.Plugin.cv;
using UnityEngine;
using System.IO;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utility
  {
    public abstract class ArucoCreator : MonoBehaviour
    {
      // Properties

      /// <summary>
      /// The dictionnary to use.
      /// </summary>
      public Dictionary Dictionary { get; set; }

      /// <summary>
      /// The generated image of the marker.
      /// </summary>
      public Mat Image { get; protected set; }

      /// <summary>
      /// The generated texture of the marker.
      /// </summary>
      public Texture2D ImageTexture { get; protected set; }

      // Methods

      /// <summary>
      /// Create the marker image and the <see cref="ImageTexture"/> of the marker.
      /// </summary>
      public abstract void Create();

      /// <summary>
      /// Draw the <see cref="ImageTexture"/> of the marker on the plane.
      /// </summary>
      /// <param name="plane">The object where the <see cref="ImageTexture"/> is drawn.</param>
      public virtual void Draw(GameObject plane)
      {
        int imageDataSize = (int)(Image.ElemSize() * Image.Total());
        ImageTexture.LoadRawTextureData(Image.dataIntPtr, imageDataSize);
        ImageTexture.Apply();

        plane.GetComponent<Renderer>().material.mainTexture = ImageTexture;
      }

      /// <summary>
      /// Save the <see cref="ImageTexture"/> on a image file.
      /// </summary>
      /// <param name="outputImage">The image file path, relative to the project file path.</param>
      public virtual void Save(string outputImage)
      {
        string imageFilePath = Path.Combine(Application.dataPath, outputImage); // TODO: use Application.persistentDataPath for iOS
        File.WriteAllBytes(imageFilePath, ImageTexture.EncodeToPNG());
      }
    }
  }

  /// \} aruco_unity_package
}