using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProceduralScore : MonoBehaviour
{
    private Vector3 startPos;
    public Transform playerPos;
    public TextMeshProUGUI scoreText;
    private float ogFontSize;
    public float score;

    private void Start() {
        startPos = playerPos.position;
        ogFontSize = scoreText.fontSize;
    }
    private void Update() {
        if(playerPos != null){
            score = playerPos.position.z - startPos.z;
        }
        
        scoreText.text = (int)score + "";
        if(scoreText.fontSize >= ogFontSize){
            float newFontSize = ogFontSize * (playerPos.gameObject.GetComponent<Rigidbody>().velocity.magnitude / 25);
            if(newFontSize > ogFontSize){
                scoreText.fontSize = newFontSize;
            }
            else{
                scoreText.fontSize = ogFontSize;
            }
        }
        

    }
}
