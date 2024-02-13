using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProceduralScoreScreen : MonoBehaviour
{
    public GameObject[] uiToDisable;
    public GameObject player;
    public GameObject lossPanel;
    public TextMeshProUGUI highScore;
    public TextMeshProUGUI thisScore;
    ProceduralScore proceduralScore;

    private void Start() {
        proceduralScore = FindObjectOfType<ProceduralScore>();
    }

    public void ShowScreen(){
        foreach (GameObject uiItem in uiToDisable)
        {
            uiItem.SetActive(false);
        }
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        player.transform.GetChild(0).gameObject.SetActive(false);
        lossPanel.SetActive(true);
        if(PlayerPrefs.GetInt("HighScore") < proceduralScore.score){
            PlayerPrefs.SetInt("HighScore", (int)proceduralScore.score);
            highScore.text = (int)proceduralScore.score + "";
            thisScore.text = (int)proceduralScore.score + "";
        }else{
            highScore.text = PlayerPrefs.GetInt("HighScore") + "";
            thisScore.text = (int)proceduralScore.score + "";
        }
    }
    public void Retry(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
