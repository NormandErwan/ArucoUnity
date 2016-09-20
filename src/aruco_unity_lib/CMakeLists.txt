cmake_minimum_required(VERSION 2.8)

project(ArucoUnityLib)


# Target
set(PROJECT_HEADERS
    ${PROJECT_INCLUDE_DIRS}/aruco_unity.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/exports.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/dictionary.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/detector_parameters.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/utility.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/mat.hpp
)
set(PROJECT_SOURCES
    ${PROJECT_SOURCE_DIR}/aruco_unity.cpp
    ${PROJECT_SOURCE_DIR}/dictionary.cpp
    ${PROJECT_SOURCE_DIR}/detector_parameters.cpp
    ${PROJECT_SOURCE_DIR}/mat.cpp
)

add_library(${CMAKE_PROJECT_NAME} SHARED ${PROJECT_HEADERS} ${PROJECT_SOURCES})
target_include_directories(${CMAKE_PROJECT_NAME} PUBLIC ${PROJECT_INCLUDE_DIRS})


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
# and installation of the dll dependencies on Windows
install(TARGETS ${CMAKE_PROJECT_NAME}
        RUNTIME DESTINATION ${UNITY_PLUGINS_DIR}
        LIBRARY DESTINATION ${UNITY_PLUGINS_DIR}
)
if (WIN32)
  install(DIRECTORY ${OpenCV_LIB_DIR}/../bin/ 
          DESTINATION ${UNITY_PLUGINS_DIR}
          FILES_MATCHING PATTERN "*.dll"
  )
endif()


# Package
configure_file("${CMAKE_SOURCE_DIR}/cmake/${CMAKE_PROJECT_NAME}Config.cmake" "${CMAKE_INSTALL_PREFIX}/${CMAKE_PROJECT_NAME}Config.cmake" @ONLY)
install(FILES "${CMAKE_INSTALL_PREFIX}/${CMAKE_PROJECT_NAME}Config.cmake" 
        DESTINATION ${LIBRARY_OUTPUT_DIRECTORY}/cmake/)