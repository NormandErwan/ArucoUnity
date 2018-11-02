# ArucoUnity Architecture

ArucoUnity is made of three parts:

1. A [plugin](https://github.com/NormandErwan/ArucoUnity/blob/master/ArucoUnityPlugin/) that wraps into a C interface the aruco, calib3d and ccalib modules of OpenCV.
2. A C# interface ([ArucoUnity.Plugin namespace](https://github.com/NormandErwan/ArucoUnity/blob/master/Assets/ArucoUnity/Scripts/Plugin/)) using the plugin to reproduce the OpenCV modules classes and functions.
3. [Unity scripts](~/api/) to calibrate cameras and to track markers directly in the editor with good performances.

You can code directly with the OpenCV C# equivalent interface but we advise you to work with and extend the Unity scripts. The Unity scripts were originally one camera display and tracking script and one calibration script. For performances and to support multiple types of camera (fisheye, stereoscopic) we decoupled these scripts (*Fig.1*).

![Class diagram](http://www.plantuml.com/plantuml/svg/ZLF1Ri8m3BtdAw8U9nLfTmviHTDssWxZ0znYKBRG8CSze8r_NrbGC12FtIhxFVizvtKM6OY7ZJTRw3vXZVQ1XYuiUiSx-vMB2tRUiSCurbpFI2leqBuqeL-vzK3GUXo_cOBtI6PlCh613HASitutf8OWASFizBkkLOues-c8g-uh3Qie-F_UawcvYriFkwZQEWBEz2cZ9LuUxWreR37InPPdY89uGadkeb-wo87OXLuNAL4tb2eaLSmjthLryNeAQIyTlhOVsF716xIH2JgsdOSOKtfJrnHxZm5eQGGiQHMzlEITne_I0yoHHsAWRFWwN4U18dzoPLq7MzrL6jQgyuwIZ49CNigCacuThmT29wYFMYRihtweT6k1pVMqLtGQGk104fot5GUSvwS1xrg2Gx2K32KZ0gTVP39HAFeFyYVhbCLiIK5UAXUz9m_hQ2uBDmQcAysmNmuR_mC0)

*Fig.1: Overview class diagram of the Unity scripts.*