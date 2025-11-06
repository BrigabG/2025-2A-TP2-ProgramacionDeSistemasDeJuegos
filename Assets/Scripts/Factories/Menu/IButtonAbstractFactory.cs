using UnityEngine;
using UnityEngine.UI;

public interface IButtonAbstractFactory
{
    void RegisterFactory<TConfig>(IButtonFactory factory) where TConfig : ButtonConfig;
    Button Create(ButtonConfig config, Button prefab, Transform parent);
}

