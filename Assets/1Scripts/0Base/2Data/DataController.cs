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
    }



    private void OnLevelStart()
    {
        GameMaster.instance.LevelStart(_allPlayersData);
        GameData.started = true;
        GameData.gameStage = Enums.ServerGameStage.gameLevel;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
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
        _allPlayersData = playersData;
        EventsProvider.OnPlayersDataChanged?.Invoke(_allPlayersData);
    }

    public string GetPlayersDataForRPC()
    {
        string result = string.Empty;

        result = JsonUtility.ToJson(new PlayersDataRPC(_allPlayersData, GameData));

        return result;
    }

    public string GetMyHeroDataForRPC()
    {
        string result = string.Empty;

        result = JsonUtility.ToJson(new HeroDataRPC(LocalData.UserId, GetMyHeroData()));

        return result;
    }

    public HeroData GetMyHeroData()
    {
        return GetMyPlayerData().heroData;
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
        GetMyPlayerData().ready = value;
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





   

}








[System.Serializable]
public class HeroData
{
    public int level;
    public int classId;
    public int raceId;
    public int originId;
    public int startItemId;

    public HeroData(int level, int classId, int raceId, int originId, int startItemId)
    {
        this.level = level;
        this.classId = classId;
        this.raceId = raceId;
        this.originId = originId;
        this.startItemId = startItemId;
    }

    public HeroData()
    {
        this.level = -1;
        this.classId = -1;
        this.raceId = -1;
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