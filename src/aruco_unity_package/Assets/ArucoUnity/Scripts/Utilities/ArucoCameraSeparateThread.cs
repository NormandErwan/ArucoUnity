using ArucoUnity.Cameras;
using ArucoUnity.Plugin;
using System;
using System.Threading;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Utilities
  {
    public class ArucoCameraSeparateThread
    {
      // Constructor

      public ArucoCameraSeparateThread(IArucoCamera arucoCamera, Action imagesUpdated, Action threadWork, Action threadException)
      {
        this.arucoCamera = arucoCamera;
        this.imagesUpdated = imagesUpdated;
        this.threadWork = threadWork;
        this.threadException = threadException;

        Images = new Cv.Mat[arucoCamera.CameraNumber];
        ImagesData = new byte[arucoCamera.CameraNumber][];
        arucoCameraImageCopyData = new byte[arucoCamera.CameraNumber][];
        for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
        {
          ImagesData[cameraId] = new byte[arucoCamera.ImageDataSizes[cameraId]];
          arucoCameraImageCopyData[cameraId] = new byte[arucoCamera.ImageDataSizes[cameraId]];

          Images[cameraId] = new Cv.Mat(arucoCamera.ImageTextures[cameraId].height, arucoCamera.ImageTextures[cameraId].width,
            CvMatExtensions.ImageType(arucoCamera.ImageTextures[cameraId].format));
          Images[cameraId].DataByte = ImagesData[cameraId];
        }
      }

      // Properties

      public bool IsStarted { get; protected set; }
      public Cv.Mat[] Images { get; protected set; }
      public byte[][] ImagesData { get; protected set; }

      // Variables

      protected IArucoCamera arucoCamera;
      protected Action imagesUpdated;
      protected Action threadWork;
      protected Action threadException;

      protected Thread thread;
      protected Mutex mutex = new Mutex();
      protected Exception exception;

      protected byte[][] arucoCameraImageCopyData;
      protected bool arucoCameraImagesUpdated = false;

      // Methods

      public void Start()
      {
        IsStarted = true;
        arucoCamera.ImagesUpdated += ArucoCamera_ImagesUpdated;

        thread = new Thread(() =>
        {
          try
          {
            while (IsStarted)
            {
              mutex.WaitOne();
              if (arucoCameraImagesUpdated)
              {
                arucoCameraImagesUpdated = false;
                for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
                {
                  Array.Copy(arucoCameraImageCopyData[cameraId], ImagesData[cameraId], arucoCamera.ImageDataSizes[cameraId]);
                }
                threadWork();
              }
              mutex.ReleaseMutex();
            }
          }
          catch (Exception e)
          {
            exception = e;
            mutex.ReleaseMutex();
          }
        });
        thread.Start();
      }

      public void Stop()
      {
        arucoCamera.ImagesUpdated -= ArucoCamera_ImagesUpdated;
        IsStarted = false;
      }

      /// <summary>
      /// Swaps the <see cref="ArucoCamera.Images"/> with the copy used by the thread, and re-throw the thread exceptions.
      /// </summary>
      protected void ArucoCamera_ImagesUpdated()
      {
        if (IsStarted)
        {
          Exception e = null;

          mutex.WaitOne();

          if (exception != null)
          {
            e = exception;
            exception = null;
          }
          else if (!arucoCameraImagesUpdated)
          {
            arucoCameraImagesUpdated = true;
            for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
            {
              Array.Copy(arucoCamera.ImageDatas[cameraId], arucoCameraImageCopyData[cameraId], arucoCamera.ImageDataSizes[cameraId]);
              Array.Copy(ImagesData[cameraId], arucoCamera.ImageDatas[cameraId], arucoCamera.ImageDataSizes[cameraId]);
            }
            imagesUpdated();
          }

          mutex.ReleaseMutex();

          if (e != null)
          {
            Stop();
            threadException();
            throw e;
          }
        }
      }
    }
  }

  /// \} aruco_unity_package
}
