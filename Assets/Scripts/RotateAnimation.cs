using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateAnimation : MonoBehaviour
{

    void Start()
    {
        transform.DORotate(new Vector3(0, 0, 360), 90, RotateMode.FastBeyond360).SetLoops(-1);
    }

}
