using UnityEngine;

[CreateAssetMenu(menuName = "Menu/Spawn Button Config")]
public class SpawnButtonConfig : ScriptableObject
{
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public CharacterConfig CharacterConfig { get; private set; }
}