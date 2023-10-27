﻿using DelvUI.Config;
using DelvUI.Config.Attributes;
using DelvUI.Enums;
using DelvUI.Helpers;
using DelvUI.Interface.Party;
using System;
using System.Collections.Generic;

namespace DelvUI.Interface.GeneralElements
{
    public class GlobalColors : IDisposable
    {
        #region Singleton
        private MiscColorConfig _miscColorConfig = null!;
        private RolesColorConfig _rolesColorConfig = null!;

        private Dictionary<uint, PluginConfigColor> ColorMap = null!;

        private GlobalColors()
        {
            ConfigurationManager.Instance.ResetEvent += OnConfigReset;
            OnConfigReset(ConfigurationManager.Instance);
        }

        private void OnConfigReset(ConfigurationManager sender)
        {
            _miscColorConfig = sender.GetConfigObject<MiscColorConfig>();
            _rolesColorConfig = sender.GetConfigObject<RolesColorConfig>();

            var tanksColorConfig = sender.GetConfigObject<TanksColorConfig>();
            var healersColorConfig = sender.GetConfigObject<HealersColorConfig>();
            var meleeColorConfig = sender.GetConfigObject<MeleeColorConfig>();
            var rangedColorConfig = sender.GetConfigObject<RangedColorConfig>();
            var castersColorConfig = sender.GetConfigObject<CastersColorConfig>();

            ColorMap = new Dictionary<uint, PluginConfigColor>()
            {
                // tanks
                [JobIDs.GLA] = tanksColorConfig.GLAColor,
                [JobIDs.MRD] = tanksColorConfig.MRDColor,
                [JobIDs.PLD] = tanksColorConfig.PLDColor,
                [JobIDs.WAR] = tanksColorConfig.WARColor,
                [JobIDs.DRK] = tanksColorConfig.DRKColor,
                [JobIDs.GNB] = tanksColorConfig.GNBColor,

                // healers
                [JobIDs.CNJ] = healersColorConfig.CNJColor,
                [JobIDs.WHM] = healersColorConfig.WHMColor,
                [JobIDs.SCH] = healersColorConfig.SCHColor,
                [JobIDs.AST] = healersColorConfig.ASTColor,
                [JobIDs.SGE] = healersColorConfig.SGEColor,

                // melee
                [JobIDs.PGL] = meleeColorConfig.PGLColor,
                [JobIDs.LNC] = meleeColorConfig.LNCColor,
                [JobIDs.ROG] = meleeColorConfig.ROGColor,
                [JobIDs.MNK] = meleeColorConfig.MNKColor,
                [JobIDs.DRG] = meleeColorConfig.DRGColor,
                [JobIDs.NIN] = meleeColorConfig.NINColor,
                [JobIDs.SAM] = meleeColorConfig.SAMColor,
                [JobIDs.RPR] = meleeColorConfig.RPRColor,

                // ranged 
                [JobIDs.ARC] = rangedColorConfig.ARCColor,
                [JobIDs.BRD] = rangedColorConfig.BRDColor,
                [JobIDs.MCH] = rangedColorConfig.MCHColor,
                [JobIDs.DNC] = rangedColorConfig.DNCColor,

                // casters
                [JobIDs.THM] = castersColorConfig.THMColor,
                [JobIDs.ACN] = castersColorConfig.ACNColor,
                [JobIDs.BLM] = castersColorConfig.BLMColor,
                [JobIDs.SMN] = castersColorConfig.SMNColor,
                [JobIDs.RDM] = castersColorConfig.RDMColor,
                [JobIDs.BLU] = castersColorConfig.BLUColor,

                // crafters
                [JobIDs.CRP] = _rolesColorConfig.HANDColor,
                [JobIDs.BSM] = _rolesColorConfig.HANDColor,
                [JobIDs.ARM] = _rolesColorConfig.HANDColor,
                [JobIDs.GSM] = _rolesColorConfig.HANDColor,
                [JobIDs.LTW] = _rolesColorConfig.HANDColor,
                [JobIDs.WVR] = _rolesColorConfig.HANDColor,
                [JobIDs.ALC] = _rolesColorConfig.HANDColor,
                [JobIDs.CUL] = _rolesColorConfig.HANDColor,

                // gatherers
                [JobIDs.MIN] = _rolesColorConfig.LANDColor,
                [JobIDs.BOT] = _rolesColorConfig.LANDColor,
                [JobIDs.FSH] = _rolesColorConfig.LANDColor
            };
        }

        public static void Initialize()
        {
            Instance = new GlobalColors();
        }

        public static GlobalColors Instance { get; private set; } = null!;

