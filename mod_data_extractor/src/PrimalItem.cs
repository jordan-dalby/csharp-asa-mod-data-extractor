using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Objects.Properties;
using ModDataExtractor;

public enum PrimalItemStatName
{
    GenericQuality,
    Armor,
    MaxDurability,
    WeaponDamagePercent,
    WeaponClipAmmo,
    HypothermalInsulation,
    Weight,
    HyperthermalInsulation
}

public class PrimalItemStat
{
    public static readonly List<PrimalItemStat> BaseStats = new List<PrimalItemStat>()
    {
        new PrimalItemStat() 
        { 
            StatName = PrimalItemStatName.GenericQuality,
            Used = false,
            CalculateAsPercent = false,
            DisplayAsPercent = true,
            RequiresSubmerged = false,
            PreventIfSubmerged = false,
            HideStatFromTooltip = false,
            DefaultModifierValue = 0,
            RandomizerRangeOverride = 0,
            RandomizerRangeMultiplier = 0f,
            StateModifierScale = 1f,
            InitialValueConstant = 0f,
            RatingValueMultiplier = 1f,
            AbsoluteMaxValue = 0f
        },
        new PrimalItemStat() 
        { 
            StatName = PrimalItemStatName.Armor,
            Used = false,
            CalculateAsPercent = false,
            DisplayAsPercent = false,
            RequiresSubmerged = false,
            PreventIfSubmerged = false,
            HideStatFromTooltip = false,
            DefaultModifierValue = 0,
            RandomizerRangeOverride = 1000,
            RandomizerRangeMultiplier = 0.2f,
            StateModifierScale = 0.001f,
            InitialValueConstant = 0f,
            RatingValueMultiplier = 1.55f,
            AbsoluteMaxValue = 0f
        },
        new PrimalItemStat() 
        { 
            StatName = PrimalItemStatName.MaxDurability,
            Used = false,
            CalculateAsPercent = false,
            DisplayAsPercent = false,
            RequiresSubmerged = false,
            PreventIfSubmerged = false,
            HideStatFromTooltip = false,
            DefaultModifierValue = 0,
            RandomizerRangeOverride = 1000,
            RandomizerRangeMultiplier = 0.25f,
            StateModifierScale = 0.001f,
            InitialValueConstant = 100f,
            RatingValueMultiplier = 0.65f,
            AbsoluteMaxValue = 0f
        },
        new PrimalItemStat() 
        { 
            StatName = PrimalItemStatName.WeaponDamagePercent,
            Used = false,
            CalculateAsPercent = false,
            DisplayAsPercent = true,
            RequiresSubmerged = false,
            PreventIfSubmerged = false,
            HideStatFromTooltip = false,
            DefaultModifierValue = 0,
            RandomizerRangeOverride = 1000,
            RandomizerRangeMultiplier = 0.1f,
            StateModifierScale = 0.001f,
            InitialValueConstant = 0f,
            RatingValueMultiplier = 1.4f,
            AbsoluteMaxValue = 0f
        },
        new PrimalItemStat() 
        { 
            StatName = PrimalItemStatName.WeaponClipAmmo,
            Used = false,
            CalculateAsPercent = false,
            DisplayAsPercent = true,
            RequiresSubmerged = false,
            PreventIfSubmerged = false,
            HideStatFromTooltip = false,
            DefaultModifierValue = 0,
            RandomizerRangeOverride = 0,
            RandomizerRangeMultiplier = 0f,
            StateModifierScale = 1f,
            InitialValueConstant = 0f,
            RatingValueMultiplier = 1.15f,
            AbsoluteMaxValue = 0f
        },
        new PrimalItemStat() 
        { 
            StatName = PrimalItemStatName.HypothermalInsulation,
            Used = false,
            CalculateAsPercent = false,
            DisplayAsPercent = false,
            RequiresSubmerged = false,
            PreventIfSubmerged = false,
            HideStatFromTooltip = false,
            DefaultModifierValue = 0,
            RandomizerRangeOverride = 1000,
            RandomizerRangeMultiplier = 0.2f,
            StateModifierScale = 0.001f,
            InitialValueConstant = 0f,
            RatingValueMultiplier = 0.9f,
            AbsoluteMaxValue = 0f
        },
        new PrimalItemStat() 
        { 
            StatName = PrimalItemStatName.Weight,
            Used = false,
            CalculateAsPercent = true,
            DisplayAsPercent = false,
            RequiresSubmerged = false,
            PreventIfSubmerged = false,
            HideStatFromTooltip = false,
            DefaultModifierValue = 0,
            RandomizerRangeOverride = 0,
            RandomizerRangeMultiplier = 0f,
            StateModifierScale = 1f,
            InitialValueConstant = 0f,
            RatingValueMultiplier = 1f,
            AbsoluteMaxValue = 0f
        },
        new PrimalItemStat() 
        { 
            StatName = PrimalItemStatName.HyperthermalInsulation,
            Used = false,
            CalculateAsPercent = false,
            DisplayAsPercent = false,
            RequiresSubmerged = false,
            PreventIfSubmerged = false,
            HideStatFromTooltip = false,
            DefaultModifierValue = 0,
            RandomizerRangeOverride = 1000,
            RandomizerRangeMultiplier = 0.2f,
            StateModifierScale = 0.001f,
            InitialValueConstant = 0f,
            RatingValueMultiplier = 0.9f,
            AbsoluteMaxValue = 0f
        },
    };

