cmake_minimum_required(VERSION 2.8)

project(ArucoUnityTests)


# Target
file(GLOB_RECURSE PROJECT_SOURCES ${PROJECT_SOURCE_DIR}/*.cpp)

add_executable(${PROJECT_NAME} ${PROJECT_SOURCES})


# Dependencies
link_directories(${RUNTIME_OUTPUT_DIRECTORY})
target_link_libraries(${PROJECT_NAME} ${CMAKE_PROJECT_NAME})


# Installation
set(RUNTIME_OUTPUT_DIRECTORY ${CMAKE_SOURCE_DIR}/bin)

install(TARGETS ${PROJECT_NAME}
        RUNTIME DESTINATION ${RUNTIME_OUTPUT_DIRECTORY}
)

# Installation of the dll dependencies on Windows
if (WIN32)
  install(DIRECTORY ${OpenCV_LIB_DIR}/../bin/
          DESTINATION ${RUNTIME_OUTPUT_DIRECTORY}
          FILES_MATCHING PATTERN "*.dll"
  )
endif()