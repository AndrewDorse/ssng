using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter 
{
    
    public void Initialize()
    {
        EventsProvider.OnPlayersDataChanged += CheckIfAllPlayersAreReady;

    }


    private void CheckIfAllPlayersAreReady(List<PlayerData> playersData)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (DataController.instance.GameData.gameStage != Enums.ServerGameStage.creatingHeroes)
        {
            return;
        }

        if (playersData.Count == 0)
        {
            return;
        }

        foreach(PlayerData player in playersData)
        {
            if(player.ready == false)
            {
                return;
            }
        }


        DataController.instance.GameData.gameStage = Enums.ServerGameStage.gameLevel;
        StartGame();
    }

    private void StartGame()
    {
        EventsProvider.OnPlayersDataChanged -= CheckIfAllPlayersAreReady;

        EventsProvider.OnGameStart?.Invoke();

        MainRPCController.instance.SendStartGameInfo();

        Master.instance.ChangeGameStage(Enums.GameStage.game);
        
    }


}
