# Aruco Unity

Bring augmented reality to your Unity project by tracking position and orientation of [ArUco markers](https://docs.opencv.org/master/d5/dae/tutorial_aruco_detection.html) in real time. Standard mono cameras, such as webcams, but also stereo cameras and fisheye lenses are supported.

It uses under the hood the OpenCV's [ArUco](http://docs.opencv.org/master/d9/d6a/group__aruco.html) Marker Detection module and the OpenCV's camera calibration modules [calib3d](http://docs.opencv.org/master/d9/d0c/group__calib3d.html) and [ccalib](http://docs.opencv.org/master/d3/ddc/group__ccalib.html).

![Demo 1](https://raw.githubusercontent.com/NormandErwan/ArucoUnity/master/docs/images/ar_roll_a_ball.gif)
![Demo 2](https://raw.githubusercontent.com/NormandErwan/ArucoUnity/master/docs/images/extended_phone_screen.gif)

*Left: AR [Roll a ball](https://unity3d.com/fr/learn/tutorials/s/roll-ball-tutorial). Right: The markers tracking allow to extend the phone's screen.*

## Installation

Download the [latest build release](https://github.com/enormand/ArucoUnity/releases), or see the Build section. Import the package in your Unity project.

## Usage

A typical workflow with Aruco Unity is:

1. Create and print some markers: see the [Create Markers](https://github.com/NormandErwan/ArucoUnity/wiki/1.-Create-Markers) wiki page.
2. Calibrate your camera using a calibration board: see the [Calibrate a Camera](https://github.com/NormandErwan/ArucoUnity/wiki/2.-Calibrate-a-Camera) wiki page.
3. Track your markers: see the [Track Markers](https://github.com/NormandErwan/ArucoUnity/wiki/3.-Track-Markers) wiki page.

## Build

Read the [Build From Sources](https://github.com/NormandErwan/ArucoUnity/wiki/Build-From-Sources) wiki page.

## Documentation

The documentation is available online:

- The library: [https://normanderwan.github.io/ArucoUnity/group__aruco__unity__lib.html](https://normanderwan.github.io/ArucoUnity/group__aruco__unity__lib.html)
- The Unity package: [https://normanderwan.github.io/ArucoUnity/group__aruco__unity__package.html](https://normanderwan.github.io/ArucoUnity/group__aruco__unity__package.html)

## Contributions

If you have any question or comment, please [open a new issue](https://github.com/NormandErwan/ArucoUnity/issues/new).

If you'd like to contribute, please [fork the repository](https://github.com/NormandErwan/ArucoUnity/fork) and use a feature branch. Pull requests are warmly welcome.

## Support

This first version of this project has been developed as part of the master thesis of [Erwan Normand](https://ca.linkedin.com/in/normanderwan) was supported by the [ÉTS - École de Technologie Supérieure](https://www.etsmtl.ca/). I'm developing the second version on my own.

[![Buy Me A Coffee](https://www.buymeacoffee.com/assets/img/custom_images/white_img.png)](https://www.buymeacoffee.com/h48VU3fny)

## Licenses

See the [LICENSE](LICENSE) file for license rights and limitations (3-clause BSD license).

See the [3rdparty folder](3rdparty/) for licenses of the third-party dependencies. Aruco Unity makes use of the
following projects:

- [OpenCV](http://opencv.org/) with the following modules: [ArUco](https://github.com/opencv/opencv_contrib/tree/master/modules/aruco), [cablid3d](http://docs.opencv.org/master/d9/d0c/group__calib3d.html), [ccalib](http://docs.opencv.org/master/d3/ddc/group__ccalib.html)
- [Google Test](https://github.com/google/googletest)

## References

The OpenCV's Aruco module implements this paper:

> Garrido-Jurado, S., Muñoz-Salinas, R., Madrid-Cuevas, F. J., & Marín-Jiménez, M. J. (2014). Automatic generation and detection of highly reliable fiducial markers under occlusion. Pattern Recognition, 47(6), 2280-2292.

The fisheye calibration operated with the OpenCV's ccalib module is based on these papers:

> Mei, C., & Rives, P. (2007, April). Single view point omnidirectional camera calibration from planar grids. In Robotics and Automation, 2007 IEEE International Conference on (pp. 3945-3950). IEEE.

> Li, B., Heng, L., Koser, K., & Pollefeys, M. (2013, November). A multiple-camera system calibration toolbox using a feature descriptor-based calibration pattern. In Intelligent Robots and Systems (IROS), 2013 IEEE/RSJ International Conference on (pp. 1301-1307). IEEE.