using System.Collections;
using System.Collections.Generic;
using DebugConsole.Commands;
using UnityEngine;

public class CharacterAnimator : Registry<CharacterAnimator>
{
    [SerializeField] private Character character;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private string speedParameter = "Speed";
    [SerializeField] private string isJumpingParameter = "IsJumping";
    [SerializeField] private string isFallingParameter = "IsFalling";

    private bool _isOverriding = false;
    private Coroutine _forceAnimationCoroutine;
    private void Reset()
    {
        character = GetComponentInParent<Character>();
        animator = GetComponentInParent<Animator>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void Awake()
    {
        if (!character)
            character = GetComponentInParent<Character>();
        if (!animator)
            animator = GetComponentInParent<Animator>();
        if (!spriteRenderer)
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (!character || !animator || !spriteRenderer)
        {
            Debug.LogError($"{name} <color=grey>({GetType().Name})</color>: At least one reference is null!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (_isOverriding)
            return;
        var speed = character.Velocity;
        animator.SetFloat(speedParameter, Mathf.Abs(speed.x));
        animator.SetBool(isJumpingParameter, character.Velocity.y > 0);
        animator.SetBool(isFallingParameter, character.Velocity.y < 0);
        spriteRenderer.flipX = speed.x < 0;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable(); 
    }
    
    public void ForceAnimation(AnimationCommandConfig animConfig, float duration = 1f)
    {
        if (_forceAnimationCoroutine != null)
            StopCoroutine(_forceAnimationCoroutine);
        
        _forceAnimationCoroutine = StartCoroutine(ForceAnimationCoroutine(animConfig, duration));
    }
    
    private void ApplyParameter(AnimationCommandConfig.AnimationParameter param)
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

    private IEnumerator ForceAnimationCoroutine(AnimationCommandConfig animConfig, float duration)
    {
        _isOverriding = true;

        foreach (var param in animConfig.parameters)
            ApplyParameter(param);

        animator.Play(animConfig.animatorStateName);

        yield return new WaitForSeconds(duration);

        foreach (var param in animConfig.exitParameters)
            ApplyParameter(param);

        _isOverriding = false;
        _forceAnimationCoroutine = null;
    }
}
