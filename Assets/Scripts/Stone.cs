using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Stone : MonoBehaviour
{
    public ScriptLinkerSO linker;
    public int id;
    public Tube tube;
    public Themes themes;
    Sequence sequence;

    private const float drop_duration = 0.3f;
    private const float pop_duration = 0.15f;
    private const float transfer_duration = 0.2f;

    public void Init(int _id)
    {
        id = _id;
        linker.StoneThemeChanged += SetTheme;
        SetTheme(linker.selected_stones);
    }

    public void SetTheme(ThemeItem themeItem)
    {
        GetComponent<SpriteRenderer>().sprite = themeItem.images[id];
    }

    public void PushBackIn()
    {
        tube.Push(this);
    }

    public void OnMoved_Pop(Transform t0)
    {
        if (sequence != null)
        {
            sequence.Kill();
        }

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(t0.position, pop_duration).SetEase(Ease.InOutQuad));
    }

    public void OnMoved_Drop(Transform t0)
    {
        if (sequence != null)
        {
            sequence.Kill();
        }
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(t0.position, drop_duration).SetEase(Ease.OutBounce));
    }

    public void OnMoved_Transfer(Transform t0, Transform t1)
    {
        if (sequence != null)
        {
            sequence.Kill();
        }

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(t0.position, transfer_duration).SetEase(Ease.InOutQuad));
        sequence.Append(transform.DOMove(t1.position, drop_duration).SetEase(Ease.OutBounce));

    }

    
    private void OnDestroy()
    {
        linker.StoneThemeChanged -= SetTheme;
        if (sequence != null )
        {
            sequence.Kill();
        }
    }
}