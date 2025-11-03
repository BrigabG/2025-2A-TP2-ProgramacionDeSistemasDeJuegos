using UnityEngine;

[CreateAssetMenu(menuName = "Spawning/Character Config")]
public class CharacterConfig : ScriptableObject
{
    [field: SerializeField] public Character Prefab { get; private set; }
    [field: SerializeField] public CharacterModel CharacterModel { get; private set; }
    [field: SerializeField] public PlayerControllerModel ControllerModel { get; private set; }
    [field: SerializeField] public RuntimeAnimatorController AnimatorController { get; private set; }
}