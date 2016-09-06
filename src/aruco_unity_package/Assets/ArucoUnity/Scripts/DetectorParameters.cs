using UnityEngine;
using System.Runtime.InteropServices;

public partial class ArucoUnity
{
  public partial class DetectorParameters
  {
    [DllImport("ArucoUnity")]
    static extern void auDestroyDetectorParameters(System.IntPtr parameters);

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

    HandleRef _handle;

    public DetectorParameters(System.IntPtr parameters)
    {
      _handle = new HandleRef(this, parameters);
    }

    ~DetectorParameters()
    {
      auDestroyDetectorParameters(ptr);
    }

    public System.IntPtr ptr
    {
      get { return _handle.Handle; }
    }

    public int AdaptiveThreshWinSizeMin {
      get { return auGetDetectorParametersAdaptiveThreshWinSizeMin(ptr); }
      set { auSetDetectorParametersAdaptiveThreshWinSizeMin(ptr, value); }
    }

    public int AdaptiveThreshWinSizeMax {
      get { return auGetDetectorParametersAdaptiveThreshWinSizeMax(ptr); }
      set { auSetDetectorParametersAdaptiveThreshWinSizeMax(ptr, value); }
    }

    public int AdaptiveThreshWinSizeStep {
      get { return auGetDetectorParametersAdaptiveThreshWinSizeStep(ptr); }
      set { auSetDetectorParametersAdaptiveThreshWinSizeStep(ptr, value); }
    }

    public double AdaptiveThreshConstant {
      get { return auGetDetectorParametersAdaptiveThreshConstant(ptr); }
      set { auSetDetectorParametersAdaptiveThreshConstant(ptr, value); }
    }

    public double MinMarkerPerimeterRate {
      get { return auGetDetectorParametersMinMarkerPerimeterRate(ptr); }
      set { auSetDetectorParametersMinMarkerPerimeterRate(ptr, value); }
    }

    public double MaxMarkerPerimeterRate {
      get { return auGetDetectorParametersMaxMarkerPerimeterRate(ptr); }
      set { auSetDetectorParametersMaxMarkerPerimeterRate(ptr, value); }
    }

    public double PolygonalApproxAccuracyRate {
      get { return auGetDetectorParametersPolygonalApproxAccuracyRate(ptr); }
      set { auSetDetectorParametersPolygonalApproxAccuracyRate(ptr, value); }
    }

    public double MinCornerDistanceRate {
      get { return auGetDetectorParametersMinCornerDistanceRate(ptr); }
      set { auSetDetectorParametersMinCornerDistanceRate(ptr, value); }
    }

    public int MinDistanceToBorder {
      get { return auGetDetectorParametersMinDistanceToBorder(ptr); }
      set { auSetDetectorParametersMinDistanceToBorder(ptr, value); }
    }

    public double MinMarkerDistanceRate {
      get { return auGetDetectorParametersMinMarkerDistanceRate(ptr); }
      set { auSetDetectorParametersMinMarkerDistanceRate(ptr, value); }
    }

    public bool DoCornerRefinement {
      get { return auGetDetectorParametersDoCornerRefinement(ptr); }
      set { auSetDetectorParametersDoCornerRefinement(ptr, value); }
    }

    public int CornerRefinementWinSize {
      get { return auGetDetectorParametersCornerRefinementWinSize(ptr); }
      set { auSetDetectorParametersCornerRefinementWinSize(ptr, value); }
    }

    public int CornerRefinementMaxIterations {
      get { return auGetDetectorParametersCornerRefinementMaxIterations(ptr); }
      set { auSetDetectorParametersCornerRefinementMaxIterations(ptr, value); }
    }

    public double CornerRefinementMinAccuracy {
      get { return auGetDetectorParametersCornerRefinementMinAccuracy(ptr); }
      set { auSetDetectorParametersCornerRefinementMinAccuracy(ptr, value); }
    }

    public int MarkerBorderBits {
      get { return auGetDetectorParametersMarkerBorderBits(ptr); }
      set { auSetDetectorParametersMarkerBorderBits(ptr, value); }
    }

    public int PerspectiveRemovePixelPerCell {
      get { return auGetDetectorParametersPerspectiveRemovePixelPerCell(ptr); }
      set { auSetDetectorParametersPerspectiveRemovePixelPerCell(ptr, value); }
    }

    public double PerspectiveRemoveIgnoredMarginPerCell {
      get { return auGetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(ptr); }
      set { auSetDetectorParametersPerspectiveRemoveIgnoredMarginPerCell(ptr, value); }
    }

    public double MaxErroneousBitsInBorderRate {
      get { return auGetDetectorParametersMaxErroneousBitsInBorderRate(ptr); }
      set { auSetDetectorParametersMaxErroneousBitsInBorderRate(ptr, value); }
    }

    public double MinOtsuStdDev {
      get { return auGetDetectorParametersMinOtsuStdDev(ptr); }
      set { auSetDetectorParametersMinOtsuStdDev(ptr, value); }
    }

    public double ErrorCorrectionRate {
      get { return auGetDetectorParametersErrorCorrectionRate(ptr); }
      set { auSetDetectorParametersErrorCorrectionRate(ptr, value); }
    }
  }
}