# ArUco Unity

OpenCV's [ArUco](http://docs.opencv.org/3.3.1/d9/d6a/group__aruco.html) Marker Detection module adapted for Unity 5.
The OpenCV's calibration modules [cablid3d](http://docs.opencv.org/3.3.1/d9/d0c/group__calib3d.html) and [ccalib](http://docs.opencv.org/3.3.1/d3/ddc/group__ccalib.html) are also included.

This project has been developed as part of the master thesis of [Erwan Normand](https://ca.linkedin.com/in/normanderwan)  was supported by the [ÉTS - École de Technologie Supérieure](https://www.etsmtl.ca/).

## Installation

Download the [latest build release](https://github.com/enormand/aruco-unity/releases), or see the Build section. Import the package in your Unity project.

## Usage

If you're not familiar with ArUco markers, read first this [OpenCV tutorial](https://docs.opencv.org/3.3.1/d5/dae/tutorial_aruco_detection.html).

### 1. Marker creation

Before track the markers, you need to create, print and place them in the environment.

Open and run the the `Assets/ArucoUnity/Scenes/Creation` scene for a working example. It demonstrates the creation of six different markers, one grid
board, one ChArUco board and two diamond markers.

### 2. Camera calibration

Marker tracking requires to calibrate the camera. If you're not familiar with camera calibration, read this [OpenCV tutorial](https://docs.opencv.org/3.3.1/d4/d94/tutorial_camera_calibration.html).

Use the Calibration scene:

- Open the `Assets/ArucoUnity/Scenes/Calibration`.
- Configure the following properties of the `ArucoCameraWebcam` GameObject:
  - The `Undistortion Type`: "Pinhole" by default.
  - The `Camera Id`: "0" by default if you have only one camera.
  - The `CameraParametersFilePath` is optional: it will be filled automatically.
- Activate the "ArucoCalibrator" GameObject corresponding to the selected `Undistortion Type`: "ArucoCalibratorPinhole" by default.
- Configure the `Calibration Board` used by the activated "ArucoCalibrator" according to the physical board you're going to use to calibrate your camera. Create a board with the Creation scene if you don't have any and print the generated image.
- Run the scene and interact with the UI to calibrate your camera. The calibration file will be automatically created and saved when the "Calibrate" button is triggered.

### 3. Marker tracking

The Tracking scene is configured to track the objects generated in the Creation scene.

- Open the `Assets/ArucoUnity/Scenes/Tracking` scene.
- Configure the `Undistortion Type` and the `Camera Id` properties of the `ArucoCameraWebcam` in the scene. If you have calibrated your camera, indicate the calibration file path (e.g. `ArucoUnity/Calibrations/<calibration_file>.xml`).
- Make sure the `ArucoCamera` property of the `ArucoTracker` object is linked to the `ArucoCameraWebcam` object configured earlier.
- Create an empty object and add it an `ArucoMarker` script and configure it according to the printed marker you want to track.
- Add 3D content as a child of the ArucoMarker object.
- Add optionaly a `ArucoObjectDisplayer` script to the ArucoMarker object if you want to visualize the ArUco object.
- Add this object to the `Aruco Objects` list property of the `ArucoTracker` object.
- Run the scene. The `ArucoTracker` will place (position, rotation, scale) any detected marker in the `Aruco Objects` list relative to the camera property.

## Build

### OpenCV dependency

You will need to provide a build of OpenCV (minimum version: 3.3.0) with the ArUco and ccalib contrib modules. As these two modules are not included in the official OpenCV releases, we need to build it on our own. You can build
the provided CMake project by running the following commands:

```bash
cd <aruco_unity_directory>
mkdir build/ && mkdir build/opencv/ && cd build/opencv/
cmake -DCMAKE_INSTALL_PREFIX=install/ -G <generator-name> ../../3rdparty/opencv_contrib/
cmake --build . --config Release
```

If you're not familiar with CMake generators, see this [list of generators](https://cmake.org/cmake/help/latest/manual/cmake-generators.7.html) to set the generator-name variable above.
On Windows, it can be "Visual Studio 15 2017 Win64".

Alternatively, you can follow the [opencv_contrib instructions](https://github.com/opencv/opencv_contrib).

### Build the ArUco Unity library

Configure the building solution of the ArUco Unity library, compile and install it by running the following commands:

```bash
cd <aruco_unity_directory>/build/
cmake -DCMAKE_INSTALL_PREFIX=.. -DOpenCV_DIR=../../build/opencv/install/ -G <generator-name> ..
cmake --build . --config Release --target INSTALL
```

The installation will copy the library into the `<aruco_unity_directory>/bin`, `<aruco_unity_directory>/lib` and `<aruco_unity_directory>/src/aruco_unity_package/Assets/Plugins` folders. It will also copy the OpenCV libraries. Make sure Unity is closed during the
installation, unless the build will fail.

### Export the ArucoUnity package

Open `<aruco_unity_directory>/src/aruco_unity_package` folder in Unity. There should be no errors on the Console panel.

On the Project panel, select the ArucoUnity folder. Make a right clik on it, select "Export package", then the "Export"
button on the opened window.

## Tests

If you want to build and execute the tests, run the following commands:

```bash
cd <aruco_unity_directory>/build/
cmake -DBUILD_TESTS=ON ..
ctest
```

The Google Test dependency will be automatically built. All tests should pass.

## Documentation

The documentation is available online:

- The library: [https://enormand.github.io/aruco-unity/group__aruco__unity__lib.html](https://enormand.github.io/aruco-unity/group__aruco__unity__lib.html)
- The Unity package: [https://enormand.github.io/aruco-unity/group__aruco__unity__package.html](https://enormand.github.io/aruco-unity/group__aruco__unity__package.html)

## Contributions

If you'd like to contribute, please fork the repository and use a feature branch. Pull requests are warmly welcome.

Some features or corrections to do:

- Add an extended tracking
- Add the autoscale feature to ArucoUnity.Controllers.ObjectTrackers.ArucoDiamondTracker (see this [ArUco sample](https://github.com/opencv/opencv_contrib/blob/master/modules/aruco/samples/detect_diamonds.cpp#L203))
- Write the tests for the library with Google Test
- Write the tests for the Unity package
- Replace the library with one of the following open-source project (should be compatible with Unity and at least OpenCV 3.3):
  - [EmguCV](http://www.emgu.com)
  - [OpenCvSharp](https://github.com/shimat/opencvsharp)
  - [OpenCV.NET](https://bitbucket.org/horizongir/opencv.net)
- Write a wiki with more detailed information and gifs about building, architecture and how using the package
- Fix the crash that occur when calling ArucoUnity.Plugin.Cv.au_cv_Exception_delete(System.IntPtr)
- Build the library for Windows x86, Linux, Mac, Android, iOS, UWP

## Licenses

See the [LICENSE](LICENSE) file for license rights and limitations (3-clause BSD license).

See the [3rdparty folder](3rdparty/) for licenses of the third-party dependencies. ArUco Unity makes use of the
following projects:

- [OpenCV](http://opencv.org/) with the following modules:
  - [ArUco](https://github.com/opencv/opencv_contrib/tree/master/modules/aruco)
  - [cablid3d](http://docs.opencv.org/3.3.1/d9/d0c/group__calib3d.html)
  - [ccalib](http://docs.opencv.org/3.3.1/d3/ddc/group__ccalib.html)
- [Google Test](https://github.com/google/googletest)
