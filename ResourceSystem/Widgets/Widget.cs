using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AOUIEditor.ResourceSystem
{
    // LuaApi/Widget.html

    public class Widget : XdbObject
    {
        [Category("Widget")]
        [Description("Системное название виджета")]
        public string Name { get; set; }

        [DefaultValue(0)]
        [Category("Widget")]
        [Description("Приоритет отображения (также влияет на обработку мышиных событий) виджета в списке виджетов своего родителя. То есть с помощью этого поля можно сформировать иерархию отображения виджетов всего аддона")]
        public int? Priority { get; set; }

        [Category("Widget")]
        [Description("Дочерние виджеты. Почти каждый виджет может содержать дочерние виджеты, за исключением особых случаев типа слайдера и т.п. Дочерние виджеты отображаются поверх родителя и перехватывают реакции (если они объявлены и на них подписаны обработчики) раньше родительского виджета за исключением особых случаев, оговоренных ниже")]
        [Editor(typeof(WidgetCollectionEditor), typeof(UITypeEditor))]
        public Widget[] Children { get; set; }

        [Category("Widget")]
        [Description("Слои для отображения какой-либо текстуры. Могут отсутствовать. BackLayer - нижний слой, FrontLayer - верхний слой")]
        public WidgetLayer BackLayer { get; set; }

        [Category("Widget")]
        [Description("Слои для отображения какой-либо текстуры. Могут отсутствовать. BackLayer - нижний слой, FrontLayer - верхний слой")]
        public WidgetLayer FrontLayer { get; set; }

        [Category("Widget")]
        [Description("Текстура с альфой. Используется для задания маски, по которой будет обрезана основная текстура данного контрола и всех его детей")]
        public UISingleTexture textureMask { get; set; }

        [Category("Widget")]
        [Description("Черно-белая текстура (по степеням 2) для задания активной (белые пиксели) области для кликов мышью. Нужно вручную выставлять mipSW = 0 при экспорте")]
        public UISingleTexture pickMask { get; set; }

        [Category("Widget")]
        [Description("Описание расположение виджета")]
        public WidgetPlacementXY Placement { get; set; }

        [DefaultValue(true)]
        [Category("Widget")]
        [Description("Виден ли виджет. По умолчанию true. если виджет не виден, то он недоступен и для реакций")]
        public bool? Visible { get; set; }

        [DefaultValue(true)]
        [Category("Widget")]
        [Description("Доступен ли виджет и все его дочерние виджеты для реакций. Может влиять на внешний вид (виджет \"засеривается\"). По умолчанию true")]
        public bool? Enabled { get; set; }

        [DefaultValue(0)]
        [Category("Widget")]
        [Description("Задаёт порядок обхода контролов по клавише Tab. По умолчанию 0(не учавствует в обходе). Для участия в обходе значение должно быть больше 0")]
        public int? TabOrder { get; set; }

        [Category("Widget")]
        [Description("Список реакций на клавиатурные нажатия. Поля: bindSection: string - название секции, bindedReactions: table of string - список реакций")]
        public WidgetBindSection[] bindSections { get; set; }

        [DefaultValue(false)]
        [Category("Widget")]
        [Description("Является ли виджет прозрачным для ввода. По умолчанию false")]
        public bool? TransparentInput { get; set; }

        [DefaultValue(false)]
        [Category("Widget")]
        [Description("Обрабатывать мышиные реакции только для детей этого виджета, игнорируя сам виджет")]
        public bool? PickChildrenOnly { get; set; }

        [DefaultValue(false)]
        [Category("Widget")]
        [Description("Игнорировать двойной клик мышью для виджета и для его детей")]
        public bool? IgnoreDblClick { get; set; }

        [DefaultValue(1.0f)]
        [Category("Widget")]
        [Description("([0.0f, 1.0f]) - визуальная прозрачность виджета. По умолчанию 1.0f - непрозрачен")]
        public float? fade { get; set; }

        [DefaultValue(true)]
        [Category("Widget")]
        [Description("Нужно ли обрезать содержимое, включая дочерние виджеты, по границам данного. По умолчанию true")]
        public bool? clipContent { get; set; }

        [Category("Widget")]
        [Description("Уведомление о наведении на виджет. (Кроме виджетов со специальной обработкой - кнопок и т.д.)")]
        public string reactionOnPointing { get; set; }

        [Category("Widget")]
        [Description("Уведомление о наведении на виджет вне зависимости от его доступности для кликов. (Использовать только при сильной необходимости - потребляет много ресурсов.)")]
        public string forceReactionOnPointing { get; set; }

        [Category("Widget")]
        [Description("Уведомление о прокрутке колёсика мыши вверх")]
        public string reactionWheelUp { get; set; }

        [Category("Widget")]
        [Description("Уведомление о прокрутке колёсика мыши вниз")]
        public string reactionWheelDown { get; set; }

        [DefaultValue(false)]
        [Category("Widget")]
        [Description("Игнорировать PickChildrenOnly при скролировании колесом мыши и наведении. Всегда обрабатывать реакцию скролла колесом мыши")]
        public bool? forceWheel { get; set; }

        [Category("Widget")]
        [Description("Звук на показывание виджета")]
        public WidgetSoundBase soundShow { get; set; }

        [Category("Widget")]
        [Description("Звук на скрытие виджета")]
        public WidgetSoundBase soundHide { get; set; }

        // it must be always false in user addons
        //[DefaultValue(false)]
        //[Category("Widget")]
        //[Description("Запрещать ли пользовательским аддонам операции с виджетом. По умолчанию false. См. также AttachWidget2D")]
        //public bool? isProtected { get; set; }

        public Widget()
        {
            Placement = new WidgetPlacementXY();
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return "(" + GetType().Name + ")";
            }
            else
            {
                return Name + " (" + GetType().Name + ")";
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WidgetBindSection
    {
        public string bindSection { get; set; }
        public string[] bindedReactions { get; set; }
    }
}
