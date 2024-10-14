using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

#region Associated with Input
public enum InputDevices
{
    Keyboard,
    Mouse,
    Gamepad,
    Count,
}

public enum JoystickMode
{
    Fixed,
    Floating,
    Count,
}
public enum InputStatus
{
    Started,
    Performed,
    Canceled,
    Count,
}
public enum ActionType
{
    Idle,
    Hit,
    Die,
    Move,
    Attack,
    Hide,
    Skill,
    Potion,
    Interact,
    Count,
}

public enum Effect
{
    Heal,
    Hit,
    Hide,
    Count,
    None
}
public enum StateLayer
{
    Effects=Effect.Count, // heal, hit, hide
    Move, // idle, move
    Attack, // attack, skill
    Die, // die
    None,
}
public enum SPUM_AnimClipList
{
    idle,
    Run,
    Attack_Normal,
    Death,
    Skill_Normal,
}

public enum AxisOptions
{
    Both,
    Horizontal,
    Vertical 
}
#endregion

public enum StatSpecies
{
    LV,
    Name,
    HP,
    MaxHP,
    plusHP,
    ATKPower,
    plusATKPower,
    ATKRate,
    plusATKRate,
    Defence,
    plusDefence,
    Speed,
    plusSpeed,
    Exp,
    MaxExp,
    HealAmount,
    
    HPLevel,
    ATKPowerLevel,
    ATKRateLevel,
    SpeedLevel,
    
    stageLV,
    stageMaxLV,
    stageExp,
    stageMaxExp,
}

public enum StatLevel
{
    Hp,
    Atkpower,
    //AtkRate,
    Speed,
    Idx
}

// This file contains all the enums
public enum UIEvent
{
    Click,
    PointerDown,
    PointerUp,
    BeginDrag,
    Drag,
    EndDrag,
    Count
}

public enum SceneType
{
    StartScene,
    Tutorial,
    MaintenanceScene,
    MaintenanceScene_SH,
    BattleScene,
    Count,
    EndingScene
}

#region Sound Assets
public enum Sounds
{
    BGM,
    Battle,
    Start1,
    Start2,
    Start3,
    Boom,
    Smoke,
    Bullet,
    TankOn,
    TankMove,
    DestroyOn1,
    DestroyOn2,
    DestroyOn3,
    DestroyOn4,
    HitOn1,
    HitOn2,
    HitOn3,
    Click,
    Count
}
public enum Clips
{
    BGM,
    Battle,
    Start1,
    Start2,
    Start3,
    Boom,
    Smoke,
    Bullet,
    TankOn,
    TankMove,
    DestroyOn1,
    DestroyOn2,
    DestroyOn3,
    DestroyOn4,
    HitOn1,
    HitOn2,
    HitOn3,
    Click,
    Count
}
#endregion
public enum Tags
{
    Player,
    StartLine,
    EndLine,
    Map,
    UI,
    Obstacle,
    Bullets,
    Wire,
    Destroyer,
    Enemy,
    Count,
}
public enum LayerMasks
{
    Count,
}

public enum BuffType
{
    SPEED_UP,
}
public enum Sound
{
    Bgm,
    Effect,
    MaxCount,
}



