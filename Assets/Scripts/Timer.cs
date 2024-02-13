using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public string currentTime;
    public string currentTimeNoColon;
    public static Timer instance;
    private float timeDuration = 99f * 60f;

    [SerializeField]
    bool countDown = true;

    public float gameTimer;

    [SerializeField]
    private TextMeshProUGUI firstMinute;
    [SerializeField]
    private TextMeshProUGUI secondMinute;
    [SerializeField]
    private TextMeshProUGUI separator;
    [SerializeField]
    private TextMeshProUGUI firstSecond;
    [SerializeField]
    private TextMeshProUGUI secondSecond;

    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameTimer > 0 && countDown)
        {
            gameTimer -= Time.deltaTime;
            UpdateTimerDisplay(gameTimer);
        }
        else if (!countDown && gameTimer < timeDuration)
        {
            gameTimer += Time.deltaTime;
            UpdateTimerDisplay(gameTimer);
        }

        
    }

    private void ResetTimer()
    {
        if (countDown)
        {
            gameTimer = timeDuration;
        }
        else
        {
            gameTimer = 0;
        }
    }

    private void UpdateTimerDisplay(float time)
    {
        

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        currentTime = string.Format("{00:00}{1:00}", minutes, seconds);
        firstMinute.text = currentTime[0].ToString();
        secondMinute.text = currentTime[1].ToString();
        firstSecond.text = currentTime[2].ToString();
        secondSecond.text = currentTime[3].ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Victory"))
        {
            float tFloat = PlayerPrefs.GetFloat("Times");
            float compTimeOld = tFloat;
            float compTimeNew = gameTimer;
            if (compTimeOld < compTimeNew)
            {

            }
            else if (tFloat == 0)
            {
                PlayerPrefs.SetFloat("Times", gameTimer);
            }
            else
            {
                PlayerPrefs.SetFloat("Times", gameTimer);
            }
        }
    }
}
