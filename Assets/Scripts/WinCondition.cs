using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class WinCondition : MonoBehaviour
{
    public CinemachineFreeLook cam;

    public bool won;

    private void Start()
    {
        cam = FindObjectOfType<CinemachineFreeLook>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Victory"))
        {
            StartCoroutine(Win());
        }
    }

    public IEnumerator Win()
    {
        if (!won)
        {
            won = true;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            cam.enabled = false;
            yield return new WaitForSecondsRealtime(3);

            SceneManager.LoadScene("Win Scene");
        }
    }
}
