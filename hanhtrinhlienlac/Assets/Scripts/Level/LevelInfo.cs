using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] PolygonCollider2D _cinemachineConfiger;
    List<MarkedPoint> _markedPoints;
    void Start()
    {
        _markedPoints = new List<MarkedPoint>(GetComponentsInChildren<MarkedPoint>(true));
    }
    public MarkedPoint GetStartPoint()
    {
        return _markedPoints.Find(e => e.type == GameEnum.PointType.STARTPOINT);
    }
    public MarkedPoint GetEndPoint() {
        return _markedPoints.Find(e => e.type == GameEnum.PointType.ENDPOINT);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
