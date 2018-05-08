# Track Markers

The Tracking scene is configured to track the objects generated in the Creation scene. You need to [calibrate your camera first](https://github.com/NormandErwan/aruco-unity/wiki/2.-Camera-calibration).

- Open the `Assets/ArucoUnity/Scenes/Tracking` scene.
- Configure the `Undistortion Type` and the `Camera Id` properties of the `ArucoCameraWebcam` in the scene. If you have calibrated your camera, indicate the calibration file path (e.g. `ArucoUnity/Calibrations/<calibration_file>.xml`).
- Make sure the `ArucoCamera` property of the `ArucoTracker` object is linked to the `ArucoCameraWebcam` object configured earlier.
- Create an empty object and add it an `ArucoMarker` script and configure it according to the printed marker you want to track.
- Add 3D content as a child of the ArucoMarker object.
- Add optionaly a `ArucoObjectDisplayer` script to the ArucoMarker object if you want to visualize the ArUco object.
- Add this object to the `Aruco Objects` list property of the `ArucoTracker` object.
- Run the scene. The `ArucoTracker` will place (position, rotation, scale) any detected marker in the `Aruco Objects` list relative to the camera property.