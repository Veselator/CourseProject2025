using UnityEngine;

public class NumsFormatter
{
    private static readonly string[] _suffixes = new string[]
    {
        "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc"
    };

    public static string FormatMoney(int money)
    {
        if (money < 1000)
        {
            // Для маленьких чисел показываем без дробной части
            return Mathf.Floor(money).ToString("F0");
        }

        // Определяем порядок величины
        int suffixIndex = 0;
        float displayValue = money;

        while (displayValue >= 1000f && suffixIndex < _suffixes.Length - 1)
        {
            displayValue /= 1000f;
            suffixIndex++;
        }

        // Форматируем с 2 знаками после запятой
        return displayValue.ToString("F2") + _suffixes[suffixIndex];
    }

    public static string FormatMoney(float money)
    {
        if (money < 1000f)
        {
            // Для маленьких чисел показываем без дробной части
            return Mathf.Floor(money).ToString("F0");
        }

        // Определяем порядок величины
        int suffixIndex = 0;
        float displayValue = money;

        while (displayValue >= 1000f && suffixIndex < _suffixes.Length - 1)
        {
            displayValue /= 1000f;
            suffixIndex++;
        }

        // Форматируем с 2 знаками после запятой
        return displayValue.ToString("F2") + _suffixes[suffixIndex];
    }

    // Небезопасный метод - возможна потеря точности
    // В рамках игры несущественно, но важно помнить
    public static string FormatMoney(double money)
    {
        if (money < 1000f)
        {
            // Для маленьких чисел показываем без дробной части
            return Mathf.Floor((float)money).ToString("F0");
        }

        // Определяем порядок величины
        int suffixIndex = 0;
        float displayValue = (float)money;

        while (displayValue >= 1000f && suffixIndex < _suffixes.Length - 1)
        {
            displayValue /= 1000f;
            suffixIndex++;
        }

        // Форматируем с 2 знаками после запятой
        return displayValue.ToString("F2") + _suffixes[suffixIndex];
    }
}
