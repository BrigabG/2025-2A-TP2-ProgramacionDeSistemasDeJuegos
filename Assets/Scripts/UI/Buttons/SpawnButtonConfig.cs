using UnityEngine;

[CreateAssetMenu(menuName = "Menu/Spawn Button Config")]
public class SpawnButtonConfig : ButtonConfig
{
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public CharacterConfig CharacterConfig { get; private set; }
}