    public PrimalItemStatName StatName;
    public bool Used = false;
    public bool CalculateAsPercent = false;
    public bool DisplayAsPercent = false;
    public bool RequiresSubmerged = false;
    public bool PreventIfSubmerged = false;
    public bool HideStatFromTooltip = false;
    public int DefaultModifierValue = 0;
    public int RandomizerRangeOverride = 0;
    public float RandomizerRangeMultiplier = 0f;
    public float StateModifierScale = 1f;
    public float InitialValueConstant = 0f;
    public float RatingValueMultiplier = 1f;
    public float AbsoluteMaxValue = 0f;

    public static List<PrimalItemStat> GetStatsFromPrimalItem(UObject export)
    {
        List<UObject> makeup = Helpers.GetClassMakeup(export);
        List<PrimalItemStat> stats = BaseStats.Select(stat => new PrimalItemStat
        {
            StatName = stat.StatName,
            Used = stat.Used,
            CalculateAsPercent = stat.CalculateAsPercent,
            DisplayAsPercent = stat.DisplayAsPercent,
            RequiresSubmerged = stat.RequiresSubmerged,
            PreventIfSubmerged = stat.PreventIfSubmerged,
            HideStatFromTooltip = stat.HideStatFromTooltip,
            DefaultModifierValue = stat.DefaultModifierValue,
            RandomizerRangeOverride = stat.RandomizerRangeOverride,
            RandomizerRangeMultiplier = stat.RandomizerRangeMultiplier,
            StateModifierScale = stat.StateModifierScale,
            InitialValueConstant = stat.InitialValueConstant,
            RatingValueMultiplier = stat.RatingValueMultiplier,
            AbsoluteMaxValue = stat.AbsoluteMaxValue
        }).ToList();
        // Goes in backwards order so overrides should override
        foreach (UObject parentClass in makeup)
        {
            foreach (FPropertyTag tag in parentClass.Properties)
            {
                if (tag.Tag == null)
                    continue;

                PrimalItemStatName statName = (PrimalItemStatName)tag.ArrayIndex;
                foreach (PrimalItemStat stat in stats)
                {
                    if (stat.StatName != statName)
                    {
                        continue;
                    }

                    if (tag.Tag.GetValue<UScriptStruct>() != null)
                    {
                        UScriptStruct? data = tag.Tag.GetValue<UScriptStruct>();
                        if (data?.StructType.GetType() != typeof(FStructFallback))
                            continue;

                        FStructFallback fallback = (FStructFallback)data.StructType;
                        foreach (FPropertyTag fallbackTag in fallback.Properties)
                        {
                            if (fallbackTag.Tag == null)
                                continue;

                            if (fallbackTag.Tag.GetType() == typeof(FloatProperty))
                            {
                                float val = fallbackTag.Tag.GetValue<float>();
                                switch (fallbackTag.Name.PlainText)
                                {
                                    case "RandomizerRangeMultiplier":
                                        stat.RandomizerRangeMultiplier = val;
                                        break;
                                    case "StateModifierScale":
                                        stat.StateModifierScale = val;
                                        break;
                                    case "InitialValueConstant":
                                        stat.InitialValueConstant = val;
                                        break;
                                    case "RatingValueMultiplier":
                                        stat.RatingValueMultiplier = val;
                                        break;
                                    case "AbsoluteMaxValue":
                                        stat.AbsoluteMaxValue = val;
                                        break;
                                }
                            }
                            else if (fallbackTag.Tag.GetType() == typeof(BoolProperty))
                            {
                                bool val = fallbackTag.Tag.GetValue<bool>();
                                switch (fallbackTag.Name.PlainText)
                                {
                                    case "bUsed":
                                        stat.Used = val;
                                        break;
                                    case "bCalculateAsPercent":
                                        stat.CalculateAsPercent = val;
                                        break;
                                    case "bDisplayAsPercent":
                                        stat.DisplayAsPercent = val;
                                        break;
                                    case "bRequiresSubmerged":
                                        stat.RequiresSubmerged = val;
                                        break;
                                    case "bPreventIfSubmerged":
                                        stat.PreventIfSubmerged = val;
                                        break;
                                    case "bHideStatFromTooltip":
                                        stat.HideStatFromTooltip = val;
                                        break;
                                }
                            }
                            else if (fallbackTag.Tag.GetType() == typeof(IntProperty))
                            {
                                int val = fallbackTag.Tag.GetValue<int>();
                                switch (fallbackTag.Name.PlainText)
                                {
                                    case "DefaultModifierValue":
                                        stat.DefaultModifierValue = val;
                                        break;
                                    case "RandomizerRangeOverride":
                                        stat.RandomizerRangeOverride = val;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
        return stats;
    }
}