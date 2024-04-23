using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



namespace Silversong.Game
{
    public class Enemy : MonoBehaviour, ITarget
    {
        [field: SerializeField] public string Id { get; private set; }

        [field: SerializeField] public string TargetId { get; private set; }
        private ITarget _target;


        [SerializeField] private HealthBar _healthBar;

        private StatsController _stats;


        [SerializeField] private EnemyMovementController _movementController;





        public void Setup(EnemyData enemyData)
        {
            Id = enemyData.id;

            _stats = new StatsController(enemyData, OnHpReduced);

            EventsProvider.TenTimesPerSecond += Tick;
        }



        public void UpdateInfo(EnemyData enemyData)
        {
            _movementController.UpdatePosition(enemyData.position);

            TargetId = enemyData.targetId;
            _target = TargetProvider.GetAllyTargetByTargetId(TargetId).Item1;

            _healthBar.SetValue(enemyData.healthPc);
        }

        public void SetTarget(string targetId)
        {
            TargetId = targetId;
            _target = TargetProvider.GetAllyTargetByTargetId(targetId).Item1;
            _movementController.Target = _target;
        }







        private void Tick()
        {
            if (TargetId == string.Empty)
            {
                (_target, TargetId) = TargetProvider.GetClosestAlly(this);

                if (_target == null)
                {
                    return;
                }

                _movementController.Target = _target;
            }
        }

        private void OnHpReduced(string attackingId, float value, bool fromRpc)
        {
            if (!fromRpc)
            {
                EventsProvider.OnEnemyRecievedDamage?.Invoke(attackingId, Id, value);
            }

            // passives here too??

            _healthBar.SetValue(_stats.CurrentHp / 100f); // change it to coroutine? to change hbar smoothly??? TODO check


            if (_stats.CurrentHp <= 0)
            {
                DeathLocal();
            }
        }













        public void DeathFromMaster()
        {
            Dispose();
            Destroy(gameObject);
        }


        private void DeathLocal()
        {
            if (PhotonNetwork.IsMasterClient == true)
            {
                GameRPCController.instance.SendEnemyDeathToOthers(Id);
                EventsProvider.OnEnemyDeathRpcRecieved(Id);
            }
        }


        private void Dispose()
        {
            //  _agroController.Dispose();
            //_stats.dispose
            EventsProvider.TenTimesPerSecond -= Tick;
        }


        private void OnDestroy()
        {
            Dispose();
        }















        // interface
        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public StatsController GetStatsController()
        {
            return _stats;
        }
    }
}