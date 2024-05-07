using Photon.Pun;
using Silversong.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Silversong.Game
{
    public class EnemiesController : MonoBehaviour
    {

        public List<Enemy> Enemies { get => _enemies; private set => _enemies = value; }

        private EnemiesIncomingDamageController _damageController;
        private List<Enemy> _enemies = new List<Enemy>();


        private EnemiesCreationHelper _enemiesCreationHelper;


        [SerializeField] private Enemy _prefab;



        private void Start()
        {
            EventsProvider.OnEnemiesDataRpcRecieved += SetEnemies;
            EventsProvider.OnLevelStart += OnLevelStarted;
            // level end


            EventsProvider.OnBecomeMaster += SubscribeAsMaster;
            EventsProvider.OnBuffDataRpcRecieved += ApplyBuffDataFromRpc;

            EventsProvider.OnEnemyDeathRpcRecieved += OnEnemyDeath;


            _enemiesCreationHelper = new EnemiesCreationHelper();
            _damageController = new EnemiesIncomingDamageController();
        }


        private void ApplyBuffDataFromRpc(BuffDataRPCSlot data)
        {
            for(int i = 0; i < data.Targets.Length; i++)
            {
                Enemy enemy = GetEnemyById(data.Targets[i]);

                BuffSlot buffSlot = new BuffSlot(InfoProvider.instance.GetBuff(data.Id), data.Level);

                enemy.GetStatsController().ApplyBuff(buffSlot);

            }




        }



        private void OnLevelStarted()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SubscribeAsMaster();
            }
        }

        private void SubscribeAsMaster()
        {
            EventsProvider.ThreeTimesPerSecond += UpdateEnemiesInfoForOthers;
        }

        private void UpdateEnemiesInfoForOthers()
        {

            if (PhotonNetwork.IsMasterClient)
            {
                GameRPCController.instance.SendEnemiesDataToOthers(GetCurrentEnemiesDataForRPC());
            }
        }





        public void CreateEnemies()
        {
            EnemiesData data = _enemiesCreationHelper.GenerateEnemiesData();

            SetEnemies(data.data);
        }

        public void CreateStoryEnemies(StoryOption storyOption)
        {
            EnemiesData data = _enemiesCreationHelper.GenerateDataForStoryOption(storyOption);

            SetEnemies(data.data);
        }





        public EnemiesData GetCurrentEnemiesDataForRPC()
        {
            List<EnemyData> data = new List<EnemyData>();

            foreach (Enemy enemy in _enemies)
            {
                data.Add(new EnemyData(enemy));
            }

            return new EnemiesData(data);
        }

        private void OnEnemyDeath(string enemyId, string killerId)
        {
            foreach (Enemy enemy in _enemies)
            {
                if (enemy.Id == enemyId)
                {
                    enemy.DeathFromMaster();
                    _enemies.Remove(enemy);

                    if(_enemies.Count == 0) // also check line to create mobs -  TODO !!!!!
                    {
                        EventsProvider.OnDeathOfAllEnemies?.Invoke();
                    }

                    break;
                }
            }
        }

        private Enemy GetEnemyById(string id)
        {
            foreach (Enemy enemy in _enemies)
            {
                if(enemy.Id == id)
                {
                    return enemy;
                }
            }

            return null;
        }




















        


        public void SetEnemies(List<EnemyData> data)
        {
            // check on level started?

            CheckEnemies();

            // check enemies we have

            List<Enemy> currentEnemies = CopyList(_enemies);
            List<Enemy> enemiesToRemove = new List<Enemy>();
            List<EnemyData> enemiesToCreate = data;

            for (int i = 0; i < currentEnemies.Count; i++)
            {
                Enemy enemy = currentEnemies[i];

                foreach (EnemyData enemyData in data)
                {
                    if (enemy.Id == enemyData.id)
                    {
                        enemy.UpdateInfo(enemyData);
                        currentEnemies.Remove(enemy);
                        enemiesToCreate.Remove(enemyData);
                        i--;
                        break;
                    }
                }
            }

            enemiesToRemove = currentEnemies;




            CreateEnemies(enemiesToCreate);
        }

        private List<Enemy> CopyList(List<Enemy> list)
        {
            List<Enemy> newList = new List<Enemy>();

            foreach (Enemy enemy in list)
            {
                newList.Add(enemy);
            }

            return newList;
        }

        private void CheckEnemies()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i] == null)
                {
                    _enemies.Remove(_enemies[i]);
                    i--;
                }
            }

        }

        private void CreateEnemies(List<EnemyData> data)
        {
            foreach (EnemyData enemyData in data)
            {
                Enemy enemy = Instantiate(_prefab, enemyData.position, Quaternion.identity);

                EnemyModelController modelController = Instantiate(InfoProvider.instance.GetMob(enemyData.mobId).Prefab, enemy.transform);


                enemy.Setup(enemyData, modelController);
                _enemies.Add(enemy);
            }
        }

    }























    [System.Serializable]
    public class EnemyData
    {
        public int mobId; // 

        public string id; // in game

        public float health;
        public float healthPc;
        public float manaPc;

        public Vector3 position;

        public string targetId;

        public EnemyData(Enemy enemy)
        {
            id = enemy.Id;
            position = enemy.transform.position;
            health = enemy.GetStatsController().CurrentHp;
            healthPc = health / enemy.GetStatsController().GetStat(Enums.Stats.hp);
        }

        public EnemyData()
        { }
    }

    [System.Serializable]
    public class EnemiesData
    {
        public List<EnemyData> data;

        public EnemiesData(List<EnemyData> data)
        {
            this.data = data;
        }
    }

















    public class EnemiesCreationHelper
    {
        private int _number;



        public EnemiesCreationHelper()
        {

        }


        public EnemiesData GenerateEnemiesData() 
        {
            int enemiesAmount = GetTotalSpawnAmount();


            List<EnemyData> list = new List<EnemyData>();
            _number = 0;

            for (int i = 0; i < enemiesAmount; i++)
            {
                EnemyData data = new EnemyData();

                data.mobId = 1;
                data.position = new Vector3(3, 0, 3 + i); // get spawn points // normal/boss/ambush
                data.targetId = string.Empty;
                data.id = _number.ToString();
                _number++;

                list.Add(data);
            }

            return new EnemiesData(list);
        }

        public EnemiesData GenerateDataForStoryOption(StoryOption storyOption)
        {
            List<EnemyData> list = new List<EnemyData>();

            _number = 0;

            for (int i = 0; i < storyOption.BattleSlot.mobs.Count; i++)
            {
                EnemyData data = new EnemyData();

                data.mobId = storyOption.BattleSlot.mobs[i].Id;
                data.position = new Vector3(3, 0, 3 + i);
                data.targetId = string.Empty;
                data.id = _number.ToString();
                _number++;

                list.Add(data);
            }

            return new EnemiesData(list);
        }




        private int GetTotalSpawnAmount()
        {
            return 4;
        }
    }


}