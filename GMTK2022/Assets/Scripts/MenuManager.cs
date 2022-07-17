using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;

    public void OnStart(int scene)
    {
        if (menu && menu.activeSelf)
        {
            SceneManager.LoadScene(scene);
            Time.timeScale = 1f;
        }
    }
}
