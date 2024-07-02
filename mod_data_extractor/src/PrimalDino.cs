using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Objects.Properties;
using ModDataExtractor;

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

    public static List<PrimalDinoStat> GetStatsFromPrimalDino(UObject export)
    {
        List<UObject> makeup = Helpers.GetClassMakeup(export);
        List<PrimalDinoStat> stats = BaseStats.Select(stat => new PrimalDinoStat
        {
            StatName = stat.StatName,
            Value = stat.Value,
            WildPerLevel = stat.WildPerLevel,
            TamedPerLevel = stat.TamedPerLevel,
            TamingReward = stat.TamingReward,
            EffectivenessReward = stat.EffectivenessReward,
            MaxGainedPerLevelUpValueIsPercent = stat.MaxGainedPerLevelUpValueIsPercent,
            CanLevelUpValue = stat.CanLevelUpValue,
            DontUseValue = stat.DontUseValue
        }).ToList();
        // Goes in backwards order so overrides should override
        foreach (UObject parentClass in makeup)
        {
            foreach (FPropertyTag tag in parentClass.Properties)
            {
                if (tag.Tag == null)
                    continue;

                PrimalDinoStatName statName = (PrimalDinoStatName)tag.ArrayIndex;
                foreach (PrimalDinoStat stat in stats)
                {
                    if (stat.StatName != statName)
                    {
                        continue;
                    }

                    if (tag.Tag.GetType() == typeof(FloatProperty))
                    {
                        float val = tag.Tag.GetValue<float>();
                        switch (tag.Name.PlainText)
                        {
                            case "MaxStatusValues":
                                stat.Value = val;
                                break;
                            case "AmountMaxGainedPerLevelUpValueTamed":
                                stat.TamedPerLevel = val;
                                break;
                            case "AmountMaxGainedPerLevelUpValue":
                                stat.WildPerLevel = val;
                                break;
                            case "TamingMaxStatMultipliers":
                                stat.EffectivenessReward = val;
                                break;
                            case "TamingMaxStatAdditions":
                                stat.TamingReward = val;
                                break;
                        }
                    }
                    else if (tag.Tag.GetType() == typeof(ByteProperty))
                    {
                        byte val = tag.Tag.GetValue<byte>();
                        switch (tag.Name.PlainText)
                        {                                
                            case "MaxGainedPerLevelUpValueIsPercent":
                                stat.MaxGainedPerLevelUpValueIsPercent = val == 1;
                                break;
                            case "CanLevelUpValue":
                                stat.CanLevelUpValue = val == 1;
                                break;
                            case "DontUseValue":
                                stat.DontUseValue = val == 1;
                                break;
                        }
                    }
                }
            }
        }
        return stats;
    }
}