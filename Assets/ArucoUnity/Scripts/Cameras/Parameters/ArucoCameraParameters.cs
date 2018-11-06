using ArucoUnity.Plugin;
using System;
using System.IO;
using System.Xml.Serialization;

namespace ArucoUnity.Cameras.Parameters
{
    /// <summary>
    /// Manage the camera parameters from a calibration.
    /// </summary>
    [Serializable]
    public class ArucoCameraParameters
    {
        // Constructors

        /// <summary>
        /// Create an empty CameraParameters and set <see cref="CalibrationDateTime"/> to now.
        /// </summary>
        /// <remarks>This constructor is needed for the serialization.</remarks>
        public ArucoCameraParameters()
        {
        }

        /// <summary>
        /// Initialize the properties.
        /// </summary>
        /// <param name="camerasNumber">The number of camera in the camera system. Must be equal to the number of cameras of the related
        /// <see cref="ArucoCamera"/>.</param>
        public ArucoCameraParameters(int camerasNumber)
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
        }

        // Properties

        /// <summary>
        /// The calibration date and time.
        /// </summary>
        public DateTime CalibrationDateTime { get; set; }

        /// <summary>
        /// The number of the camera during the calibration.
        /// </summary>
        public int CameraNumber { get; set; }

        /// <summary>
        /// The image height during the calibration.
        /// </summary>
        public int[] ImageHeights { get; set; }

        /// <summary>
        /// The image width during the calibration.
        /// </summary>
        public int[] ImageWidths { get; set; }

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
        public Cv.Mat[] CameraMatrices { get; set; }

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
        /// Parameters from possible stereo calibration on the camera system.
        /// </summary>
        public StereoArucoCameraParameters StereoCameraParameters { get; set; }

        /// <summary>
        /// The file path of the parameters.
        /// </summary>
        [XmlIgnore]
        public string FilePath { get; protected set; }

        // Variables

        protected Cv.Mat[] cameraMatrices;

        // Methods

        /// <summary>
        /// Create a new CameraParameters object from a previously saved camera parameters XML file.
        /// </summary>
        /// <param name="cameraParametersFilePath">The file path to load.</param>
        /// <exception cref="ArgumentException">If the camera parameters couldn't be loaded because of a wrong file path.</exception>
        /// <returns>The CameraParameters loaded from the XML file or null if the file coulnd't be loaded.</returns>
        public static ArucoCameraParameters LoadFromXmlFile(string cameraParametersFilePath)
        {
            ArucoCameraParameters cameraParameters = null;

            // Try to load the file and deserialize it
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(cameraParametersFilePath);
                XmlSerializer serializer = new XmlSerializer(typeof(ArucoCameraParameters));
                cameraParameters = serializer.Deserialize(reader) as ArucoCameraParameters;
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
            cameraParameters.CameraMatrices = CreateProperty(cameraParameters.CameraMatricesType, cameraParameters.CameraMatricesValues);
            cameraParameters.DistCoeffs = CreateProperty(cameraParameters.DistCoeffsType, cameraParameters.DistCoeffsValues);
            cameraParameters.OmnidirXis = CreateProperty(cameraParameters.OmnidirXisType, cameraParameters.OmnidirXisValues);

            // Populate non-serialized properties of the stereo camera parameters
            if (cameraParameters.StereoCameraParameters != null)
            {
                cameraParameters.StereoCameraParameters.UpdateNonSerializedProperties();
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
            if (StereoCameraParameters != null)
            {
                StereoCameraParameters.UpdateSerializedProperties();
            }

            // Try to serialize the object and save it to the file
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(cameraParametersFilePath);
                XmlSerializer serializer = new XmlSerializer(typeof(ArucoCameraParameters));
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

        /// <summary>
        /// Update the <paramref name="propertyValues"/> value from <paramref name="property"/>.
        /// </summary>
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

        /// <summary>
        /// Return a property created from a <paramref name="propertyType"/> type and a array of values.
        /// </summary>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="propertyValues">The content of the propery.</param>
        /// <returns>The property</returns>
        internal static Cv.Mat[] CreateProperty(Cv.Type propertyType, double[][][] propertyValues)
        {
            int cameraNumber = propertyValues.Length;

            var property = new Cv.Mat[cameraNumber];
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