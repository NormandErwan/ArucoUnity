cmake_minimum_required(VERSION 2.8)

project(ArucoUnityLib)


# Inputs
include_directories(${ArucoUnity_INCLUDE_DIRS})

file(GLOB_RECURSE ArucoUnityLib_HEADERS ${ArucoUnityLib_INCLUDE_DIRS}/*.hpp)
file(GLOB_RECURSE ArucoUnityLib_SOURCES ${PROJECT_SOURCE_DIR}/*.cpp)


# Outputs
add_library(${CMAKE_PROJECT_NAME} SHARED ${ArucoUnityLib_HEADERS} ${ArucoUnityLib_SOURCES})


# Dependencies
find_package(OpenCV REQUIRED aruco)
target_link_libraries(${CMAKE_PROJECT_NAME} ${OpenCV_LIBS})


# Installation
install(TARGETS ${CMAKE_PROJECT_NAME}
        RUNTIME DESTINATION ${RUNTIME_OUTPUT_DIRECTORY}
        ARCHIVE DESTINATION ${ARCHIVE_OUTPUT_DIRECTORY}
        LIBRARY DESTINATION ${LIBRARY_OUTPUT_DIRECTORY}
)

# Installation of the library in the Unity project folder
install(TARGETS ${CMAKE_PROJECT_NAME}
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


# Package
configure_file("${CMAKE_SOURCE_DIR}/cmake/${CMAKE_PROJECT_NAME}Config.cmake" "${CMAKE_SOURCE_DIR}/${CMAKE_PROJECT_NAME}Config.cmake" @ONLY)
install(FILES "${CMAKE_SOURCE_DIR}/${CMAKE_PROJECT_NAME}Config.cmake" 
        DESTINATION ${LIBRARY_OUTPUT_DIRECTORY}/cmake/)