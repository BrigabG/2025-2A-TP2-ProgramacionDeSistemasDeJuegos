using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour, ISetup<SpawnButtonConfig>
{
    [SerializeField] Button button;
    [SerializeField] TMP_Text label;
    [SerializeField] Text legacyLabel;

    SpawnButtonConfig config;

    void OnEnable()
    {
        if (!button)
        {
            enabled = false;
            return;
        }
        button.onClick.AddListener(HandleClick);
    }

    void OnDisable()
    {
        if (button)
            button.onClick.RemoveListener(HandleClick);
    }

    public void Setup(SpawnButtonConfig value)
    {
        config = value;
        ApplyLabel();
    }

    void ApplyLabel()
    {
        var text = config ? config.Title : string.Empty;
        if (label)
            label.text = text;
        if (legacyLabel)
            legacyLabel.text = text;
    }

    void HandleClick()
    {
        if (config == null)
            return;

        if (!ServiceLocator.TryGet(out IGameServices services))
            return;
        var spawner = services.CharacterSpawner;
        if (spawner == null)
            return;

        spawner.Spawn(config.CharacterConfig);
    }
}