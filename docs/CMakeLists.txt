cmake_minimum_required(VERSION 3.0)

project(ArucoUnityDocs)

# Configuration
set(DOXYFILE ${PROJECT_SOURCE_DIR}/Doxyfile)
set(DOXYGEN_OUTPUT_DIRECTORY "docs")

# Dependencies
find_package(Doxygen REQUIRED)

# Target
configure_file(Doxyfile.in ${DOXYFILE} @ONLY)

add_custom_target(${PROJECT_NAME} ALL
                  COMMAND ${DOXYGEN_EXECUTABLE} ${DOXYFILE}
)

# Installation
install(DIRECTORY ${PROJECT_BINARY_DIR}/${DOXYGEN_OUTPUT_DIRECTORY}/html/
        DESTINATION ${PROJECT_SOURCE_DIR}
)