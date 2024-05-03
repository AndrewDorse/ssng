using Photon.Pun;
using Photon.Realtime;
using Silversong.Game;
using System.Collections.Generic;
using UnityEngine;

public  class DataController : MonoBehaviour // remove mono TODO ??
{
    public static DataController instance;

    public LocalData LocalData;
    public GameData GameData;


    [SerializeField] private List<PlayerData> _allPlayersData;

    public PlayerData LocalPlayerData => GetMyPlayerData();




    private void Awake()
    {
        instance = this;

        // load / ini
        _allPlayersData = new List<PlayerData>();

        LocalData = SaveController.LoadLocalData();
        GameData = new GameData();


        EventsProvider.OnOtherPlayerEnteredRoom += TryToAddPlayer;
        EventsProvider.OnOtherPlayerLeftRoom += TryToRemovePlayer;
        EventsProvider.OnHeroDataChanged += OnLocalPlayerHeroDataChanged;
        EventsProvider.OnLeftRoom += ClearPlayersData;

        EventsProvider.OnLevelStart += OnLevelStart;
        EventsProvider.OnLevelEnd += OnLevelEnd;
    }



    private void OnLevelStart()
    {
        GameMaster.instance.LevelStart(_allPlayersData);
        GameData.started = true;
        GameData.gameStage = Enums.ServerGameStage.gameLevel;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;

            foreach (PlayerData data in _allPlayersData)
            {
                data.ready = false;
            }
        }
    }

    private void OnLevelEnd()
    {
        GameData.gameStage = Enums.ServerGameStage.camp;
    }




    private void ClearPlayersData()
    {
        _allPlayersData = new List<PlayerData>();
    }

    public void TryToAddPlayer(Player newPlayer)
    {
        foreach (PlayerData player in _allPlayersData)
        {
            if(player.nickname == newPlayer.NickName) // SHOULD BE some silversong id
            {
                player.userId = newPlayer.UserId;

                EventsProvider.OnPlayersDataChanged?.Invoke(_allPlayersData);

                return;
            }
        }



        _allPlayersData.Add(new PlayerData(newPlayer));

        EventsProvider.OnPlayersDataChanged?.Invoke(_allPlayersData);
    }

    public void TryToRemovePlayer(Player player)
    {
        for (int i = 0; i < _allPlayersData.Count; i++)
        {
            if (_allPlayersData[i].userId == player.UserId)
            {
                if (GameData.started)
                {
                    _allPlayersData[i].active = false;
                }
                else
                {
                    _allPlayersData.Remove(_allPlayersData[i]);
                }
                break;
            }
        }

        EventsProvider.OnPlayersDataChanged?.Invoke(_allPlayersData);
    }

    public void SetAllPlayersData(List<PlayerData > playersData) // for others
    {
        if(PhotonNetwork.IsMasterClient)
        {
            return;
        }

        PlayerData localData = LocalPlayerData;

        _allPlayersData = playersData;

        for (int i = 0; i < _allPlayersData.Count; i++)
        {
            if (LocalData.UserId == _allPlayersData[i].userId)
            {
                _allPlayersData[i] = localData;
            }
        }

        EventsProvider.OnPlayersDataChanged?.Invoke(_allPlayersData);
    }

    public string GetPlayersDataForRPC()
    {
        return JsonUtility.ToJson(new PlayersDataRPC(_allPlayersData, GameData));
    }

    public string GetMyHeroDataForRPC()
    {
        return JsonUtility.ToJson(new HeroDataRPC(LocalData.UserId, GetMyHeroData()));
    }

   

    public void AskToUpdatePlayersData()
    {
        EventsProvider.OnPlayersDataChanged?.Invoke(_allPlayersData);
    }

    private void OnLocalPlayerHeroDataChanged(HeroData heroData)
    {
        ChangeHeroDataByUserId(heroData, LocalData.UserId);          
    }

    public void ChangeHeroDataByUserId(HeroData heroData, string UserId)
    {
        foreach (PlayerData data in _allPlayersData)
        {
            if (data.userId == UserId)
            {
                data.heroData = heroData;
            }
        }

        if(PhotonNetwork.IsMasterClient)
        {
            EventsProvider.OnPlayersDataChanged?.Invoke(_allPlayersData);
        }
        else
        {
            MainRPCController.instance.SendHeroDataToMaster();
        }
    }

    public void SetLocalPlayerReady(bool value = true)
    {
        LocalPlayerData.ready = value;
        EventsProvider.OnPlayersDataChanged?.Invoke(_allPlayersData);
    }

    public void SetPlayerReady(string UserId, bool value = true)
    {
        foreach (PlayerData data in _allPlayersData)
        {
            if (data.userId == UserId)
            {
                data.ready = value;
            }
        }

        EventsProvider.OnPlayersDataChanged?.Invoke(_allPlayersData);
    }













    private PlayerData GetMyPlayerData()
    {
        foreach (PlayerData data in _allPlayersData)
        {
            if (data.userId == LocalData.UserId)
            {
                return data;
            }
        }

        return null;
    }




    // to separate class TODO!!!!

    //inv
    public HeroData GetMyHeroData()
    {
        return LocalPlayerData.heroData;
    }

    public bool IsEnoughMoney(int value)
    {
        return LocalPlayerData.heroData.Gold >= value;
    }

    public bool IsEnoughPlaceInInventory()
    {
        return LocalPlayerData.heroData.items.Count >= 10;
    }

    public void AddItem(int id)
    {
        LocalPlayerData.heroData.items.Add(new ItemSlot(id));
        EventsProvider.OnHeroDataChanged?.Invoke(LocalPlayerData.heroData);
        EventsProvider.OnInventoryItemsChanged?.Invoke(LocalPlayerData.heroData.items);
    }


    //ability
    public int GetActiveAbilityLevel(int id)
    {
        foreach (ActiveAbilityDataSlot data in LocalPlayerData.heroData.activeTalents)
        {
            if (data.Id == id)
            {
                return data.Level;
            }
        }

        return 0;
    }

    public int GetPassiveAbilityLevel(int id)
    {
        foreach (PassiveAbilityDataSlot data in LocalPlayerData.heroData.PassiveAbilities)
        {
            if (data.Id == id)
            {
                return data.Level;
            }
        }

        return 0;
    }
    public bool IsEnoughPointsToLearn(int value)
    {
        return LocalPlayerData.heroData.TalentPoints >= value;
    }

    public bool IsEnoughSlotsToLearn()
    {
        return LocalPlayerData.heroData.activeTalents.Count < 10;
    }

    public bool IsRequirementsFullfilled(ActiveAbility ability)
    {
        return true;
    }

    public void AddActiveAbility(int id)
    {
        int pointsCost = InfoProvider.instance.GetAbility(id).LevelInfo[GetActiveAbilityLevel(id)].RequiredPoints;

        foreach (ActiveAbilityDataSlot slot in LocalPlayerData.heroData.activeTalents)
        {
            if(slot.Id == id)
            {
                slot.Level++;
                LocalPlayerData.heroData.TalentPoints -= pointsCost;
                EventsProvider.OnHeroDataChanged?.Invoke(LocalPlayerData.heroData);
                return;
            }
        }

        LocalPlayerData.heroData.TalentPoints -= pointsCost;
        LocalPlayerData.heroData.activeTalents.Add(new ActiveAbilityDataSlot(id, 1));
        EventsProvider.OnHeroDataChanged?.Invoke(LocalPlayerData.heroData);
    }

    public void AddPassiveAbility(int id)
    {
        PassiveAbility passive = InfoProvider.instance.GetPassive(id);

        int pointsCost = passive.LevelInfo[GetPassiveAbilityLevel(id)].RequiredPoints;

        foreach (PassiveAbilityDataSlot slot in LocalPlayerData.heroData.PassiveAbilities)
        {
            if (slot.Id == id)
            {
                slot.Level++;
                LocalPlayerData.heroData.TalentPoints -= pointsCost;
                EventsProvider.OnHeroDataChanged?.Invoke(LocalPlayerData.heroData);
                return;
            }
        }

        LocalPlayerData.heroData.TalentPoints -= pointsCost;
        LocalPlayerData.heroData.PassiveAbilities.Add(new PassiveAbilityDataSlot(id, 1));
        EventsProvider.OnHeroDataChanged?.Invoke(LocalPlayerData.heroData);


        EventsProvider.OnPassiveAbilityLearnt?.Invoke(passive);
    }
}








