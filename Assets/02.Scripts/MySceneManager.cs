using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void GoToScene(int _sceneIndex)
    {
        SceneManager.LoadScene(_sceneIndex);
    }
    public void GoToScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
}
