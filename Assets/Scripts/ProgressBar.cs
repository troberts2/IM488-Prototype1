using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform curPoint;

    [SerializeField] private RectTransform barStart;
    [SerializeField] private RectTransform barEnd;
    [SerializeField] private RectTransform playerToken;

    private GameObject cheese;

    public float maxBar;
    public float curBar;

    [SerializeField] private Image fillBar;

    private WinCondition win;
    private bool won;

    // Start is called before the first frame update
    void Start()
    {
        cheese = FindObjectOfType<Movement>().gameObject;

        startPoint = cheese.transform;
        curPoint = cheese.transform;

        maxBar = Vector3.Distance(startPoint.position, endPoint.position);

        win = FindObjectOfType<WinCondition>();

        playerToken.position = barStart.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        curPoint = cheese.transform;

        curBar = Vector3.Distance(curPoint.position, endPoint.position);

        FillCheck();

        if (curBar <= 5 && !won)
        {
            won = true;
            StartCoroutine(win.Win());
        }

        float barLine = curBar / maxBar;
        Debug.Log( 1 - (barLine));
        playerToken.localPosition = new Vector3((1 - barLine) * 200, playerToken.localPosition.y);


    }

    private void FillCheck()
    {
        float fill = curBar / maxBar;
        fillBar.fillAmount = fill;
    }
}
