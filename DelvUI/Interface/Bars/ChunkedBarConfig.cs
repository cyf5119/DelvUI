﻿using DelvUI.Config;
using DelvUI.Config.Attributes;
using DelvUI.Enums;
using DelvUI.Interface.GeneralElements;
using System.Numerics;

namespace DelvUI.Interface.Bars
{
    [Exportable(false)]
    public class ChunkedBarConfig : BarConfig
    {
        [DragInt("填充", min = -4000, max = 4000)]
        [Order(45)]
        public int Padding = 2;

        public ChunkedBarConfig(
            Vector2 position,
            Vector2 size,
            PluginConfigColor fillColor,
            int padding = 2) : base(position, size, fillColor)
        {
            Padding = padding;
        }
    }

    [Exportable(false)]
    public class ChunkedProgressBarConfig : ChunkedBarConfig
    {
        [Checkbox("在区块中显示", spacing = true)]
        [Order(46)]
        public bool UseChunks = true;

        [RadioSelector("在所有区块上显示文本", "在活跃区块上显示文本")]
        [Order(47, collapseWith = nameof(UseChunks))]
        public LabelMode LabelMode;

        [Checkbox("使用部分填充色", spacing = true)]
        [Order(50)]
        public bool UsePartialFillColor = false;

        [ColorEdit4("部分填充色")]
        [Order(55, collapseWith = nameof(UsePartialFillColor))]
        public PluginConfigColor PartialFillColor;

        [NestedConfig("条的文本", 1000, separator = false, spacing = true)]
        public NumericLabelConfig Label;

        public ChunkedProgressBarConfig(
            Vector2 position,
            Vector2 size,
            PluginConfigColor fillColor,
            int padding = 2,
            PluginConfigColor? partialFillColor = null) : base(position, size, fillColor, padding)
        {
            Label = new NumericLabelConfig(Vector2.Zero, "", DrawAnchor.Center, DrawAnchor.Center);
            Label.Enabled = false;

            PartialFillColor = partialFillColor ?? new PluginConfigColor(new(180f / 255f, 180f / 255f, 180f / 255f, 100f / 100f));
        }
    }

    public enum LabelMode
    {
        AllChunks,
        ActiveChunk
    }
}
