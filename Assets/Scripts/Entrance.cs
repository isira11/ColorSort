using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Entrance : MonoBehaviour
{
    AsyncOperation scene;
    public Image image;
    float currentValue;

    private void Start()
    {

        scene = SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
        scene.allowSceneActivation = false;
        image.fillAmount = 0;
    }

    private void Update()
    {
        print("LOAD:"+ scene.progress);

        float targetValue = scene.progress / 0.9f;

        currentValue = Mathf.MoveTowards(currentValue, targetValue, 0.25f * Time.deltaTime);


        image.fillAmount = currentValue;//Mathf.Lerp(currentValue, scene.progress / 0.9f, Time.deltaTime * 2);

        if (Mathf.Approximately(currentValue, 1))
        {
            scene.allowSceneActivation = true;
        }

    }

}
