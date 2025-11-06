using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Menu/Spawn Menu Config")]
public class SpawnMenuConfig : ScriptableObject
{
    [field: SerializeField] public CharacterSpawnerModel SpawnerModel { get; private set; }
    [field: SerializeField] public Button ButtonPrefab { get; private set; }
    [field: SerializeField] public ButtonConfig[] Buttons { get; private set; }
}