        ~GlobalColors()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            ConfigurationManager.Instance.ResetEvent -= OnConfigReset;
            Instance = null!;
        }
        #endregion

        public PluginConfigColor? ColorForJobId(uint jobId) => ColorMap.TryGetValue(jobId, out PluginConfigColor? color) ? color : null;

        public PluginConfigColor SafeColorForJobId(uint jobId) => ColorForJobId(jobId) ?? _miscColorConfig.NPCNeutralColor;

        public PluginConfigColor? RoleColorForJobId(uint jobId)
        {
            JobRoles role = JobsHelper.RoleForJob(jobId);

            return role switch
            {
                JobRoles.Tank => _rolesColorConfig.TankRoleColor,
                JobRoles.Healer => _rolesColorConfig.HealerRoleColor,
                JobRoles.DPSMelee => _rolesColorConfig.UseSpecificDPSColors ? _rolesColorConfig.MeleeDPSRoleColor : _rolesColorConfig.DPSRoleColor,
                JobRoles.DPSRanged => _rolesColorConfig.UseSpecificDPSColors ? _rolesColorConfig.RangedDPSRoleColor : _rolesColorConfig.DPSRoleColor,
                JobRoles.DPSCaster => _rolesColorConfig.UseSpecificDPSColors ? _rolesColorConfig.CasterDPSRoleColor : _rolesColorConfig.DPSRoleColor,
                JobRoles.Gatherer => _rolesColorConfig.LANDColor,
                JobRoles.Crafter => _rolesColorConfig.HANDColor,
                _ => null
            };
        }

        public PluginConfigColor SafeRoleColorForJobId(uint jobId) => RoleColorForJobId(jobId) ?? _miscColorConfig.NPCNeutralColor;

