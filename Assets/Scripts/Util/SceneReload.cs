using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReload : MonoBehaviour
{
    [SerializeField] string sceneName;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) {
            SceneManager.LoadScene(sceneName);
        }
    }

}
