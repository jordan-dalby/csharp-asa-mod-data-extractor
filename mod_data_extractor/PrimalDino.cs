public enum PrimalDinoStatName
{
    Health = 0,
    Stamina = 1,
    Torpidity = 2,
    Oxygen = 3,
    Food = 4,
    Water = 5,
    Temperature = 6,
    Weight = 7,
    MeleeDamageMultiplier = 8,
    SpeedMultiplier = 9,
    Fortitude = 10,
    CraftingSpeedMultiplier = 11
}

public class PrimalDinoStat
{
    public static readonly List<PrimalDinoStat> BaseStats = new List<PrimalDinoStat>()
    {
        new PrimalDinoStat()
        {
            StatName = PrimalDinoStatName.Health,
            Value = 100.0f,
            WildPerLevel = 0.2f,
            TamedPerLevel = 0.2f,
            TamingReward = 0.5f,
            EffectivenessReward = 0.0f,
            MaxGainedPerLevelUpValueIsPercent = true,
            CanLevelUpValue = true,
            DontUseValue = false
        },
        new PrimalDinoStat()
        {
            StatName = PrimalDinoStatName.Stamina,
            Value = 100.0f,
            WildPerLevel = 0.1f,
            TamedPerLevel = 0.1f,
            TamingReward = 0.0f,
            EffectivenessReward = 0.0f,
            MaxGainedPerLevelUpValueIsPercent = true,
            CanLevelUpValue = true,
            DontUseValue = false
        },
        new PrimalDinoStat()
        {
            StatName = PrimalDinoStatName.Torpidity,
            Value = 100.0f,
            WildPerLevel = 0.0f,
            TamedPerLevel = 0.0f,
            TamingReward = 0.5f,
            EffectivenessReward = 0.0f,
            MaxGainedPerLevelUpValueIsPercent = true,
            CanLevelUpValue = false,
            DontUseValue = false
        },
        new PrimalDinoStat()
        {
            StatName = PrimalDinoStatName.Oxygen,
            Value = 150.0f,
            WildPerLevel = 0.1f,
            TamedPerLevel = 0.1f,
            TamingReward = 0.0f,
            EffectivenessReward = 0.0f,
            MaxGainedPerLevelUpValueIsPercent = true,
            CanLevelUpValue = true,
            DontUseValue = false
        },
        new PrimalDinoStat()
        {
            StatName = PrimalDinoStatName.Food,
            Value = 100.0f,
            WildPerLevel = 0.1f,
            TamedPerLevel = 0.1f,
            TamingReward = 0.0f,
            EffectivenessReward = 0.0f,
            MaxGainedPerLevelUpValueIsPercent = true,
            CanLevelUpValue = true,
            DontUseValue = false
        },
        new PrimalDinoStat()
        {
            StatName = PrimalDinoStatName.Water,
            Value = 100.0f,
            WildPerLevel = 0.1f,
            TamedPerLevel = 0.1f,
            TamingReward = 0.0f,
            EffectivenessReward = 0.0f,
            MaxGainedPerLevelUpValueIsPercent = true,
            CanLevelUpValue = false,
            DontUseValue = true
        },
        new PrimalDinoStat()
        {
            StatName = PrimalDinoStatName.Temperature,
            Value = 0.0f,
            WildPerLevel = 0.0f,
            TamedPerLevel = 0.0f,
            TamingReward = 0.0f,
            EffectivenessReward = 0.0f,
            MaxGainedPerLevelUpValueIsPercent = true,
            CanLevelUpValue = false,
            DontUseValue = true
        },
        new PrimalDinoStat()
        {
            StatName = PrimalDinoStatName.Weight,
            Value = 100.0f,
            WildPerLevel = 0.02f,
            TamedPerLevel = 0.04f,
            TamingReward = 0.0f,
            EffectivenessReward = 0.0f,
            MaxGainedPerLevelUpValueIsPercent = true,
            CanLevelUpValue = true,
            DontUseValue = false
        },
        new PrimalDinoStat()
        {
            StatName = PrimalDinoStatName.MeleeDamageMultiplier,
            Value = 0.0f,
            WildPerLevel = 0.05f,
            TamedPerLevel = 0.1f,
            TamingReward = 0.5f,
            EffectivenessReward = 0.4f,
            MaxGainedPerLevelUpValueIsPercent = true,
            CanLevelUpValue = true,
            DontUseValue = false
        },
        new PrimalDinoStat()
        {
            StatName = PrimalDinoStatName.SpeedMultiplier,
            Value = 0.0f,
            WildPerLevel = 0.0f,
            TamedPerLevel = 0.01f,
            TamingReward = 0.0f,
            EffectivenessReward = 0.0f,
            MaxGainedPerLevelUpValueIsPercent = true,
            CanLevelUpValue = true,
            DontUseValue = false
        },
        new PrimalDinoStat()
        {
            StatName = PrimalDinoStatName.Fortitude,
            Value = 0.0f,
            WildPerLevel = 0.0f,
            TamedPerLevel = 0.0f,
            TamingReward = 0.0f,
            EffectivenessReward = 0.0f,
            MaxGainedPerLevelUpValueIsPercent = true,
            CanLevelUpValue = false,
            DontUseValue = true
        },
        new PrimalDinoStat()
        {
            StatName = PrimalDinoStatName.CraftingSpeedMultiplier,
            Value = 0.0f,
            WildPerLevel = 0.0f,
            TamedPerLevel = 0.0f,
            TamingReward = 0.0f,
            EffectivenessReward = 0.0f,
            MaxGainedPerLevelUpValueIsPercent = true,
            CanLevelUpValue = false,
            DontUseValue = true
        }
    };

    public PrimalDinoStatName StatName;
    public float Value;
    public float WildPerLevel;
    public float TamedPerLevel;
    public float TamingReward;
    public float EffectivenessReward;
    public bool MaxGainedPerLevelUpValueIsPercent;
    public bool CanLevelUpValue;
    public bool DontUseValue;
}