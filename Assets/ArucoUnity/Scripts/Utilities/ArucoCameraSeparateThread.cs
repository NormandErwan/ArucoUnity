using ArucoUnity.Cameras;
using ArucoUnity.Plugin;
using System;
using System.Threading;

namespace ArucoUnity.Utilities
{
    public class ArucoCameraSeparateThread
    {
        private const int BuffersCount = 3;

        public ArucoCameraSeparateThread(IArucoCamera arucoCamera, Action<Cv.Mat[]> threadWork)
        {
            this.arucoCamera = arucoCamera;
            this.threadWork = threadWork;
            CopyBackImages = false;

            for (int bufferId = 0; bufferId < BuffersCount; bufferId++)
            {
                imageBuffers[bufferId] = new Cv.Mat[arucoCamera.CameraNumber];
                imageDataBuffers[bufferId] = new byte[arucoCamera.CameraNumber][];

                for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
                {
                    imageBuffers[bufferId][cameraId] = new Cv.Mat(arucoCamera.Textures[cameraId].height, arucoCamera.Textures[cameraId].width,
                      CvMatExtensions.ImageType(arucoCamera.Textures[cameraId].format));

                    imageDataBuffers[bufferId][cameraId] = new byte[arucoCamera.ImageDataSizes[cameraId]];
                    imageBuffers[bufferId][cameraId].DataByte = imageDataBuffers[bufferId][cameraId];
                }
            }
        }

        public bool CopyBackImages { get; set; }
        public bool IsStarted { get; protected set; }
        public bool ImagesUpdated { get; protected set; }

        protected IArucoCamera arucoCamera;
        protected Action<Cv.Mat[]> threadWork;

        protected uint currentBuffer = 0;
        protected Cv.Mat[][] imageBuffers = new Cv.Mat[BuffersCount][];
        protected byte[][][] imageDataBuffers = new byte[BuffersCount][][];

        protected Thread thread;
        protected Mutex mutex = new Mutex();
        protected Exception threadException, exception;
        protected bool threadUpdated, imagesUpdated;

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
                        {
                            imagesUpdated = ImagesUpdated;
                        }
                        mutex.ReleaseMutex();

                        if (imagesUpdated)
                        {
                            threadWork(imageBuffers[currentBuffer]);

                            mutex.WaitOne();
                            {
                                currentBuffer = NextBuffer();
                                ImagesUpdated = false;
                            }
                            mutex.ReleaseMutex();
                        }
                    }
                }
                catch (Exception e)
                {
                    threadException = e;
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
        public void Update(byte[][] cameraImageDatas)
        {
            if (IsStarted)
            {
                mutex.WaitOne();
                {
                    exception = threadException;
                    threadUpdated = !ImagesUpdated;
                }
                mutex.ReleaseMutex();

                if (exception != null)
                {
                    Stop();
                    throw exception;
                }
                else
                {
                    if (threadUpdated)
                    {
                        for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
                        {
                            Array.Copy(cameraImageDatas[cameraId], imageDataBuffers[NextBuffer()][cameraId], arucoCamera.ImageDataSizes[cameraId]);
                        }
                    }

                    if (CopyBackImages)
                    {
                        for (int cameraId = 0; cameraId < arucoCamera.CameraNumber; cameraId++)
                        {
                            Array.Copy(imageDataBuffers[PreviousBuffer()][cameraId], cameraImageDatas[cameraId], arucoCamera.ImageDataSizes[cameraId]);
                        }
                    }

                    if (threadUpdated)
                    {
                        mutex.WaitOne();
                        {
                            ImagesUpdated = true;
                        }
                        mutex.ReleaseMutex();
                    }
                }
            }
        }

        /// <summary>
        /// Returns the index of the next buffer.
        /// </summary>
        protected uint NextBuffer()
        {
            return (currentBuffer + 1) % BuffersCount;
        }

        /// <summary>
        /// Returns the index of the previous buffer.
        /// </summary>
        protected uint PreviousBuffer()
        {
            return (currentBuffer + BuffersCount - 1) % BuffersCount;
        }
    }
}