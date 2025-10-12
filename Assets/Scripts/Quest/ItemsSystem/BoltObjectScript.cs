using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltObjectScript : InteractableItem
{
    [SerializeField] private VentilyatsiyaScript vent;
    [SerializeField] private Sprite openedBolt;

    private bool isOpened = false;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer == null)
        {
            Debug.LogError($"SpriteRenderer not found on {gameObject.name}!");
        }
    }

    public override void ProcessMessage(string message)
    {
        if (message == QuestMessage.TAKE_BOLT && !isOpened)
        {
            isOpened = true;

            if (_spriteRenderer != null)
            {
                _spriteRenderer.sprite = openedBolt;
            }

            vent.CheckBolt();
        }
    }
}