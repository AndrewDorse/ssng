using Photon.Pun;
using Silversong.Game.Statistics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Silversong.Game
{
    public class GameMaster : MonoBehaviour
    {
        public static GameMaster instance;

        public List<OtherHero> OtherHeroes;
        public List<Enemy> Enemies => _enemiesController.Enemies;
        public LocalHero LocalHero { get; private set; }





        [SerializeField] private PlayerCreator _playerCreator;
        [SerializeField] private EnemiesController _enemiesController;
        private StatisticsController _statisticsController;

        private OtherHeroesController _otherHeroesController = new OtherHeroesController();


        private void Awake()
        {
            instance = this;


        }

        public void LevelStart(List<PlayerData> playersData) // enemies info here???? TODO
        {
            LocalHero = CreateLocalHero(DataController.instance.GetMyHeroData());

            OtherHeroes = CreateOtherHeroes(playersData);

            EventsProvider.ThreeTimesPerSecond += SendLocalHeroDataToOthers;

            _statisticsController = new StatisticsController();


            if (PhotonNetwork.IsMasterClient)
            {
                _enemiesController.TEMPStart(); // TODO TEMP!!!!!
            }
            else
            {
                GameRPCController.instance.AskMasterForEnemiesInfo();
            }


            EventsProvider.OnDeathOfAllEnemies += OnDeathOfAllEnemies;
        }

        private void LevelEnd()
        {

        }


        public EnemiesData GetEnemiesData()
        {
            return _enemiesController.GetEnemiesData();
        }


        public List<PlayerStatisticsSlot> GetLevelStatistics()
        {
            return _statisticsController.GetLevelStatistics();
        }

















        private LocalHero CreateLocalHero(HeroData heroData)
        {
            return _playerCreator.CreateLocalHero(heroData);
        }

        private List<OtherHero> CreateOtherHeroes(List<PlayerData> playersData)
        {
            List<OtherHero> result = new List<OtherHero>();

            foreach (PlayerData data in playersData)
            {
                if (data.userId != DataController.instance.LocalData.UserId)
                {
                    result.Add(_playerCreator.CreateOtherHero(data.heroData, data.userId, data.nickname));
                }
            }

            return result;
        }

        private void SendLocalHeroDataToOthers() //TODO Check it
        {
            GameRPCController.instance.UpdateLocalHeroPosition(LocalHero);
        }




        private void OnDeathOfAllEnemies()
        {
            // temp   // change scene on exit collider





            LevelDispose();
            Master.instance.ChangeGameStage(Enums.GameStage.statistics);
           
        }


        private void LevelDispose()  // on exit level
        {
            EventsProvider.ThreeTimesPerSecond -= SendLocalHeroDataToOthers;
            EventsProvider.OnDeathOfAllEnemies -= OnDeathOfAllEnemies;
        }


    }
}