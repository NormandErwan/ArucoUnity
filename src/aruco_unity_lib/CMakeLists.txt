cmake_minimum_required(VERSION 2.8)

project(aruco_unity_lib)


# Dependencies
find_package(OpenCV REQUIRED aruco)


# Inputs
set(aruco_unity_lib_INCLUDE_DIRS ${CMAKE_SOURCE_DIR}/include)
include_directories(${aruco_unity_lib_INCLUDE_DIRS})
file(GLOB aruco_unity_lib_headers ${aruco_unity_lib_INCLUDE_DIRS}/*)

file(GLOB aruco_unity_lib_sources ${PROJECT_SOURCE_DIR}/*)


# Outputs
set(RUNTIME_OUTPUT_DIRECTORY ${CMAKE_SOURCE_DIR}/bin) # Destination of the dll for dll platform
set(ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_SOURCE_DIR}/lib) # Destination of the corresponding import libraries of the dlls for dll platform
set(LIBRARY_OUTPUT_DIRECTORY ${CMAKE_SOURCE_DIR}/lib) # Destination of the librairies for non-dll platform

add_library(${PROJECT_NAME} SHARED ${aruco_unity_lib_headers} ${aruco_unity_lib_sources})

target_link_libraries(${PROJECT_NAME} ${OpenCV_LIBS})


# Installation
install(TARGETS ${PROJECT_NAME}
        RUNTIME DESTINATION ${RUNTIME_OUTPUT_DIRECTORY}
        ARCHIVE DESTINATION ${ARCHIVE_OUTPUT_DIRECTORY}
        LIBRARY DESTINATION ${LIBRARY_OUTPUT_DIRECTORY}
)

# Installation of the library in the Unity project folder
install(TARGETS ${PROJECT_NAME}
        RUNTIME DESTINATION ${UNITY_PLUGINS_DIR}
        LIBRARY DESTINATION ${UNITY_PLUGINS_DIR}
)

# Installation of the dll dependencies on Windows
if (WIN32)
  install(DIRECTORY ${aruco_LIB_DIR}/../bin/ ${OpenCV_LIB_DIR}/../bin/ 
          DESTINATION ${UNITY_PLUGINS_DIR}
          FILES_MATCHING PATTERN "*.dll"
  )
endif()
