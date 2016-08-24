# ===================================================================================
#  ArucoUnity CMake configuration file
#
#             ** File generated automatically, do not modify **
#
#  Usage from an external project:
#    In your CMakeLists.txt, add these lines:
#
#    FIND_PACKAGE(ArucoUnity REQUIRED)
#    TARGET_LINK_LIBRARIES(MY_TARGET_NAME ${ArucoUnity_LIBS})
#
#    If the module is found then ArucoUnity_FOUND is set to TRUE.
#
#    This file will define the following variables:
#      - ArucoUnity_LIBS          : The list of libraries to links against.
#      - ArucoUnity_LIB_DIR       : The directory where lib files are. Calling LINK_DIRECTORIES
#                                       with this path is NOT needed.
#      - ArucoUnity_INCLUDE_DIRS  : The ArucoUnity include directories.
#      - ArucoUnity_VERSION       : The version of this ArucoUnity build. Example: "1.2.0"
#      - ArucoUnity_VERSION_MAJOR : Major version part of ArucoUnity_VERSION. Example: "1"
#      - ArucoUnity_VERSION_MINOR : Minor version part of ArucoUnity_VERSION. Example: "2"
#      - ArucoUnity_VERSION_PATCH : Patch version part of ArucoUnity_VERSION. Example: "0"
#
# ===================================================================================
SET(ArucoUnity_INCLUDE_DIRS "D:/ETS/Maitrise/Prototype/aruco-unity/include")
INCLUDE_DIRECTORIES("D:/ETS/Maitrise/Prototype/aruco-unity/include")

SET(ArucoUnity_LIB_DIR "D:/ETS/Maitrise/Prototype/aruco-unity/lib")
LINK_DIRECTORIES("D:/ETS/Maitrise/Prototype/aruco-unity/lib")

SET(ArucoUnity_LIBS ArucoUnity)

SET(ArucoUnity_FOUND TRUE)

SET(ArucoUnity_VERSION        0.1.0)
SET(ArucoUnity_VERSION_MAJOR  0)
SET(ArucoUnity_VERSION_MINOR  1)
SET(ArucoUnity_VERSION_PATCH  0)
