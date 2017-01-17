using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Samples
  {
    /// <summary>
    /// Editor manager for the <see cref="DetectorParameters"/>.
    /// </summary>
    public class DetectorParametersManager : MonoBehaviour
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The minimum window size for adaptive thresholding before finding contours (default 3).")]
      private int AdaptiveThreshWinSizeMin = 3;

      [SerializeField]
      [Tooltip("The maximum window size for adaptive thresholding before finding contours (default 23).")]
      private int AdaptiveThreshWinSizeMax = 23;

      [SerializeField]
      [Tooltip("The increments from adaptiveThreshWinSizeMin to adaptiveThreshWinSizeMax during the thresholding (default 10).")]
      private int AdaptiveThreshWinSizeStep = 10;

      [SerializeField]
      [Tooltip("The constant for adaptive thresholding before finding contours (default 7).")]
      private double AdaptiveThreshConstant = 7;

      [SerializeField]
      [Tooltip("The minimum perimeter for marker contour to be detected. This is defined as a rate respect to the maximum dimension of the input image (default 0.03).")]
      private double MinMarkerPerimeterRate = 0.03;

      [SerializeField]
      [Tooltip("The maximum perimeter for marker contour to be detected. This is defined as a rate respect to the maximum dimension of the input image (default 4.0).")]
      private double MaxMarkerPerimeterRate = 4.0;

      [SerializeField]
      [Tooltip("The minimum accuracy during the polygonal approximation process to determine which contours are squares.")]
      private double PolygonalApproxAccuracyRate = 0.03;

      [SerializeField]
      [Tooltip("The minimum distance between corners for detected markers relative to its perimeter (default 0.05).")]
      private double MinCornerDistanceRate = 0.05;

      [SerializeField]
      [Tooltip("The minimum distance of any corner to the image border for detected markers (in pixels) (default 3).")]
      private int MinDistanceToBorder = 3;

      [SerializeField]
      [Tooltip("The minimum mean distance beetween two marker corners to be considered similar, so that the smaller one is removed. The rate is relative to the smaller perimeter of the two markers (default 0.05).")]
      private double MinMarkerDistanceRate = 0.05;

      [SerializeField]
      [Tooltip("If there is a subpixel refinement or not.")]
      private bool DoCornerRefinement = false;

      [SerializeField]
      [Tooltip("The window size for the corner refinement process (in pixels) (default 5).")]
      private int CornerRefinementWinSize = 5;

      [SerializeField]
      [Tooltip("The maximum number of iterations for stop criteria of the corner refinement process (default 30).")]
      private int CornerRefinementMaxIterations = 30;

      [SerializeField]
      [Tooltip("The minimum error for the stop cristeria of the corner refinement process (default: 0.1).")]
      private double CornerRefinementMinAccuracy = 0.1;

      [SerializeField]
      [Tooltip("The number of bits of the marker border, i.e. marker border width (default 1).")]
      private int MarkerBorderBits = 1;

      [SerializeField]
      [Tooltip("The number of bits (per dimension) for each cell of the marker when removing the perspective (default 8).")]
      private int PerspectiveRemovePixelPerCell = 8;

      [SerializeField]
      [Tooltip("The width of the margin of pixels on each cell not considered for the determination of the cell bit. Represents the rate respect to the total size of the cell, i.e. perpectiveRemovePixelPerCell (default 0.13).")]
      private double PerspectiveRemoveIgnoredMarginPerCell = 0.13;

      [SerializeField]
      [Tooltip("The maximum number of accepted erroneous bits in the border (i.e. number of allowed white bits in the border). Represented as a rate respect to the total number of bits per marker (default 0.35).")]
      private double MaxErroneousBitsInBorderRate = 0.35;

      [SerializeField]
      [Tooltip("The minimun standard deviation in pixels values during the decodification step to apply Otsu thresholding (otherwise, all the bits are sets to 0 or 1 depending on mean higher than 128 or not) (default 5.0).")]
      private double MinOtsuStdDev = 5.0;

      [SerializeField]
      [Tooltip("The maximun error correction capability for each dictionary (default 0.6).")]
      private double ErrorCorrectionRate = 0.6;

      // Variables

      /// <summary>
      /// The managed DetectorParameters.
      /// </summary>
      public DetectorParameters detectorParameters;

      // MonoBehaviour methods

      /// <summary>
      /// Set the value of the <see cref="detectorParameters"/> from the editor fields.
      /// </summary>
      void Start()
      {
        detectorParameters = new DetectorParameters();

        detectorParameters.AdaptiveThreshWinSizeMin = AdaptiveThreshWinSizeMin;
        detectorParameters.AdaptiveThreshWinSizeMax = AdaptiveThreshWinSizeMax;
        detectorParameters.AdaptiveThreshWinSizeStep = AdaptiveThreshWinSizeStep;
        detectorParameters.AdaptiveThreshConstant = AdaptiveThreshConstant;
        detectorParameters.MinMarkerPerimeterRate = MinMarkerPerimeterRate;
        detectorParameters.MaxMarkerPerimeterRate = MaxMarkerPerimeterRate;
        detectorParameters.PolygonalApproxAccuracyRate = PolygonalApproxAccuracyRate;
        detectorParameters.MinCornerDistanceRate = MinCornerDistanceRate;
        detectorParameters.MinDistanceToBorder = MinDistanceToBorder;
        detectorParameters.MinMarkerDistanceRate = MinMarkerDistanceRate;
        detectorParameters.DoCornerRefinement = DoCornerRefinement;
        detectorParameters.CornerRefinementWinSize = CornerRefinementWinSize;
        detectorParameters.CornerRefinementMaxIterations = CornerRefinementMaxIterations;
        detectorParameters.CornerRefinementMinAccuracy = CornerRefinementMinAccuracy;
        detectorParameters.MarkerBorderBits = MarkerBorderBits;
        detectorParameters.PerspectiveRemovePixelPerCell = PerspectiveRemovePixelPerCell;
        detectorParameters.PerspectiveRemoveIgnoredMarginPerCell = PerspectiveRemoveIgnoredMarginPerCell;
        detectorParameters.MaxErroneousBitsInBorderRate = MaxErroneousBitsInBorderRate;
        detectorParameters.MinOtsuStdDev = MinOtsuStdDev;
        detectorParameters.ErrorCorrectionRate = ErrorCorrectionRate;
      }
    }
  }

  /// \} aruco_unity_package
}