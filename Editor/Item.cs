using AOUIEditor.ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor
{
    /// <summary>
    /// Прокси-класс, который собирает необходимую информацию о виджетах и их потомках.
    /// Проверяет все виджеты на null и не добавляет их. Сортирует виджеты в порядке Priority.
    /// Используется для отрисовки в дереве объектов, графическом окне и тулбарах плэйсмента.
    /// Жуткий код. Всё пересоздаётся при изменении любого свойства, т.е. имеем кучу мусора для GC
    /// </summary>
    public class Item : IComparable<Item>
    {
        public static Item rootItem;
        public static Item selectedItem;

        public int priority;
        public Widget widget;
        public WidgetLayer backLayer;
        public WidgetLayer frontLayer;

        public WidgetLayer layerHighlight;
        public WidgetLayer stateNormal;
        public WidgetLayer statePushed;
        public WidgetLayer stateHighlighted;
        public WidgetLayer statePushedHighlighted;
        public WidgetLayer stateDisabled;

        // Сюда собирается плэйсмент со всех прототипов, затем изменяются размеры с учетом SIZING_INTERNAL и SIZING_CHILDREN
        // Используется для отрисовки
        public WidgetPlacementXY placement;

        // Сюда копируется плэйсмент из предыдущего до изменения размеров, затем все null-значения заменяются на дефолтные
        // Используется для тулбаров с координатами выделенного виджета
        public WidgetPlacementXY realPlacement;

        public List<Item> children;
        public Item parent;
        public bool hasPrototype;
        public bool isPrototypeChild;
        public bool directChild;

        public float posX;
        public float posY;
        public float sizeX;
        public float sizeY;

        public Item()
        {
            placement = new WidgetPlacementXY();
        }

        public static void FillData()
        {
            rootItem = new Item();
            selectedItem = null;
            Add(rootItem, Project.Addon, false, true);
            FinalPlacement(rootItem);
        }

        public static void Sort()
        {
            Sort(rootItem);
        }

        private static void Sort(Item item)
        {
            if (item == null || item.children == null)
                return;
            item.children.Sort();
            for (int i = 0; i < item.children.Count; i++)
            {
                Sort(item.children[i]);
            }
        }

        private static WidgetPlacementXY CopyPlacementNonNull(WidgetPlacementXY original)
        {
            WidgetPlacementXY result = new WidgetPlacementXY();
            result.QuantumScale = original.QuantumScale ?? false;
            result.sizingWidget = original.sizingWidget;
            result.sizingWidgets = original.sizingWidgets;

            result.X.Align = original.X.Align ?? WidgetAlign.WIDGET_ALIGN_LOW;
            result.X.Sizing = original.X.Sizing ?? WidgetSizing.WIDGET_SIZING_DEFAULT;
            result.X.Pos = original.X.Pos ?? 0;
            result.X.HighPos = original.X.HighPos ?? 0;
            result.X.Size = original.X.Size ?? 0;

            result.Y.Align = original.Y.Align ?? WidgetAlign.WIDGET_ALIGN_LOW;
            result.Y.Sizing = original.Y.Sizing ?? WidgetSizing.WIDGET_SIZING_DEFAULT;
            result.Y.Pos = original.Y.Pos ?? 0;
            result.Y.HighPos = original.Y.HighPos ?? 0;
            result.Y.Size = original.Y.Size ?? 0;
            return result;
        }

        private static void CollectPlacement(Item item, Widget widget)
        {
            WidgetPlacementXY itemPlacement = item.placement;
            WidgetPlacementXY widgetPlacement = widget.Placement;
            itemPlacement.QuantumScale = itemPlacement.QuantumScale ?? widgetPlacement.QuantumScale;
            itemPlacement.sizingWidget = itemPlacement.sizingWidget ?? widgetPlacement.sizingWidget;
            itemPlacement.sizingWidgets = itemPlacement.sizingWidgets ?? widgetPlacement.sizingWidgets;

            itemPlacement.X.Align = itemPlacement.X.Align ?? widgetPlacement.X.Align;
            itemPlacement.X.Sizing = itemPlacement.X.Sizing ?? widgetPlacement.X.Sizing;
            itemPlacement.X.Pos = itemPlacement.X.Pos ?? widgetPlacement.X.Pos;
            itemPlacement.X.HighPos = itemPlacement.X.HighPos ?? widgetPlacement.X.HighPos;
            itemPlacement.X.Size = itemPlacement.X.Size ?? widgetPlacement.X.Size;

            itemPlacement.Y.Align = itemPlacement.Y.Align ?? widgetPlacement.Y.Align;
            itemPlacement.Y.Sizing = itemPlacement.Y.Sizing ?? widgetPlacement.Y.Sizing;
            itemPlacement.Y.Pos = itemPlacement.Y.Pos ?? widgetPlacement.Y.Pos;
            itemPlacement.Y.HighPos = itemPlacement.Y.HighPos ?? widgetPlacement.Y.HighPos;
            itemPlacement.Y.Size = itemPlacement.Y.Size ?? widgetPlacement.Y.Size;
        }

        private static void FinalPlacement(Item item)
        {
            WidgetPlacementXY itemPlacement = item.placement;
            itemPlacement.X.Align = itemPlacement.X.Align ?? WidgetAlign.WIDGET_ALIGN_LOW;
            itemPlacement.X.Sizing = itemPlacement.X.Sizing ?? WidgetSizing.WIDGET_SIZING_DEFAULT;
            itemPlacement.X.Pos = itemPlacement.X.Pos ?? 0;
            itemPlacement.X.HighPos = itemPlacement.X.HighPos ?? 0;
            itemPlacement.X.Size = itemPlacement.X.Size ?? 0;

            itemPlacement.Y.Align = itemPlacement.Y.Align ?? WidgetAlign.WIDGET_ALIGN_LOW;
            itemPlacement.Y.Sizing = itemPlacement.Y.Sizing ?? WidgetSizing.WIDGET_SIZING_DEFAULT;
            itemPlacement.Y.Pos = itemPlacement.Y.Pos ?? 0;
            itemPlacement.Y.HighPos = itemPlacement.Y.HighPos ?? 0;
            itemPlacement.Y.Size = itemPlacement.Y.Size ?? 0;

            if (itemPlacement.X.Sizing == WidgetSizing.WIDGET_SIZING_INTERNAL)
            {
                itemPlacement.X.Size = 45;
            }
            if (itemPlacement.Y.Sizing == WidgetSizing.WIDGET_SIZING_INTERNAL)
            {
                itemPlacement.Y.Size = 15;
            }

            if (itemPlacement.X.Sizing == WidgetSizing.WIDGET_SIZING_CHILDREN)
            {
                itemPlacement.X.Size = 0;
                if (item.children != null && item.children.Count > 0)
                {
                    if (itemPlacement.sizingWidgets != null && item.placement.sizingWidgets.Length > 0)
                    {
                        float maxSize = 0;
                        for (int i = 0; i < itemPlacement.sizingWidgets.Length; i++)
                        {
                            for (int j = 0; j < item.children.Count; j++)
                            {
                                if (item.children[j].widget == itemPlacement.sizingWidgets[i])
                                {
                                    float size = item.children[j].placement.X.Size ?? 0;
                                    if (item.children[j].placement.X.Sizing == WidgetSizing.WIDGET_SIZING_INTERNAL)
                                        size = 45;
                                    float pos = item.children[j].placement.X.Pos ?? 0;
                                    float high = item.children[j].placement.X.HighPos ?? 0;
                                    maxSize = Math.Max(maxSize, size + pos + high);
                                    break;
                                }
                            }
                        }
                        itemPlacement.X.Size = maxSize;
                    }
                    else if (itemPlacement.sizingWidget != null)
                    {
                        for (int j = 0; j < item.children.Count; j++)
                        {
                            if (item.children[j].widget == itemPlacement.sizingWidget)
                            {
                                float size = item.children[j].placement.X.Size ?? 0;
                                if (item.children[j].placement.X.Sizing == WidgetSizing.WIDGET_SIZING_INTERNAL)
                                    size = 45;
                                float pos = item.children[j].placement.X.Pos ?? 0;
                                float high = item.children[j].placement.X.HighPos ?? 0;
                                itemPlacement.X.Size = size + pos + high;
                                break;
                            }
                        }
                    }
                }
            }

            if (itemPlacement.Y.Sizing == WidgetSizing.WIDGET_SIZING_CHILDREN)
            {
                itemPlacement.Y.Size = 0;
                if (item.children != null && item.children.Count > 0)
                {
                    if (itemPlacement.sizingWidgets != null && itemPlacement.sizingWidgets.Length > 0)
                    {
                        float maxSize = 0;
                        for (int i = 0; i < itemPlacement.sizingWidgets.Length; i++)
                        {
                            for (int j = 0; j < item.children.Count; j++)
                            {
                                if (item.children[j].widget == itemPlacement.sizingWidgets[i])
                                {
                                    float size = item.children[j].placement.Y.Size ?? 0;
                                    if (item.children[j].placement.Y.Sizing == WidgetSizing.WIDGET_SIZING_INTERNAL)
                                        size = 15;
                                    float pos = item.children[j].placement.Y.Pos ?? 0;
                                    float high = item.children[j].placement.Y.HighPos ?? 0;
                                    maxSize = Math.Max(maxSize, size + pos + high);
                                    break;
                                }
                            }
                        }
                        itemPlacement.Y.Size = maxSize;
                    }
                    else if (itemPlacement.sizingWidget != null)
                    {
                        for (int j = 0; j < item.children.Count; j++)
                        {
                            if (item.children[j].widget == itemPlacement.sizingWidget)
                            {
                                float size = item.children[j].placement.Y.Size ?? 0;
                                if (item.children[j].placement.Y.Sizing == WidgetSizing.WIDGET_SIZING_INTERNAL)
                                    size = 15;
                                float pos = item.children[j].placement.Y.Pos ?? 0;
                                float high = item.children[j].placement.Y.HighPos ?? 0;
                                itemPlacement.Y.Size = size + pos + high;
                                break;
                            }
                        }
                    }
                }
            }

            if (item.children != null)
            {
                for (int i = 0; i < item.children.Count; i++)
                {
                    FinalPlacement(item.children[i]);
                }
            }
        }

        private static void Add(Item parent, XdbObject xdb, bool isPrototypeChild, bool directChild)
        {
            if (xdb == null)
                return;

            if (xdb.GetType() == typeof(UIAddon))
            {
                UIAddon addon = (UIAddon)xdb;
                if (addon.Forms == null)
                    return;
                for (int i = 0; i < addon.Forms.Length; i++)
                {
                    Add(parent, addon.Forms[i].Form, false, true);
                }
            }

            if (xdb.GetType() == typeof(Widget) || xdb.GetType().IsSubclassOf(typeof(Widget)))
            {
                Widget widget = (Widget)xdb;
                Item item = new Item();
                item.widget = widget;
                item.hasPrototype = widget.Header.Prototype != null;
                item.isPrototypeChild = isPrototypeChild;
                item.directChild = directChild;

                int? priority = widget.Priority;
                item.backLayer = widget.BackLayer;
                item.frontLayer = widget.FrontLayer;
                CollectPlacement(item, widget);
                Widget prototype = widget.Header.Prototype as Widget;
                while (prototype != null)
                {
                    priority = priority ?? prototype.Priority;
                    item.backLayer = item.backLayer ?? prototype.BackLayer;
                    item.frontLayer = item.frontLayer ?? prototype.FrontLayer;
                    CollectPlacement(item, prototype);
                    prototype = prototype.Header.Prototype as Widget;
                }

                item.priority = priority ?? 0;
                item.realPlacement = CopyPlacementNonNull(item.placement);

                if (xdb.GetType() == typeof(WidgetButton))
                {
                    WidgetButton widgetButton = (WidgetButton)xdb;
                    if (widgetButton.Variants != null && widgetButton.Variants.Length > 0)
                    {
                        item.layerHighlight = widgetButton.Variants[0].LayerHighlight;
                        item.stateNormal = widgetButton.Variants[0].StateNormal.LayerMain;
                        item.statePushed = widgetButton.Variants[0].StatePushed.LayerMain;
                        item.stateHighlighted = widgetButton.Variants[0].StateHighlighted.LayerMain;
                        item.statePushedHighlighted = widgetButton.Variants[0].StatePushedHighlighted.LayerMain;
                        item.stateDisabled = widgetButton.Variants[0].StateDisabled.LayerMain;
                    }
                    WidgetButton buttonPrototype = widgetButton.Header.Prototype as WidgetButton;
                    while (buttonPrototype != null)
                    {
                        if (buttonPrototype.Variants != null && buttonPrototype.Variants.Length > 0)
                        {
                            item.layerHighlight = item.layerHighlight ?? buttonPrototype.Variants[0].LayerHighlight;
                            item.stateNormal = item.stateNormal ?? buttonPrototype.Variants[0].StateNormal.LayerMain;
                            item.statePushed = item.statePushed ?? buttonPrototype.Variants[0].StatePushed.LayerMain;
                            item.stateHighlighted = item.stateHighlighted ?? buttonPrototype.Variants[0].StateHighlighted.LayerMain;
                            item.statePushedHighlighted = item.statePushedHighlighted ?? buttonPrototype.Variants[0].StatePushedHighlighted.LayerMain;
                            item.stateDisabled = item.stateDisabled ?? buttonPrototype.Variants[0].StateDisabled.LayerMain;
                        }
                        buttonPrototype = buttonPrototype.Header.Prototype as WidgetButton;
                    }
                }

                if (parent.children == null)
                    parent.children = new List<Item>();
                parent.children.Add(item);
                item.parent = parent;

                AddChildren(widget, item, isPrototypeChild);

                prototype = widget.Header.Prototype as Widget;
                while (prototype != null)
                {
                    AddChildren(prototype, item, true);
                    prototype = prototype.Header.Prototype as Widget;
                }
            }
        }

        private static void AddChildren(Widget widget, Item item, bool isPrototypeChild)
        {
            if (widget.Children != null)
            {
                for (int i = 0; i < widget.Children.Length; i++)
                {
                    Add(item, widget.Children[i], isPrototypeChild, true);
                }
            }
            if (widget.GetType() == typeof(WidgetContainer) || widget.GetType().IsSubclassOf(typeof(WidgetContainer)))
            {
                WidgetContainer container = (WidgetContainer)widget;
                Add(item, container.border, isPrototypeChild, false);
            }
            if (widget.GetType() == typeof(WidgetScrollableContainer))
            {
                WidgetScrollableContainer container = (WidgetScrollableContainer)widget;
                Add(item, container.scrollBar, isPrototypeChild, false);
            }
            if (widget.GetType() == typeof(WidgetTextContainer))
            {
                WidgetTextContainer container = (WidgetTextContainer)widget;
                Add(item, container.scrollBar, isPrototypeChild, false);
            }
            if (widget.GetType() == typeof(WidgetEditBase) || widget.GetType().IsSubclassOf(typeof(WidgetEditBase)))
            {
                WidgetEditBase editBase = (WidgetEditBase)widget;
                Add(item, editBase.placeholder, isPrototypeChild, false);
            }
            if (widget.GetType() == typeof(WidgetEditBox))
            {
                WidgetEditBox editBox = (WidgetEditBox)widget;
                Add(item, editBox.scrollBar, isPrototypeChild, false);
            }
            if (widget.GetType() == typeof(WidgetScrollBar) || widget.GetType().IsSubclassOf(typeof(WidgetScrollBar)))
            {
                WidgetScrollBar scrollBar = (WidgetScrollBar)widget;
                Add(item, scrollBar.incButton, isPrototypeChild, false);
                Add(item, scrollBar.decButton, isPrototypeChild, false);
            }
            if (widget.GetType() == typeof(WidgetGlideScrollBar))
            {
                WidgetGlideScrollBar scrollBar = (WidgetGlideScrollBar)widget;
                Add(item, scrollBar.slider, isPrototypeChild, false);
            }
            if (widget.GetType() == typeof(WidgetDiscreteScrollBar))
            {
                WidgetDiscreteScrollBar scrollBar = (WidgetDiscreteScrollBar)widget;
                Add(item, scrollBar.slider, isPrototypeChild, false);
            }
            if (widget.GetType() == typeof(WidgetSlider) || widget.GetType().IsSubclassOf(typeof(WidgetSlider)))
            {
                WidgetSlider slider = (WidgetSlider)widget;
                Add(item, slider.sliderButton, isPrototypeChild, false);
            }
        }

        public int CompareTo(Item other)
        {
            if (other == null)
                return 1;
            return this.priority.CompareTo(other.priority);
        }
    }
}
