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
}