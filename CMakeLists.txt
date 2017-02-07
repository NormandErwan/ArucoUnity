cmake_minimum_required(VERSION 3.0)

project(ArucoUnity)
set(ArucoUnity_PROJECT_NAME "${CMAKE_PROJECT_NAME}")

# Version
set(VERSION_MAJOR 0)
set(VERSION_MINOR 1)
set(VERSION_PATCH 0)
set(VERSION "${VERSION_MAJOR}.${VERSION_MINOR}.${VERSION_PATCH}")

# Configuration
option(AUTO_BUILD_OPENCV "Build automatically the latest OpenCV version with the ArUco module (or compile it and provide your version with the option -DOpenCV_DIR)." ON)
option(BUILD_DOCS "Build the docs." OFF)
option(BUILD_TESTS "Build the tests." OFF)

if (CMAKE_INSTALL_PREFIX_INITIALIZED_TO_DEFAULT)
  set(CMAKE_INSTALL_PREFIX "${CMAKE_SOURCE_DIR}" CACHE PATH "Install path prefix" FORCE)
endif()

set(PROJECT_INCLUDE_DIRS "${CMAKE_SOURCE_DIR}/include" CACHE INTERNAL "The ArUco Unity library include directories.")

set(Unity_PROJECT_DIR "${CMAKE_SOURCE_DIR}/src/aruco_unity_package") # The Unity project directory where the compiled plugin will be installed.
set(Unity_PLUGINS_DIR "${Unity_PROJECT_DIR}/Assets/Plugins/x86_64")
set(Unity_PROJECT_SCRIPT_DIRS "${Unity_PROJECT_DIR}/Assets/ArucoUnity/Scripts" CACHE INTERNAL "The ArUco Unity package scripts.")

set(RUNTIME_OUTPUT_DIRECTORY "${CMAKE_INSTALL_PREFIX}/bin") # Destination of the dll for dll platform
set(ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_INSTALL_PREFIX}/lib") # Destination of the corresponding import libraries of the dlls for dll platform
set(LIBRARY_OUTPUT_DIRECTORY "${CMAKE_INSTALL_PREFIX}/lib") # Destination of the librairies for non-dll platform

# Build OpenCV's Aruco module at configuration time unless the user deactivated it, to be able to use find_package command in aruco_unity_lib's CMakeLists
if (AUTO_BUILD_OPENCV)
  set(OpenCVAruco_BUILD_DIR "${CMAKE_BINARY_DIR}/3rdparty/opencv_contrib")
  set(OpenCVAruco_INSTALL_DIR "${OpenCVAruco_BUILD_DIR}/OpenCVAruco-prefix/src/OpenCVAruco-build/install") # Default OpenCV installation directory

  configure_file("${CMAKE_SOURCE_DIR}/3rdparty/opencv_contrib/CMakeLists.txt" "${OpenCVAruco_BUILD_DIR}/CMakeLists.txt" COPYONLY)
  execute_process(COMMAND ${CMAKE_COMMAND} -G "${CMAKE_GENERATOR}" .
                  WORKING_DIRECTORY "${OpenCVAruco_BUILD_DIR}"
  )
  execute_process(COMMAND ${CMAKE_COMMAND} --build . --config Release
                  WORKING_DIRECTORY "${OpenCVAruco_BUILD_DIR}"
  )
endif()

# Build the ArucoUnity lib
add_subdirectory(${CMAKE_SOURCE_DIR}/src/aruco_unity_lib)

# Build the docs
if (BUILD_DOCS)
  add_subdirectory(${CMAKE_SOURCE_DIR}/docs)
endif()

# Build the tests
if (BUILD_TESTS)
  enable_testing()
  add_subdirectory(${CMAKE_SOURCE_DIR}/3rdparty/googletest)
  add_subdirectory(${CMAKE_SOURCE_DIR}/test/aruco_unity_lib)
endif()