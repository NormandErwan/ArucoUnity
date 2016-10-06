using UnityEngine;

public partial class ArucoUnity
{
  public class Marker : MonoBehaviour
  {
    public ArucoUnity.Mat marker;

    [Header("Marker configuration")]
    [SerializeField]
    private ArucoUnity.PREDEFINED_DICTIONARY_NAME dictionaryName;

    [SerializeField]
    private int markerId;

    [SerializeField]
    private int markerSize;

    [SerializeField]
    private int markerBorderBits;

    [Header("Draw the marker")]
    [SerializeField]
    private bool drawMarker;

    [SerializeField]
    private GameObject markerPlane;

    void Start()
    {
      marker = Create(dictionaryName, markerId, markerSize, markerBorderBits);

      if (drawMarker)
      {
        Texture2D markerTexture = CreateTexture(marker);
        Draw(marker, markerPlane, markerTexture);
      }
    }

    public ArucoUnity.Mat Create(ArucoUnity.PREDEFINED_DICTIONARY_NAME dictionaryName, int markerId, int markerSize, int markerBorderBits)
    {
      ArucoUnity.Dictionary dictionary = ArucoUnity.GetPredefinedDictionary(dictionaryName);
      ArucoUnity.Mat marker = new ArucoUnity.Mat();

      dictionary.DrawMarker(markerId, markerSize, ref marker, markerBorderBits);

      return marker;
    }

    public Texture2D CreateTexture(ArucoUnity.Mat marker)
    {
      return new Texture2D(marker.cols, marker.rows, TextureFormat.RGB24, false);
    }

    public void Draw(ArucoUnity.Mat marker, GameObject markerPlane, Texture2D markerTexture)
    {
      int markerDataSize = (int)(marker.ElemSize() * marker.Total());
      markerTexture.LoadRawTextureData(marker.data, markerDataSize);
      markerTexture.Apply();

      markerPlane.GetComponent<Renderer>().material.mainTexture = markerTexture;
    }
  }
}