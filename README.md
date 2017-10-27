# ArUco Unity

OpenCV's [ArUco](http://docs.opencv.org/master/d9/d6a/group__aruco.html) Marker Detection module adapted for Unity 5.
The OpenCV's calibration modules [cablid3d](http://docs.opencv.org/master/d9/d0c/group__calib3d.html) and
[ccalib](http://docs.opencv.org/master/d3/ddc/group__ccalib.html) are also included.

This project has been developed as part of the master thesis of [Erwan Normand](https://ca.linkedin.com/in/normanderwan)
 and was supported by the [ÉTS - École de Technologie Supérieure](https://www.etsmtl.ca/).

## Usage

Download the [latest build release](https://github.com/enormand/aruco-unity/releases), or see the Build section. Import the package in your Unity project.

### Marker tracking

- Open the `Assets/ArucoUnity/Scenes/Tracking` scene.
- Configure the `Undistortion Type` and the `Camera Id` properties of the `ArucoCameraWebcam` in the scene. If you have calibrated your camera, indicate the calibration file path (e.g. `ArucoUnity/Calibrations/<calibration_file>.xml`).
- Make sure the `ArucoCamera` property of the `ArucoTracker` object is linked to the `ArucoCameraWebcam` object configured earlier.
- Create an empty object and add it an `ArucoMarker` script and configure it according to the printed marker you want to track. Add 3D content as a child of this object.
- Add this object to the `Aruco Objects` list property of the `ArucoTracker` object.
- Run the scene. The `ArucoTracker` will place any detected marker in the `Aruco Objects` list relative to the camera of the `ArucoCamera` property.

### Camera calibration

- Open the `Assets/ArucoUnity/Scenes/Calibration`.
- Configure the `Undistortion Type` and the `Camera Id` properties of the `ArucoCameraWebcam` in the scene. The `CameraParametersFilePath` is optional: it will be filled automatically.
- Activate the `ArucoCalibrator` corresponding to the selected `Undistortion Type`.
- Configure the `Calibration Board` used by the prefab according to the board you're going to use to calibrate your camera. Create a board with the Creation scene if you don't have any and print the generated image.
- Run the scene and interact with the UI to calibrate your camera. The calibration file will be automatically created and saved when the 'Calibrate' button is triggered.

### Marker creation

Open and run the the `Assets/ArucoUnity/Scenes/Creation` scene. It demonstrates the creation of two different markers, one grid
board, one ChArUco board and one diamond marker.

## Build

### Dependencies

You will need to provide a build of OpenCV (minimum version: 3.3.0) with the ArUco module as dependency. You can build
the provided CMake project by running the following commands:

```bash
$ cd <opencv_build_directory>
$ cmake -DCMAKE_INSTALL_PREFIX=<opencv_install_directory> -G <generator-name> <aruco_unity_directory>/3rdparty/opencv_contrib/
$ cmake --build . --config Release
```

Alternatively, you can follow these instructions: [https://github.com/opencv/opencv_contrib](https://github.com/opencv/opencv_contrib).

### Build the library

Configure the building solution of the ArUco Unity library, compile and install it by running the following commands:

```bash
$ cd <aruco_unity_build_directory>
$ cmake -DCMAKE_INSTALL_PREFIX=<aruco_unity_install_directory> -DOpenCV_DIR=<opencv_install_directory> -G <generator-name> <aruco_unity_directory>
$ cmake --build . --config Release --target INSTALL
```

The installation will copy the library into the `<aruco_unity_directory>/bin`, `<aruco_unity_directory>/lib`
and `<aruco_unity_directory>/src/aruco_unity_package/Assets/Plugins` folders. Make sure Unity is closed during the
installation, unless the build will fail.

### Export the Unity package

Open `<aruco_unity_directory>/src/aruco_unity_package` folder in Unity. There should be no errors on the Console panel.

On the Project panel, select the ArucoUnity folder. Make a right clik on it, select "Export package", then the "Export"
button on the opened window.

## Tests

If you want to build and execute the tests, run the following commands:

```bash
$ cd <aruco_unity_build_directory>
$ cmake -DBUILD_TESTS=ON <aruco_unity_directory>
$ ctest
```

The Google Test dependency will be automatically built. All tests should pass.

## Documentation

The documentation of available online:

- The library: [https://enormand.github.io/aruco-unity/group__aruco__unity__lib.html](https://enormand.github.io/aruco-unity/group__aruco__unity__lib.html)
- The Unity package: [https://enormand.github.io/aruco-unity/group__aruco__unity__package.html](https://enormand.github.io/aruco-unity/group__aruco__unity__package.html)

## Licenses

See the [LICENSE](LICENSE) file for license rights and limitations (3-clause BSD license).

See the [3rdparty folder](3rdparty/) for licenses of the third-party dependencies. ArUco Unity makes use of the
following projects:

- [OpenCV](http://opencv.org/) and the [ArUco](https://github.com/opencv/opencv_contrib/tree/master/modules/aruco), [cablid3d](http://docs.opencv.org/master/d9/d0c/group__calib3d.html) and [ccalib](http://docs.opencv.org/master/d3/ddc/group__ccalib.html) extra modules.
- [Google Test](https://github.com/google/googletest)
