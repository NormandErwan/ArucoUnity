# Build From Sources

## Build OpenCV

We need a build of OpenCV (minimum version: 3.3.0) with the calib3d module and the aruco and ccalib *contrib* modules. These two contrib modules are not included in the official OpenCV releases.

A CMake project is already configured ready to be build. Run the following commands:

```bash
cd <aruco_unity_directory>/ArucoUnityPlugin/
mkdir -p build/opencv/ && cd build/opencv/
cmake -DCMAKE_INSTALL_PREFIX=install/ -G <generator-name> ../3rdparty/opencv_contrib/
cmake --build . --config Release
```

If you're not familiar with CMake generators, see this [list of generators](https://cmake.org/cmake/help/latest/manual/cmake-generators.7.html) to set the `<generator-name>` variable above. On Windows, I use "Visual Studio 15 2017 Win64" with Visual Studio 2017 installed on my machine.

Alternatively, you can follow the [opencv_contrib instructions](https://github.com/opencv/opencv_contrib).

## Build the Aruco Unity plugin

First, make sure Unity is closed during the installation, unless the plugin installation will fail. Configure the building solution of the ArUco Unity plugin, compile and install it by running the following commands:

```bash
cd <aruco_unity_directory>/ArucoUnityPlugin/build/
cmake -DCMAKE_INSTALL_PREFIX=.. -DOpenCV_DIR=../../build/opencv/install/ -G <generator-name> ..
cmake --build . --config Release --target INSTALL
```

The installation will copy the plugin into the `bin/` and `lib/` folders. It will also copy the plugin and the OpenCV libraries to the `<aruco_unity_directory>/Assets/Plugins/` folder.

## Export Aruco Unity

Open the `<aruco_unity_directory>` folder in Unity. No errors should be diplayed in the Console panel.

A new entry named "Aruco Unity" is in the menu bar. Open it and select "Export package".

![Export Aruco Unity package](../images/export_package.jpg)

## Documentation

Install the documentation generator [DocFX](http://dotnet.github.io/docfx/tutorial/docfx_getting_started.html).

Run the following commands:

```bash
cd <aruco_unity_directory>
docfx && docfx serve docs/
```

Open the generated website on [http://localhost:8080](http://localhost:8080).