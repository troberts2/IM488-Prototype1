using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashController : MonoBehaviour
{
   [SerializeField] FlashImage flashImage = null;
   [SerializeField] Color newColor = Color.red;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            flashImage.StartFlash(.25f, .5f, newColor);
        }

    }
}