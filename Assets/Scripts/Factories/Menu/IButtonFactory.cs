using UnityEngine.UI;
using UnityEngine;


public interface IButtonFactory
{
    bool CanHandle(ButtonConfig config);
    Button Create(ButtonConfig config, Button prefab, Transform parent);
}

