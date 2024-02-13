using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeSave : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeTest;

    private void Start()
    {
        timeTest.text = PlayerPrefs.GetFloat("Times").ToString();
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
