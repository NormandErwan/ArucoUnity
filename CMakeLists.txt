cmake_minimum_required(VERSION 3.0)

project(ArucoUnity)

# Version
set(VERSION_MAJOR 0)
set(VERSION_MINOR 1)
set(VERSION_PATCH 0)
set(VERSION "${VERSION_MAJOR}.${VERSION_MINOR}.${VERSION_PATCH}")

# Configuration
option(BUILD_DOCS "Build the docs." OFF)
option(BUILD_TESTS "Build the tests." OFF)

if (CMAKE_INSTALL_PREFIX_INITIALIZED_TO_DEFAULT)
  set(CMAKE_INSTALL_PREFIX ${CMAKE_SOURCE_DIR} CACHE PATH "Install path prefix" FORCE)
endif()

set(PROJECT_INCLUDE_DIRS ${CMAKE_SOURCE_DIR}/include CACHE INTERNAL "The ArUco Unity include directories.")

set(UNITY_PROJECT ${CMAKE_SOURCE_DIR}/src/aruco_unity_package) # The Unity project directory where the compiled plugin will be installed.
set(UNITY_PLUGINS_DIR ${UNITY_PROJECT}/Assets/Plugins/x86_64)

set(RUNTIME_OUTPUT_DIRECTORY ${CMAKE_INSTALL_PREFIX}/bin) # Destination of the dll for dll platform
set(ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_INSTALL_PREFIX}/lib) # Destination of the corresponding import libraries of the dlls for dll platform
set(LIBRARY_OUTPUT_DIRECTORY ${CMAKE_INSTALL_PREFIX}/lib) # Destination of the librairies for non-dll platform

# Build OpenCV's Aruco module
add_subdirectory(${CMAKE_SOURCE_DIR}/3rdparty/opencv_contrib)

# Build the ArucoUnity lib
add_subdirectory(${CMAKE_SOURCE_DIR}/src/aruco_unity_lib)

# Build docs
if (BUILD_DOCS)
  add_subdirectory(${CMAKE_SOURCE_DIR}/docs)
endif()

# Build tests
if (BUILD_TESTS)
  enable_testing()
  add_subdirectory(${CMAKE_SOURCE_DIR}/3rdparty/googletest)
  add_subdirectory(${CMAKE_SOURCE_DIR}/test/aruco_unity_lib)
endif()