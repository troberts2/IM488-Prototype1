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

        grapple = FindObjectOfType<GrappleHook>();
        movement = FindObjectOfType<Movement>();

        if (movement != null)
        {
            paused = false;
            pauseScreen = transform.GetChild(0).gameObject;
            pauseScreen.SetActive(false);
        }
        else
        {
            this.enabled = false;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        //if (Application.isEditor)
        //{
        //    UnityEditor.EditorApplication.isPlaying = false;
        //}
        //else
        //{
        //    Application.Quit();
        //}

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
