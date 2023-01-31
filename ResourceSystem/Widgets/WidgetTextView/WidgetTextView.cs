using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/WidgetTextView.html

    public class WidgetTextView : Widget
    {
        [Category("WidgetTextView")]
        [Description("Ссылка на текстовый файл. Если внутри файла правильно отформатированный XHTML документ, то текстовый виджет будет содержать ValuedText, иначе простой WString")]
        public TextObject FormatFileRef { get; set; }

        [Category("WidgetTextView")]
        [Description("Список подстановочных значений по умолчанию. Каждый элемент имеет поля: Tag: string - название подстановочного значение, TagValueFileRef: TextFileRef - ссылка на текстовый файл со значением")]
        public WidgetTextTaggedValue[] TextValues { get; set; }

        [Category("WidgetTextView")]
        [Description("Тег по-умолчанию. Используется для удобства, если формат текста подразумевает использование только одного значения подстановки")]
        public string DefaultTag { get; set; }

        [Category("WidgetTextView")]
        [Description("Стиль текста")]
        public WidgetTextStyle TextStyle { get; set; }

        [DefaultValue(0f)]
        [Category("WidgetTextView")]
        [Description("Минимальная ширина виджета при переразмеривании. По умолчанию 0.0f")]
        public float? minWidth { get; set; }

        [DefaultValue(0f)]
        [Category("WidgetTextView")]
        [Description("Максимальная ширина виджета при переразмеривании. По умолчанию 0.0f")]
        public float? maxWidth { get; set; }

        [DefaultValue(false)]
        [Category("WidgetTextView")]
        [Description("Кликабельны только объекты ValuedObject в тексте, но не сам контрол. По умолчанию false")]
        public bool? pickObjectsOnly { get; set; }

        [DefaultValue(false)]
        [Category("WidgetTextView")]
        [Description("Эскейпинг вводимого через SetVal теста. Позволяет обрабатывать ввод теста с переносами строк, предотвращает HTML-вандализм. По умолчанию false")]
        public bool? isHtmlEscaping { get; set; }

        public WidgetTextView()
        {
            TextStyle = new WidgetTextStyle();
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WidgetTextTaggedValue
    {
        public string Tag { get; set; }
        public TextObject TagValueFileRef { get; set; }
    }
}
