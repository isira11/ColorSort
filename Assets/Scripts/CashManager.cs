using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CashManager : MonoBehaviour
{

    public ScriptLinkerSO linker;

    private string COIN_KEY = "COIN";

    public void Init()
    {
        linker.Transact += (int amount, Action<bool> action)=> { action.Invoke(Trasact(amount)); };
        linker.Deposit  += (int amount) => { Deposit(amount); };
    }

    private bool Trasact(int amount)
    {
        int current = PlayerPrefs.GetInt(COIN_KEY, 0);
        int remaining = current - amount;
        if (remaining >= 0)
        {
            PlayerPrefs.SetInt(COIN_KEY, remaining);
            linker.CoinsChanged.Invoke(remaining);

            return true;
        }
        return false;
    }

    private  void Deposit(int amount)
    {
        int current = PlayerPrefs.GetInt(COIN_KEY, 0);
        int remaining = current + amount;
        PlayerPrefs.SetInt(COIN_KEY, remaining);
        linker.CoinsChanged.Invoke(remaining);
        print("Deposited "+ amount);
     
    }

}
