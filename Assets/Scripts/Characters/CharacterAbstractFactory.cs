using UnityEngine;

public class CharacterAbstractFactory : ICharacterAbstractFactory
{
    readonly ICharacterFactory factory;

    public CharacterAbstractFactory(ICharacterFactory factory)
    {
        this.factory = factory;
    }

    public GameObject Create(CharacterConfig config, Pose pose)
    {
        if (factory == null)
            return null;
        return factory.Create(config, pose);
    }
}