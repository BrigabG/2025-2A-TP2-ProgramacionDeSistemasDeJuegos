using System;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private CharacterConfig characterConfig;

    private void Reset()
        => button = GetComponent<Button>();

    private void Awake()
    {
        if (!button)
            button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (!button)
        {
            Debug.LogError($"{name} <color=grey>({GetType().Name})</color>: {nameof(button)} is null!");
            enabled = false;
            return;
        }
        button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        button?.onClick?.RemoveListener(HandleClick);
    }

    private void HandleClick()
    {
        if (!ServiceLocator.TryGet<IGameServices>(out var services))
            return;
        var spawner = services.CharacterSpawner;
        if (spawner == null || characterConfig == null)
            return;
        spawner.Spawn(characterConfig);
    }
}
