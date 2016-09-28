using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class DetectorParameters : HandleCvPtr
  {
    [DllImport("ArucoUnity")]
    static extern System.IntPtr au_DetectorParameters_create();

    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_delete(System.IntPtr parameters);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_getAdaptiveThreshWinSizeMin(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setAdaptiveThreshWinSizeMin(System.IntPtr parameters, int adaptiveThreshWinSizeMin);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_getAdaptiveThreshWinSizeMax(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setAdaptiveThreshWinSizeMax(System.IntPtr parameters, int adaptiveThreshWinSizeMax);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_getAdaptiveThreshWinSizeStep(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setAdaptiveThreshWinSizeStep(System.IntPtr parameters, int adaptiveThreshWinSizeStep);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_getAdaptiveThreshConstant(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setAdaptiveThreshConstant(System.IntPtr parameters, double adaptiveThreshConstant);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_getMinMarkerPerimeterRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setMinMarkerPerimeterRate(System.IntPtr parameters, double minMarkerPerimeterRate);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_getMaxMarkerPerimeterRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setMaxMarkerPerimeterRate(System.IntPtr parameters, double maxMarkerPerimeterRate);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_getPolygonalApproxAccuracyRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setPolygonalApproxAccuracyRate(System.IntPtr parameters, double polygonalApproxAccuracyRate);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_getMinCornerDistanceRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setMinCornerDistanceRate(System.IntPtr parameters, double minCornerDistanceRate);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_getMinDistanceToBorder(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setMinDistanceToBorder(System.IntPtr parameters, int minDistanceToBorder);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_getMinMarkerDistanceRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setMinMarkerDistanceRate(System.IntPtr parameters, double minMarkerDistanceRate);

    [DllImport("ArucoUnity")] [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool au_DetectorParameters_getDoCornerRefinement(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setDoCornerRefinement(System.IntPtr parameters, [MarshalAs(UnmanagedType.Bool)] bool doCornerRefinement);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_getCornerRefinementWinSize(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setCornerRefinementWinSize(System.IntPtr parameters, int cornerRefinementWinSize);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_getCornerRefinementMaxIterations(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setCornerRefinementMaxIterations(System.IntPtr parameters, int cornerRefinementMaxIterations);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_getCornerRefinementMinAccuracy(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setCornerRefinementMinAccuracy(System.IntPtr parameters, double cornerRefinementMinAccuracy);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_getMarkerBorderBits(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setMarkerBorderBits(System.IntPtr parameters, int markerBorderBits);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_getPerspectiveRemovePixelPerCell(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setPerspectiveRemovePixelPerCell(System.IntPtr parameters, int perspectiveRemovePixelPerCell);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_getPerspectiveRemoveIgnoredMarginPerCell(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setPerspectiveRemoveIgnoredMarginPerCell(System.IntPtr parameters, double perspectiveRemoveIgnoredMarginPerCell);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_getMaxErroneousBitsInBorderRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setMaxErroneousBitsInBorderRate(System.IntPtr parameters, double maxErroneousBitsInBorderRate);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_getMinOtsuStdDev(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setMinOtsuStdDev(System.IntPtr parameters, double minOtsuStdDev);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_getErrorCorrectionRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_setErrorCorrectionRate(System.IntPtr parameters, double errorCorrectionRate);
    
    public DetectorParameters() : base(au_DetectorParameters_create())
    {
    }

    ~DetectorParameters()
    {
      au_DetectorParameters_delete(cvPtr);
    }

    public int AdaptiveThreshWinSizeMin {
      get { return au_DetectorParameters_getAdaptiveThreshWinSizeMin(cvPtr); }
      set { au_DetectorParameters_setAdaptiveThreshWinSizeMin(cvPtr, value); }
    }

    public int AdaptiveThreshWinSizeMax {
      get { return au_DetectorParameters_getAdaptiveThreshWinSizeMax(cvPtr); }
      set { au_DetectorParameters_setAdaptiveThreshWinSizeMax(cvPtr, value); }
    }

    public int AdaptiveThreshWinSizeStep {
      get { return au_DetectorParameters_getAdaptiveThreshWinSizeStep(cvPtr); }
      set { au_DetectorParameters_setAdaptiveThreshWinSizeStep(cvPtr, value); }
    }

    public double AdaptiveThreshConstant {
      get { return au_DetectorParameters_getAdaptiveThreshConstant(cvPtr); }
      set { au_DetectorParameters_setAdaptiveThreshConstant(cvPtr, value); }
    }

    public double MinMarkerPerimeterRate {
      get { return au_DetectorParameters_getMinMarkerPerimeterRate(cvPtr); }
      set { au_DetectorParameters_setMinMarkerPerimeterRate(cvPtr, value); }
    }

    public double MaxMarkerPerimeterRate {
      get { return au_DetectorParameters_getMaxMarkerPerimeterRate(cvPtr); }
      set { au_DetectorParameters_setMaxMarkerPerimeterRate(cvPtr, value); }
    }

    public double PolygonalApproxAccuracyRate {
      get { return au_DetectorParameters_getPolygonalApproxAccuracyRate(cvPtr); }
      set { au_DetectorParameters_setPolygonalApproxAccuracyRate(cvPtr, value); }
    }

    public double MinCornerDistanceRate {
      get { return au_DetectorParameters_getMinCornerDistanceRate(cvPtr); }
      set { au_DetectorParameters_setMinCornerDistanceRate(cvPtr, value); }
    }

    public int MinDistanceToBorder {
      get { return au_DetectorParameters_getMinDistanceToBorder(cvPtr); }
      set { au_DetectorParameters_setMinDistanceToBorder(cvPtr, value); }
    }

    public double MinMarkerDistanceRate {
      get { return au_DetectorParameters_getMinMarkerDistanceRate(cvPtr); }
      set { au_DetectorParameters_setMinMarkerDistanceRate(cvPtr, value); }
    }

    public bool DoCornerRefinement {
      get { return au_DetectorParameters_getDoCornerRefinement(cvPtr); }
      set { au_DetectorParameters_setDoCornerRefinement(cvPtr, value); }
    }

    public int CornerRefinementWinSize {
      get { return au_DetectorParameters_getCornerRefinementWinSize(cvPtr); }
      set { au_DetectorParameters_setCornerRefinementWinSize(cvPtr, value); }
    }

    public int CornerRefinementMaxIterations {
      get { return au_DetectorParameters_getCornerRefinementMaxIterations(cvPtr); }
      set { au_DetectorParameters_setCornerRefinementMaxIterations(cvPtr, value); }
    }

    public double CornerRefinementMinAccuracy {
      get { return au_DetectorParameters_getCornerRefinementMinAccuracy(cvPtr); }
      set { au_DetectorParameters_setCornerRefinementMinAccuracy(cvPtr, value); }
    }

    public int MarkerBorderBits {
      get { return au_DetectorParameters_getMarkerBorderBits(cvPtr); }
      set { au_DetectorParameters_setMarkerBorderBits(cvPtr, value); }
    }

    public int PerspectiveRemovePixelPerCell {
      get { return au_DetectorParameters_getPerspectiveRemovePixelPerCell(cvPtr); }
      set { au_DetectorParameters_setPerspectiveRemovePixelPerCell(cvPtr, value); }
    }

    public double PerspectiveRemoveIgnoredMarginPerCell {
      get { return au_DetectorParameters_getPerspectiveRemoveIgnoredMarginPerCell(cvPtr); }
      set { au_DetectorParameters_setPerspectiveRemoveIgnoredMarginPerCell(cvPtr, value); }
    }

    public double MaxErroneousBitsInBorderRate {
      get { return au_DetectorParameters_getMaxErroneousBitsInBorderRate(cvPtr); }
      set { au_DetectorParameters_setMaxErroneousBitsInBorderRate(cvPtr, value); }
    }

    public double MinOtsuStdDev {
      get { return au_DetectorParameters_getMinOtsuStdDev(cvPtr); }
      set { au_DetectorParameters_setMinOtsuStdDev(cvPtr, value); }
    }

    public double ErrorCorrectionRate {
      get { return au_DetectorParameters_getErrorCorrectionRate(cvPtr); }
      set { au_DetectorParameters_setErrorCorrectionRate(cvPtr, value); }
    }
  }
}