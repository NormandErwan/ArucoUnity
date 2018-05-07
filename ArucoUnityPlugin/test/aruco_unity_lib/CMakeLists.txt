cmake_minimum_required(VERSION 3.0)

project(ArucoUnityTests)

# Dependencies
find_package(Threads REQUIRED)

# Target
file(GLOB_RECURSE PROJECT_SOURCES ${PROJECT_SOURCE_DIR}/*.cpp)

add_executable(${PROJECT_NAME} ${PROJECT_SOURCES})
add_test(${PROJECT_NAME} ${PROJECT_NAME})

# Link gtest
target_include_directories(${PROJECT_NAME} PUBLIC ${GTEST_INCLUDE_DIRS})
target_link_libraries(${PROJECT_NAME} gtest gtest_main)

# Link ArucoUnity
link_directories(${RUNTIME_OUTPUT_DIRECTORY})
target_link_libraries(${PROJECT_NAME} ${CMAKE_PROJECT_NAME})