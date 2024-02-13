using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeSave : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI BestTime;
    [SerializeField] TextMeshProUGUI CurrentTime;

    

    private void Start()
    {
        if (PlayerPrefs.GetFloat("Times") == 0)
        {
            PlayerPrefs.SetFloat("Times", 9999f);
        }
        Debug.Log(PlayerPrefs.GetFloat("Times").ToString());
        BestTime.text = PlayerPrefs.GetFloat("Times").ToString();
        CurrentTime.text = PlayerPrefs.GetFloat("Your Time").ToString();

        float minutes = Mathf.FloorToInt(PlayerPrefs.GetFloat("Your Time") / 60);
        float seconds = Mathf.FloorToInt(PlayerPrefs.GetFloat("Your Time") % 60);

        float bMinutes = Mathf.FloorToInt(PlayerPrefs.GetFloat("Times") / 60);
        float bSeconds = Mathf.FloorToInt(PlayerPrefs.GetFloat("Times") % 60);

        BestTime.text = string.Format("{00:00}{1:00}", bMinutes, bSeconds);
        CurrentTime.text = string.Format("{00:00}{1:00}", minutes, seconds);

    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("Times", Timer.instance.gameTimer);
    }

    public void LoadData()
    {
        PlayerPrefs.GetFloat("Times");
    }

   /// private void OnTriggerEnter(Collider other)
   /// {
    //    if (other.CompareTag("Victory"))
    //    {
     //       float tFloat = PlayerPrefs.GetFloat("Times");
      //      float compTimeOld = tFloat;
       //     float compTimeNew = Timer.instance.gameTimer;
        //    if (compTimeOld < compTimeNew)
         //   {
         //       PlayerPrefs.SetFloat("Times", Timer.instance.gameTimer);
         //  }
          //  else
          //  {
           //     PlayerPrefs.SetFloat("Times", Timer.instance.gameTimer);
           // }
       // }
   // }
}
