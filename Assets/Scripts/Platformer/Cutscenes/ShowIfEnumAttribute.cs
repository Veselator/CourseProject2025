using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ShowIfEnumAttribute : PropertyAttribute
{
    // Чисто для удобства
    // Что-бы скрывать ненужные поля в инспекторе

    public string enumFieldName;
    public object[] enumValues;

    public ShowIfEnumAttribute(string enumFieldName, params object[] enumValues)
    {
        this.enumFieldName = enumFieldName;
        this.enumValues = enumValues;
    }
}