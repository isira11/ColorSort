using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

[CreateAssetMenu(menuName = "ScriptableObjects/Themes", order = 1)]
public class Themes : ScriptableObject
{
    [Header("StoneThemes : id =  1")]
    public List<ThemeItem> stone_themes;
    [Header("tubes : id =  2")]
    public List<ThemeItem> tubes;
    [Header("backgrounds : id =  3")]
    public List<ThemeItem> backgrounds;


    public List<Sprite> _easy_tube_adder;
    public List<Sprite> _easy_background_adder;


    [Button]
    public void EasyAddTubes()
    {
        tubes.Clear();
        foreach (Sprite item in _easy_tube_adder)
        {
            tubes.Add(new ThemeItem(item, new Sprite[1] {item }));
        }

        _easy_tube_adder.Clear();
    }

    [Button]
    public void EasyAddBackGrounds()
    {
        backgrounds.Clear();
        foreach (Sprite item in _easy_background_adder)
        {
            backgrounds.Add(new ThemeItem(item, new Sprite[1] { item }));
        }

        _easy_background_adder.Clear();
    }

}



[System.Serializable]
public class ThemeItem
{
    [SerializeField] public Sprite icon;
    [SerializeField] public Sprite[] images;

    public ThemeItem(Sprite icon, Sprite[] images)
    {
        this.icon = icon;
        this.images = images;
    }
}



