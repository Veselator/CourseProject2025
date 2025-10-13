using UnityEngine;

public class QuestShaker : MonoBehaviour, IPossibleToGetBool
{
    // �����, ���-�� ����� ���� ����������, ���������� �� ���������� ��� �����
    [SerializeField] private PeremikachAtLaba[] _peremikaches;
    [SerializeField] private int[] _correctValues;

    private void Start()
    {
        if (_peremikaches == null || _correctValues == null || _peremikaches.Length != _correctValues.Length) Debug.LogError("������ ����������� ��������");
    }

    public bool GetBool()
    {
        for (int i = 0; i < _peremikaches.Length; i++)
        {
            if (_peremikaches[i].CurrentValue != _correctValues[i]) return false;
        }

        return true;
    }
}
