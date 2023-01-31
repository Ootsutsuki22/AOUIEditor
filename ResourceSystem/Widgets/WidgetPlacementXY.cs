using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetPlacementXY.html

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WidgetPlacementXY
    {
        [DefaultValue(false)]
        [Description("Скалировать ли виджет стипенчато при изменении размеров окна. Поскольку шрифты могут быть четкими только при отображении пиксель в пиксель, их размер можно менять только дискретно, по целым числам. Соответственно, для сохранения пропорций контролов последние также должны менять свои размеры дискретно. По умолчанию false")]
        public bool? QuantumScale { get; set; }

        [Description("Описание размеров и расположения по горизонтали")]
        public WidgetPlacement X { get; set; }

        [Description("Описание размеров и расположения по вертикали")]
        public WidgetPlacement Y { get; set; }

        [Description("Виджет, по которому считается размер (используется в случае WIDGET_SIZING_CHILD)")]
        public Widget sizingWidget { get; set; }

        [Description("Виджеты, по которым считается размер (используется в случае WIDGET_SIZING_CHILDREN)")]
        [Editor(typeof(WidgetCollectionEditor), typeof(UITypeEditor))]
        public Widget[] sizingWidgets { get; set; }

        public WidgetPlacementXY()
        {
            X = new WidgetPlacement();
            Y = new WidgetPlacement();
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WidgetPlacement
    {
        [DefaultValue(WidgetAlign.WIDGET_ALIGN_LOW)]
        [Description("Привязка к краям родительского виджета. По умолчанию WIDGET_ALIGN_LOW")]
        public WidgetAlign? Align { get; set; }

        [DefaultValue(WidgetSizing.WIDGET_SIZING_DEFAULT)]
        [Description("Относительно каких виджетов будет происходить вычисление размера.По умолчанию WIDGET_SIZING_DEFAULT")]
        public WidgetSizing? Sizing { get; set; }

        [DefaultValue(0f)]
        [Description("Смещение относительно меньшего края")]
        public float? Pos { get; set; }

        [DefaultValue(0f)]
        [Description("Смещение относительно большего края")]
        public float? HighPos { get; set; }

        [DefaultValue(0f)]
        [Description("Размер")]
        public float? Size { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }

    public enum WidgetAlign
    {
        WIDGET_ALIGN_LOW,
        WIDGET_ALIGN_HIGH,
        WIDGET_ALIGN_CENTER,
        WIDGET_ALIGN_BOTH,
        WIDGET_ALIGN_LOW_ABS
    }

    public enum WidgetSizing
    {
        WIDGET_SIZING_DEFAULT,
        WIDGET_SIZING_CHILDREN,
        WIDGET_SIZING_INTERNAL
    }
}
