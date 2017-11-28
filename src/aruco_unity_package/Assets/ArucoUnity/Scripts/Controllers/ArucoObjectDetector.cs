using ArucoUnity.Plugin;
using System;
using UnityEngine;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Controllers
  {
    /// <summary>
    /// Detects ArUco objects for a <see cref="ArucoCamera"/> camera system according to <see cref="DetectorParameters"/>.
    /// </summary>
    public abstract class ArucoObjectDetector : ArucoCameraController, IHasDetectorParameter
    {
      // Editor fields

      [SerializeField]
      [Tooltip("The parameters to use for the marker detection.")]
      private DetectorParametersController detectorParametersController;

      // Properties

      /// <summary>
      /// Gets or sets the parameters to use for the detection.
      /// </summary>
      public Aruco.DetectorParameters DetectorParameters { get; set; }

      // ArucoCameraController methods

      public override void Configure()
      {
        base.Configure();

        DetectorParameters = detectorParametersController.DetectorParameters;

        if (DetectorParameters == null)
        {
          throw new ArgumentNullException("DetectorParameters", "This property needs to be set for the configuration.");
        }
      }
    }
  }

  /// \} aruco_unity_package
}