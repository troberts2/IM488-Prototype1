using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioClip landing;
    public AudioClip objectCollision;
    public AudioClip buttonClick;
    public AudioClip buttonHover;
    Vector3 camPos;

    // Update is called once per frame
    void Update()
    {
        camPos = Camera.main.transform.position;
    }

    public void ButtonScroll()
    {
        AudioSource.PlayClipAtPoint(buttonHover, camPos);
    }

    public void ButtonClick()
    {
        AudioSource.PlayClipAtPoint(buttonClick, camPos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            AudioSource.PlayClipAtPoint(landing, camPos);
        }
        
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AudioSource.PlayClipAtPoint(objectCollision, camPos);
        }
    }



}
