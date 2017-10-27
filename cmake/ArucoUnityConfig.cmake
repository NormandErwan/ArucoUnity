# ===================================================================================
#  @CMAKE_PROJECT_NAME@ CMake configuration file
#
#             ** File generated automatically, do not modify **
#
#  Usage from an external project:
#    In your CMakeLists.txt, add these lines:
#
#    FIND_PACKAGE(@CMAKE_PROJECT_NAME@ REQUIRED)
#    TARGET_LINK_LIBRARIES(MY_TARGET_NAME ${@CMAKE_PROJECT_NAME@_LIBS})
#
#    If the module is found then @CMAKE_PROJECT_NAME@_FOUND is set to TRUE.
#
#    This file will define the following variables:
#      - @CMAKE_PROJECT_NAME@_LIBS          : The list of libraries to links against.
#      - @CMAKE_PROJECT_NAME@_LIB_DIR       : The directory where lib files are. Calling LINK_DIRECTORIES
#                                       with this path is NOT needed.
#      - @CMAKE_PROJECT_NAME@_INCLUDE_DIRS  : The @CMAKE_PROJECT_NAME@ include directories.
#      - @CMAKE_PROJECT_NAME@_VERSION       : The version of this @CMAKE_PROJECT_NAME@ build. Example: "@VERSION_MAJOR@.@VERSION_MINOR@.@VERSION_PATCH@"
#      - @CMAKE_PROJECT_NAME@_VERSION_MAJOR : Major version part of @CMAKE_PROJECT_NAME@_VERSION. Example: "@VERSION_MAJOR@"
#      - @CMAKE_PROJECT_NAME@_VERSION_MINOR : Minor version part of @CMAKE_PROJECT_NAME@_VERSION. Example: "@VERSION_MINOR@"
#      - @CMAKE_PROJECT_NAME@_VERSION_PATCH : Patch version part of @CMAKE_PROJECT_NAME@_VERSION. Example: "@VERSION_PATCH@"
#
# ===================================================================================
SET(@CMAKE_PROJECT_NAME@_INCLUDE_DIRS "@CMAKE_INSTALL_PREFIX@/include")
INCLUDE_DIRECTORIES("@CMAKE_INSTALL_PREFIX@/include")

SET(@CMAKE_PROJECT_NAME@_LIB_DIR "@CMAKE_INSTALL_PREFIX@/lib")
LINK_DIRECTORIES("@CMAKE_INSTALL_PREFIX@/lib")

SET(@CMAKE_PROJECT_NAME@_LIBS @CMAKE_PROJECT_NAME@)

SET(@CMAKE_PROJECT_NAME@_FOUND TRUE)

SET(@CMAKE_PROJECT_NAME@_VERSION        @VERSION@)
SET(@CMAKE_PROJECT_NAME@_VERSION_MAJOR  @VERSION_MAJOR@)
SET(@CMAKE_PROJECT_NAME@_VERSION_MINOR  @VERSION_MINOR@)
SET(@CMAKE_PROJECT_NAME@_VERSION_PATCH  @VERSION_PATCH@)
