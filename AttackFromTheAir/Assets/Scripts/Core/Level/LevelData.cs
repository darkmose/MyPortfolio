using Core.Buildings;
using Core.GameLogic;
using Core.Tools;
using Core.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Level
{
    public enum GameMode
    {
        Attack,
        Defence,
        Survival
    }

    public enum AttackGameMode
    {
        PrisonerRelease,
        FlyingOverEnemy,
        BreakTheConvoy,
        BattleWithShips,
        ObjectProtection,
        SearchAndDestroy,
        DestroyingEnemyBase,
        CombatSupport
    }

    public enum DefenceGameMode
    {
        SimpleDefence,
        WavesDefence,
        TimeDefence
    }

    public enum GameEventType
    {
        EnemyInfantryKilled,
        EnemyMediumEquipmentDestroyed,
        EnemyHeavyEquipmentDestroyed,
        AllyInfantryKilled,
        AllyMediumEquipmentDestroyed,
        AllyHeavyEquipmentDestroyed,
        TimerEnd,
        EnemyBuildingDestroyed,
        PlayerHealthDown,
        TestTimeDelay,
        PrisonerKilled,
        AllyBuildingDestroyed
    }

    public class LevelData : MonoBehaviour
    {
        public Transform PlayerStartPoint;
        public List<UnitSpawnPointsDescriptor> UnitSpawnPointsDescriptors;
        public List<PatrolPointsDescriptor> PatrolPointsDescriptors;
        public List<BuildingViewsDescriptor> BuildingViewsDescriptors;
        public List<SafePointsDescriptor> SafePointsDescriptors;
    }

    [System.Serializable]
    public class BuildingViewsDescriptor
    {
        public UnitFraction UnitFraction;
        public List<BaseBuildingView> Buildings;
    }

    [System.Serializable]
    public class UnitSpawnPointsDescriptor
    {
        public UnitSpawnerType UnitSpawnerType;
        public InfantryType InfantryType;
        public MediumEquipmentType MediumEquipmentType;
        public HeavyEquipmentType HeavyEquipmentType;
        public UnitFraction UnitFraction;
        public UnitInitStateDescriptor InitStateDescriptor;
        public List<Transform> SpawnPoints;
    }

    [System.Serializable]
    public class UnitInitStateDescriptor
    {
        public UnitStates UnitInitState;
        public TriggerToUnitStateConfiguration TriggerToUnitStateMatrix;
    }

    [System.Serializable]
    public class SafePointsDescriptor
    {
        public UnitFraction UnitFraction;
        public List<Transform> SafePoints;
    }

    [System.Serializable]
    public class PatrolPointsDescriptor
    {
        [Range(3f, 30f)]
        public float StayDurationMin = 5f;
        [Range(3f, 30f)]
        public float StayDurationMax = 10f;
        public UnitFraction UnitsFraction;
        public List<Transform> PatrolPoints;
    }
}