using UnityEngine;

public class Startup : MonoBehaviour
{
    GameServices services;

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
    }

    void UnregisterServices()
    {
        ServiceLocator.Unregister<IGameServices>();
        services = null;
    }
}