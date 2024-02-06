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

    private void Start()
    {
        LoadFromJson();
        compTime = float.Parse(timeHold);
    }

    public void SaveToJson()
    {
        
        timeFloat = Timer.instance.timer;
        if (compTime > timeFloat)
        {
            newTime = Timer.instance.timer.ToString();
            newTime = JsonUtility.ToJson(time);
            string filePath = Application.persistentDataPath + "/SaveData.json";
            System.IO.File.WriteAllText(filePath, newTime);
        }
        else
        {
            newTime = timeHold;
            newTime = JsonUtility.ToJson(time);
            string filePath = Application.persistentDataPath + "/SaveData.json";
            System.IO.File.WriteAllText(filePath, newTime);
        }
        
    }

    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/SaveData.json";
        string newTime = System.IO.File.ReadAllText(filePath);

        time = JsonUtility.FromJson<Times>(timeHold);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Victory") || collision.gameObject.CompareTag("Enemy"))
        {
            SaveToJson();
        }
    }
}

public class Times
{
    public string bestTime;
}