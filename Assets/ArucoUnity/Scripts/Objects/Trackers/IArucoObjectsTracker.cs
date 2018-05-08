using ArucoUnity.Cameras;
using ArucoUnity.Cameras.Displays;
using ArucoUnity.Plugin;

namespace ArucoUnity.Objects.Trackers
{
  /// <summary>
  /// 
  /// </summary>
  public interface IArucoObjectsTracker : IArucoCameraController, IArucoObjectsController, IHasDetectorParameter
  {
    // Properties

    /// <summary>
    /// Gets or sets the optional camera display associated with the ArucoCamera.
    /// </summary>
    IArucoCameraDisplay ArucoCameraDisplay { get; set; }

    /// <summary>
    /// Gets or sets if using refine strategy to detect more markers using the <see cref="ArucoBoard"/> in the
    /// <see cref="IArucoObjectsController.ArucoObjects"/> list.
    /// </summary>
    bool RefineDetectedMarkers { get; set; }

    /// <summary>
    /// Get or sets if displaying the detected <see cref="ArucoMarker"/> in the <see cref="IArucoCamera.Textures"/>.
    /// </summary>
    bool DrawDetectedMarkers { get; set; }

    /// <summary>
    /// Get or sets if displaying the rejected markers candidates.
    /// </summary>
    bool DrawRejectedCandidates { get; set; }

    /// <summary>
    /// Get or sets if displaying the axes of each detected <see cref="ArucoBoard"/> and <see cref="ArucoDiamond"/>.
    /// </summary>
    bool DrawAxes { get; set; }

    /// <summary>
    /// Get or sets if displaying the markers of each detected <see cref="ArucoCharucoBoard"/>.
    /// </summary>
    bool DrawDetectedCharucoMarkers { get; set; }

    /// <summary>
    /// Get or sets if displaying each detected <see cref="ArucoDiamond"/>.
    /// </summary>
    bool DrawDetectedDiamonds { get; set; }

    /// <summary>
    /// Gets the ArUco markers tracker used.
    /// </summary>
    ArucoMarkerTracker MarkerTracker { get; }

    // Methods

    /// <summary>
    /// Hides all the ArUco objects.
    /// </summary>
    void DeactivateArucoObjects();

    /// <summary>
    /// Detects the ArUco objects on a set of custom images.
    /// </summary>
    /// <param name="images">The images set.</param>
    void Detect(Cv.Mat[] images);

    /// <summary>
    /// Detects the ArUco objects on the current <see cref="IArucoCamera.Images"/>.
    /// </summary>
    void Detect();

    /// <summary>
    /// Draws each detected <see cref="ArucoObject"/> on a set of custom images.
    /// </summary>
    /// <param name="images">The images set to draw.</param>
    void Draw(Cv.Mat[] images);

    /// <summary>
    /// Draws each detected <see cref="ArucoObject"/> on the <see cref="IArucoCamera.Images"/>.
    /// </summary>
    void Draw();

    /// <summary>
    /// Estimates the transforms of each detected <see cref="ArucoObject"/> on the current <see cref="IArucoCamera.Images"/>.
    /// </summary>
    void EstimateTransforms();

    /// <summary>
    /// Updates transforms of each detected <see cref="ArucoObject"/>. <see cref="ArucoCameraDisplay"/> must be set.
    /// </summary>
    void UpdateTransforms();
  }
}