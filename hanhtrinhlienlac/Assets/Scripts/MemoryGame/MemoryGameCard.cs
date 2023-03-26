using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameCard : MonoBehaviour
{
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject front;
    [SerializeField] private float rotateSpeed;

    private float targetAngleY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        ManageFrontBackVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        var diffAngleY = targetAngleY - transform.rotation.eulerAngles.y;
        float direction = diffAngleY > 0 ? 1 : diffAngleY < 0 ? -1 : 0;

        if (direction != 0) {
            var delta = Mathf.Min(Mathf.Abs(rotateSpeed * Time.deltaTime), Mathf.Abs(diffAngleY));
            transform.Rotate(0, direction * delta, 0);
            ManageFrontBackVisibility();
        }
    }

    public void SetFrontImage(Sprite sprite)
    {
        front.GetComponent<Image>().sprite = sprite;
    }

    public Sprite GetFrontImage()
    {
        return front.GetComponent<Image>().sprite;
    }

    public void SetState(bool open)
    {
        targetAngleY = open ? 0 : 180;
    }

    private void ManageFrontBackVisibility()
    {
        var camera = GetComponentInParent<Canvas>().worldCamera;
        float angle = Vector3.Angle( (transform.position - camera.transform.position) , transform.forward );
        var degreeY = transform.rotation.eulerAngles.y;
        bool frontVisible = angle <= 90 || angle > 270;
        back.SetActive(!frontVisible);
        front.SetActive(frontVisible);
    }
}
