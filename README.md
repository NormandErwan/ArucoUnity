# ArucoUnity

Bring augmented reality to Unity by tracking [ArUco markers](https://docs.opencv.org/3.4/d5/dae/tutorial_aruco_detection.html)
in real time. Standard mono cameras, such as webcams, but also stereo cameras and fisheye lenses are supported.

![Demo 1](https://normanderwan.github.io/ArucoUnity/images/ar_roll_a_ball.gif)
![Demo 2](https://normanderwan.github.io/ArucoUnity/images/extended_phone_screen.gif)

*Left: AR [Roll a ball](https://unity3d.com/fr/learn/tutorials/s/roll-ball-tutorial). Right: The markers tracking*
*allows to extend the phone's screen.*

## Install

Get ArucoUnity:

1. Download the [latest build release](https://github.com/NormandErwan/ArucoUnity/releases). Or see the
[Build From Sources](https://normanderwan.github.io/ArucoUnity/manual/build-from-sources.html) documentation page.
2. Import `ArucoUnity.package` in your Unity project.

Get ArucoUnityPlugin (C bindings to OpenCV):

1. Download the [latest build release](https://github.com/NormandErwan/ArucoUnityPlugin/releases) corresponding to
your platform. Windows and Linux x64 only are supported (see issue [#6](https://github.com/NormandErwan/ArucoUnity/issues/6)).
2. Copy the `Assets/` folder to your Unity project.

## Usage

1. [Create Markers](https://normanderwan.github.io/ArucoUnity/manual/create-markers.html), print and place them in the
environment.
2. [Calibrate a Camera](https://normanderwan.github.io/ArucoUnity/manual/calibrate-a-camera.html) using a calibration
board.
3. [Track Markers](https://normanderwan.github.io/ArucoUnity/manual/track-markers.html).

See the documentation online for details: [https://normanderwan.github.io/ArucoUnity/](https://normanderwan.github.io/ArucoUnity).

## Contributing

For any question or comment, please [open a new issue](https://github.com/NormandErwan/ArucoUnity/issues/new).

If you'd like to contribute, please [fork the repository](https://github.com/NormandErwan/ArucoUnity/fork) and use a
feature branch. Pull requests are warmly welcome.

## Support

The first version of this project has been developed as part of the master thesis of
[Erwan Normand](https://linkedin.com/in/normanderwan) and was supported by the
[ÉTS - École de Technologie Supérieure](https://www.etsmtl.ca).

I'm developing the second version on my own. If this project helped you, please consider buying me a coffee in return :)

[![Buy Me A Coffee](https://www.buymeacoffee.com/assets/img/custom_images/white_img.png)](https://www.buymeacoffee.com/h48VU3fny)

## Licenses

See the [LICENSE](https://github.com/NormandErwan/ArucoUnity/blob/master/LICENSE) file for license rights and
limitations (3-clause BSD license).

See [https://github.com/NormandErwan/ArucoUnityPlugin/tree/master/3rdparty](https://github.com/NormandErwan/ArucoUnityPlugin/tree/master/3rdparty)
for the OpenCV license. ArucoUnity uses the following OpenCV modules:

- [ArUco marker detection (aruco)](http://docs.opencv.org/3.4/d9/d6a/group__aruco.html)
- [Camera Calibration and 3D Reconstruction (calib3d)](http://docs.opencv.org/3.4/d9/d0c/group__calib3d.html)
- [Custom Calibration Pattern for 3D reconstruction (ccalib)](http://docs.opencv.org/3.4/d3/ddc/group__ccalib.html)
