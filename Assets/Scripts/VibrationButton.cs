using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;

public class VibrationButton : MonoBehaviour
{
    public GameObject image;


    bool on;

    private void Start()
    {
        on = true;
        MMVibrationManager.SetHapticsActive(on);
        image.SetActive(!on);
    }

    public void toggleVib()
    {
        if (on)
        {
            on = false;
        }
        else
        {
            on = true;
        }
        image.SetActive(!on);
        MMVibrationManager.SetHapticsActive(on);
    }

}
