using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class Tube : MonoBehaviour
{
    //if difference color no from prev
    public ScriptLinkerSO linker;
    public List<Transform> slots = new List<Transform>();
    public Transform pop_location;
    public Transform stone_folder;
    public ParticleSystem win_confetti;
    public SpriteRenderer tube_image;
    public SpriteMask mask;

    public PresetDemoItem tube_pop;

    private Stack<Stone> stones = new Stack<Stone>();

    public bool solved;

    public void Init()
    {
        linker.tubes.Add(this);
        ChangeImage(linker.selected_tube);
        linker.TubeThemeChanged += ChangeImage;
        linker.TubeColorChanged += ChangeColor;
        ChangeColor(linker.tube_color);
    }

    public void ChangeColor(Color32 color)
    {
        tube_image.color = color;
    }

    public void ChangeImage(ThemeItem item)
    {
        tube_image.sprite = item.images[0];
        mask.sprite = item.images[0];
    }

    public bool PushStone(Stone stone)
    {
        if (stones.Count == 0)
        {
            Push(stone);
            return true;
        }
        else if (stone.id == stones.Peek().id && stones.Count < 4)
        {
            Push(stone);
            return true;
        }

        return false;
    }

    public void Fill(Stone stone)
    {
        if (stones.Count < 4)
        {
            Push(stone);
        }
    }

    public void Push(Stone stone, bool record_history = true)
    {
        stone.transform.parent = stone_folder;
        stone.transform.localScale = Vector3.one;
        if (stone.tube)
        {
            if (stone.tube != this)
            {
                stone.OnMoved_Transfer(pop_location, slots[stones.Count]);

                if (record_history)
                {
                    linker.StoneTransfered.Invoke(stone.tube, this);
                }
            }
            else
            {
                stone.OnMoved_Drop(slots[stones.Count]);
            }

            linker.PoppedStone = null;
        }
        else
        {
            stone.transform.position = slots[stones.Count].position;
        }

        stone.tube = this;
        stones.Push(stone);

        CheckWin();
    }

    public void CheckWin()
    {
        if (stones.Count == 4)
        {
            int prev_id = stones.Peek().id;
            foreach (Stone item in stones)
            {
                if (item.id != prev_id)
                {
                    solved = false;
                    break;
                }
                solved = true;
                prev_id = item.id;
            }

            if (solved)
            {

                StartCoroutine(CR_Won_Heptic());
                win_confetti.Play();
                linker.TubeSolved.Invoke();
            }
        }
        else
        {
            win_confetti.Stop();
            solved = false;
        }
    }

    IEnumerator CR_Won_Heptic()
    {
        MMVibrationManager.StopAllHaptics(true);
        MMVibrationManager.StopContinuousHaptic(true);
        yield return new WaitForSeconds(0.01f);
        MMVibrationManager.AdvancedHapticPattern(tube_pop.AHAPFile.text,
     tube_pop.WaveFormAsset.WaveForm.Pattern, tube_pop.WaveFormAsset.WaveForm.Amplitudes, -1,
     tube_pop.RumbleWaveFormAsset.WaveForm.Pattern, tube_pop.RumbleWaveFormAsset.WaveForm.LowFrequencyAmplitudes,
     tube_pop.RumbleWaveFormAsset.WaveForm.HighFrequencyAmplitudes, -1,
     HapticTypes.LightImpact, this, -1, false);
    }

    public int GetCount()
    {
        return stones.Count;
    }

    public void PopStone()
    {

        if (stones.Count > 0)
        {
            stones.Peek().OnMoved_Pop(pop_location.transform);

            linker.PoppedStone = stones.Pop();
        }
    }

    public void OnMouseDown()
    {

        if (linker.playing)
        {

            if (linker.PoppedStone != null)
            {
                if (linker.PoppedStone.tube == this)
                {
                    MMVibrationManager.Haptic(HapticTypes.SoftImpact, false, true, this);

                    Push(linker.PoppedStone);
                    // linker.PoppedStone = null;
                }
                else
                {
                    if (PushStone(linker.PoppedStone))
                    {
                        MMVibrationManager.Haptic(HapticTypes.RigidImpact, false, true, this);
                        //linker.PoppedStone = null;
                    }
                    else
                    {
                        MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);
                        linker.PoppedStone.PushBackIn();
                        PopStone();
                    }
                }
            }
            else
            {
                MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);
                PopStone();
            }
        }
    }

    private void OnDestroy()
    {
        linker.TubeThemeChanged -= ChangeImage;
        linker.TubeColorChanged -= ChangeColor;
    }

}
