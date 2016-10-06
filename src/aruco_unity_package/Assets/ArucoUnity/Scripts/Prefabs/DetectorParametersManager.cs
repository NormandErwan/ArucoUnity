using UnityEngine;

namespace ArucoUnity
{
  namespace Examples
  {
    public partial class ArucoUnity
    {
      public class DetectorParametersManager : MonoBehaviour
      {

        public DetectorParameters detectorParameters;

        void Start()
        {
          detectorParameters = new DetectorParameters();
        }
      }
    }
  }
}