        public PluginConfigColor EmptyUnitFrameColor => _miscColorConfig.EmptyUnitFrameColor;
        public PluginConfigColor EmptyColor => _miscColorConfig.EmptyColor;
        public PluginConfigColor PartialFillColor => _miscColorConfig.PartialFillColor;
        public PluginConfigColor NPCFriendlyColor => _miscColorConfig.NPCFriendlyColor;
        public PluginConfigColor NPCHostileColor => _miscColorConfig.NPCHostileColor;
        public PluginConfigColor NPCNeutralColor => _miscColorConfig.NPCNeutralColor;
    }

    [Disableable(false)]
    [Section("颜色")]
    [SubSection("防护", 0)]
    public class TanksColorConfig : PluginConfigObject
    {
        public new static TanksColorConfig DefaultConfig() { return new TanksColorConfig(); }

        [ColorEdit4("骑士", spacing = true)]
        [Order(5)]
        public PluginConfigColor PLDColor = new PluginConfigColor(new(168f / 255f, 210f / 255f, 230f / 255f, 100f / 100f));

        [ColorEdit4("暗黑骑士")]
        [Order(10)]
        public PluginConfigColor DRKColor = new PluginConfigColor(new(209f / 255f, 38f / 255f, 204f / 255f, 100f / 100f));

        [ColorEdit4("战士")]
        [Order(15)]
        public PluginConfigColor WARColor = new PluginConfigColor(new(207f / 255f, 38f / 255f, 33f / 255f, 100f / 100f));

        [ColorEdit4("绝枪战士")]
        [Order(20)]
        public PluginConfigColor GNBColor = new PluginConfigColor(new(121f / 255f, 109f / 255f, 48f / 255f, 100f / 100f));

        [ColorEdit4("剑术师", spacing = true)]
        [Order(25)]
        public PluginConfigColor GLAColor = new PluginConfigColor(new(168f / 255f, 210f / 255f, 230f / 255f, 100f / 100f));

        [ColorEdit4("斧术师")]
        [Order(30)]
        public PluginConfigColor MRDColor = new PluginConfigColor(new(207f / 255f, 38f / 255f, 33f / 255f, 100f / 100f));
    }

    [Disableable(false)]
    [Section("颜色")]
    [SubSection("治疗", 0)]
    public class HealersColorConfig : PluginConfigObject
    {
        public new static HealersColorConfig DefaultConfig() { return new HealersColorConfig(); }

        [ColorEdit4("学者", spacing = true)]
        [Order(5)]
        public PluginConfigColor SCHColor = new PluginConfigColor(new(134f / 255f, 87f / 255f, 255f / 255f, 100f / 100f));

        [ColorEdit4("白魔法师")]
        [Order(10)]
        public PluginConfigColor WHMColor = new PluginConfigColor(new(255f / 255f, 240f / 255f, 220f / 255f, 100f / 100f));

        [ColorEdit4("占星术士")]
        [Order(15)]
        public PluginConfigColor ASTColor = new PluginConfigColor(new(255f / 255f, 231f / 255f, 74f / 255f, 100f / 100f));

        [ColorEdit4("贤者")]
        [Order(20)]
        public PluginConfigColor SGEColor = new PluginConfigColor(new(144f / 255f, 176f / 255f, 255f / 255f, 100f / 100f));

        [ColorEdit4("幻术师", spacing = true)]
        [Order(25)]
        public PluginConfigColor CNJColor = new PluginConfigColor(new(255f / 255f, 240f / 255f, 220f / 255f, 100f / 100f));
    }

    [Disableable(false)]
    [Section("颜色")]
    [SubSection("近战", 0)]
    public class MeleeColorConfig : PluginConfigObject
    {
        public new static MeleeColorConfig DefaultConfig() { return new MeleeColorConfig(); }

        [ColorEdit4("武僧", spacing = true)]
        [Order(5)]
        public PluginConfigColor MNKColor = new PluginConfigColor(new(214f / 255f, 156f / 255f, 0f / 255f, 100f / 100f));

        [ColorEdit4("忍者")]
        [Order(10)]
        public PluginConfigColor NINColor = new PluginConfigColor(new(175f / 255f, 25f / 255f, 100f / 255f, 100f / 100f));

        [ColorEdit4("龙骑士")]
        [Order(15)]
        public PluginConfigColor DRGColor = new PluginConfigColor(new(65f / 255f, 100f / 255f, 205f / 255f, 100f / 100f));

        [ColorEdit4("武士")]
        [Order(20)]
        public PluginConfigColor SAMColor = new PluginConfigColor(new(228f / 255f, 109f / 255f, 4f / 255f, 100f / 100f));

        [ColorEdit4("钐镰客")]
        [Order(25)]
        public PluginConfigColor RPRColor = new PluginConfigColor(new(150f / 255f, 90f / 255f, 144f / 255f, 100f / 100f));

        [ColorEdit4("格斗家", spacing = true)]
        [Order(30)]
        public PluginConfigColor PGLColor = new PluginConfigColor(new(214f / 255f, 156f / 255f, 0f / 255f, 100f / 100f));

        [ColorEdit4("双剑师")]
        [Order(35)]
        public PluginConfigColor ROGColor = new PluginConfigColor(new(175f / 255f, 25f / 255f, 100f / 255f, 100f / 100f));

        [ColorEdit4("枪术师")]
        [Order(40)]
        public PluginConfigColor LNCColor = new PluginConfigColor(new(65f / 255f, 100f / 255f, 205f / 255f, 100f / 100f));
    }

    [Disableable(false)]
    [Section("颜色")]
    [SubSection("远敏", 0)]
    public class RangedColorConfig : PluginConfigObject
    {
        public new static RangedColorConfig DefaultConfig() { return new RangedColorConfig(); }

        [ColorEdit4("诗人", spacing = true)]
        [Order(5)]
        public PluginConfigColor BRDColor = new PluginConfigColor(new(145f / 255f, 186f / 255f, 94f / 255f, 100f / 100f));

        [ColorEdit4("机工士")]
        [Order(10)]
        public PluginConfigColor MCHColor = new PluginConfigColor(new(110f / 255f, 225f / 255f, 214f / 255f, 100f / 100f));

        [ColorEdit4("舞者")]
        [Order(15)]
        public PluginConfigColor DNCColor = new PluginConfigColor(new(226f / 255f, 176f / 255f, 175f / 255f, 100f / 100f));

        [ColorEdit4("弓箭手", separator = true)]
        [Order(20)]
        public PluginConfigColor ARCColor = new PluginConfigColor(new(145f / 255f, 186f / 255f, 94f / 255f, 100f / 100f));
    }

    [Disableable(false)]
    [Section("颜色")]
    [SubSection("法系", 0)]
    public class CastersColorConfig : PluginConfigObject
    {
        public new static CastersColorConfig DefaultConfig() { return new CastersColorConfig(); }

        [ColorEdit4("黑魔法师", spacing = true)]
        [Order(5)]
        public PluginConfigColor BLMColor = new PluginConfigColor(new(165f / 255f, 121f / 255f, 214f / 255f, 100f / 100f));

        [ColorEdit4("召唤师")]
        [Order(10)]
        public PluginConfigColor SMNColor = new PluginConfigColor(new(45f / 255f, 155f / 255f, 120f / 255f, 100f / 100f));

        [ColorEdit4("赤魔法师")]
        [Order(15)]
        public PluginConfigColor RDMColor = new PluginConfigColor(new(232f / 255f, 123f / 255f, 123f / 255f, 100f / 100f));

        [ColorEdit4("青魔法师", spacing = true)]
        [Order(20)]
        public PluginConfigColor BLUColor = new PluginConfigColor(new(0f / 255f, 185f / 255f, 247f / 255f, 100f / 100f));

        [ColorEdit4("咒术师")]
        [Order(25)]
        public PluginConfigColor THMColor = new PluginConfigColor(new(165f / 255f, 121f / 255f, 214f / 255f, 100f / 100f));

        [ColorEdit4("秘术师")]
        [Order(30)]
        public PluginConfigColor ACNColor = new PluginConfigColor(new(45f / 255f, 155f / 255f, 120f / 255f, 100f / 100f));
    }

    [Disableable(false)]
    [Section("颜色")]
    [SubSection("职能", 0)]
    public class RolesColorConfig : PluginConfigObject
    {
        public new static RolesColorConfig DefaultConfig() { return new RolesColorConfig(); }

        [ColorEdit4("防护")]
        [Order(10)]
        public PluginConfigColor TankRoleColor = new PluginConfigColor(new(21f / 255f, 28f / 255f, 100f / 255f, 100f / 100f));

        [ColorEdit4("DPS")]
        [Order(15)]
        public PluginConfigColor DPSRoleColor = new PluginConfigColor(new(153f / 255f, 23f / 255f, 23f / 255f, 100f / 100f));

        [ColorEdit4("治疗")]
        [Order(20)]
        public PluginConfigColor HealerRoleColor = new PluginConfigColor(new(46f / 255f, 125f / 255f, 50f / 255f, 100f / 100f));

        [ColorEdit4("大地使者", spacing = true)]
        [Order(25)]
        public PluginConfigColor LANDColor = new PluginConfigColor(new(99f / 255f, 172f / 255f, 14f / 255f, 100f / 100f));

        [ColorEdit4("能工巧匠")]
        [Order(30)]
        public PluginConfigColor HANDColor = new PluginConfigColor(new(99f / 255f, 172f / 255f, 14f / 255f, 100f / 100f));

        [Checkbox("使用特定的DPS颜色", spacing = true)]
        [Order(35)]
        public bool UseSpecificDPSColors = false;

        [ColorEdit4("近战DPS")]
        [Order(40, collapseWith = nameof(UseSpecificDPSColors))]
        public PluginConfigColor MeleeDPSRoleColor = new PluginConfigColor(new(151f / 255f, 56f / 255f, 56f / 255f, 100f / 100f));

        [ColorEdit4("远敏DPS")]
        [Order(40, collapseWith = nameof(UseSpecificDPSColors))]
        public PluginConfigColor RangedDPSRoleColor = new PluginConfigColor(new(250f / 255f, 185f / 255f, 67f / 255f, 100f / 100f));

        [ColorEdit4("法系DPS")]
        [Order(40, collapseWith = nameof(UseSpecificDPSColors))]
        public PluginConfigColor CasterDPSRoleColor = new PluginConfigColor(new(154f / 255f, 82f / 255f, 193f / 255f, 100f / 100f));
    }

    [Disableable(false)]
    [Section("颜色")]
    [SubSection("杂项", 0)]
    public class MiscColorConfig : PluginConfigObject
    {
        public new static MiscColorConfig DefaultConfig() { return new MiscColorConfig(); }

        [Combo("条的渐变类型", "平铺", "右", "左", "上", "下", "水平居中", spacing = true)]
        [Order(4)]
        public GradientDirection GradientDirection = GradientDirection.Down;

        [ColorEdit4("空单元界面", separator = true)]
        [Order(5)]
        public PluginConfigColor EmptyUnitFrameColor = new PluginConfigColor(new(0f / 255f, 0f / 255f, 0f / 255f, 95f / 100f));

        [ColorEdit4("空的条")]
        [Order(10)]
        public PluginConfigColor EmptyColor = new PluginConfigColor(new(0f / 255f, 0f / 255f, 0f / 255f, 50f / 100f));

        [ColorEdit4("部分填充的条")]
        [Order(15)]
        public PluginConfigColor PartialFillColor = new PluginConfigColor(new(180f / 255f, 180f / 255f, 180f / 255f, 100f / 100f));

        [ColorEdit4("友好NPC", separator = true)]
        [Order(20)]
        public PluginConfigColor NPCFriendlyColor = new PluginConfigColor(new(99f / 255f, 172f / 255f, 14f / 255f, 100f / 100f));

        [ColorEdit4("敌对NPC")]
        [Order(25)]
        public PluginConfigColor NPCHostileColor = new PluginConfigColor(new(233f / 255f, 4f / 255f, 4f / 255f, 100f / 100f));

        [ColorEdit4("中立NPC")]
        [Order(30)]
        public PluginConfigColor NPCNeutralColor = new PluginConfigColor(new(218f / 255f, 157f / 255f, 46f / 255f, 100f / 100f));
    }

    [Exportable(false)]
    public class ColorByHealthValueConfig : PluginConfigObject
    {

        [Checkbox("使用最大生命值颜色")]
        [Order(5)]
        public bool UseMaxHealthColor = false;

        [ColorEdit4("最大生命值颜色")]
        [Order(10, collapseWith = nameof(UseMaxHealthColor))]
        public PluginConfigColor MaxHealthColor = new PluginConfigColor(new(18f / 255f, 18f / 255f, 18f / 255f, 100f / 100f));

        [Checkbox("职业颜色设为最大生命值颜色")]
        [Order(15, collapseWith = nameof(UseMaxHealthColor))]
        public bool UseJobColorAsMaxHealth = false;

        [Checkbox("职能颜色设为最大生命值颜色")]
        [Order(20, collapseWith = nameof(UseMaxHealthColor))]
        public bool UseRoleColorAsMaxHealth = false;

        [ColorEdit4("高生命值颜色")]
        [Order(25)]
        public PluginConfigColor FullHealthColor = new PluginConfigColor(new(0f / 255f, 255f / 255f, 0f / 255f, 100f / 100f));

        [ColorEdit4("低生命值颜色")]
        [Order(30)]
        public PluginConfigColor LowHealthColor = new PluginConfigColor(new(255f / 255f, 0f / 255f, 0f / 255f, 100f / 100f));

        [DragFloat("最大生命值颜色高于的百分比", min = 50f, max = 100f, velocity = 1f)]
        [Order(35)]
        public float FullHealthColorThreshold = 75f;

        [DragFloat("低生命值颜色低于的百分比", min = 0f, max = 50f, velocity = 1f)]
        [Order(40)]
        public float LowHealthColorThreshold = 25f;

        [Combo("混合模式", "LAB", "LChab", "XYZ", "RGB", "LChuv", "Luv", "Jzazbz", "JzCzhz")]
        [Order(45)]
        public BlendMode BlendMode = BlendMode.LAB;
    }

    public class ColorByHealthFieldsConverter : PluginConfigObjectConverter
    {
        public ColorByHealthFieldsConverter()
        {
            SameTypeFieldConverter<bool> enabled =
                new SameTypeFieldConverter<bool>("ColorByHealth.Enabled", false);
            FieldConvertersMap.Add("UseColorBasedOnHealthValue", enabled);

            SameClassFieldConverter<PluginConfigColor> fullHealth =
                new SameClassFieldConverter<PluginConfigColor>("ColorByHealth.FullHealthColor", new PluginConfigColor(new(0f / 255f, 255f / 255f, 0f / 255f, 100f / 100f)));
            FieldConvertersMap.Add("FullHealthColor", fullHealth);

            SameClassFieldConverter<PluginConfigColor> lowHealth =
                new SameClassFieldConverter<PluginConfigColor>("ColorByHealth.LowHealthColor", new PluginConfigColor(new(255f / 255f, 0f / 255f, 0f / 255f, 100f / 100f)));
            FieldConvertersMap.Add("LowHealthColor", lowHealth);

            SameTypeFieldConverter<float> fullThreshold = new SameTypeFieldConverter<float>("ColorByHealth.FullHealthColorThreshold", 75f);
            FieldConvertersMap.Add("FullHealthColorThreshold", fullThreshold);

            SameTypeFieldConverter<float> lowThreshold = new SameTypeFieldConverter<float>("ColorByHealth.LowHealthColorThreshold", 25f);
            FieldConvertersMap.Add("LowHealthColorThreshold", lowThreshold);

            SameTypeFieldConverter<BlendMode> blendMode = new SameTypeFieldConverter<BlendMode>("ColorByHealth.BlendMode", BlendMode.LAB);
            FieldConvertersMap.Add("blendMode", blendMode);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PartyFramesColorsConfig) ||
                   objectType == typeof(UnitFrameConfig) ||
                   objectType == typeof(PlayerUnitFrameConfig) ||
                   objectType == typeof(TargetUnitFrameConfig) ||
                   objectType == typeof(TargetOfTargetUnitFrameConfig) ||
                   objectType == typeof(FocusTargetUnitFrameConfig);
        }
    }
}
