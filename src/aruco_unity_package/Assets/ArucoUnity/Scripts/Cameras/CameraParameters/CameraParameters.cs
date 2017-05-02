using ArucoUnity.Plugin;
using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;

namespace ArucoUnity
{
  /// \addtogroup aruco_unity_package
  /// \{

  namespace Cameras
  {
    /// <summary>
    /// Manage OpenCV's camera parameters from a calibration.
    /// 
    /// See the OpenCV documentation for more information about the calibration: http://docs.opencv.org/3.1.0/da/d13/tutorial_aruco_calibration.html
    /// </summary>
    [Serializable]
    public class CameraParameters
    {
      // Constructors

      /// <summary>
      /// Create an empty CameraParameters and set <see cref="CalibrationDateTime"/> to now.
      /// </summary>
      /// <remarks>This constructor is needed for the serialization.</remarks>
      public CameraParameters()
      {
      }

      public CameraParameters(int camerasNumber)
      {
        CalibrationDateTime = DateTime.Now;
        CameraNumber = camerasNumber;

        ImageHeights = new int[CameraNumber];
        ImageWidths = new int[CameraNumber];
        ReprojectionErrors = new double[CameraNumber];
        CameraMatrices = new Cv.Mat[CameraNumber];
        CameraMatricesValues = new double[CameraNumber][][];
        DistCoeffs = new Cv.Mat[CameraNumber];
        DistCoeffsValues = new double[CameraNumber][][];
        OmnidirXis = new Cv.Mat[CameraNumber];
        OmnidirXisValues = new double[CameraNumber][][];
        CameraFocalLengths = new Vector2[CameraNumber];
        CameraOpticalCenters = new Vector2[CameraNumber];
        OpticalCenters = new Vector3[CameraNumber];
      }

      // Properties

      /// <summary>
      /// The calibration date and time.
      /// </summary>
      public DateTime CalibrationDateTime { get; set; }

      /// <summary>
      /// The number of the camera during the calibration.
      /// </summary>
      public int CameraNumber { get; protected set; }

      /// <summary>
      /// The image height during the calibration.
      /// </summary>
      public int[] ImageHeights
      {
        get { return imageHeights; }
        set
        {
          imageHeights = value;
          UpdateCameraMatrixDerivedVariables();
        }
      }

      /// <summary>
      /// The image width during the calibration.
      /// </summary>
      public int[] ImageWidths
      {
        get { return imageWidths; }
        set
        {
          imageWidths = value;
          UpdateCameraMatrixDerivedVariables();
        }
      }

      /// <summary>
      /// The calibration flags used.
      /// </summary>
      public int CalibrationFlagsValue { get; set; }

      /// <summary>
      /// Non null if there is a fix image aspect ratio.
      /// </summary>
      public float FixAspectRatioValue { get; set; }

      /// <summary>
      /// The average re-projection error of the calibration.
      /// </summary>
      public double[] ReprojectionErrors { get; set; }

      /// <summary>
      /// The camera matrices of the calibration.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="CameraMatricesType"/> and 
      /// <see cref="CameraMatricesValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Mat[] CameraMatrices
      {
        get { return cameraMatrices; }
        set
        {
          cameraMatrices = value;
          UpdateCameraMatrixDerivedVariables();
        }
      }

      /// <summary>
      /// The camera matrix type of the calibration. Equals to <see cref="CameraMatrices.Type()"/> and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public Cv.Type CameraMatricesType { get; set; }

      /// <summary>
      /// The camera matrix values of the calibration. Equals to the <see cref="CameraMatrices"/> content and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public double[][][] CameraMatricesValues { get; set; }

      /// <summary>
      /// The distorsition coefficients of the calibration.
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="DistCoeffsType"/> and 
      /// <see cref="DistCoeffsValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Mat[] DistCoeffs { get; set; }

      /// <summary>
      /// The distorsition coefficients type of the calibration. Equals to <see cref="DistCoeffs.Type()"/> and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public Cv.Type DistCoeffsType { get; set; }

      /// <summary>
      /// The distorsition coefficients values of the calibration. Equals to the <see cref="DistCoeffs"/> content and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public double[][][] DistCoeffsValues { get; set; }

      /// <summary>
      /// The xi parameter used in the omnidir calibration process (ccalib module).
      /// </summary>
      /// <remarks>When <see cref="SaveToXmlFile(string)"/> is called, it's serialized with the <see cref="OmnidirXisType"/> and 
      /// <see cref="OmnidirXisValues"/> properties.</remarks>
      [XmlIgnore]
      public Cv.Mat[] OmnidirXis { get; set; }

