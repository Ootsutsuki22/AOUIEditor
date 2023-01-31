using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetEditBase.html

    public class WidgetEditBase : Widget
    {
        [Category("WidgetEdit")]
        [Description("Должно быть задано обязательно! Текстурный слой для показа курсора в состоянии 1")]
        public WidgetLayer Cursor1Layer { get; set; }

        [Category("WidgetEdit")]
        [Description("Должно быть задано обязательно! Текстурный слой для показа курсора в состоянии 2")]
        public WidgetLayer Cursor2Layer { get; set; }

        [DefaultValue(2)]
        [Category("WidgetEdit")]
        [Description("Ширина курсора. По умолчанию 2")]
        public int? CursorWidth { get; set; }

        [DefaultValue(500)]
        [Category("WidgetEdit")]
        [Description("Время переключения состояний курсора в миллисекундах. По умолчанию 500")]
        public int? CursorChangeTimeMs { get; set; }

        [DefaultValue(-1)]
        [Category("WidgetEdit")]
        [Description("Максимальное количество символов в строке. по умолчанию -1 - количество не ограниченно")]
        public int? maxSymbolsCount { get; set; }

        [DefaultValue(true)]
        [Category("WidgetEdit")]
        [Description("Если false, то в строку ввода невозможно вставить текст из буфера обмена. По умолчанию true")]
        public bool? canPaste { get; set; }

        [Category("WidgetEdit")]
        [Description("Формат вывода текста на экран (разрывы строк, многострочность и т.п.)")]
        public WidgetTextStyle TextStyle { get; set; }

        [Category("WidgetEdit")]
        [Description("Должно быть задано обязательно! Стиль оформления основного текста (например, ColorBlue)")]
        public string globalClassName { get; set; }

        [Category("WidgetEdit")]
        [Description("Должно быть задано обязательно! Стиль оформления текста выделения (например, ColorWhite)")]
        public string selectionClassName { get; set; }

        [Category("WidgetEdit")]
        [Description("Должно быть задано обязательно! Текстурный слой для отрисовки подложки выделения")]
        public WidgetLayer selectionLayer { get; set; }

        [Category("WidgetEdit")]
        [Description("Название фильтра, разрешающего только буквы, перечисленные в нём. Значения: \"RUSSIAN\", \"NUMBERS\", \"INTEGER\". См. EditBaseTextFilter")]
        public string filterAlias { get; set; }

        [Category("WidgetEdit")]
        [Description("Заменяющий текст")]
        public Widget placeholder { get; set; }

        [Category("WidgetEdit")]
        [Description("Нажатие Esc")]
        public string ReactionEsc { get; set; }

        [Category("WidgetEdit")]
        [Description("В поле ввода изменилось значение")]
        public string ReactionChanged { get; set; }

        [Category("WidgetEdit")]
        [Description("Был вставлен текст, в случае canPaste == false можно оповестить о невозможности вставки текста")]
        public string reactionPaste { get; set; }

        [Category("WidgetEdit")]
        [Description("Изменился фокус (поле реакции active = true - появился, false - исчез)")]
        public string reactionFocusChanged { get; set; }

        [Category("WidgetEdit")]
        [Description("Реакция на смену состояния клавиши Caps Lock (поле реакции active состояние CapsLock (true - включен, false - выключен) )")]
        public string reactionCapsLock { get; set; }

        public WidgetEditBase()
        {
            TextStyle = new WidgetTextStyle();
        }
    }
}
