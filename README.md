# ArucoUnity

Bring augmented reality to Unity by tracking [ArUco markers](https://docs.opencv.org/3.4/d5/dae/tutorial_aruco_detection.html) in real time. Standard mono cameras, such as webcams, but also stereo cameras and fisheye lenses are supported.

It uses the OpenCV's [ArUco](http://docs.opencv.org/3.4/d9/d6a/group__aruco.html) Marker Detection module and the OpenCV's camera calibration modules [calib3d](http://docs.opencv.org/3.4/d9/d0c/group__calib3d.html) and [ccalib](http://docs.opencv.org/3.4/d3/ddc/group__ccalib.html).

![Demo 1](https://normanderwan.github.io/ArucoUnity/images/ar_roll_a_ball.gif)
![Demo 2](https://normanderwan.github.io/ArucoUnity/images/extended_phone_screen.gif)

*Left: AR [Roll a ball](https://unity3d.com/fr/learn/tutorials/s/roll-ball-tutorial). Right: The markers tracking allows to extend the phone's screen.*

## Usage

Get ArucoUnity:

- Download the [latest build release](https://github.com/NormandErwan/ArucoUnity/releases).
- Or see the [Build From Sources](https://normanderwan.github.io/ArucoUnity/manual/build-from-sources.html) documentation page.

Then, import `ArucoUnity.package` in your Unity project.

A typical workflow with ArucoUnity is:

1. [Create Markers](https://normanderwan.github.io/ArucoUnity/manual/create-markers.html), print and place them in the environment.
2. [Calibrate a Camera](https://normanderwan.github.io/ArucoUnity/manual/calibrate-a-camera.html) using a calibration board.
3. [Track Markers](https://normanderwan.github.io/ArucoUnity/manual/track-markers.html).

See the documentation online for details: [https://normanderwan.github.io/ArucoUnity/](https://normanderwan.github.io/ArucoUnity).

## Contributions

- For any question or comment, please [open a new issue](https://github.com/NormandErwan/ArucoUnity/issues/new).
- If you'd like to contribute, please [fork the repository](https://github.com/NormandErwan/ArucoUnity/fork) and use a feature branch. Pull requests are warmly welcome.

## Support

The first version of this project has been developed as part of the master thesis of [Erwan Normand](https://linkedin.com/in/normanderwan) and was supported by the [ÉTS - École de Technologie Supérieure](https://www.etsmtl.ca).

I'm developing the second version on my own. If this project helped you, please consider buying me a coffee in return :)

[![Buy Me A Coffee](https://www.buymeacoffee.com/assets/img/custom_images/white_img.png)](https://www.buymeacoffee.com/h48VU3fny)

## Licenses

See the [LICENSE](https://github.com/NormandErwan/ArucoUnity/blob/master/LICENSE) file for license rights and limitations (3-clause BSD license).

See [https://github.com/NormandErwan/ArucoUnityPlugin/tree/master/3rdparty](https://github.com/NormandErwan/ArucoUnityPlugin/tree/master/3rdparty) for the OpenCV license. ArucoUnity uses the following modules: [aruco](http://docs.opencv.org/3.4/d9/d6a/group__aruco.html), [cablid3d](http://docs.opencv.org/3.4/d9/d0c/group__calib3d.html), [ccalib](http://docs.opencv.org/3.4/d3/ddc/group__ccalib.html)

## References

The OpenCV's Aruco module implements this paper:

> Garrido-Jurado, S., Muñoz-Salinas, R., Madrid-Cuevas, F. J., & Marín-Jiménez, M. J. (2014). Automatic generation and detection of highly reliable fiducial markers under occlusion. Pattern Recognition, 47(6), 2280-2292.

The fisheye calibration operated with the OpenCV's ccalib module is based on these papers:

> Mei, C., & Rives, P. (2007, April). Single view point omnidirectional camera calibration from planar grids. In Robotics and Automation, 2007 IEEE International Conference on (pp. 3945-3950). IEEE.

> Li, B., Heng, L., Koser, K., & Pollefeys, M. (2013, November). A multiple-camera system calibration toolbox using a feature descriptor-based calibration pattern. In Intelligent Robots and Systems (IROS), 2013 IEEE/RSJ International Conference on (pp. 1301-1307). IEEE.
