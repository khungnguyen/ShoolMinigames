using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<MapItemData> _mapData = new();

    private List<MarkedPoint> _markedPoints = new();
    private List<Parallax> _parallaxLayers = new();
    // public void Loadmap
    public void LoadMap(GameEnum.MapLevel level)
    {
        var data = _mapData.Find(e => e.level == level);
        if (data.mapdata != null)
        {
            GameObject map = Instantiate(data.mapdata, transform);
            _markedPoints = new List<MarkedPoint>(map.GetComponentsInChildren<MarkedPoint>(true));
            _parallaxLayers = new List<Parallax>(map.GetComponentsInChildren<Parallax>(true));
        }

    }
    public MarkedPoint GetStartPoint()
    {
        return _markedPoints.Find(e => e.type == GameEnum.PointType.STARTPOINT);
    }
    public void SetCameraForParallax(Transform cam)
    {
        _parallaxLayers.ForEach(e => e.setCameraTransform(cam));
    }
}
[System.Serializable]
public struct MapItemData
{
    public GameObject mapdata;
    public GameEnum.MapLevel level;
}