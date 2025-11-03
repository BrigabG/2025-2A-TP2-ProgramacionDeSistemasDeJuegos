using UnityEngine;
using UnityEngine.UI;

public class SpawnButtonFactory : ISpawnButtonFactory
{
    public Button Create(SpawnButtonConfig config, Button prefab, Transform parent)
    {
        if (!config || !prefab || !parent)
            return null;

        var instance = Object.Instantiate(prefab, parent);
        if (instance.TryGetComponent(out ISetup<SpawnButtonConfig> setup))
            setup.Setup(config);
        return instance;
    }
}