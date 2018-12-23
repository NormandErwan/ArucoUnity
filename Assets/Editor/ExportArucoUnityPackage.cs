using UnityEditor;

/// <summary>
/// Exports ArucoUnity as a Unity package.
/// </summary>
public static class ExportArucoUnityPackage
{
    /// <summary>
    /// Returns the assets list of ArucoUnity.
    /// </summary>
    private static string[] assets = new string[]
    {
        "Assets/ArucoUnity",
        "Assets/StreamingAssets/ArucoUnity",
        "Assets/csc.rsp",
        "Assets/gmcs.rsp",
        "Assets/mcs.rsp",
        "Assets/smcs.rsp",
        "ProjectSettings/TagManager.asset",
    };

    /// <summary>
    /// Exports the ArucoUnity package.
    /// </summary>
    [MenuItem("ArucoUnity/Export package")]
    public static void ExportPackage()
    {
        AssetDatabase.ExportPackage(assets, "ArucoUnity.unitypackage", ExportPackageOptions.Interactive | ExportPackageOptions.Recurse);
    }
}
