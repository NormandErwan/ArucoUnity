cmake_minimum_required(VERSION 3.0)

project(OpenCVAruco)

# Dependencies
include(ExternalProject)

set(OPENCV_CONTRIB_PROJECT OpenCVContribDownload)
ExternalProject_Add(${OPENCV_CONTRIB_PROJECT}
  GIT_REPOSITORY https://github.com/opencv/opencv_contrib.git
  CONFIGURE_COMMAND ""
  BUILD_COMMAND ""
  INSTALL_COMMAND ""
)
ExternalProject_Get_Property(${OPENCV_CONTRIB_PROJECT} SOURCE_DIR)
set(OPENCV_CONTRIB_PATH ${SOURCE_DIR})

# Target
ExternalProject_Add(${PROJECT_NAME}
  DEPENDS ${OPENCV_CONTRIB_PROJECT}
  GIT_REPOSITORY https://github.com/opencv/opencv.git
  CMAKE_ARGS
    -DOPENCV_EXTRA_MODULES_PATH=${OPENCV_CONTRIB_PATH}/modules
    -DBUILD_opencv_apps=OFF
    -DBUILD_opencv_aruco=ON
    -DBUILD_opencv_bgsegm=OFF
    -DBUILD_opencv_bioinspired=OFF
    -DBUILD_opencv_calib3d=ON
    -DBUILD_opencv_ccalib=OFF
    -DBUILD_opencv_contrib_world=OFF
    -DBUILD_opencv_core=ON
    -DBUILD_opencv_datasets=OFF
    -DBUILD_opencv_dnn=OFF
    -DBUILD_opencv_dpm=OFF
    -DBUILD_opencv_face=OFF
    -DBUILD_opencv_feature2d=ON
    -DBUILD_opencv_flann=ON
    -DBUILD_opencv_fuzzy=OFF
    -DBUILD_opencv_highgui=OFF
    -DBUILD_opencv_imgcodecs=OFF
    -DBUILD_opencv_imgproc=ON
    -DBUILD_opencv_line_descriptor=OFF
    -DBUILD_opencv_ml=ON
    -DBUILD_opencv_objdetect=OFF
    -DBUILD_opencv_optflow=OFF
    -DBUILD_opencv_phase_unwrapping=OFF
    -DBUILD_opencv_photo=OFF
    -DBUILD_opencv_plot=OFF
    -DBUILD_opencv_reg=OFF
    -DBUILD_opencv_rgbd=OFF
    -DBUILD_opencv_saliency=OFF
    -DBUILD_opencv_shape=OFF
    -DBUILD_opencv_stereo=OFF
    -DBUILD_opencv_stitching=OFF
    -DBUILD_opencv_structured_light=OFF
    -DBUILD_opencv_superres=OFF
    -DBUILD_opencv_surface_matching=OFF
    -DBUILD_opencv_text=OFF
    -DBUILD_opencv_tracking=OFF
    -DBUILD_opencv_ts=OFF
    -DBUILD_opencv_video=OFF
    -DBUILD_opencv_videoio=OFF
    -DBUILD_opencv_videostab=OFF
    -DBUILD_opencv_world=OFF
    -DBUILD_opencv_xfeature2d=OFF
    -DBUILD_opencv_ximgproc=OFF
    -DBUILD_opencv_xobjdetect=OFF
    -DBUILD_opencv_xphoto=OFF
)