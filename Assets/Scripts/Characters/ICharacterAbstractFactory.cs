using UnityEngine;
public interface ICharacterAbstractFactory
{
    GameObject Create(CharacterConfig config, Pose pose);
}