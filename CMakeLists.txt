cmake_minimum_required(VERSION 2.8)

project(aruco_unity)


# Version
set(${PROJECT_NAME}_VERSION_MAJOR 0)
set(${PROJECT_NAME}_VERSION_MINOR 1)
set(${PROJECT_NAME}_VERSION_PATCH 0)


# Dependencies
find_package(OpenCV REQUIRED)

set(aruco_DIR ${CMAKE_INSTALL_PREFIX}/../aruco/lib/cmake/ CACHE PATH "The directory containing a CMake configuration file for Aruco.")
find_package(aruco REQUIRED)


# Inputs
file(GLOB headers include/*)
file(GLOB sources src/*)


# Outputs
set(RUNTIME_OUTPUT_DIRECTORY bin) # dll destination
set(LIBRARY_OUTPUT_DIRECTORY lib)
set(ARCHIVE_OUTPUT_DIRECTORY lib)

add_library(${PROJECT_NAME} SHARED ${headers} ${sources})
target_link_libraries(${PROJECT_NAME} ${aruco_LIBS})


# Installation (to the Unity folder)
set(UNITY_PROJECT ${PROJECT_SOURCE_DIR}/CopyToUnityProject CACHE PATH "The Unity project directory where the compiled plugin will be installed.")
install(TARGETS ${PROJECT_NAME}
        ARCHIVE DESTINATION ${PROJECT_SOURCE_DIR}/lib
        LIBRARY DESTINATION ${PROJECT_SOURCE_DIR}/lib
        RUNTIME DESTINATION ${PROJECT_SOURCE_DIR}/bin
)
install(TARGETS ${PROJECT_NAME}
        RUNTIME DESTINATION ${UNITY_PROJECT}/Assets/Plugins/x86_64
)
