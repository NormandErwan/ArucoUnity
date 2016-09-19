cmake_minimum_required(VERSION 2.8)

project(ArucoUnity)


# Version
set(VERSION_MAJOR 0)
set(VERSION_MINOR 1)
set(VERSION_PATCH 0)


# Variable definitions
if (CMAKE_INSTALL_PREFIX_INITIALIZED_TO_DEFAULT)
  set(CMAKE_INSTALL_PREFIX ${CMAKE_SOURCE_DIR} CACHE PATH "Install path prefix" FORCE)
endif()

set(UNITY_PROJECT ${PROJECT_SOURCE_DIR}/src/aruco_unity_package) # The Unity project directory where the compiled plugin will be installed.
set(UNITY_PLUGINS_DIR ${UNITY_PROJECT}/Assets/Plugins/x86_64)

set(RUNTIME_OUTPUT_DIRECTORY ${CMAKE_INSTALL_PREFIX}/bin) # Destination of the dll for dll platform
set(ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_INSTALL_PREFIX}/lib) # Destination of the corresponding import libraries of the dlls for dll platform
set(LIBRARY_OUTPUT_DIRECTORY ${CMAKE_INSTALL_PREFIX}/lib) # Destination of the librairies for non-dll platform

option(BUILD_TESTS "Build the unitary tests." ON)


# Add subdirectories
add_subdirectory(${PROJECT_SOURCE_DIR}/src/aruco_unity_lib)
if (BUILD_TESTS)
  add_subdirectory(${PROJECT_SOURCE_DIR}/test/aruco_unity_lib)
endif()