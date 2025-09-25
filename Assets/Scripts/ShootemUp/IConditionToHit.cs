using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConditionToHit
{
    // Проверяет условие, при котором объекту может быть нанесён урон
    // SRP - класс, имплементирующий интерйефс, без понятия, кому он проверяет
    // Он просто выполняет свою работу

    abstract bool IsPossibleToHit();
    // При имплементации можно прописать условие, к примеру, есть ли у объекта щит
    // Или есть ли глобальный флаг
    // Или любое другое условие
}
