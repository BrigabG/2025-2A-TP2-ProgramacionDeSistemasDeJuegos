using UnityEngine;

public interface ICharacterFactory
{
    GameObject Create(CharacterConfig config, Pose pose);
}