using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] PolygonCollider2D _cinemachineConfiger;
    [SerializeField] GameEnum.LevelType _levelType;
    [SerializeField] Transform _playerLayer;
    List<MarkedPoint> _markedPoints;

    

    private Bounds _levelBound;
    private void Awake()
    {
        _markedPoints = new List<MarkedPoint>(GetComponentsInChildren<MarkedPoint>(true));
        _markedPoints.Sort((a,b)=>(a.getPosition().x-b.getPosition().x)>0?1:-1);
        _levelBound = _cinemachineConfiger.bounds;
    }
    void Start()
    {

    }
    public MarkedPoint GetStartPoint()
    {
        return _markedPoints.Find(e => e.type == GameEnum.PointType.STARTPOINT);
    }
    public MarkedPoint GetEndPoint()
    {
        return _markedPoints.Find(e => e.type == GameEnum.PointType.ENDPOINT);
    }
     public List<MarkedPoint> GetAllRevivePoints()
    {
        return _markedPoints.FindAll(e => e.type == GameEnum.PointType.REVIVEPOINT);
    }
    // Update is called once per frame
    public GameEnum.LevelType getLevelType()
    {
        return _levelType;
    }
    public PolygonCollider2D GetCinemachinConfinerData()
    {
        return _cinemachineConfiger;
    }
    public Bounds GetLevelBounds() {
        return _levelBound;
    }
    public Transform GetPlayerLayer() {
        return _playerLayer;
    }
    public void SetActive(bool b) {
        gameObject.SetActive(b);
    }
}
