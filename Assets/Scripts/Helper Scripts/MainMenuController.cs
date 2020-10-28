using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string[] sceneNames;
    public Animator cameraAnim;

    private string launchScene;

    public void PlayGame()
    {
        cameraAnim.Play("CamSlide");
    }

    public void SetScene(string name)
    {
        for (int i = 0; i < sceneNames.Length; i++)
        {
            if (sceneNames[i] == name)
            {
                launchScene = sceneNames[i];
            }
        }
    }

    public void LaunchScene()
    {
        SceneManager.LoadScene(launchScene);
    }

}
