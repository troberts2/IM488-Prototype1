using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuFunctions : MonoBehaviour
{
    private bool paused;

    private float curTime;

    private GameObject pauseScreen;

    private GrappleHook grapple;
    private Movement movement;

    private void Start()
    {
        Debug.Log("hi");

        paused = false;
        pauseScreen = transform.GetChild(0).gameObject;
        Debug.Log(pauseScreen);
        pauseScreen.SetActive(false);

        grapple = FindObjectOfType<GrappleHook>();
        movement = FindObjectOfType<Movement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("hi");
            Pause();
        }
    }

    public void SceneChange(int sceneID)//sets up scene changing
    {
        SceneManager.LoadScene(sceneID);
    }

    public static void QuitGame()
    {

        Application.Quit();
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }

    }

    public void Pause()
    {
        if (!paused)
        {
            paused = true;

            pauseScreen.SetActive(true);

            curTime = Time.timeScale;
            Time.timeScale = 0;

            grapple.StopAllCoroutines();
            grapple.enabled = false;
            movement.enabled = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else
        {
            paused = false;

            pauseScreen.SetActive(false);

            Time.timeScale = curTime;

            grapple.enabled = true;
            movement.enabled = true;

            Cursor.visible = false;

            Debug.Log(Cursor.visible);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}
