using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class DetectorParameters : HandleCvPtr
  {
    [DllImport("ArucoUnity")]
    static extern System.IntPtr au_DetectorParameters_Create();

    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_Delete(System.IntPtr parameters);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_GetAdaptiveThreshWinSizeMin(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetAdaptiveThreshWinSizeMin(System.IntPtr parameters, int adaptiveThreshWinSizeMin);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_GetAdaptiveThreshWinSizeMax(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetAdaptiveThreshWinSizeMax(System.IntPtr parameters, int adaptiveThreshWinSizeMax);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_GetAdaptiveThreshWinSizeStep(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetAdaptiveThreshWinSizeStep(System.IntPtr parameters, int adaptiveThreshWinSizeStep);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_GetAdaptiveThreshConstant(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetAdaptiveThreshConstant(System.IntPtr parameters, double adaptiveThreshConstant);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_GetMinMarkerPerimeterRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetMinMarkerPerimeterRate(System.IntPtr parameters, double minMarkerPerimeterRate);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_GetMaxMarkerPerimeterRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetMaxMarkerPerimeterRate(System.IntPtr parameters, double maxMarkerPerimeterRate);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_GetPolygonalApproxAccuracyRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetPolygonalApproxAccuracyRate(System.IntPtr parameters, double polygonalApproxAccuracyRate);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_GetMinCornerDistanceRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetMinCornerDistanceRate(System.IntPtr parameters, double minCornerDistanceRate);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_GetMinDistanceToBorder(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetMinDistanceToBorder(System.IntPtr parameters, int minDistanceToBorder);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_GetMinMarkerDistanceRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetMinMarkerDistanceRate(System.IntPtr parameters, double minMarkerDistanceRate);

    [DllImport("ArucoUnity")] [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool au_DetectorParameters_GetDoCornerRefinement(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetDoCornerRefinement(System.IntPtr parameters, [MarshalAs(UnmanagedType.Bool)] bool doCornerRefinement);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_GetCornerRefinementWinSize(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetCornerRefinementWinSize(System.IntPtr parameters, int cornerRefinementWinSize);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_GetCornerRefinementMaxIterations(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetCornerRefinementMaxIterations(System.IntPtr parameters, int cornerRefinementMaxIterations);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_GetCornerRefinementMinAccuracy(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetCornerRefinementMinAccuracy(System.IntPtr parameters, double cornerRefinementMinAccuracy);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_GetMarkerBorderBits(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetMarkerBorderBits(System.IntPtr parameters, int markerBorderBits);

    [DllImport("ArucoUnity")]
    static extern int au_DetectorParameters_GetPerspectiveRemovePixelPerCell(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetPerspectiveRemovePixelPerCell(System.IntPtr parameters, int perspectiveRemovePixelPerCell);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_GetPerspectiveRemoveIgnoredMarginPerCell(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetPerspectiveRemoveIgnoredMarginPerCell(System.IntPtr parameters, double perspectiveRemoveIgnoredMarginPerCell);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_GetMaxErroneousBitsInBorderRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetMaxErroneousBitsInBorderRate(System.IntPtr parameters, double maxErroneousBitsInBorderRate);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_GetMinOtsuStdDev(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetMinOtsuStdDev(System.IntPtr parameters, double minOtsuStdDev);

    [DllImport("ArucoUnity")]
    static extern double au_DetectorParameters_GetErrorCorrectionRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void au_DetectorParameters_SetErrorCorrectionRate(System.IntPtr parameters, double errorCorrectionRate);
    
    public DetectorParameters() : base(au_DetectorParameters_Create())
    {
    }

    ~DetectorParameters()
    {
      au_DetectorParameters_Delete(cvPtr);
    }

    public int AdaptiveThreshWinSizeMin {
      get { return au_DetectorParameters_GetAdaptiveThreshWinSizeMin(cvPtr); }
      set { au_DetectorParameters_SetAdaptiveThreshWinSizeMin(cvPtr, value); }
    }

    public int AdaptiveThreshWinSizeMax {
      get { return au_DetectorParameters_GetAdaptiveThreshWinSizeMax(cvPtr); }
      set { au_DetectorParameters_SetAdaptiveThreshWinSizeMax(cvPtr, value); }
    }

    public int AdaptiveThreshWinSizeStep {
      get { return au_DetectorParameters_GetAdaptiveThreshWinSizeStep(cvPtr); }
      set { au_DetectorParameters_SetAdaptiveThreshWinSizeStep(cvPtr, value); }
    }

    public double AdaptiveThreshConstant {
      get { return au_DetectorParameters_GetAdaptiveThreshConstant(cvPtr); }
      set { au_DetectorParameters_SetAdaptiveThreshConstant(cvPtr, value); }
    }

    public double MinMarkerPerimeterRate {
      get { return au_DetectorParameters_GetMinMarkerPerimeterRate(cvPtr); }
      set { au_DetectorParameters_SetMinMarkerPerimeterRate(cvPtr, value); }
    }

    public double MaxMarkerPerimeterRate {
      get { return au_DetectorParameters_GetMaxMarkerPerimeterRate(cvPtr); }
      set { au_DetectorParameters_SetMaxMarkerPerimeterRate(cvPtr, value); }
    }

    public double PolygonalApproxAccuracyRate {
      get { return au_DetectorParameters_GetPolygonalApproxAccuracyRate(cvPtr); }
      set { au_DetectorParameters_SetPolygonalApproxAccuracyRate(cvPtr, value); }
    }

    public double MinCornerDistanceRate {
      get { return au_DetectorParameters_GetMinCornerDistanceRate(cvPtr); }
      set { au_DetectorParameters_SetMinCornerDistanceRate(cvPtr, value); }
    }

    public int MinDistanceToBorder {
      get { return au_DetectorParameters_GetMinDistanceToBorder(cvPtr); }
      set { au_DetectorParameters_SetMinDistanceToBorder(cvPtr, value); }
    }

    public double MinMarkerDistanceRate {
      get { return au_DetectorParameters_GetMinMarkerDistanceRate(cvPtr); }
      set { au_DetectorParameters_SetMinMarkerDistanceRate(cvPtr, value); }
    }

    public bool DoCornerRefinement {
      get { return au_DetectorParameters_GetDoCornerRefinement(cvPtr); }
      set { au_DetectorParameters_SetDoCornerRefinement(cvPtr, value); }
    }

    public int CornerRefinementWinSize {
      get { return au_DetectorParameters_GetCornerRefinementWinSize(cvPtr); }
      set { au_DetectorParameters_SetCornerRefinementWinSize(cvPtr, value); }
    }

    public int CornerRefinementMaxIterations {
      get { return au_DetectorParameters_GetCornerRefinementMaxIterations(cvPtr); }
      set { au_DetectorParameters_SetCornerRefinementMaxIterations(cvPtr, value); }
    }

    public double CornerRefinementMinAccuracy {
      get { return au_DetectorParameters_GetCornerRefinementMinAccuracy(cvPtr); }
      set { au_DetectorParameters_SetCornerRefinementMinAccuracy(cvPtr, value); }
    }

    public int MarkerBorderBits {
      get { return au_DetectorParameters_GetMarkerBorderBits(cvPtr); }
      set { au_DetectorParameters_SetMarkerBorderBits(cvPtr, value); }
    }

    public int PerspectiveRemovePixelPerCell {
      get { return au_DetectorParameters_GetPerspectiveRemovePixelPerCell(cvPtr); }
      set { au_DetectorParameters_SetPerspectiveRemovePixelPerCell(cvPtr, value); }
    }

    public double PerspectiveRemoveIgnoredMarginPerCell {
      get { return au_DetectorParameters_GetPerspectiveRemoveIgnoredMarginPerCell(cvPtr); }
      set { au_DetectorParameters_SetPerspectiveRemoveIgnoredMarginPerCell(cvPtr, value); }
    }

    public double MaxErroneousBitsInBorderRate {
      get { return au_DetectorParameters_GetMaxErroneousBitsInBorderRate(cvPtr); }
      set { au_DetectorParameters_SetMaxErroneousBitsInBorderRate(cvPtr, value); }
    }

    public double MinOtsuStdDev {
      get { return au_DetectorParameters_GetMinOtsuStdDev(cvPtr); }
      set { au_DetectorParameters_SetMinOtsuStdDev(cvPtr, value); }
    }

    public double ErrorCorrectionRate {
      get { return au_DetectorParameters_GetErrorCorrectionRate(cvPtr); }
      set { au_DetectorParameters_SetErrorCorrectionRate(cvPtr, value); }
    }
  }
}