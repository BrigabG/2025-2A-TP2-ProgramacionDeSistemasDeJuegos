using UnityEngine;

public class Startup : MonoBehaviour
{
    GameServices services;
    ICharacterFactory characterFactory;
    ICharacterAbstractFactory characterAbstractFactory;
    ISpawnButtonFactory buttonFactory;

    void Awake()
    {
        ServiceLocator.Clear();
        RegisterServices();
    }

    void OnDestroy()
    {
        UnregisterServices();
        ServiceLocator.Clear();
    }

    void RegisterServices()
    {
        services = new GameServices();
        ServiceLocator.Register<IGameServices>(services);

        characterFactory = new CharacterFactory();
        characterAbstractFactory = new CharacterAbstractFactory(characterFactory);
        ServiceLocator.Register<ICharacterFactory>(characterFactory);
        ServiceLocator.Register<ICharacterAbstractFactory>(characterAbstractFactory);

        buttonFactory = new SpawnButtonFactory();
        ServiceLocator.Register<ISpawnButtonFactory>(buttonFactory);
    }

    void UnregisterServices()
    {
        ServiceLocator.Unregister<ICharacterAbstractFactory>();
        ServiceLocator.Unregister<ICharacterFactory>();
        ServiceLocator.Unregister<ISpawnButtonFactory>();
        ServiceLocator.Unregister<IGameServices>();
        services = null;
        characterFactory = null;
        characterAbstractFactory = null;
        buttonFactory = null;
    }
}
