using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScriptLinkerSO", order = 1)]
public class ScriptLinkerSO : ScriptableObject
{
    public bool playing;
    public Stone PoppedStone;
    public List<Tube> tubes;

    public Action TubeSolved = delegate {};
    public Action<Tube,Tube> StoneTransfered = delegate { };
    public Action TubeAdded = delegate {};

    public ThemeItem selected_stones;
    public ThemeItem selected_background;
    public ThemeItem selected_tube;

    public Action<ThemeItem> StoneThemeChanged = delegate { };
    public Action<ThemeItem> TubeThemeChanged = delegate { };
    public Action<ThemeItem> BackgroundThemeChanged = delegate { };

    public Action<int> CoinsChanged = delegate { };
    public Action<int, Action<bool>> Transact = delegate { };
    public Action<int> Deposit = delegate { };

    public Color32 tube_color;
    public Action<Color32> TubeColorChanged = delegate {};

    public Action AdsReady = delegate { };
    public bool ads_ready = false;

    public Action LevelCompleted = delegate { };

}
