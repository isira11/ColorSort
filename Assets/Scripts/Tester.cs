using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;
using Doozy.Engine;
using TMPro;
using EasyMobile;
using MoreMountains.NiceVibrations;

public class Tester : MonoBehaviour
{
    public TextMeshProUGUI cash_txt;
    public TextMeshProUGUI level_txt;
    public TextMeshProUGUI level_complete_txt;
    public ParticleSystem level_completed_confeti;
    public GameObject blur_screen;
    public ScriptLinkerSO linker;
    public GameObject tube_prefab;
    public GameObject stone_prefab;
    public ShopManager shopManager;
    public CashManager cashManager;
    public AdsManager adsManager;
    public SpriteRenderer background;
    public TextMeshProUGUI helper_txt;
    public PresetDemoItem won_heptic;

    public const string LEVEL_KEY = "LEVEL";

    Stack<Move> moves = new Stack<Move>();

    public int level = 0;

    private void Start()
    {
        adsManager.Init();

        cashManager.Init();
        shopManager.Init();
        GetComponent<TubeGrid>().Init();

        linker.TubeSolved += OnTubeSolved;
        linker.StoneTransfered += OnTransfered;


        linker.CoinsChanged += (int coins) => { cash_txt.SetText("" + coins); };

        linker.Deposit.Invoke(100);

        linker.BackgroundThemeChanged += (ThemeItem theme_item) =>
        {
            print(theme_item);
            background.sprite = theme_item.images[0];
            background.size = new Vector2(40, 40);
            Color c = AverageColorFromTexture(theme_item.images[0].texture);
            linker.tube_color = c;
            linker.TubeColorChanged.Invoke(c);

        };


        linker.BackgroundThemeChanged.Invoke(linker.selected_background);



    }


