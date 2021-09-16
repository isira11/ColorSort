using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using DG.Tweening;

public class TubeGrid : MonoBehaviour
{
    public ScriptLinkerSO linker;
    public float x_padding;
    public float y_padding;
    public float x_gap;
    public float y_gap;
    public float max_columns;
    public float tube_width;
    public float tube_height;

    public bool button;
    List<List<Transform>> tubes;

    public void Init()
    {
        linker.TubeAdded += Rearrange;
    }

    private void Update()
    {
        if (button)
        {
            Rearrange();
            button = false;
        }
    }


    [Button]
    public void Rearrange()
    {
        StartCoroutine(CR_Rearrange());
 
    }

    IEnumerator CR_Rearrange()
    {
        yield return new WaitForSeconds(0.001f);
        ColumnSetter();

        int index = 0;

        tubes = new List<List<Transform>>();

 

        foreach (Transform item in transform)
        {
            if (index % max_columns == 0)
            {
                tubes.Add(new List<Transform>());
            }
            tubes[tubes.Count - 1].Add(item);

            index++;
        }


        float row = 0;
        float y_total = y_gap * (tubes.Count - 1);

        PerfectSettings(out float x_gap,out Vector3 scale);

        foreach (List<Transform> items in tubes)
        {
            float x_total = x_gap * (items.Count - 1);
            float pos = 0;

            foreach (Transform item in items)
            {
                Vector3 new_pos = new Vector3(pos - (x_total / 2.0f), row + (y_total / 2.0f), 0);
                item.transform.DOLocalMove(new_pos, 0.2f).SetEase(Ease.InOutQuint);
                pos += x_gap;
            }

            row -= y_gap;
        }

        transform.parent.localScale = scale;

    }

    public void ColumnSetter()
    {
        int count = transform.childCount;

        switch (count)
        {
            case 3:
                max_columns = 3;
                break;
            case 4:
                max_columns = 3;
                break;
            case 5:
                max_columns = 3;
                break;
            case 6:
                max_columns = 3;
                break;
            case 7:
                max_columns = 4;
                break;
            case 8:
                max_columns = 4;
                break;
            case 9:
                max_columns = 5;
                break;
            case 10:
                max_columns = 5;
                break;
            default:
                max_columns = 6;
                break;
        }
    }


    public void PerfectSettings(out float x_gap, out Vector3 scale)
    {


        transform.parent.localScale = Vector3.one;

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2;

        Bounds bounds = new Bounds(Camera.main.transform.position,new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        float min_columns = Mathf.Clamp(transform.childCount, 1, max_columns);
        float all_tubes_x_extent = tube_width* min_columns;
        float all_tubes_y_extent = tube_height * tubes.Count;
        float max_y_bounds = bounds.extents.y * 2 - y_padding * (bounds.extents.y * 2);
        float max_x_bounds = bounds.extents.x * 2 - x_padding * (bounds.extents.x * 2); ;
        float x_scale = max_x_bounds / all_tubes_x_extent;
        float clamped = Mathf.Clamp(x_scale,0, max_y_bounds/ all_tubes_y_extent);

        x_gap = (max_x_bounds/clamped - (all_tubes_x_extent)) /(min_columns-1) + tube_width;

        scale = new Vector3(clamped, clamped, clamped);

    }


}
