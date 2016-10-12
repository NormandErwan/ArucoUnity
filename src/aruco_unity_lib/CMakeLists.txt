cmake_minimum_required(VERSION 3.0)

project(ArucoUnityLib)


# Dependencies
find_package(OpenCV REQUIRED aruco)


# Target
set(PROJECT_HEADERS
    ${PROJECT_INCLUDE_DIRS}/aruco_unity.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/board.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/dictionary.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/detector_parameters.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/exports.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/grid_board.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/utility.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/exception.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/mat.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/point2f.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/point3f.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/scalar.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/size.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/term_criteria.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/vector_int.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/vector_mat.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/vector_point2f.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/vector_point3f.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/vector_vector_int.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/vector_vector_point2f.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/vector_vector_point3f.hpp
    ${PROJECT_INCLUDE_DIRS}/aruco_unity/utility/vector_vector_vector_point2f.hpp
)
set(PROJECT_SOURCES
    ${PROJECT_SOURCE_DIR}/aruco_unity.cpp
    ${PROJECT_SOURCE_DIR}/board.cpp
    ${PROJECT_SOURCE_DIR}/detector_parameters.cpp
    ${PROJECT_SOURCE_DIR}/dictionary.cpp
    ${PROJECT_SOURCE_DIR}/exception.cpp
    ${PROJECT_SOURCE_DIR}/grid_board.cpp
    ${PROJECT_SOURCE_DIR}/mat.cpp
    ${PROJECT_SOURCE_DIR}/point2f.cpp
    ${PROJECT_SOURCE_DIR}/point3f.cpp
    ${PROJECT_SOURCE_DIR}/scalar.cpp
    ${PROJECT_SOURCE_DIR}/size.cpp
    ${PROJECT_SOURCE_DIR}/term_criteria.cpp
    ${PROJECT_SOURCE_DIR}/vector_int.cpp
    ${PROJECT_SOURCE_DIR}/vector_mat.cpp
    ${PROJECT_SOURCE_DIR}/vector_point2f.cpp
    ${PROJECT_SOURCE_DIR}/vector_point3f.cpp
    ${PROJECT_SOURCE_DIR}/vector_vector_int.cpp
    ${PROJECT_SOURCE_DIR}/vector_vector_point2f.cpp
    ${PROJECT_SOURCE_DIR}/vector_vector_point3f.cpp
    ${PROJECT_SOURCE_DIR}/vector_vector_vector_point2f.cpp
)

add_library(${CMAKE_PROJECT_NAME} SHARED ${PROJECT_HEADERS} ${PROJECT_SOURCES})
target_include_directories(${CMAKE_PROJECT_NAME} PUBLIC ${PROJECT_INCLUDE_DIRS})

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