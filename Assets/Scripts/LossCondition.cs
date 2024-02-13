using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LossCondition : MonoBehaviour
{
    private WinCondition winCon;
    private GrappleHook grappleHook;

    private void Start()
    {
        winCon = FindObjectOfType<WinCondition>();
        grappleHook = FindObjectOfType<GrappleHook>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Lose();
        }
    }

    public void Lose()
    {
        if (!winCon.won)
        {   

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if(!grappleHook.isEndless){
                SceneManager.LoadScene("Loss Scene");
            }else{
                FindObjectOfType<ProceduralScoreScreen>().ShowScreen();
            }
            
        }
    }

}
