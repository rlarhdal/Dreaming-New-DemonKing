using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;

public class PlayerStatHandler
{
    StatBase[] stats;

    public UnityAction stageLevelUpEvents;

    public PlayerStatHandler()
    {
        stats = new StatBase[Enum.GetNames(typeof(StatSpecies)).Length];

        // Info
        stats[(int)StatSpecies.LV] = new Stat<int>(StatSpecies.LV, 1);
        stats[(int)StatSpecies.Name] = new Stat<string>(StatSpecies.Name, "");

        // Stats
        stats[(int)StatSpecies.HP] = new Stat<int>(StatSpecies.HP,100);
        stats[(int)StatSpecies.MaxHP] = new Stat<int>(StatSpecies.MaxHP, 100);
        stats[(int)StatSpecies.plusHP] = new Stat<int>(StatSpecies.plusHP,0);
        stats[(int)StatSpecies.ATKPower] = new Stat<int>(StatSpecies.ATKPower, 5); 
        stats[(int)StatSpecies.plusATKPower] = new Stat<int>(StatSpecies.plusATKPower,0);
        stats[(int)StatSpecies.Exp] = new Stat<int>(StatSpecies.Exp,0);
        stats[(int)StatSpecies.MaxExp] = new Stat<int>(StatSpecies.MaxExp,20);
        
        stats[(int)StatSpecies.ATKRate] = new Stat<float>(StatSpecies.ATKRate,1); 
        stats[(int)StatSpecies.plusATKRate] = new Stat<float>(StatSpecies.plusATKRate,0);
        stats[(int)StatSpecies.Speed] = new Stat<float>(StatSpecies.Speed,4f); 
        stats[(int)StatSpecies.plusSpeed] = new Stat<float>(StatSpecies.plusSpeed,0);
        stats[(int)StatSpecies.HealAmount] = new Stat<float>(StatSpecies.HealAmount, .1f);

        // Level
        stats[(int)StatSpecies.HPLevel] = new Stat<int>(StatSpecies.HPLevel, 1);
        stats[(int)StatSpecies.ATKPowerLevel] = new Stat<int>(StatSpecies.ATKPowerLevel, 1);
        stats[(int)StatSpecies.ATKRateLevel] = new Stat<int>(StatSpecies.ATKRateLevel, 1);
        stats[(int)StatSpecies.SpeedLevel] = new Stat<int>(StatSpecies.SpeedLevel, 1);
        
    }
    //public PlayerStatHandler( save file )

    public Stat<T> GetStat<T>(StatSpecies species)
    {
        return stats[(int)species] as Stat<T>;
    }

    public void AddExp(int exp)
    {
        GetStat<int>(StatSpecies.Exp).value += exp;
        LevelUp();
    }

    void LevelUp()
    {
        int curExp = GetStat<int>(StatSpecies.Exp).value;
        int maxExp = GetStat<int>(StatSpecies.MaxExp).value;
        while (curExp >= maxExp)
        {
            GetStat<int>(StatSpecies.LV).value++;
            GetStat<int>(StatSpecies.Exp).value -= maxExp;
            int newMax = GetStat<int>(StatSpecies.LV).value*10;
            GetStat<int>(StatSpecies.MaxExp).value = newMax;
            LevelUp();
        }
    }

    #region Stage Level logics
//    public void AddStageExp(int exp)
//    {
//        GetStat<int>(StatSpecies.stageExp).AddValue(exp);
//        StageLevelUp();
//    }
//
//    void StageLevelUp()
//    {
//        int curExp = GetStat<int>(StatSpecies.stageExp).value;
//        int maxExp = GetStat<int>(StatSpecies.stageMaxExp).value;
//        while (curExp >= maxExp)
//        {
//            // choose the skill option either passive or active?
//            stageLevelUpEvents?.Invoke();
//            
//            GetStat<int>(StatSpecies.stageLV).AddValue(1);
//            GetStat<int>(StatSpecies.stageExp).AddValue(-maxExp);
//            int newMax = GetStat<int>(StatSpecies.stageLV).value*10;
//            GetStat<int>(StatSpecies.stageMaxExp).AddValue(-maxExp + newMax);
//            StageLevelUp();
//        }
//        
//    }
#endregion

    /// <summary>
    /// Ω∫≈» ∞≠»≠
    /// </summary>
    public void ReinforceStat(StatLevel type)
    {
        float currentvalue;
        int levelValue;

        switch (type)
        {
            case StatLevel.Hp:
                currentvalue = GetStat<int>(StatSpecies.MaxHP).value;
                levelValue = GetStat<int>(StatSpecies.HPLevel).value;

                GetStat<int>(StatSpecies.HPLevel).value = (levelValue + 1);
                GetStat<int>(StatSpecies.MaxHP).value = ((int)(currentvalue + 10));
                break;

            case StatLevel.Atkpower:
                currentvalue = GetStat<int>(StatSpecies.ATKPower).value;
                levelValue = GetStat<int>(StatSpecies.ATKPowerLevel).value;

                GetStat<int>(StatSpecies.ATKPowerLevel).value = (levelValue + 1);
                GetStat<int>(StatSpecies.ATKPower).value =((int)(currentvalue + 1));
                break;

            //case StatLevel.AtkRate:
            //    currentvalue = GetStat<float>(StatSpecies.ATKRate).value;
            //    levelValue = GetStat<int>(StatSpecies.ATKRateLevel).value;

            //    GetStat<int>(StatSpecies.ATKRateLevel).value =(levelValue + 1);
            //    GetStat<float>(StatSpecies.ATKRate).value =(currentvalue + 1);
            //    break;

            case StatLevel.Speed:
                currentvalue = GetStat<float>(StatSpecies.Speed).value;
                levelValue = GetStat<int>(StatSpecies.SpeedLevel).value;

                GetStat<int>(StatSpecies.SpeedLevel).value = (levelValue + 1);
                GetStat<float>(StatSpecies.Speed).value = (currentvalue + 0.1f);
                break;
        }
    }
}


public class StatBase
{
    public StatSpecies species { get; protected set; }
    
}

public class Stat<T> : StatBase
{
    public T value { get; set; }

    public Stat(StatSpecies species, T value)
    {
        this.species = species;
        this.value = value;
    }
}