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
            // ��� ��������� ����� ���������� ��� ������� �����
            return Mathf.Floor(money).ToString("F0");
        }

        // ���������� ������� ��������
        int suffixIndex = 0;
        float displayValue = money;

        while (displayValue >= 1000f && suffixIndex < _suffixes.Length - 1)
        {
            displayValue /= 1000f;
            suffixIndex++;
        }

        // ����������� � 2 ������� ����� �������
        return displayValue.ToString("F2") + _suffixes[suffixIndex];
    }

    public static string FormatMoney(float money)
    {
        if (money < 1000f)
        {
            // ��� ��������� ����� ���������� ��� ������� �����
            return Mathf.Floor(money).ToString("F0");
        }

        // ���������� ������� ��������
        int suffixIndex = 0;
        float displayValue = money;

        while (displayValue >= 1000f && suffixIndex < _suffixes.Length - 1)
        {
            displayValue /= 1000f;
            suffixIndex++;
        }

        // ����������� � 2 ������� ����� �������
        return displayValue.ToString("F2") + _suffixes[suffixIndex];
    }

    // ������������ ����� - �������� ������ ��������
    // � ������ ���� �������������, �� ����� �������
    public static string FormatMoney(double money)
    {
        if (money < 1000f)
        {
            // ��� ��������� ����� ���������� ��� ������� �����
            return Mathf.Floor((float)money).ToString("F0");
        }

        // ���������� ������� ��������
        int suffixIndex = 0;
        float displayValue = (float)money;

        while (displayValue >= 1000f && suffixIndex < _suffixes.Length - 1)
        {
            displayValue /= 1000f;
            suffixIndex++;
        }

        // ����������� � 2 ������� ����� �������
        return displayValue.ToString("F2") + _suffixes[suffixIndex];
    }
}
