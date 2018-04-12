# ArUco Unity

Easily track in real time position and orientation of ArUco markers, bringing augmented reality to your Unity project. Standard mono cameras such as webcams, stereo cameras and fisheye lenses are supported.

It uses underhood the OpenCV's [ArUco](http://docs.opencv.org/master/d9/d6a/group__aruco.html) Marker Detection module and the OpenCV's camera calibration modules [calib3d](http://docs.opencv.org/master/d9/d0c/group__calib3d.html) and [ccalib](http://docs.opencv.org/master/d3/ddc/group__ccalib.html).

This first version of this project has been developed as part of the master thesis of [Erwan Normand](https://ca.linkedin.com/in/normanderwan)  was supported by the [ÉTS - École de Technologie Supérieure](https://www.etsmtl.ca/).

## Installation

Download the [latest build release](https://github.com/enormand/aruco-unity/releases), or see the Build section. Import the package in your Unity project.

## Usage

Please read the wiki:

1. [Marker Creation](https://github.com/NormandErwan/aruco-unity/wiki/Marker-Creation)
1. [Camera Calibration](https://github.com/NormandErwan/aruco-unity/wiki/2.-Camera-Calibration)
1. [Marker Tracking](https://github.com/NormandErwan/aruco-unity/wiki/3.-Marker-Tracking)

## Build

### OpenCV dependency

You will need to provide a build of OpenCV (minimum version: 3.3.0) with the ArUco and ccalib contrib modules. As these two modules are not included in the official OpenCV releases, we need to build it on our own. You can build
the provided CMake project by running the following commands:

```bash
cd <aruco_unity_directory>
mkdir -p build/opencv/ && cd build/opencv/
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

The installation will copy the library into the `<aruco_unity_directory>/bin`, `<aruco_unity_directory>/lib` and `<aruco_unity_directory>/src/aruco_unity_package/Assets/Plugins` folders. It will also copy the OpenCV libraries. Make sure Unity is closed during the installation, unless the build will fail.

### Export the ArucoUnity package

Open `<aruco_unity_directory>/src/aruco_unity_package` folder in Unity. There should be no errors on the Console panel.

A new entry named "ArUco Unity" is in the menu bar. Open it and select "Export package".

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

## Support

<a href="https://www.buymeacoffee.com/h48VU3fny" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png" alt="Buy Me A Coffee" style="height: auto !important;width: auto !important;" ></a>

## Licenses

See the [LICENSE](LICENSE) file for license rights and limitations (3-clause BSD license).

See the [3rdparty folder](3rdparty/) for licenses of the third-party dependencies. ArUco Unity makes use of the
following projects:

- [OpenCV](http://opencv.org/) with the following modules:
  - [ArUco](https://github.com/opencv/opencv_contrib/tree/master/modules/aruco)
  - [cablid3d](http://docs.opencv.org/master/d9/d0c/group__calib3d.html)
  - [ccalib](http://docs.opencv.org/master/d3/ddc/group__ccalib.html)
- [Google Test](https://github.com/google/googletest)

The OpenCV's Aruco module implements this paper:

> Garrido-Jurado, S., Muñoz-Salinas, R., Madrid-Cuevas, F. J., & Marín-Jiménez, M. J. (2014). Automatic generation and detection of highly reliable fiducial markers under occlusion. Pattern Recognition, 47(6), 2280-2292.