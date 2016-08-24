cmake_minimum_required(VERSION 2.8)

project(ArucoUnityLibTests)


# Inputs
file(GLOB_RECURSE ArucoUnityLibTests_SOURCES ${PROJECT_SOURCE_DIR}/*.cpp)


# Outputs
set(RUNTIME_OUTPUT_DIRECTORY ${CMAKE_SOURCE_DIR}/bin)

add_executable(${PROJECT_NAME} ${ArucoUnityLibTests_SOURCES})


# Dependencies
find_package(OpenCV REQUIRED aruco)
target_link_libraries(${PROJECT_NAME} ${OpenCV_LIBS})

include_directories(${ArucoUnity_INCLUDE_DIRS})
link_directories(${LIBRARY_OUTPUT_DIRECTORY})
target_link_libraries(${PROJECT_NAME} ${ArucoUnity_LIBS})


# Installation
install(TARGETS ${PROJECT_NAME}
        RUNTIME DESTINATION ${RUNTIME_OUTPUT_DIRECTORY}/test
)