using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private Movement move;
    [SerializeField] private LossCondition loss;

    [SerializeField] private GameObject hpL;
    [SerializeField] private GameObject hpM;
    [SerializeField] private GameObject hpR;

    private void Start()
    {
        move = FindObjectOfType<Movement>();
        loss = FindObjectOfType<LossCondition>();
    }

    public void UpdateHealth()
    {
        hpL.SetActive(true);
        hpM.SetActive(true);
        hpR.SetActive(true);
        //Debug.Log(move.hp);

        switch (move.hp)
        {
            case 1:
                hpR.SetActive(false);
                hpM.SetActive(false);
                break;
            case 2:
                hpR.SetActive(false);
                break;
            case 0:
                loss.Lose();
                break;
        }

        if (move.hp > 3)
        {
            move.hp = 3;
        }
    }
}