      /// <summary>
      /// The xi parameter type of the calibration. Equals to <see cref="OmnidirXis.Type()"/> and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public Cv.Type OmnidirXisType { get; set; }

      /// <summary>
      /// The xi parameter values of the calibration. Equals to the <see cref="OmnidirXis"/> content and automatically written when 
      /// <see cref="SaveToXmlFile(string)"/> is called.
      /// </summary>
      /// <remarks>This property is be public for the serialization.</remarks>
      public double[][][] OmnidirXisValues { get; set; }

      /// <summary>
      /// Parameters from stereo calibration on the camera system.
      /// </summary>
      public StereoCameraParameters[] StereoCameraParametersList { get; set; }

      /// <summary>
      /// The camera focal length expressed in pixels coordinates. Equals to <see cref="CameraMatrices.AtDouble(0, 0)"/> on the x-axis
      /// and to to <see cref="CameraMatrices.AtDouble(1, 1)"/> on the y-axis.
      /// </summary>
      [XmlIgnore]
      public Vector2[] CameraFocalLengths { get; protected set; }

      /// <summary>
      /// The camera optical center expressed in pixels coordinates. Equals to <see cref="CameraMatrices.AtDouble(0, 2)"/> on the x-axis
      /// and to <see cref="CameraMatrices.AtDouble(1, 2)"/> on the y-axis.
      /// </summary>
      [XmlIgnore]
      public Vector2[] CameraOpticalCenters { get; protected set; }

      /// <summary>
      /// The camera optical center in the Unity world space.
      /// </summary>
      [XmlIgnore]
      public Vector3[] OpticalCenters { get; protected set; }

      /// <summary>
      /// The file path of the parameters.
      /// </summary>
      [XmlIgnore]
      public string FilePath { get; protected set; }

      // Variables

      protected int[] imageHeights, imageWidths;
      protected Cv.Mat[] cameraMatrices;

      // Methods

      /// <summary>
      /// Create a new CameraParameters object from a previously saved camera parameters XML file.
      /// </summary>
      /// <param name="cameraParametersFilePath">The file path to load.</param>
      /// <exception cref="ArgumentException">If the camera parameters couldn't be loaded because of a wrong file path.</exception>
      /// <returns>The CameraParameters loaded from the XML file or null if the file coulnd't be loaded.</returns>
      public static CameraParameters LoadFromXmlFile(string cameraParametersFilePath)
      {
        CameraParameters cameraParameters = null;

        // Try to load the file and deserialize it
        StreamReader reader = null;
        try
        {
          reader = new StreamReader(cameraParametersFilePath);
          XmlSerializer serializer = new XmlSerializer(typeof(CameraParameters));
          cameraParameters = serializer.Deserialize(reader) as CameraParameters;
        }
        catch (Exception ex)
        {
          throw new ArgumentException("Couldn't load the camera parameters file path '" + cameraParametersFilePath + ". ",
            "cameraParametersFilePath", ex);
        }
        finally
        {
          if (reader != null)
          {
            reader.Close();
          }
        }
        cameraParameters.FilePath = cameraParametersFilePath;

        // Populate non-serialized properties
        cameraParameters.CameraFocalLengths = new Vector2[cameraParameters.CameraNumber];
        cameraParameters.CameraOpticalCenters = new Vector2[cameraParameters.CameraNumber];
        cameraParameters.OpticalCenters = new Vector3[cameraParameters.CameraNumber];
        cameraParameters.CameraMatrices = CreateProperty(cameraParameters.CameraMatricesType, cameraParameters.CameraMatricesValues);
        cameraParameters.DistCoeffs = CreateProperty(cameraParameters.DistCoeffsType, cameraParameters.DistCoeffsValues);
        cameraParameters.OmnidirXis = CreateProperty(cameraParameters.OmnidirXisType, cameraParameters.OmnidirXisValues);
        cameraParameters.UpdateCameraMatrixDerivedVariables();

        // Populate non-serialized properties of the stereo camera parameters
        foreach (var currentStereoCameraParameters in cameraParameters.StereoCameraParametersList)
        {
          currentStereoCameraParameters.UpdateNonSerializedProperties();
        }

        return cameraParameters;
      }

