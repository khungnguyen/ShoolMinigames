using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEarningAnim : MonoBehaviour
{
    [SerializeField] private int score = 10;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private float maxDelay = 0.5f;
    [SerializeField] private float minStartX;
    [SerializeField] private float maxStartX;
    [SerializeField] private float minStartY;
    [SerializeField] private float maxStartY;
    [SerializeField] private float speedMovingUp = 200f;
    [SerializeField] private float minDuration = 1f;
    [SerializeField] private float maxDuration = 1.5f;

    private List<GameObject> pointObjects = new List<GameObject>();
    private List<float> pointObjectsRemainingTime = new List<float>();
    // Update is called once per frame
    void Update()
    {
        for (int i = pointObjects.Count - 1; i >= 0; i--) {
            var remainingTime = pointObjectsRemainingTime[i];
            if (remainingTime <= 0) continue;
            pointObjectsRemainingTime[i] -= Time.deltaTime;
            remainingTime = pointObjectsRemainingTime[i];

            var obj = pointObjects[i];
            if (remainingTime <= 0) {
                pointObjects.RemoveAt(i);
                pointObjectsRemainingTime.RemoveAt(i);
                GameObject.Destroy(obj);
                continue;
            }
            obj.transform.position = obj.transform.position + (speedMovingUp * Time.deltaTime * Vector3.up);
            obj.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, remainingTime * remainingTime / minDuration);
        }
    }

    public void Play() 
    {
        var rectTrans = transform as RectTransform;
        for (int i = 0; i < score; i++) {
            float x = UnityEngine.Random.Range(minStartX, maxStartX);
            float y = UnityEngine.Random.Range(minStartY, maxStartY);
            float duration = UnityEngine.Random.Range(minDuration, maxDuration);
            float delay = UnityEngine.Random.Range(0f, maxDelay);
            StartCoroutine(CreatePointObject(x, y, duration, delay));
        }
    }

    private IEnumerator CreatePointObject(float x, float y, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return new WaitForEndOfFrame();
        var obj = Instantiate(pointPrefab);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = new Vector3(x, y, 0);
        pointObjects.Add(obj);
        pointObjectsRemainingTime.Add(duration);
    }
}
