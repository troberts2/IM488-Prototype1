using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform curPoint;

    private GameObject cheese;

    public float maxBar;
    public float curBar;

    [SerializeField] private Image fillBar;

    private WinCondition win;

    // Start is called before the first frame update
    void Start()
    {
        cheese = FindObjectOfType<Movement>().gameObject;

        startPoint = cheese.transform;
        curPoint = cheese.transform;

        maxBar = Vector3.Distance(startPoint.position, endPoint.position);

        win = FindObjectOfType<WinCondition>();
    }

    // Update is called once per frame
    void Update()
    {
        curPoint = cheese.transform;

        curBar = Vector3.Distance(curPoint.position, endPoint.position);

        FillCheck();

        if (curBar <= 5)
        {
            Debug.Log("you should win");
            StartCoroutine(win.Win());
        }
    }

    private void FillCheck()
    {
        float fill = curBar / maxBar;
        fillBar.fillAmount = fill;
    }
}
