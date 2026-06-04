using System;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace IdsLibrary.Converter;
public class Helper
{
    public static string GetXmlEnumValue(Enum value)
    {
        var type = value.GetType();
        var memInfo = type.GetMember(value.ToString());
        if (memInfo.Length > 0)
        {
            if (memInfo[0].GetCustomAttributes(typeof(XmlEnumAttribute), false).FirstOrDefault() is XmlEnumAttribute attr)
            {
                return attr.Name ?? value.ToString();
            }
        }
        return value.ToString();
    }

    public static T GetEnumFromXmlValue<T>(string xmlValue) where T : Enum
    {
        foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attr = field.GetCustomAttribute<XmlEnumAttribute>();
            if (attr != null && attr.Name == xmlValue)
            {
                return (T)field.GetValue(null)!;
            }
        }

        throw new ArgumentException($"'{xmlValue}' is not a valid XML value for enum {typeof(T).Name}");
    }
}
