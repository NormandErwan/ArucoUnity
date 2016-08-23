# ArUco Unity

OpenCV ArUco module for Unity 5.

## Build

- Build OpenCV with the extra module ArUco by following this tutoriel: https://github.com/opencv/opencv_contrib

- Use CMake to config and generate the ArUco Unity library. Here is the command:

```
$ cd <aruco_unity_build_directory>
$ cmake -DOPENCV_DIR=<opencv_install_dir> <aruco_unity_source_directory>
```

- Build and install the ArUco Unity library in the `<aruco_unity_build_directory>` folder. The library will be copied during the installation in the `<aruco_unity_source_directory>/bin` and `<aruco_unity_source_directory>/lib` folders, and in the `<aruco_unity_source_directory>/src/aruco_unity_package/Assets/Plugins` folder.


## Usage

- Retrieve the ArucoUnity Unity package on the Unity Asset Store (available soon). Or you can build the library source code (see Build section), then copy the generated library and the `<aruco_unity_source_directory>/src/aruco_unity_package/Assets/ArucoUnity` folder in your Unity project.

- Open your Unity project and use the prefabs of the ArucoUnity package.
