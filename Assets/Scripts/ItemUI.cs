using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon;

    public string id;

    public GameObject locked;

    public Button button;

    public void Unlock()
    {
        locked.SetActive(false);
    }
}
