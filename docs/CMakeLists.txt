cmake_minimum_required(VERSION 2.8.8)

project(ArucoUnityDocs)


# Configuration
set(DOXYFILE ${PROJECT_SOURCE_DIR}/Doxyfile)
set(DOXYGEN_OUTPUT_DIRECTORY "docs")


# Target
configure_file(Doxyfile.in ${DOXYFILE} @ONLY)

add_custom_target(${PROJECT_NAME}
                  COMMAND ${DOXYGEN_EXECUTABLE} ${DOXYFILE}
)


# Dependencies
find_package(Doxygen REQUIRED)


# Installation
install(FILES ${PROJECT_BINARY_DIR}/${DOXYGEN_OUTPUT_DIRECTORY}/html/*
        DESTINATION ${PROJECT_SOURCE_DIR}/
)