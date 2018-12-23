using ArucoUnity.Plugin;
using System;
using UnityEngine;

namespace ArucoUnity.Utilities
{
    /// <summary>
    /// Extensions methods for <see cref="ArucoUnity.Plugin.Cv"/>.
    /// </summary>
    public static class CvMatExtensions
    {
        /// <summary>
        /// Gets the camera focal lengths in a camera matrix, expressed in pixels units. Equals to
        /// <c>F = (AtDouble(0, 0), AtDouble(1, 1))</c>
        /// </summary>
        public static Vector2 GetCameraFocalLengths(this Cv.Mat mat)
        {
            return new Vector2((float)mat.AtDouble(0, 0), (float)mat.AtDouble(1, 1));
        }

        /// <summary>
        /// Gets the camera principal point in a camera matrix, expressed in pixels units. Equals to
        /// <c>C = (AtDouble(0, 2), AtDouble(1, 2))</c>
        /// </summary>
        public static Vector2 GetCameraPrincipalPoint(this Cv.Mat mat)
        {
            return new Vector2((float)mat.AtDouble(0, 2), (float)mat.AtDouble(1, 2));
        }

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
                    throw new ArgumentException("This type of texture is actually not supported: " + textureFormat
                        + ".", "textureFormat");
            }
            return type;
        }
    }
}