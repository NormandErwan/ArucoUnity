cmake_minimum_required(VERSION 2.8)

project(aruco_unity)


# Version
set(${PROJECT_NAME}_VERSION_MAJOR 0)
set(${PROJECT_NAME}_VERSION_MINOR 1)
set(${PROJECT_NAME}_VERSION_PATCH 0)


# Variable definitions
set(UNITY_PROJECT ${PROJECT_SOURCE_DIR}/src/unity_package) # The Unity project directory where the compiled plugin will be installed.
set(UNITY_PLUGINS_DIR ${UNITY_PROJECT}/Assets/Plugins/x86_64)


# Add subdirectories
add_subdirectory(${PROJECT_SOURCE_DIR}/src/aruco_unity_lib)