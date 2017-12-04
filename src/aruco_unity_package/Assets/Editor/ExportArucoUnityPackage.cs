using UnityEditor;

public static class ExportArucoUnityPackage
{
  [MenuItem("Aruco Unity/Export package")]
  public static void ExportPackage()
  {
    string[] projectContent = new string[]
    {
      "Assets/ArucoUnity/Materials",
      "Assets/ArucoUnity/Plugins",
      "Assets/ArucoUnity/Prefabs",
      "Assets/ArucoUnity/Scenes",
      "Assets/ArucoUnity/Scripts",
      "Assets/ArucoUnity/Scenes",
      "ProjectSettings/TagManager.asset"
    };
    AssetDatabase.ExportPackage(projectContent, "ArucoUnity.unitypackage", ExportPackageOptions.Interactive | ExportPackageOptions.Recurse);
  }
}
