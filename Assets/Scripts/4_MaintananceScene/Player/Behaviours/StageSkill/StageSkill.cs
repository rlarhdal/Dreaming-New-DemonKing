using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageSkill
{
    public Sprite skillChoiceImage;
    public string skillName;
    public string skillDescription;
}

public abstract class StageSkillPassive : StageSkill
{
    PlayerStatHandler statHandler;
    (StatSpecies statSpecies, float value) enhanceStatInfo;

    protected abstract void ApplyStat();
}

public class StageSkillRelatedToAttack : StageSkill
{
}