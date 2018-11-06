namespace ArucoUnity.Cameras.Parameters
{
    /// <summary>
    /// Contains a <see cref="Parameters.ArucoCameraParameters"/>.
    /// </summary>
    public interface IHasArucoCameraParameters
    {
        // Properties

        /// <summary>
        /// Gets or sets the <see cref="IArucoCamera"/> camera parameters.
        /// </summary>
        ArucoCameraParameters CameraParameters { get; set; }
    }
}