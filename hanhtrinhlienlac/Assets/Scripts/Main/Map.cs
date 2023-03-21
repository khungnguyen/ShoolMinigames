using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLevelClicked(string sceneName) {
        Debug.Log("Select level " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
