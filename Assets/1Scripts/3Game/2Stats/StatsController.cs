using Silversong.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class StatsController
    {

        public float CurrentHp { get; private set; }
        public float CurrentMp { get; private set; }

        private StatsHolder _stats;
        private Action<string, float, bool> _onHpReducedCallback;


        public StatsController(HeroClass heroClass, Subrace subrace)
        {
            _stats = new StatsHolder(heroClass.attribute);

            // apply hero class and subrace
            // ApplyStatList(heroClass.stats on choose);
        }

        public StatsController(EnemyData enemyData, Action<string, float, bool> onHpChangeCallback)
        {
            _stats = new StatsHolder();

            CurrentHp = _stats.stats[0];
            CurrentMp = _stats.stats[2];

            _onHpReducedCallback = onHpChangeCallback;

            // apply enemy stats list + level increase
        }



        public float GetStat(Enums.Stats stat)
        {
            return _stats.stats[(int)stat];
        }



        public void ReduceHealth(string attackingId, float damage)
        {
            CurrentHp -= damage;

            _onHpReducedCallback?.Invoke(attackingId, damage, false);
            Debug.Log("#ReduceHealth#  hp = " + CurrentHp);
        }

        public void ReduceHealthFromRpc(float damage)
        {
            CurrentHp -= damage;
            _onHpReducedCallback?.Invoke("", damage, true);
        }




















        private void ChangeStat(int statId, float value) // + value
        {
            _stats.stats[statId] += value;
        }




        private void ApplyStatList(StatSlot[] list, Enums.StatType type)
        {
            int offset = GetOffsetAmount(type);

            foreach (StatSlot slot in list)
            {
                ChangeStat((int)slot.stat, slot.value);
            }

            _stats.CalculateStats();
        }

        private int GetOffsetAmount(Enums.StatType type)
        {
            int result = 100;

            if (type == Enums.StatType.basic)
            {
                result = 100;
            }
            else if (type == Enums.StatType.passive)
            {
                result = 200;
            }
            else if (type == Enums.StatType.item)
            {
                result = 300;
            }
            else if (type == Enums.StatType.buff)
            {
                result = 400;
            }

            return result;
        }









    }




    



[System.Serializable]
public class StatSlot
{
    public Enums.Stats stat;
    public float value;
}