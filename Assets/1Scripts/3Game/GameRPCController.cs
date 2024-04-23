using Photon.Pun;
using Silversong.Game;
using Silversong.Game.Statistics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRPCController : MonoBehaviourPun
{
    public static GameRPCController instance;


    private void Awake()
    {
        instance = this;
    }

    public void UpdateLocalHeroPosition(LocalHero localHero)
    {
        HeroInfoRPC data = new HeroInfoRPC(localHero);

        photonView.RPC("UpdateLocalHeroPositionRPC", RpcTarget.Others, JsonUtility.ToJson(data));
    }

    [PunRPC]
    private void UpdateLocalHeroPositionRPC(string data)
    {
        HeroInfoRPC info = JsonUtility.FromJson<HeroInfoRPC>(data);

        EventsProvider.OnOtherHeroInfoRpcRecieved.Invoke(info);
    }



    // Enemies
    public void AskMasterForEnemiesInfo()
    {
        photonView.RPC("AskMasterForEnemiesInfoRPC", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void AskMasterForEnemiesInfoRPC()
    {
        SendEnemiesDataToOthers(GameMaster.instance.GetEnemiesData());
    }

    public void SendEnemiesDataToOthers(EnemiesData data)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            string str = JsonUtility.ToJson(data);
            photonView.RPC("SendEnemiesDataToOthersRPC", RpcTarget.Others, str);
        }
    }
    [PunRPC]
    private void SendEnemiesDataToOthersRPC(string data)
    {
        EventsProvider.OnEnemiesDataRpcRecieved?.Invoke(JsonUtility.FromJson<EnemiesData>(data).data);
    }



    // hero hit enemy
    public void SendLocalNoMasterDamageDataToMaster(string data)
    {
        photonView.RPC("SendLocalNoMasterDamageDataToMasterRPC", RpcTarget.MasterClient, data);

    }
    [PunRPC]
    private void SendLocalNoMasterDamageDataToMasterRPC(string data)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            EventsProvider.OnAllEnemiesRecievedDamageDataRpc?.Invoke(JsonUtility.FromJson<AllEnemiesRecievedDamageData>(data));
        }
    }


    // enemy death
    public void SendEnemyDeathToOthers(string enemyId)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SendEnemyDeathToOthersRPC", RpcTarget.Others, enemyId);
        }
    }
    [PunRPC]
    private void SendEnemyDeathToOthersRPC(string data)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            EventsProvider.OnEnemyDeathRpcRecieved?.Invoke(data);
        }
    }




    // statistics
    public void SendLocalStatisticsToOthers(string data)
    {
        photonView.RPC("SendLocalStatisticsToOthersRPC", RpcTarget.Others, data);
    }
    [PunRPC]
    private void SendLocalStatisticsToOthersRPC(string data)
    {
        EventsProvider.OnStatisticsRpcRecieved?.Invoke(JsonUtility.FromJson<LevelStatisticsData>(data));
    }

    
}








    [System.Serializable]
public class HeroInfoRPC
{
    public string userId;
    public Vector3 position;
    public Vector3 direction;

    public float healthPc;
    public float manaPc;

    // other efects? states? stun frozen etc?


    public HeroInfoRPC(LocalHero localHero)
    {
        userId = DataController.instance.LocalData.UserId;
        position = localHero.gameObject.transform.position;
        direction = localHero.gameObject.transform.forward;

        healthPc = 1;
        manaPc = 1;
    }
}


    