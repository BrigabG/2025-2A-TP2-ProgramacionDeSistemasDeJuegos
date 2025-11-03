using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFactory : MonoBehaviour, ISetup<SpawnMenuConfig>
{
    [SerializeField] SpawnMenuConfig config;
    [SerializeField] Transform container;

    readonly List<Button> spawnedButtons = new List<Button>();

void Start()
    {
        if (config)
            Setup(config);
    }

    void OnDestroy()
    {
        Clear();
    }

    public void Setup(SpawnMenuConfig value)
    {
        config = value;
        Rebuild();
    }

    void Rebuild()
    {
        Clear();
        if (!config || !container)
            return;

        if (!ServiceLocator.TryGet(out ISpawnButtonFactory buttonFactory))
            return;
        if (!ServiceLocator.TryGet(out IGameServices services) || services.CharacterSpawner == null)
            return;

        if (config.SpawnerModel)
            services.CharacterSpawner.Setup(config.SpawnerModel);

        var entries = config.Buttons;
        if (entries == null)
            return;

        foreach (var entry in entries)
        {
            if (!entry)
                continue;
            var button = buttonFactory.Create(entry, config.ButtonPrefab, container);
            if (button)
                spawnedButtons.Add(button);
        }
    }

    void Clear()
    {
        foreach (var button in spawnedButtons)
        {
            if (button)
                Destroy(button.gameObject);
        }
        spawnedButtons.Clear();
    }
}
