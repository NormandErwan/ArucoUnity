using System;
using System.Runtime.InteropServices;

namespace ArucoUnity.Plugin
{
  public static partial class Aruco
  {
    public class DetectorParameters : HandleCppPtr
    {
      // Native functions

      [DllImport("ArucoUnityPlugin")]
      static extern IntPtr au_DetectorParameters_create();

      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_delete(IntPtr parameters);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_DetectorParameters_getAdaptiveThreshWinSizeMin(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setAdaptiveThreshWinSizeMin(IntPtr parameters, int adaptiveThreshWinSizeMin);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_DetectorParameters_getAdaptiveThreshWinSizeMax(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setAdaptiveThreshWinSizeMax(IntPtr parameters, int adaptiveThreshWinSizeMax);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_DetectorParameters_getAdaptiveThreshWinSizeStep(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setAdaptiveThreshWinSizeStep(IntPtr parameters, int adaptiveThreshWinSizeStep);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_DetectorParameters_getAdaptiveThreshConstant(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setAdaptiveThreshConstant(IntPtr parameters, double adaptiveThreshConstant);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_DetectorParameters_getMinMarkerPerimeterRate(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setMinMarkerPerimeterRate(IntPtr parameters, double minMarkerPerimeterRate);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_DetectorParameters_getMaxMarkerPerimeterRate(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setMaxMarkerPerimeterRate(IntPtr parameters, double maxMarkerPerimeterRate);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_DetectorParameters_getPolygonalApproxAccuracyRate(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setPolygonalApproxAccuracyRate(IntPtr parameters, double polygonalApproxAccuracyRate);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_DetectorParameters_getMinCornerDistanceRate(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setMinCornerDistanceRate(IntPtr parameters, double minCornerDistanceRate);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_DetectorParameters_getMinDistanceToBorder(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setMinDistanceToBorder(IntPtr parameters, int minDistanceToBorder);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_DetectorParameters_getMinMarkerDistanceRate(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setMinMarkerDistanceRate(IntPtr parameters, double minMarkerDistanceRate);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_DetectorParameters_getCornerRefinementMethod(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setCornerRefinementMethod(IntPtr parameters, int cornerRefinementMethod);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_DetectorParameters_getCornerRefinementWinSize(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setCornerRefinementWinSize(IntPtr parameters, int cornerRefinementWinSize);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_DetectorParameters_getCornerRefinementMaxIterations(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setCornerRefinementMaxIterations(IntPtr parameters, int cornerRefinementMaxIterations);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_DetectorParameters_getCornerRefinementMinAccuracy(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setCornerRefinementMinAccuracy(IntPtr parameters, double cornerRefinementMinAccuracy);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_DetectorParameters_getMarkerBorderBits(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setMarkerBorderBits(IntPtr parameters, int markerBorderBits);

      [DllImport("ArucoUnityPlugin")]
      static extern int au_DetectorParameters_getPerspectiveRemovePixelPerCell(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setPerspectiveRemovePixelPerCell(IntPtr parameters, int perspectiveRemovePixelPerCell);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_DetectorParameters_getPerspectiveRemoveIgnoredMarginPerCell(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setPerspectiveRemoveIgnoredMarginPerCell(IntPtr parameters, double perspectiveRemoveIgnoredMarginPerCell);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_DetectorParameters_getMaxErroneousBitsInBorderRate(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setMaxErroneousBitsInBorderRate(IntPtr parameters, double maxErroneousBitsInBorderRate);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_DetectorParameters_getMinOtsuStdDev(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setMinOtsuStdDev(IntPtr parameters, double minOtsuStdDev);

      [DllImport("ArucoUnityPlugin")]
      static extern double au_DetectorParameters_getErrorCorrectionRate(IntPtr parameters);
      [DllImport("ArucoUnityPlugin")]
      static extern void au_DetectorParameters_setErrorCorrectionRate(IntPtr parameters, double errorCorrectionRate);

      public DetectorParameters() : base(au_DetectorParameters_create())
      {
      }

      protected override void DeleteCppPtr()
      {
        au_DetectorParameters_delete(CppPtr);
      }

      public int AdaptiveThreshWinSizeMin
      {
        get { return au_DetectorParameters_getAdaptiveThreshWinSizeMin(CppPtr); }
        set { au_DetectorParameters_setAdaptiveThreshWinSizeMin(CppPtr, value); }
      }

      public int AdaptiveThreshWinSizeMax
      {
        get { return au_DetectorParameters_getAdaptiveThreshWinSizeMax(CppPtr); }
        set { au_DetectorParameters_setAdaptiveThreshWinSizeMax(CppPtr, value); }
      }

      public int AdaptiveThreshWinSizeStep
      {
        get { return au_DetectorParameters_getAdaptiveThreshWinSizeStep(CppPtr); }
        set { au_DetectorParameters_setAdaptiveThreshWinSizeStep(CppPtr, value); }
      }

      public double AdaptiveThreshConstant
      {
        get { return au_DetectorParameters_getAdaptiveThreshConstant(CppPtr); }
        set { au_DetectorParameters_setAdaptiveThreshConstant(CppPtr, value); }
      }

      public double MinMarkerPerimeterRate
      {
        get { return au_DetectorParameters_getMinMarkerPerimeterRate(CppPtr); }
        set { au_DetectorParameters_setMinMarkerPerimeterRate(CppPtr, value); }
      }

      public double MaxMarkerPerimeterRate
      {
        get { return au_DetectorParameters_getMaxMarkerPerimeterRate(CppPtr); }
        set { au_DetectorParameters_setMaxMarkerPerimeterRate(CppPtr, value); }
      }

      public double PolygonalApproxAccuracyRate
      {
        get { return au_DetectorParameters_getPolygonalApproxAccuracyRate(CppPtr); }
        set { au_DetectorParameters_setPolygonalApproxAccuracyRate(CppPtr, value); }
      }

      public double MinCornerDistanceRate
      {
        get { return au_DetectorParameters_getMinCornerDistanceRate(CppPtr); }
        set { au_DetectorParameters_setMinCornerDistanceRate(CppPtr, value); }
      }

      public int MinDistanceToBorder
      {
        get { return au_DetectorParameters_getMinDistanceToBorder(CppPtr); }
        set { au_DetectorParameters_setMinDistanceToBorder(CppPtr, value); }
      }

      public double MinMarkerDistanceRate
      {
        get { return au_DetectorParameters_getMinMarkerDistanceRate(CppPtr); }
        set { au_DetectorParameters_setMinMarkerDistanceRate(CppPtr, value); }
      }

      public CornerRefineMethod CornerRefinementMethod
      {
        get { return (CornerRefineMethod)au_DetectorParameters_getCornerRefinementMethod(CppPtr); }
        set { au_DetectorParameters_setCornerRefinementMethod(CppPtr, (int)value); }
      }

      public int CornerRefinementWinSize
      {
        get { return au_DetectorParameters_getCornerRefinementWinSize(CppPtr); }
        set { au_DetectorParameters_setCornerRefinementWinSize(CppPtr, value); }
      }

      public int CornerRefinementMaxIterations
      {
        get { return au_DetectorParameters_getCornerRefinementMaxIterations(CppPtr); }
        set { au_DetectorParameters_setCornerRefinementMaxIterations(CppPtr, value); }
      }

      public double CornerRefinementMinAccuracy
      {
        get { return au_DetectorParameters_getCornerRefinementMinAccuracy(CppPtr); }
        set { au_DetectorParameters_setCornerRefinementMinAccuracy(CppPtr, value); }
      }

      public int MarkerBorderBits
      {
        get { return au_DetectorParameters_getMarkerBorderBits(CppPtr); }
        set { au_DetectorParameters_setMarkerBorderBits(CppPtr, value); }
      }

      public int PerspectiveRemovePixelPerCell
      {
        get { return au_DetectorParameters_getPerspectiveRemovePixelPerCell(CppPtr); }
        set { au_DetectorParameters_setPerspectiveRemovePixelPerCell(CppPtr, value); }
      }

      public double PerspectiveRemoveIgnoredMarginPerCell
      {
        get { return au_DetectorParameters_getPerspectiveRemoveIgnoredMarginPerCell(CppPtr); }
        set { au_DetectorParameters_setPerspectiveRemoveIgnoredMarginPerCell(CppPtr, value); }
      }

      public double MaxErroneousBitsInBorderRate
      {
        get { return au_DetectorParameters_getMaxErroneousBitsInBorderRate(CppPtr); }
        set { au_DetectorParameters_setMaxErroneousBitsInBorderRate(CppPtr, value); }
      }

      public double MinOtsuStdDev
      {
        get { return au_DetectorParameters_getMinOtsuStdDev(CppPtr); }
        set { au_DetectorParameters_setMinOtsuStdDev(CppPtr, value); }
      }

      public double ErrorCorrectionRate
      {
        get { return au_DetectorParameters_getErrorCorrectionRate(CppPtr); }
        set { au_DetectorParameters_setErrorCorrectionRate(CppPtr, value); }
      }
    }
  }
}