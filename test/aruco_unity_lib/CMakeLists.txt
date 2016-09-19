cmake_minimum_required(VERSION 2.8)

project(ArucoUnityTests)


# Target
file(GLOB_RECURSE PROJECT_SOURCES ${PROJECT_SOURCE_DIR}/*.cpp)

add_executable(${PROJECT_NAME} ${PROJECT_SOURCES})


# Dependencies
link_directories(${RUNTIME_OUTPUT_DIRECTORY})
target_link_libraries(${PROJECT_NAME} ${CMAKE_PROJECT_NAME})


# Installation
install(TARGETS ${PROJECT_NAME}
        RUNTIME DESTINATION ${RUNTIME_OUTPUT_DIRECTORY}
)