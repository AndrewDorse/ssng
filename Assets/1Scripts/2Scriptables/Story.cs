using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Story")]

public class Story : ScriptableObject
{
    public int Id = 0;
    public string Text;
    public Sprite Icon;


    public StoryOption[] Options;

}



[System.Serializable]
public class StoryOption
{
    public string Text;
    public Sprite Icon;

    public Enums.Tag RequiredTag;

    public RewardSlot[] RewardSlot;

    public BattleSlot BattleSlot;
}


[System.Serializable]
public class RewardSlot
{
    public Enums.RewardType RewardType;
    public Enums.Tag RequiredTag;
    public int Value;

}


[System.Serializable]
public class BattleSlot
{
    public List<Mob> mobs;
}