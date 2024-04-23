using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FightCalculation
{
    
    public static void CalculateDamage(StatsController attacking, StatsController attacked, string attackingId)
    {

        attacked.ReduceHealth(attackingId, 21);


    }














}
