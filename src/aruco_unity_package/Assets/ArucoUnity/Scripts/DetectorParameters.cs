using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class DetectorParameters : HandleDllObject
  {
    [DllImport("ArucoUnity")]
    static extern void auDeleteDetectorParameters(System.IntPtr parameters);

    [DllImport("ArucoUnity")]
    static extern int auGetDetectorParametersAdaptiveThreshWinSizeMin(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersAdaptiveThreshWinSizeMin(System.IntPtr parameters, int adaptiveThreshWinSizeMin);

    [DllImport("ArucoUnity")]
    static extern int auGetDetectorParametersAdaptiveThreshWinSizeMax(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersAdaptiveThreshWinSizeMax(System.IntPtr parameters, int adaptiveThreshWinSizeMax);

    [DllImport("ArucoUnity")]
    static extern int auGetDetectorParametersAdaptiveThreshWinSizeStep(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersAdaptiveThreshWinSizeStep(System.IntPtr parameters, int adaptiveThreshWinSizeStep);

    [DllImport("ArucoUnity")]
    static extern double auGetDetectorParametersAdaptiveThreshConstant(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersAdaptiveThreshConstant(System.IntPtr parameters, double adaptiveThreshConstant);

    [DllImport("ArucoUnity")]
    static extern double auGetDetectorParametersMinMarkerPerimeterRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersMinMarkerPerimeterRate(System.IntPtr parameters, double minMarkerPerimeterRate);

    [DllImport("ArucoUnity")]
    static extern double auGetDetectorParametersMaxMarkerPerimeterRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersMaxMarkerPerimeterRate(System.IntPtr parameters, double maxMarkerPerimeterRate);

    [DllImport("ArucoUnity")]
    static extern double auGetDetectorParametersPolygonalApproxAccuracyRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersPolygonalApproxAccuracyRate(System.IntPtr parameters, double polygonalApproxAccuracyRate);

    [DllImport("ArucoUnity")]
    static extern double auGetDetectorParametersMinCornerDistanceRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersMinCornerDistanceRate(System.IntPtr parameters, double minCornerDistanceRate);

    [DllImport("ArucoUnity")]
    static extern int auGetDetectorParametersMinDistanceToBorder(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersMinDistanceToBorder(System.IntPtr parameters, int minDistanceToBorder);

    [DllImport("ArucoUnity")]
    static extern double auGetDetectorParametersMinMarkerDistanceRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersMinMarkerDistanceRate(System.IntPtr parameters, double minMarkerDistanceRate);

    [DllImport("ArucoUnity")] [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool auGetDetectorParametersDoCornerRefinement(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersDoCornerRefinement(System.IntPtr parameters, [MarshalAs(UnmanagedType.Bool)] bool doCornerRefinement);

    [DllImport("ArucoUnity")]
    static extern int auGetDetectorParametersCornerRefinementWinSize(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersCornerRefinementWinSize(System.IntPtr parameters, int cornerRefinementWinSize);

    [DllImport("ArucoUnity")]
    static extern int auGetDetectorParametersCornerRefinementMaxIterations(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersCornerRefinementMaxIterations(System.IntPtr parameters, int cornerRefinementMaxIterations);

    [DllImport("ArucoUnity")]
    static extern double auGetDetectorParametersCornerRefinementMinAccuracy(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersCornerRefinementMinAccuracy(System.IntPtr parameters, double cornerRefinementMinAccuracy);

    [DllImport("ArucoUnity")]
    static extern int auGetDetectorParametersMarkerBorderBits(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersMarkerBorderBits(System.IntPtr parameters, int markerBorderBits);

    [DllImport("ArucoUnity")]
    static extern int auGetDetectorParametersPerspectiveRemovePixelPerCell(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersPerspectiveRemovePixelPerCell(System.IntPtr parameters, int perspectiveRemovePixelPerCell);

    [DllImport("ArucoUnity")]
    static extern double auGetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(System.IntPtr parameters, double perspectiveRemoveIgnoredMarginPerCell);

    [DllImport("ArucoUnity")]
    static extern double auGetDetectorParametersMaxErroneousBitsInBorderRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersMaxErroneousBitsInBorderRate(System.IntPtr parameters, double maxErroneousBitsInBorderRate);

    [DllImport("ArucoUnity")]
    static extern double auGetDetectorParametersMinOtsuStdDev(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersMinOtsuStdDev(System.IntPtr parameters, double minOtsuStdDev);

    [DllImport("ArucoUnity")]
    static extern double auGetDetectorParametersErrorCorrectionRate(System.IntPtr parameters);
    [DllImport("ArucoUnity")]
    static extern void auSetDetectorParametersErrorCorrectionRate(System.IntPtr parameters, double errorCorrectionRate);

    public DetectorParameters(System.IntPtr parameters) : base(parameters)
    {
    }

    ~DetectorParameters()
    {
      auDeleteDetectorParameters(dllPtr);
    }

    public int AdaptiveThreshWinSizeMin {
      get { return auGetDetectorParametersAdaptiveThreshWinSizeMin(dllPtr); }
      set { auSetDetectorParametersAdaptiveThreshWinSizeMin(dllPtr, value); }
    }

    public int AdaptiveThreshWinSizeMax {
      get { return auGetDetectorParametersAdaptiveThreshWinSizeMax(dllPtr); }
      set { auSetDetectorParametersAdaptiveThreshWinSizeMax(dllPtr, value); }
    }

    public int AdaptiveThreshWinSizeStep {
      get { return auGetDetectorParametersAdaptiveThreshWinSizeStep(dllPtr); }
      set { auSetDetectorParametersAdaptiveThreshWinSizeStep(dllPtr, value); }
    }

    public double AdaptiveThreshConstant {
      get { return auGetDetectorParametersAdaptiveThreshConstant(dllPtr); }
      set { auSetDetectorParametersAdaptiveThreshConstant(dllPtr, value); }
    }

    public double MinMarkerPerimeterRate {
      get { return auGetDetectorParametersMinMarkerPerimeterRate(dllPtr); }
      set { auSetDetectorParametersMinMarkerPerimeterRate(dllPtr, value); }
    }

    public double MaxMarkerPerimeterRate {
      get { return auGetDetectorParametersMaxMarkerPerimeterRate(dllPtr); }
      set { auSetDetectorParametersMaxMarkerPerimeterRate(dllPtr, value); }
    }

    public double PolygonalApproxAccuracyRate {
      get { return auGetDetectorParametersPolygonalApproxAccuracyRate(dllPtr); }
      set { auSetDetectorParametersPolygonalApproxAccuracyRate(dllPtr, value); }
    }

    public double MinCornerDistanceRate {
      get { return auGetDetectorParametersMinCornerDistanceRate(dllPtr); }
      set { auSetDetectorParametersMinCornerDistanceRate(dllPtr, value); }
    }

    public int MinDistanceToBorder {
      get { return auGetDetectorParametersMinDistanceToBorder(dllPtr); }
      set { auSetDetectorParametersMinDistanceToBorder(dllPtr, value); }
    }

    public double MinMarkerDistanceRate {
      get { return auGetDetectorParametersMinMarkerDistanceRate(dllPtr); }
      set { auSetDetectorParametersMinMarkerDistanceRate(dllPtr, value); }
    }

    public bool DoCornerRefinement {
      get { return auGetDetectorParametersDoCornerRefinement(dllPtr); }
      set { auSetDetectorParametersDoCornerRefinement(dllPtr, value); }
    }

    public int CornerRefinementWinSize {
      get { return auGetDetectorParametersCornerRefinementWinSize(dllPtr); }
      set { auSetDetectorParametersCornerRefinementWinSize(dllPtr, value); }
    }

    public int CornerRefinementMaxIterations {
      get { return auGetDetectorParametersCornerRefinementMaxIterations(dllPtr); }
      set { auSetDetectorParametersCornerRefinementMaxIterations(dllPtr, value); }
    }

    public double CornerRefinementMinAccuracy {
      get { return auGetDetectorParametersCornerRefinementMinAccuracy(dllPtr); }
      set { auSetDetectorParametersCornerRefinementMinAccuracy(dllPtr, value); }
    }

    public int MarkerBorderBits {
      get { return auGetDetectorParametersMarkerBorderBits(dllPtr); }
      set { auSetDetectorParametersMarkerBorderBits(dllPtr, value); }
    }

    public int PerspectiveRemovePixelPerCell {
      get { return auGetDetectorParametersPerspectiveRemovePixelPerCell(dllPtr); }
      set { auSetDetectorParametersPerspectiveRemovePixelPerCell(dllPtr, value); }
    }

    public double PerspectiveRemoveIgnoredMarginPerCell {
      get { return auGetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(dllPtr); }
      set { auSetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(dllPtr, value); }
    }

    public double MaxErroneousBitsInBorderRate {
      get { return auGetDetectorParametersMaxErroneousBitsInBorderRate(dllPtr); }
      set { auSetDetectorParametersMaxErroneousBitsInBorderRate(dllPtr, value); }
    }

    public double MinOtsuStdDev {
      get { return auGetDetectorParametersMinOtsuStdDev(dllPtr); }
      set { auSetDetectorParametersMinOtsuStdDev(dllPtr, value); }
    }

    public double ErrorCorrectionRate {
      get { return auGetDetectorParametersErrorCorrectionRate(dllPtr); }
      set { auSetDetectorParametersErrorCorrectionRate(dllPtr, value); }
    }
  }
}