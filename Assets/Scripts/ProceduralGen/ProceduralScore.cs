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

    private void Start() {
        startPos = playerPos.position;
    }
    private void Update() {
        float score = playerPos.position.z - startPos.z;
        scoreText.text = (int)score + "";
    }
}
