using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabUI : MonoBehaviour
{
    GameObject current;

    public void OnTabSelected(GameObject obj)
    {
        if (current)
        {
            current.GetComponent<Image>().color = Color.white;
        }

        current = obj;
        current.GetComponent<Image>().color = Color.black;
    }
}