[System.Serializable]
public class HeroData
{
    public int level = 1;
    public int classId;
    public int SubraceId;
    public int originId;
    public int startItemId;


    public int Gold = 0;
    public int TalentPoints = 0;
    public List<ItemSlot> items; // change to slot class
    public List<ActiveAbilityDataSlot> activeTalents; // change to slot class
    public List<PassiveAbilityDataSlot> PassiveAbilities; // change to slot class

    public HeroData(int level, int classId, int raceId, int originId, int startItemId)
    {
        items = new List<ItemSlot>();
        activeTalents = new List<ActiveAbilityDataSlot>();
        PassiveAbilities = new List<PassiveAbilityDataSlot>();

        this.level = level;
        this.classId = classId;
        this.SubraceId = raceId;
        this.originId = originId;
        this.startItemId = startItemId;
    }

    public HeroData()
    {
        items = new List<ItemSlot>();
        activeTalents = new List<ActiveAbilityDataSlot>();
        PassiveAbilities = new List<PassiveAbilityDataSlot>();


        this.level = 1;
        this.classId = -1;
        this.SubraceId = -1;
        this.originId = -1;
        this.startItemId = -1;
    }
}

[System.Serializable]
public class PlayerData
{
    public string userId;
    public string nickname;

    public int heroPhotonId;

