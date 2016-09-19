cmake_minimum_required(VERSION 2.8)

project(ArucoUnityDoc)


# Configuration
set(DOXYFILE ${RUNTIME_OUTPUT_DIRECTORY}/Doxyfile)
set(DOXYGEN_OUTPUT_DIRECTORY "doc")


# Target
configure_file(Doxyfile.in ${DOXYFILE} @ONLY)

add_custom_target(${PROJECT_NAME}
                  COMMAND ${DOXYGEN_EXECUTABLE} ${DOXYFILE}
)

# Dependencies
find_package(Doxygen REQUIRED)


# Installation
install(DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/${DOXYGEN_OUTPUT_DIRECTORY}/html/
        DESTINATION ${RUNTIME_OUTPUT_DIRECTORY}/doc
)