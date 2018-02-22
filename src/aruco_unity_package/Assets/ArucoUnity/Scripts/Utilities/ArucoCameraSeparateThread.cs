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
      // Constants

      private const int buffersCount = 2;

      // Constructor

      public ArucoCameraSeparateThread(IArucoCamera arucoCamera, Action<Cv.Mat[]> threadWork)
      {
        this.arucoCamera = arucoCamera;
        this.threadWork = threadWork;

        for (int bufferId = 0; bufferId < buffersCount; bufferId++)
        {
          imageBuffers[bufferId] = new Cv.Mat[arucoCamera.CameraNumber];
          imageDataBuffers[bufferId] = new byte[arucoCamera.CameraNumber][];

          for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
          {
            imageBuffers[bufferId][cameraId] = new Cv.Mat(arucoCamera.ImageTextures[cameraId].height, arucoCamera.ImageTextures[cameraId].width,
              CvMatExtensions.ImageType(arucoCamera.ImageTextures[cameraId].format));

            imageDataBuffers[bufferId][cameraId] = new byte[arucoCamera.ImageDataSizes[cameraId]];
            imageBuffers[bufferId][cameraId].DataByte = imageDataBuffers[bufferId][cameraId];
          }
        }
      }

      // Properties

      public bool IsStarted { get; protected set; }
      public bool ImagesUpdated { get; protected set; }

      // Variables

      protected IArucoCamera arucoCamera;
      protected Action<Cv.Mat[]> threadWork;

      protected uint currentBuffer = 0;
      protected Cv.Mat[][] imageBuffers = new Cv.Mat[buffersCount][];
      protected byte[][][] imageDataBuffers = new byte[buffersCount][][];

      protected Thread thread;
      protected Mutex mutex = new Mutex();
      protected Exception exception;

      // Methods

      public void Start()
      {
        IsStarted = true;
        ImagesUpdated = false;

        thread = new Thread(() =>
        {
          try
          {
            while (IsStarted)
            {
              mutex.WaitOne();
              if (ImagesUpdated)
              {
                ImagesUpdated = false;
                threadWork(imageBuffers[currentBuffer]);
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
        IsStarted = false;
      }

      /// <summary>
      /// Swaps the images with the copy used by the thread, and re-throw the thread exceptions.
      /// </summary>
      public void Update(byte[][] imageDatas)
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
          else if (!ImagesUpdated)
          {
            ImagesUpdated = true;
            for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
            {
              Array.Copy(imageDatas[cameraId], imageDataBuffers[NextBuffer()][cameraId], arucoCamera.ImageDataSizes[cameraId]);
              Array.Copy(imageDataBuffers[currentBuffer][cameraId], imageDatas[cameraId], arucoCamera.ImageDataSizes[cameraId]);
            }
            currentBuffer = NextBuffer();
          }

          mutex.ReleaseMutex();

          if (e != null)
          {
            Stop();
            throw e;
          }
        }
      }

      /// <summary>
      /// Returns the index of the next buffer.
      /// </summary>
      protected uint NextBuffer()
      {
        return (currentBuffer + 1) % buffersCount;
      }
    }
  }

  /// \} aruco_unity_package
}
