using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOUIEditor.ResourceSystem
{
    public class XdbObjectConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value != null)
            {
                XdbObject xdb = (XdbObject)value;
                if (xdb.isIngame)
                {
                    return xdb.GetFullPath();
                }
                else
                {
                    return Project.GetRelativePath(xdb.GetFullPath());
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string str = value as string;
                if (!string.IsNullOrEmpty(str))
                {
                    Type type = context.PropertyDescriptor.PropertyType;
                    MethodInfo method = typeof(XdbObject).GetMethod("Load").MakeGenericMethod(new Type[] { type });
                    if (Project.IsIngameHref(str))
                    {
                        return method.Invoke(this, new object[] { str, null, true });
                    }
                    else if (File.Exists(str))
                    {
                        return method.Invoke(this, new object[] { str, null, false });
                    }
                    else
                    {
                        str = Project.GetFullPath(str);
                        if (File.Exists(str))
                        {
                            return method.Invoke(this, new object[] { str, null, false });
                        }
                    }
                }
                return null;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
