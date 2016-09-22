cmake_minimum_required(VERSION 3.0)

project(ArucoUnityDocs)


# Configuration
set(DOXYFILE ${PROJECT_SOURCE_DIR}/Doxyfile)
set(DOXYGEN_OUTPUT_DIRECTORY "docs")


# Target
configure_file(Doxyfile.in ${DOXYFILE} @ONLY)

add_custom_target(${PROJECT_NAME} ALL
                  COMMAND ${DOXYGEN_EXECUTABLE} ${DOXYFILE}
)


# Dependencies
find_package(Doxygen REQUIRED)


# Installation
install(DIRECTORY ${PROJECT_BINARY_DIR}/${DOXYGEN_OUTPUT_DIRECTORY}/html/
        DESTINATION ${PROJECT_SOURCE_DIR}/
)