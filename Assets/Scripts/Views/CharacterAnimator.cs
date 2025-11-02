using System.Collections;
using System.Collections.Generic;
using DebugConsole.Commands;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    static readonly List<CharacterAnimator> instances = new List<CharacterAnimator>();
    public static IReadOnlyList<CharacterAnimator> Instances => instances;

    [SerializeField] Character character;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] string speedParameter = "Speed";
    [SerializeField] string isJumpingParameter = "IsJumping";
    [SerializeField] string isFallingParameter = "IsFalling";

    bool isOverriding;
    Coroutine forceAnimationCoroutine;

    void Reset()
    {
        character = GetComponentInParent<Character>();
        animator = GetComponentInParent<Animator>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    void Awake()
    {
        if (!character)
            character = GetComponentInParent<Character>();
        if (!animator)
            animator = GetComponentInParent<Animator>();
        if (!spriteRenderer)
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    void OnEnable()
    {
        if (!instances.Contains(this))
            instances.Add(this);
        if (!character || !animator || !spriteRenderer)
        {
            Debug.LogError($"{name} <color=grey>({GetType().Name})</color>: At least one reference is null!");
            enabled = false;
        }
    }

    void Update()
    {
        if (isOverriding)
            return;
        var speed = character.Velocity;
        animator.SetFloat(speedParameter, Mathf.Abs(speed.x));
        animator.SetBool(isJumpingParameter, character.Velocity.y > 0);
        animator.SetBool(isFallingParameter, character.Velocity.y < 0);
        spriteRenderer.flipX = speed.x < 0;
    }

    void OnDisable()
    {
        instances.Remove(this);
    }

    public void ForceAnimation(AnimationCommandConfig animConfig, float duration = 1f)
    {
        if (forceAnimationCoroutine != null)
            StopCoroutine(forceAnimationCoroutine);

        forceAnimationCoroutine = StartCoroutine(ForceAnimationCoroutine(animConfig, duration));
    }

    void ApplyParameter(AnimationCommandConfig.AnimationParameter param)
    {
        switch (param.type)
        {
            case AnimatorControllerParameterType.Bool:
                animator.SetBool(param.name, param.boolValue);
                break;
            case AnimatorControllerParameterType.Float:
                animator.SetFloat(param.name, param.floatValue);
                break;
            case AnimatorControllerParameterType.Trigger:
                animator.SetTrigger(param.name);
                break;
        }
    }

    IEnumerator ForceAnimationCoroutine(AnimationCommandConfig animConfig, float duration)
    {
        isOverriding = true;

        foreach (var param in animConfig.parameters)
            ApplyParameter(param);

        animator.Play(animConfig.animatorStateName);

        yield return new WaitForSeconds(duration);

        foreach (var param in animConfig.exitParameters)
            ApplyParameter(param);

        isOverriding = false;
        forceAnimationCoroutine = null;
    }
}
