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
        new PrimalDinoStat() { StatName = PrimalDinoStatName.Health },
        new PrimalDinoStat() { StatName = PrimalDinoStatName.Stamina },
        new PrimalDinoStat() { StatName = PrimalDinoStatName.Torpidity },
        new PrimalDinoStat() { StatName = PrimalDinoStatName.Oxygen },
        new PrimalDinoStat() { StatName = PrimalDinoStatName.Food },
        new PrimalDinoStat() { StatName = PrimalDinoStatName.Water },
        new PrimalDinoStat() { StatName = PrimalDinoStatName.Temperature },
        new PrimalDinoStat() { StatName = PrimalDinoStatName.Weight },
        new PrimalDinoStat() { StatName = PrimalDinoStatName.MeleeDamageMultiplier },
        new PrimalDinoStat() { StatName = PrimalDinoStatName.SpeedMultiplier },
        new PrimalDinoStat() { StatName = PrimalDinoStatName.Fortitude },
        new PrimalDinoStat() { StatName = PrimalDinoStatName.CraftingSpeedMultiplier }
    };

    public PrimalDinoStatName StatName;
    public float Value;
    public float WildPerLevel;
    public float TamedPerLevel;
    public float TamingReward;
    public float EffectivenessReward;
    public bool MaxGainedPerLevelUpIsPercent;
    public bool CanLevelUpValue;
    public bool DontUseValue;
}