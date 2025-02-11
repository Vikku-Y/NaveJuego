using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventCaller : MonoBehaviour
{
    public void releaseCounter()
    {
        GameObject.Find("NaveControl").GetComponent<PlayerManager>().releaseCounter();
    }

    public void retreatCounter()
    {
        GameObject.Find("NaveControl").GetComponent<PlayerManager>().retreatCounter();
    }
}