    public bool ready;
    public bool active;

    public HeroData heroData;


    public PlayerData (Player player)
    {
        userId = player.UserId;
        nickname = player.NickName;

        heroPhotonId = -1;
        ready = false;
        active = true;
        heroData = new HeroData();
    }
}


[System.Serializable]
public class LocalData
{
    public string Nickname;
    public string regionCode = "asia";
    public string UserId;
    public string OldUserId;
    public string RoomId;
    
}

[System.Serializable]
public class GameData
{
    public bool started;
    public string roomId;

    public Enums.ServerGameStage gameStage = Enums.ServerGameStage.creatingHeroes;
    public string sceneName;
    public int Level = 1;

}


[System.Serializable]
public class PlayersDataRPC
{
    public List<PlayerData> allPlayersData;
    public GameData gameData;

    public PlayersDataRPC(List<PlayerData> allPlayersData, GameData gameData)
    {
        this.gameData = gameData;
        this.allPlayersData = allPlayersData;
    }
}

[System.Serializable]
public class HeroDataRPC
{
    public string UserId;
    public HeroData heroData;

    public HeroDataRPC(string UserId, HeroData heroData)
    {
        this.UserId = UserId;
        this.heroData = heroData;
    }
}




[System.Serializable]
public class ItemSlot
{
    public int Id;

    public ItemSlot(int id)
    {
        Id = id;
    }
}

[System.Serializable]
public class ActiveAbilityDataSlot
{
    public int Id;
    public int Level;

    public ActiveAbilityDataSlot(int id, int level)
    {
        Id = id;
        Level = level;
    }
}


[System.Serializable]
public class PassiveAbilityDataSlot
{
    public int Id;
    public int Level;

    public PassiveAbilityDataSlot(int id, int level)
    {
        Id = id;
        Level = level;
    }
}