cmake_minimum_required(VERSION 3.0)

project(ArucoUnity)

# Version
set(VERSION_MAJOR 2)
set(VERSION_MINOR 0)
set(VERSION_PATCH 0)
set(VERSION "${VERSION_MAJOR}.${VERSION_MINOR}.${VERSION_PATCH}")

# Configuration
option(BUILD_PLUGIN "Build the Aruco Unity plugin." ON)
option(BUILD_DOCS "Build the docs." OFF)
option(BUILD_TESTS "Build the tests." OFF)

if (CMAKE_INSTALL_PREFIX_INITIALIZED_TO_DEFAULT)
  set(CMAKE_INSTALL_PREFIX "${CMAKE_SOURCE_DIR}" CACHE PATH "Install path prefix" FORCE)
endif()

set(ArucoUnity_DIR "${CMAKE_SOURCE_DIR}/src/ArucoUnity") # The Unity project directory where the compiled plugin will be installed.
set(ArucoUnity_PLUGINS_DIR "${ArucoUnity_DIR}/Assets/ArucoUnity/Plugins/x64")

# Build the ArucoUnity plugin
if (BUILD_PLUGIN)
  add_subdirectory("${CMAKE_SOURCE_DIR}/src/aruco_unity_plugin")
endif()

# Build the docs
if (BUILD_DOCS)
  add_subdirectory("${CMAKE_SOURCE_DIR}/docs")
endif()

# Build the tests
if (BUILD_TESTS)
  enable_testing()
  add_subdirectory("${CMAKE_SOURCE_DIR}/3rdparty/googletest")
  add_subdirectory("${CMAKE_SOURCE_DIR}/test/aruco_unity_plugin")
endif()