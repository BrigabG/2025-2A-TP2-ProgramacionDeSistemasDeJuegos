using UnityEngine.UI;
using UnityEngine;

public interface ISpawnButtonFactory
{
    Button Create(SpawnButtonConfig config, Button prefab, Transform parent);
}