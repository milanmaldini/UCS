using System.Collections.Generic;
using UCS.Core;

namespace UCS.GameFiles
{
    class CharacterData : CombatItemData
    {
        public CharacterData(CSVRow row, DataTable dt)
            : base(row, dt)
        {
            LoadData(this, GetType(), row);
        }

        public string TID { get; set; }
        public string InfoTID { get; set; }
        public string SWF { get; set; }
        public int HousingSpace { get; set; }
        public int BarrackLevel { get; set; }
        public List<int> LaboratoryLevel { get; set; }
        public int Speed { get; set; }
        public int Hitpoints { get; set; }
        public List<int> TrainingTime { get; set; }
        public string TrainingResource { get; set; }
        public List<int> TrainingCost { get; set; }
        public List<int> UpgradeTimeH { get; set; }
        public List<string> UpgradeResource { get; set; }
        public List<int> UpgradeCost { get; set; }
        public int AttackRange { get; set; }
        public int AttackSpeed { get; set; }
        public int Damage { get; set; }
        public int PreferedTargetDamageMod { get; set; }
        public int DamageRadius { get; set; }
        public bool AreaDamageIgnoresWalls { get; set; }
        public bool SelfAsAoeCenter { get; set; }
        public string IconSWF { get; set; }
        public string IconExportName { get; set; }
        public string BigPicture { get; set; }
        public string BigPictureSWF { get; set; }
        public string Projectile { get; set; }
        public string PreferedTargetBuilding { get; set; }
        public string PreferedTargetBuildingClass { get; set; }
        public string DeployEffect { get; set; }
        public string AttackEffect { get; set; }
        public string HitEffect { get; set; }
        public bool IsFlying { get; set; }
        public bool AirTargets { get; set; }
        public bool GroundTargets { get; set; }
        public int AttackCount { get; set; }
        public string DieEffect { get; set; }
        public string Animation { get; set; }
        public int UnitOfType { get; set; }
        public bool IsJumper { get; set; }
        public int MovementOffsetAmount { get; set; }
        public int MovementOffsetSpeed { get; set; }
        public string TombStone { get; set; }
        public int DieDamage { get; set; }
        public int DieDamageRadius { get; set; }
        public string DieDamageEffect { get; set; }
        public int DieDamageDelay { get; set; }
        public bool DisableProduction { get; set; }
        public string SecondaryTroop { get; set; }
        public bool IsSecondaryTroop { get; set; }
        public int SecondaryTroopCnt { get; set; }
        public int SecondarySpawnDist { get; set; }
        public bool RandomizeSecSpawnDist { get; set; }
        public bool PickNewTargetAfterPushback { get; set; }
        public int PushbackSpeed { get; set; }
        public string SummonTroop { get; set; }
        public int SummonTroopCount { get; set; }
        public int SummonCooldown { get; set; }
        public string SummonEffect { get; set; }
        public int SummonLimit { get; set; }
        public int SpawnIdle { get; set; }
        public int StrengthWeight { get; set; }
        public string ChildTroop { get; set; }
        public int ChildTroopCount { get; set; }
        public int SpeedDecreasePerChildTroopLost { get; set; }
        public int ChildTroop0_X { get; set; }
        public int ChildTroop0_Y { get; set; }
        public int ChildTroop1_X { get; set; }
        public int ChildTroop1_Y { get; set; }
        public int ChildTroop2_X { get; set; }
        public int ChildTroop2_Y { get; set; }
        public bool AttackMultipleBuildings { get; set; }
        public bool IncreasingDamage { get; set; }
        public int DamageLv2 { get; set; }
        public int DamageLv3 { get; set; }
        public int DamageLv4 { get; set; }
        public int Lv2SwitchHits { get; set; }
        public int Lv3SwitchHits { get; set; }
        public int Lv4SwitchHits { get; set; }
        public int AttackSpeedLv2 { get; set; }
        public int AttackSpeedLv3 { get; set; }
        public int AttackSpeedLv4 { get; set; }
        public string AttackEffectLv2 { get; set; }
        public string AttackEffectLv3 { get; set; }
        public string AttackEffectLv4 { get; set; }
        public string TransitionEffectLv2 { get; set; }
        public string TransitionEffectLv3 { get; set; }
        public string TransitionEffectLv4 { get; set; }
        public int HitEffectOffset { get; set; }
        public int TargetedEffectOffset { get; set; }
        public int SecondarySpawnOffset { get; set; }

        public override int GetHousingSpace()
        {
            return HousingSpace;
        }

        public override int GetCombatItemType()
        {
            return 0;
        }

        public override int GetRequiredLaboratoryLevel(int level)
        {
            return LaboratoryLevel[level];
        }

        public override int GetRequiredProductionHouseLevel()
        {
            return BarrackLevel;
        }

        public override int GetTrainingTime(int level)
        {
            return TrainingTime[level];
        }

        public override int GetTrainingCost(int level)
        {
            return TrainingCost[level];
        }

        public override ResourceData GetTrainingResource()
        {
            return ObjectManager.DataTables.GetResourceByName(TrainingResource);
        }

        public override int GetUpgradeCost(int level)
        {
            return UpgradeCost[level];
        }

        public override int GetUpgradeLevelCount()
        {
            return UpgradeCost.Count;
        }

        public override ResourceData GetUpgradeResource(int level)
        {
            return ObjectManager.DataTables.GetResourceByName(UpgradeResource[level]);
        }

        public override int GetUpgradeTime(int level)
        {
            return UpgradeTimeH[level]*3600;
        }
    }
}