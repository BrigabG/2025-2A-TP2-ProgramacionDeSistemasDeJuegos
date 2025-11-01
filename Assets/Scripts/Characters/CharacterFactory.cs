using UnityEngine;

public class CharacterFactory : ICharacterFactory
{
    public GameObject Create(CharacterConfig config, Pose pose)
    {
        if (!config || !config.Prefab)
            return null;

        var instance = Object.Instantiate(config.Prefab, pose.position, pose.rotation);

        ApplyCharacter(instance.gameObject, config.CharacterModel);
        ApplyController(instance.gameObject, config.ControllerModel);
        ApplyAnimator(instance.gameObject, config.AnimatorController);

        return instance.gameObject;
    }

    static void ApplyCharacter(GameObject gameObject, CharacterModel model)
    {
        if (model == null)
            return;

        if (gameObject.TryGetComponent(out ISetup<CharacterModel> setup))
        {
            setup.Setup(model);
            return;
        }

        var character = gameObject.GetComponent<Character>();
        if (!character)
            character = gameObject.AddComponent<Character>();
        character.Setup(model);
    }

    static void ApplyController(GameObject gameObject, PlayerControllerModel model)
    {
        if (model == null)
            return;

        if (gameObject.TryGetComponent(out ISetup<IPlayerControllerModel> setup))
        {
            setup.Setup(model);
            return;
        }

        var controller = gameObject.GetComponent<PlayerController>();
        if (!controller)
            controller = gameObject.AddComponent<PlayerController>();
        controller.Setup(model);
    }

    static void ApplyAnimator(GameObject gameObject, RuntimeAnimatorController controller)
    {
        if (!controller)
            return;

        var animator = gameObject.GetComponentInChildren<Animator>();
        if (!animator)
            animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = controller;
    }
}