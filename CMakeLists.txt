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
install(TARGETS ${PROJECT_NAME}
        ARCHIVE DESTINATION ${PROJECT_SOURCE_DIR}/lib
        LIBRARY DESTINATION ${PROJECT_SOURCE_DIR}/lib
        RUNTIME DESTINATION ${PROJECT_SOURCE_DIR}/bin
)
if (WIN32)
  set(UNITY_PROJECT "" CACHE PATH "The Unity project directory where the compiled plugin will be installed.")
  if (EXISTS ${UNITY_PROJECT})
    set(UNITY_PLUGINS ${UNITY_PROJECT}/Assets/Plugins/x86_64)
    install(TARGETS ${PROJECT_NAME}
            RUNTIME DESTINATION ${UNITY_PLUGINS}
    )
    install(DIRECTORY ${aruco_LIB_DIR}/../bin/ ${OpenCV_LIB_DIR}/../bin/ 
            DESTINATION ${UNITY_PLUGINS}
            FILES_MATCHING PATTERN "*.dll"
    )
  endif()
endif()
