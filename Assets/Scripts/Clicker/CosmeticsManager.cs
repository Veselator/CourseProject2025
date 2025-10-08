using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticsManager : MonoBehaviour
{
    // Скрипт, отвечающий за косметику - смену брони, оружия, появление предметов и тд

    [SerializeField] private CharacterData[] characters;
    [SerializeField] private GameObject catMaxwell;
    [SerializeField] private GameObject tourch;
    private int currentIndex = 0;
    public static CosmeticsManager Instance {  get; private set; }

    private void Awake()
    {
        if(Instance == null) Instance = this;
        InitCharacters();
    }

    private void InitCharacters()
    {
        // Скрываем все коюстмы и показываем нужный
        foreach(CharacterData character in characters)
        {
            foreach(var hat in character.hats) hat.SetActive(false);
            foreach (var weapon in character.weapons) weapon.SetActive(false);
            foreach (var body in character.bodies) body.SetActive(false);
        }

        ShowCostumes(currentIndex);
    }

    private void UpdateCharacters(int id)
    {
        HideCostumes(currentIndex);
        currentIndex = id;
        ShowCostumes(currentIndex);
    }

    private void ShowCostumes(int id)
    {
        // Показываем костюм
        foreach (CharacterData character in characters)
        {
            character.hats[id].SetActive(true);
            character.weapons[id].SetActive(true);
            character.bodies[id].SetActive(true);
        }
    }

    private void HideCostumes(int id)
    {
        // Прячем костюм
        foreach (CharacterData character in characters)
        {
            character.hats[id].SetActive(false);
            character.weapons[id].SetActive(false);
            character.bodies[id].SetActive(false);
        }
    }

    public void UnlockCosmetic(int id)
    {
        if (id < 2)
        {
            UpdateCharacters(id + 1);
            return;
        }

        switch (id)
        {
            case 10:
                catMaxwell.SetActive(true);
                break;

            case 11:
                tourch.SetActive(true);
                break;
        }
    }
}