    Color32 AverageColorFromTexture(Texture2D tex)
    {

        Color32[] texColors = tex.GetPixels32();

        int total = texColors.Length;

        float r = 0;
        float g = 0;
        float b = 0;

        for (int i = 0; i < total; i++)
        {

            r += texColors[i].r;

            g += texColors[i].g;

            b += texColors[i].b;

        }
        Color c = new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), 255);
        Color.RGBToHSV(c, out float H, out float S, out float V);

        if (S < 0.1)
        {
            S = 0;
            V = 0;
        }
        else
        {
            S = 1;
        }
        print(S);
        c = Color.HSVToRGB(H, S, V);
        return c;

    }

    public int GetNumberOfColors(int level)
    {
        int c;


        if (false){}
        else if (level <=2)
        {
            c = 1;
        }
        else if (level <= 5)
        {
            c = 2;
        }
        else if (level <= 10)
        {
            c = 3;
        }
        else if (level <= 15)
        {
            c = 4;
        }
        else if (level <= 20)
        {
            c = 5;
        }
        else if (level <= 25)
        {
            c = 6;
        }
        else if (level <= 40)
        {
            c = 7;
        }
        else if (level <= 50)
        {
            c = 8;
        }
        else 
        {
            c = 9;
        }

        return c;
    }

    public void CreateLevel()
    {
        ClearLevel();
        linker.playing = true;
        int level = GetLevel();
        level_txt.SetText("LEVEL " + level);
        Random.InitState(level);


        int number_of_colors = Random.Range((level<3)?1:2,GetNumberOfColors(level)+1);

        List<Tube> temp_tubes = new List<Tube>();
        List<GameObject> stones = new List<GameObject>();
        int[] random_ids = new int[number_of_colors];

        List<int> numbers = new List<int>() {0, 1, 2, 3, 4, 5, 6, 7, 8 };

        for (int i = 0; i < number_of_colors; i++)
        {
   
            int choosen = numbers[Random.Range(0, numbers.Count)];
            numbers.Remove(choosen);
            random_ids[i] = choosen;
        }


        for (int i = 0; i < number_of_colors; i++)
        {
            temp_tubes.Add(AddTube().GetComponentInChildren<Tube>());
            for (int j = 0; j < 4; j++)
            {
                GameObject stone_obj = Instantiate(stone_prefab);
                stone_obj.GetComponent<Stone>().Init(random_ids[i]);
                stones.Add(stone_obj);
            }
        }

        if (number_of_colors == 1)
        {
            temp_tubes.Add(AddTube().GetComponentInChildren<Tube>());

            int c = Random.Range(1,4);
            int temp = 0;
            foreach (Tube item in temp_tubes)
            {
                if (temp == 1)
                {
                    c = 4 - c;
                }

                for (int i = 0; i < c; i++)
                {
                    int s_index = Random.Range(0, stones.Count);
                    GameObject obj = stones[s_index];
                    stones.RemoveAt(s_index);
                    item.Push(obj.GetComponent<Stone>(), false);
                }

                temp++;

            }

        }
        else
        {
            temp_tubes.Add(AddTube().GetComponentInChildren<Tube>());
            temp_tubes.Add(AddTube().GetComponentInChildren<Tube>());


            while(temp_tubes.Count > 0)
            {
                int index = Random.Range(0, temp_tubes.Count);
                for (int i = 0; i < 4; i++)
                {
                    if (stones.Count == 0)
                    {
                        break;
                    }

                    int s_index = Random.Range(0, stones.Count);
                    GameObject obj = stones[s_index];
                    stones.RemoveAt(s_index);
        
                    temp_tubes[index].Push(obj.GetComponent<Stone>(), false);
                }

                temp_tubes.RemoveAt(index);

            }


        }


        if (level == 1)
        {
            helper_txt.gameObject.SetActive(true);
            helper_txt.SetText("Place similar looking pebbles into one tube");
        }
        else if (level == 3)
        {
            helper_txt.SetText("You can only place a pebble on top of a similar one");
        }
        else
        {
            helper_txt.SetText("");
        }

        linker.TubeAdded.Invoke();
    }

    public void  ClearLevel()
    {
        linker.playing = false;
        blur_screen.SetActive(false);
        linker.tubes.Clear();
        moves.Clear();

        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }

        if (linker.PoppedStone)
        {
            Destroy(linker.PoppedStone.gameObject);
            linker.PoppedStone = null;
        }

        level_completed_confeti.Stop();
    }

    public void AddTubeButton()
    {
        AdsManager.instance.ShowRewarded(() => { AddTube(); });
    }

    public GameObject AddTube()
    {
        GameObject obj = Instantiate(tube_prefab, transform);
        obj.GetComponentInChildren<Tube>().Init();
        linker.TubeAdded.Invoke();

        return obj;
    }

    public void ResetLevel()
    {
        CreateLevel();
    }

    public void SkipLevel()
    {
        AdsManager.instance.ShowRewarded(() => {
            LevelUp();
            CreateLevel();
        });
    }

    public void Undo()
    {
        MMVibrationManager.SetHapticsActive(true);

        if (moves.Count > 0)
        {
            if (linker.PoppedStone)
            {
                linker.PoppedStone.PushBackIn();
            }
            Move move = moves.Pop();
            move.to.PopStone();
            move.from.Push(linker.PoppedStone, false);
        }
    }

    public void OnTransfered(Tube from, Tube to)
    {
        moves.Push(new Move(from, to));
    }

    public void OnTubeSolved()
    {
        print("Tube Solved");

        bool solved = false;

        foreach (Tube item in linker.tubes)
        {

            if (item.solved || item.GetCount() == 0)
            {
                solved = true;
            }
            else
            {
                solved = false;
                break;
            }
        }

        if (solved)
        {

            StartCoroutine(CR_Won_Heptic());
            OnLevelCompeleted();
        }
    }

    IEnumerator CR_Won_Heptic()
    {
        MMVibrationManager.StopAllHaptics(true);
        MMVibrationManager.StopContinuousHaptic(true);
        yield return new WaitForSeconds(0.05f);
        MMVibrationManager.AdvancedHapticPattern(won_heptic.AHAPFile.text,
         won_heptic.WaveFormAsset.WaveForm.Pattern, won_heptic.WaveFormAsset.WaveForm.Amplitudes, -1,
         won_heptic.RumbleWaveFormAsset.WaveForm.Pattern, won_heptic.RumbleWaveFormAsset.WaveForm.LowFrequencyAmplitudes,
         won_heptic.RumbleWaveFormAsset.WaveForm.HighFrequencyAmplitudes, -1,
         HapticTypes.LightImpact, this, -1, false);
    }

    public void OnLevelCompeleted()
    {
        linker.playing = false;
        level_completed_confeti.Play();
        level_complete_txt.SetText("LEVEL" + GetLevel() + "\nCompleted");
        linker.LevelCompleted.Invoke();
        LevelUp();
        GameEventMessage.SendEvent("on_completed");
    }

    public void LevelUp()
    {
        blur_screen.SetActive(true);
        int current_level = GetLevel();
        current_level++;
        PlayerPrefs.SetInt(LEVEL_KEY, current_level);
    }

    public void ShowBanner()
    {
        AdsManager.instance.SetActiveBanner(true);
    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt(LEVEL_KEY, 1);
    }
}
