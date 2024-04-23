using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums 
{

    // MAIN
    public enum GameStage { launch ,loading, login, menu, inRoom, heroCreation, 
        
        game, statistics, rewards, heroUpgrade, 
        
        camp, inventory, talents, campHeroInfo 
    
    }
    public enum ServerGameStage { creatingHeroes, gameLevel, camp, finished }

    public enum HeroCreationPage { heroClass, race, origin, startItem, overview }





    //statistics
    public enum StatisticsType { damage, tank, heal, control, caster }





    // hero
    public enum Races { human, elf, orc, dwarf }
    public enum ClassHelmTypes : byte { none, openFaceHelm, mask, fullHelmet }




    // animator
    public enum Animations
    {
        Idle, Run, Fight, FightRange, Cast, // bool
        CastTaunt = 10, CastJumpAttack, CastWhirlAttack, CastArea, CastShieldAttack, CastProjectile, CastHandUp, CastSpell, DrinkPotion,      // trigger
        Stun = 50, Frozen, Dead, Knocked // bool control
    }
    public enum AnimatorStates { normal, casting, underControl, dead, playingSpecialAnimation }






    // Universal Button
    public enum UniversalButtonType : byte
    {
        available, unavailable, choosen,  // nfts
        noPlace, notEnoughMoney, canBuy, takeFromLoot, putToLoot, canSell, cannotSell, // items


        backButton // info
    }








    // Inventory
    public enum Rarity : byte { common, uncommon, rare, epic, legendary }












    // stats 
    public enum StatType {  summ, basic, passive, item, buff }

    public enum MainAttributes : byte { strength, agility, intelligence, 
        strengthAgility, strengthIntelligence, agilityIntelligence,
        strengthAgilityIntelligence }

    public enum Stats : byte
    {
        hp, hpPc, mp, mpPc,

        strength, strengthPc, agility, agilityPc, intelligence, intelligencePc, stamina, staminaPc, spirit, spiritPc, charisma, charismaPc, luck, luckPc,

        coeffAllDamage = 18, coeffAllArmor, coeffAllAttackSpeed, coeffAllMovementSpeed, coeffAllSpellSpeed, coeffAllIncomingDamage, coeffAllIncomingHeal,

        meleeAttack = 25, meleeAttackPc, rangeAttack, rangeAttackPc, attackSpeedFinal, attackSpeedRate, masteryFinal, masteryRate,
        critChanceFinalPc, critChanceRate, critDamageFinalPc, critDamageRate, armorPenetrationFinal, armorPenetrationRate,

        defensePc, armor, armorPc, blockFinalPc, blockRate, evadeFinalPc, evadeRate, parryFinalPc, parryRate, hpRegeneration,

        spellPower, spellPowerPc, spellDamage, spellDamagePc, spellHeal, spellHealPc, spellCritChanceFinalPc, spellCritChanceRate, spellCritDamageFinalPc,
        spellCritDamageRate, spellSpeedRate, spellSpeedFinal, magicDefenseFinalPc, magicDefenseRate, concentrationRate,
        spellPenetrationFinalPc, spellPenetrationRate, mpRegeneration,

        firePowerPc, icePowerPc, electricPowerPc, arcanePowerPc, darkArtsPowerPc, lightPowerPc, poisonPowerPc, bloodShedPowerPc,

        fireResistPc, iceResistPc, electricResistPc, arcaneResistPc, darkArtsResistPc, lightResistPc, poisonResistPc, bloodShedResistPc,

        physicalResilience, magicResilience, mentalResilience, damageReturnMelee,

        //
        accuracy,

        meleeDamage = 90, rangeDamage = 91,

        //
        stealth, backstabDamage, damageFromFront, damageFromBack, lifestealPc, spellLifestealPc, statusResistance, movementSpeed,
        //

        none = 100
    }

















}
