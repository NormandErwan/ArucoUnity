cmake_minimum_required(VERSION 2.8)

project(ArucoUnity)


# Version
set(VERSION_MAJOR 0)
set(VERSION_MINOR 1)
set(VERSION_PATCH 0)


# Variable definitions
set(UNITY_PROJECT ${PROJECT_SOURCE_DIR}/src/aruco_unity_package) # The Unity project directory where the compiled plugin will be installed.
set(UNITY_PLUGINS_DIR ${UNITY_PROJECT}/Assets/Plugins/x86_64)


# Add subdirectories
add_subdirectory(${PROJECT_SOURCE_DIR}/src/aruco_unity_lib)
#add_subdirectory(${PROJECT_SOURCE_DIR}/test/aruco_unity_lib)