cmake_minimum_required(VERSION 3.0)

project(googletest)

# Configuration
option(GTEST_BUILD_SHARED_LIBS "Build shared libraries (DLLs)." OFF)
option(GTEST_FORCE_SHARED_CRT "Use shared (DLL) run-time lib even when Google Test is built as static lib." ON)
option(GTEST_DISABLE_PTHREADS "Disable uses of pthreads in gtest." OFF)

# Dependencies
find_package(Threads REQUIRED)

# Target
include(ExternalProject)

ExternalProject_Add(${PROJECT_NAME}
  GIT_REPOSITORY https://github.com/google/googletest.git
  CMAKE_ARGS
    -DBUILD_SHARED_LIBS=${GTEST_BUILD_SHARED_LIBS}
    -Dgtest_force_shared_crt=${GTEST_FORCE_SHARED_CRT}
    -Dgtest_disable_pthreads=${GTEST_DISABLE_PTHREADS}
    -DBUILD_GMOCK=OFF
    -DBUILD_GTEST=ON
    -DCMAKE_INSTALL_PREFIX=${PROJECT_BINARY_DIR}
  PREFIX ${PROJECT_BINARY_DIR}
  INSTALL_DIR ${PROJECT_BINARY_DIR}
)

# Package
ExternalProject_Get_Property(${PROJECT_NAME} install_dir)
set(GTEST_INCLUDE_DIRS ${install_dir}/include PARENT_SCOPE)
set(GTEST_LIB_DIR ${install_dir}/lib PARENT_SCOPE)

set(GTEST_LIBRARIES gtest)
add_library(${GTEST_LIBRARIES} IMPORTED STATIC GLOBAL)
add_dependencies(${GTEST_LIBRARIES} googletest)
set_target_properties(${GTEST_LIBRARIES} 
    PROPERTIES
    IMPORTED_LOCATION ${install_dir}/lib/${CMAKE_FIND_LIBRARY_PREFIXES}${GTEST_LIBRARIES}${CMAKE_FIND_LIBRARY_SUFFIXES}
    IMPORTED_LINK_INTERFACE_LIBRARIES "${CMAKE_THREAD_LIBS_INIT}"
)

set(GTEST_MAIN_LIBRARIES gtest_main)
add_library(${GTEST_MAIN_LIBRARIES} IMPORTED STATIC GLOBAL)
add_dependencies(${GTEST_MAIN_LIBRARIES} googletest)
set_target_properties(${GTEST_MAIN_LIBRARIES} 
    PROPERTIES
    IMPORTED_LOCATION ${install_dir}/lib/${CMAKE_FIND_LIBRARY_PREFIXES}${GTEST_MAIN_LIBRARIES}${CMAKE_FIND_LIBRARY_SUFFIXES}
    IMPORTED_LINK_INTERFACE_LIBRARIES "${CMAKE_THREAD_LIBS_INIT}"
)

set(GTEST_BOTH_LIBRARIES ${GTEST_LIBRARIES} ${GTEST_MAIN_LIBRARIES})