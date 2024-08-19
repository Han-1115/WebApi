using System;
using System.Collections.Generic;
using System.Reflection;

public static class ReflectionExtensions
{
    public static List<string> GetFieldAndPropertyNames(this Type type)
    {
        List<string> names = new List<string>();

        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            names.Add(field.Name);
        }

        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo property in properties)
        {
            names.Add(property.Name);
        }

        return names;
    }
}