      /// <summary>
      /// Save the camera parameters to a XML file.
      /// </summary>
      /// <param name="cameraParametersFilePath">The file path where to save the object.</param>
      /// <exception cref="ArgumentException">If the camera parameters couldn't be saved because of a wrong file path.</exception>
      public void SaveToXmlFile(string cameraParametersFilePath)
      {
        // Update CameraMatrixValues and CameraMatrixType
        CameraMatricesType = CameraMatrices[0].Type();
        UpdatePropertyValues(CameraMatrices, CameraMatricesValues);

        // Update DistCoeffsValues and DistCoeffsType
        DistCoeffsType = DistCoeffs[0].Type();
        UpdatePropertyValues(DistCoeffs, DistCoeffsValues);

        // Update OmnidirXisValues and OmnidirXisType
        OmnidirXisType = OmnidirXis[0].Type();
        UpdatePropertyValues(OmnidirXis, OmnidirXisValues);

        // Update properties for serialization of the stereo camera parameters
        foreach (var currentStereoCameraParameters in StereoCameraParametersList)
        {
          currentStereoCameraParameters.UpdateSerializedProperties();
        }

        // Try to serialize the object and save it to the file
        StreamWriter writer = null;
        try
        {
          writer = new StreamWriter(cameraParametersFilePath);
          XmlSerializer serializer = new XmlSerializer(typeof(CameraParameters));
          serializer.Serialize(writer, this);
        }
        catch
        {
          throw new ArgumentException("Couldn't save the camera parameters to the file path '" + cameraParametersFilePath + ".", "cameraParametersFilePath");
        }
        finally
        {
          if (writer != null)
          {
            writer.Close();
          }
        }
      }

      protected void UpdateCameraMatrixDerivedVariables()
      {
        for (int cameraId = 0; cameraId < CameraNumber; cameraId++)
        {
          if (ImageWidths == null || ImageWidths[cameraId] == 0 || ImageHeights == null || ImageHeights[cameraId] == 0 || CameraMatrices == null
            || CameraMatrices[cameraId] == null || CameraMatrices[cameraId].Size.Width != 3 || CameraMatrices[cameraId].Size.Height != 3)
          {
            return;
          }

          // Camera parameter's focal lenghts and optical centers
          CameraFocalLengths[cameraId] = new Vector2((float)CameraMatrices[cameraId].AtDouble(0, 0), (float)CameraMatrices[cameraId].AtDouble(1, 1));
          CameraOpticalCenters[cameraId] = new Vector2((float)CameraMatrices[cameraId].AtDouble(0, 2), (float)CameraMatrices[cameraId].AtDouble(1, 2));

          // Optical center in the Unity world space; based on: http://stackoverflow.com/a/36580522
          // TODO: take account of FixAspectRatio
          OpticalCenters[cameraId] = new Vector3(CameraOpticalCenters[cameraId].x / ImageWidths[cameraId], CameraOpticalCenters[cameraId].y / ImageHeights[cameraId], CameraFocalLengths[cameraId].y);
        }
      }

      internal static void UpdatePropertyValues(Cv.Mat[] property, double[][][] propertyValues)
      {
        int cameraNumber = property.Length,
            rows = property[0].Rows,
            cols = property[0].Cols;

        for (int cameraId = 0; cameraId < cameraNumber; cameraId++)
        {
          propertyValues[cameraId] = new double[rows][];
          for (int i = 0; i < rows; i++)
          {
            propertyValues[cameraId][i] = new double[cols];
            for (int j = 0; j < cols; j++)
            {
              propertyValues[cameraId][i][j] = property[cameraId].AtDouble(i, j);
            }
          }
        }
      }

      internal static Cv.Mat[] CreateProperty(Cv.Type propertyType, double[][][] propertyValues)
      {
        int cameraNumber = propertyValues.Length;

        Cv.Mat[] property = new Cv.Mat[cameraNumber];
        for (int cameraId = 0; cameraId < cameraNumber; cameraId++)
        {
          int rows = propertyValues[cameraId].Length,
              cols = (rows > 0) ? propertyValues[cameraId][0].Length : 0;
          property[cameraId] = new Cv.Mat(rows, cols, propertyType);
          for (int i = 0; i < rows; i++)
          {
            for (int j = 0; j < cols; j++)
            {
              property[cameraId].AtDouble(i, j, propertyValues[cameraId][i][j]);
            }
          }
        }

        return property;
      }
    }
  }

  /// \} aruco_unity_package
}