cmake_minimum_required(VERSION 3.0)

project(ArucoUnityPlugin)

# Configuration

if (NOT OpenCV_DIR)
  set(OpenCV_DIR "${CMAKE_BINARY_DIR}/opencv/install" CACHE PATH "OpenCV install directory" FORCE)
endif()

set(PROJECT_INCLUDE_DIR "${CMAKE_SOURCE_DIR}/include" CACHE INTERNAL "The ArUco Unity plugin include directories.")
set(PROJECT_SOURCE_DIR "${CMAKE_SOURCE_DIR}/src/aruco_unity_plugin" CACHE INTERNAL "The ArUco Unity plugin source directories.")

set(ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_INSTALL_PREFIX}/lib") # Destination of the corresponding import libraries of the dlls for dll platform
set(LIBRARY_OUTPUT_DIRECTORY "${CMAKE_INSTALL_PREFIX}/lib") # Destination of the librairies for non-dll platform
set(RUNTIME_OUTPUT_DIRECTORY "${CMAKE_INSTALL_PREFIX}/bin") # Destination of the dll for dll platform

# Dependencies
find_package(OpenCV REQUIRED COMPONENTS aruco calib3d ccalib)

# Target
set(PROJECT_HEADERS
    ${PROJECT_INCLUDE_DIR}/aruco_unity.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/board.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/charuco_board.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/dictionary.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/detector_parameters.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/grid_board.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/utility/exports.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/calib3d.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/ccalib.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/core.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/cv.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/exception.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/imgproc.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/mat.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/point2f.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/point3f.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/rect.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/scalar.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/size.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/term_criteria.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/vec3d.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/cv/vec4i.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/std/std.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/std/vector_double.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/std/vector_int.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/std/vector_mat.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/std/vector_point2f.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/std/vector_point3f.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/std/vector_vec3d.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/std/vector_vec4i.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/std/vector_vector_int.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/std/vector_vector_point2f.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/std/vector_vector_point3f.hpp
    ${PROJECT_INCLUDE_DIR}/aruco_unity/std/vector_vector_vector_point2f.hpp
)
set(PROJECT_SOURCES
    ${PROJECT_SOURCE_DIR}/aruco_unity.cpp
    ${PROJECT_SOURCE_DIR}/board.cpp
    ${PROJECT_SOURCE_DIR}/calib3d.cpp
    ${PROJECT_SOURCE_DIR}/ccalib.cpp
    ${PROJECT_SOURCE_DIR}/charuco_board.cpp
    ${PROJECT_SOURCE_DIR}/core.cpp
    ${PROJECT_SOURCE_DIR}/detector_parameters.cpp
    ${PROJECT_SOURCE_DIR}/dictionary.cpp
    ${PROJECT_SOURCE_DIR}/exception.cpp
    ${PROJECT_SOURCE_DIR}/grid_board.cpp
    ${PROJECT_SOURCE_DIR}/imgproc.cpp
    ${PROJECT_SOURCE_DIR}/mat.cpp
    ${PROJECT_SOURCE_DIR}/point2f.cpp
    ${PROJECT_SOURCE_DIR}/point3f.cpp
    ${PROJECT_SOURCE_DIR}/rect.cpp
    ${PROJECT_SOURCE_DIR}/scalar.cpp
    ${PROJECT_SOURCE_DIR}/size.cpp
    ${PROJECT_SOURCE_DIR}/term_criteria.cpp
    ${PROJECT_SOURCE_DIR}/vec3d.cpp
    ${PROJECT_SOURCE_DIR}/vec4i.cpp
    ${PROJECT_SOURCE_DIR}/vector_double.cpp
    ${PROJECT_SOURCE_DIR}/vector_int.cpp
    ${PROJECT_SOURCE_DIR}/vector_mat.cpp
    ${PROJECT_SOURCE_DIR}/vector_point2f.cpp
    ${PROJECT_SOURCE_DIR}/vector_point3f.cpp
    ${PROJECT_SOURCE_DIR}/vector_vec3d.cpp
    ${PROJECT_SOURCE_DIR}/vector_vec4i.cpp
    ${PROJECT_SOURCE_DIR}/vector_vector_int.cpp
    ${PROJECT_SOURCE_DIR}/vector_vector_point2f.cpp
    ${PROJECT_SOURCE_DIR}/vector_vector_point3f.cpp
    ${PROJECT_SOURCE_DIR}/vector_vector_vector_point2f.cpp
)

add_library(${PROJECT_NAME} SHARED ${PROJECT_HEADERS} ${PROJECT_SOURCES})
target_include_directories(${PROJECT_NAME} PUBLIC ${PROJECT_INCLUDE_DIR})
target_link_libraries(${PROJECT_NAME} ${OpenCV_LIBS})

# Installation
install(TARGETS ${PROJECT_NAME}
        ARCHIVE DESTINATION ${ARCHIVE_OUTPUT_DIRECTORY}
        LIBRARY DESTINATION ${LIBRARY_OUTPUT_DIRECTORY}
        RUNTIME DESTINATION ${RUNTIME_OUTPUT_DIRECTORY}
)
install(TARGETS ${PROJECT_NAME} # Installation of the library in the Unity project folder
        LIBRARY DESTINATION ${ArucoUnity_PLUGINS_DIR}
        RUNTIME DESTINATION ${ArucoUnity_PLUGINS_DIR}
)
if (WIN32) # Installation of the OpenCV dll dependencies on Windows
  file(GLOB_RECURSE OpenCV_RUNTIME_LIBS ${OpenCV_LIB_PATH}/../bin/*.dll)
  install(FILES ${OpenCV_RUNTIME_LIBS} DESTINATION ${ArucoUnity_PLUGINS_DIR})
endif()

# Package
configure_file("${CMAKE_SOURCE_DIR}/cmake/${PROJECT_NAME}Config.cmake" "${CMAKE_INSTALL_PREFIX}/${PROJECT_NAME}Config.cmake" @ONLY)
install(FILES "${CMAKE_INSTALL_PREFIX}/${PROJECT_NAME}Config.cmake" 
        DESTINATION ${LIBRARY_OUTPUT_DIRECTORY}/cmake/)