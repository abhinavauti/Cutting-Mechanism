using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public void Play()
    {
        GetComponent<AudioSource>().Play();
    }
}
