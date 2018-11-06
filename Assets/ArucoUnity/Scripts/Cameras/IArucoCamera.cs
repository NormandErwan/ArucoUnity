using ArucoUnity.Plugin;
using ArucoUnity.Utilities;
using System;
using UnityEngine;

namespace ArucoUnity.Cameras
{
    /// <summary>
    /// Captures images of a camera.
    /// </summary>
    public interface IArucoCamera : IController
    {
        // Events

        /// <summary>
        /// Called when the <see cref="Images"/> have been updated.
        /// </summary>
        event Action ImagesUpdated;

        /// <summary>
        /// Callback to undistort and rectify the images in parameters.
        /// </summary>
        event Action<Cv.Mat[], byte[][]> UndistortRectifyImages;

        // Properties

        /// <summary>
        /// Gets the number of cameras in the system.
        /// </summary>
        int CameraNumber { get; }

        /// <summary>
        /// Gets the name of the camera system used.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the the current images manipulated by Unity. There are <see cref="CameraNumber"/> images: one for each camera.
        /// </summary>
        Texture2D[] Textures { get; }

        /// <summary>
        /// Gets or sets the current images manipulated by OpenCV. There are <see cref="CameraNumber"/> images: one for each camera.
        /// </summary>
        Cv.Mat[] Images { get; }

        /// <summary>
        /// Gets the <see cref="Images"/> content.
        /// </summary>
        byte[][] ImageDatas { get; }

        /// <summary>
        /// Gets the size of each <see cref="ImageDatas"/>.
        /// </summary>
        int[] ImageDataSizes { get; }

        /// <summary>
        /// Gets the ratios of each <see cref="Images"/>.
        /// </summary>
        float[] ImageRatios { get; }
    }
}