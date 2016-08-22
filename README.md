# Aruco Unity

OpenCV ArUco module for Unity 5.

# Build

1. Build opencv with the extra module aruco by following this tutoriel: https://github.com/opencv/opencv_contrib

2. Use CMake to config and generate the library. Here is the command:

```
$ cd <aruco_unity_build_directory>
$ cmake -DOPENCV_DIR=<opencv_install_dir> <aruco_unity_source_directory>
```

3. Build and install the library in the `<aruco_unity_build_directory>` folder. The library will be copied in the `<aruco_unity_source_directory>/bin` and `<aruco_unity_source_directory>/lib` folders, and in the `<aruco_unity_source_directory>/src/aruco_unity_package` folder.


# Usage

1. Retrieve the ArucoUnity Unity package on the Unity Asset Store (available soon). Or you can build the library source code (see Build section), then copy the generated library and the `<aruco_unity_source_directory>/src/aruco_unity_package/Assets/ArucoUnity` folder in your Unity project.

2. Open your Unity project and use the prefabs of the ArucoUnity package.
