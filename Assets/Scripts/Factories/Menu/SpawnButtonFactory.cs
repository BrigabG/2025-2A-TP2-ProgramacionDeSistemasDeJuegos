using UnityEngine;
using UnityEngine.UI;

public class SpawnButtonFactory : IButtonFactory
{
    public bool CanHandle(ButtonConfig config)
        => config is SpawnButtonConfig;

    public Button Create(ButtonConfig config, Button prefab, Transform parent)
    {
        if (config is not SpawnButtonConfig spawnConfig)
            return null;
        if (!prefab || parent == null)
            return null;

        var instance = Object.Instantiate(prefab, parent);
        if (instance.TryGetComponent(out ISetup<SpawnButtonConfig> setup))
            setup.Setup(spawnConfig);
        return instance;
    }
}

