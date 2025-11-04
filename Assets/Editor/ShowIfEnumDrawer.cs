using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomPropertyDrawer(typeof(ShowIfEnumAttribute))]
public class ShowIfEnumDrawer : PropertyDrawer
{
    // ВНИМАНИЕ! ИИ-КОД! Я В НЕГО НЕ ВНИКАЛ! ДЕРЖАТЬСЯ НА РАССТОЯНИИ НЕ МЕНЕЕ 100 КМ!
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute showIf = (ShowIfEnumAttribute)attribute;

        // Отримуємо батьківський об'єкт
        object parent = GetParentObject(property);

        if (parent == null)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        // Отримуємо поле enum через рефлексію
        FieldInfo enumField = parent.GetType().GetField(showIf.enumFieldName);

        if (enumField == null)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        // Отримуємо поточне значення enum
        object enumValue = enumField.GetValue(parent);

        // Перевіряємо чи поточне значення є в списку допустимих
        bool shouldShow = false;
        foreach (var value in showIf.enumValues)
        {
            if (enumValue.Equals(value))
            {
                shouldShow = true;
                break;
            }
        }

        // Показуємо поле тільки якщо умова виконана
        if (shouldShow)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute showIf = (ShowIfEnumAttribute)attribute;
        object parent = GetParentObject(property);

        if (parent == null) return EditorGUI.GetPropertyHeight(property, label);

        FieldInfo enumField = parent.GetType().GetField(showIf.enumFieldName);
        if (enumField == null) return EditorGUI.GetPropertyHeight(property, label);

        object enumValue = enumField.GetValue(parent);

        bool shouldShow = false;
        foreach (var value in showIf.enumValues)
        {
            if (enumValue.Equals(value))
            {
                shouldShow = true;
                break;
            }
        }

        return shouldShow ? EditorGUI.GetPropertyHeight(property, label) : 0;
    }

    private object GetParentObject(SerializedProperty property)
    {
        var path = property.propertyPath.Replace(".Array.data[", "[");
        object obj = property.serializedObject.targetObject;
        var elements = path.Split('.');

        for (int i = 0; i < elements.Length - 1; i++)
        {
            var element = elements[i];
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue_Imp(obj, elementName, index);
            }
            else
            {
                obj = GetValue_Imp(obj, element);
            }
        }
        return obj;
    }

    private object GetValue_Imp(object source, string name)
    {
        if (source == null) return null;
        var type = source.GetType();

        while (type != null)
        {
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (f != null) return f.GetValue(source);

            var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p != null) return p.GetValue(source, null);

            type = type.BaseType;
        }
        return null;
    }

    private object GetValue_Imp(object source, string name, int index)
    {
        var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
        if (enumerable == null) return null;
        var enm = enumerable.GetEnumerator();
        for (int i = 0; i <= index; i++)
        {
            if (!enm.MoveNext()) return null;
        }
        return enm.Current;
    }
}