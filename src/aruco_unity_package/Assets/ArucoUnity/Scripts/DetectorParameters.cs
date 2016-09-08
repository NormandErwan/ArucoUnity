using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class DetectorParameters : HandleCvPtr
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
      auDeleteDetectorParameters(cvPtr);
    }

    public int AdaptiveThreshWinSizeMin {
      get { return auGetDetectorParametersAdaptiveThreshWinSizeMin(cvPtr); }
      set { auSetDetectorParametersAdaptiveThreshWinSizeMin(cvPtr, value); }
    }

    public int AdaptiveThreshWinSizeMax {
      get { return auGetDetectorParametersAdaptiveThreshWinSizeMax(cvPtr); }
      set { auSetDetectorParametersAdaptiveThreshWinSizeMax(cvPtr, value); }
    }

    public int AdaptiveThreshWinSizeStep {
      get { return auGetDetectorParametersAdaptiveThreshWinSizeStep(cvPtr); }
      set { auSetDetectorParametersAdaptiveThreshWinSizeStep(cvPtr, value); }
    }

    public double AdaptiveThreshConstant {
      get { return auGetDetectorParametersAdaptiveThreshConstant(cvPtr); }
      set { auSetDetectorParametersAdaptiveThreshConstant(cvPtr, value); }
    }

    public double MinMarkerPerimeterRate {
      get { return auGetDetectorParametersMinMarkerPerimeterRate(cvPtr); }
      set { auSetDetectorParametersMinMarkerPerimeterRate(cvPtr, value); }
    }

    public double MaxMarkerPerimeterRate {
      get { return auGetDetectorParametersMaxMarkerPerimeterRate(cvPtr); }
      set { auSetDetectorParametersMaxMarkerPerimeterRate(cvPtr, value); }
    }

    public double PolygonalApproxAccuracyRate {
      get { return auGetDetectorParametersPolygonalApproxAccuracyRate(cvPtr); }
      set { auSetDetectorParametersPolygonalApproxAccuracyRate(cvPtr, value); }
    }

    public double MinCornerDistanceRate {
      get { return auGetDetectorParametersMinCornerDistanceRate(cvPtr); }
      set { auSetDetectorParametersMinCornerDistanceRate(cvPtr, value); }
    }

    public int MinDistanceToBorder {
      get { return auGetDetectorParametersMinDistanceToBorder(cvPtr); }
      set { auSetDetectorParametersMinDistanceToBorder(cvPtr, value); }
    }

    public double MinMarkerDistanceRate {
      get { return auGetDetectorParametersMinMarkerDistanceRate(cvPtr); }
      set { auSetDetectorParametersMinMarkerDistanceRate(cvPtr, value); }
    }

    public bool DoCornerRefinement {
      get { return auGetDetectorParametersDoCornerRefinement(cvPtr); }
      set { auSetDetectorParametersDoCornerRefinement(cvPtr, value); }
    }

    public int CornerRefinementWinSize {
      get { return auGetDetectorParametersCornerRefinementWinSize(cvPtr); }
      set { auSetDetectorParametersCornerRefinementWinSize(cvPtr, value); }
    }

    public int CornerRefinementMaxIterations {
      get { return auGetDetectorParametersCornerRefinementMaxIterations(cvPtr); }
      set { auSetDetectorParametersCornerRefinementMaxIterations(cvPtr, value); }
    }

    public double CornerRefinementMinAccuracy {
      get { return auGetDetectorParametersCornerRefinementMinAccuracy(cvPtr); }
      set { auSetDetectorParametersCornerRefinementMinAccuracy(cvPtr, value); }
    }

    public int MarkerBorderBits {
      get { return auGetDetectorParametersMarkerBorderBits(cvPtr); }
      set { auSetDetectorParametersMarkerBorderBits(cvPtr, value); }
    }

    public int PerspectiveRemovePixelPerCell {
      get { return auGetDetectorParametersPerspectiveRemovePixelPerCell(cvPtr); }
      set { auSetDetectorParametersPerspectiveRemovePixelPerCell(cvPtr, value); }
    }

    public double PerspectiveRemoveIgnoredMarginPerCell {
      get { return auGetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cvPtr); }
      set { auSetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(cvPtr, value); }
    }

    public double MaxErroneousBitsInBorderRate {
      get { return auGetDetectorParametersMaxErroneousBitsInBorderRate(cvPtr); }
      set { auSetDetectorParametersMaxErroneousBitsInBorderRate(cvPtr, value); }
    }

    public double MinOtsuStdDev {
      get { return auGetDetectorParametersMinOtsuStdDev(cvPtr); }
      set { auSetDetectorParametersMinOtsuStdDev(cvPtr, value); }
    }

    public double ErrorCorrectionRate {
      get { return auGetDetectorParametersErrorCorrectionRate(cvPtr); }
      set { auSetDetectorParametersErrorCorrectionRate(cvPtr, value); }
    }
  }
}