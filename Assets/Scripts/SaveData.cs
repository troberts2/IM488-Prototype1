using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveData : MonoBehaviour
{
    public static SaveData instance;
    public Times time = new Times();
    public string newTime;
    public string timeHold;
    private float compTime;
    private float timeFloat;

    public void SetString(string BestTime, string Time)
    {
        PlayerPrefs.SetString(BestTime, Time);
    }

    public string GetString(string BestTime)
    {
        return PlayerPrefs.GetString(BestTime);
    }

    private void Start()
    {
        GetString(timeHold);

        compTime = float.Parse(timeHold);
    }


    public void Update()
    {
        
        timeFloat = Timer.instance.gameTimer;
        if (compTime > timeFloat)
        {
            newTime = Timer.instance.gameTimer.ToString();
            SetString(timeHold, Timer.instance.gameTimer.ToString());
        }
        else
        {
            newTime = timeHold;
            SetString(timeHold, Timer.instance.gameTimer.ToString());
        }
        
    }

    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/SaveData.json";
        string newTime = System.IO.File.ReadAllText(filePath);

        time = JsonUtility.FromJson<Times>(timeHold);
    }

}

public class Times
{
    public string bestTime;
}