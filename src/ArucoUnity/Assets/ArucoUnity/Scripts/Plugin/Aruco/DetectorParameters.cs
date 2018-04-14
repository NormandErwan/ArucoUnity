using System.Runtime.InteropServices;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Plugin
  {
    public static partial class Aruco
    {
      public class DetectorParameters : Utility.HandleCppPtr
      {
        // Native functions

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

        [DllImport("ArucoUnity")]
        static extern int au_DetectorParameters_getCornerRefinementMethod(System.IntPtr parameters);
        [DllImport("ArucoUnity")]
        static extern void au_DetectorParameters_setCornerRefinementMethod(System.IntPtr parameters, int cornerRefinementMethod);

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

  /// \} aruco_unity_package
}