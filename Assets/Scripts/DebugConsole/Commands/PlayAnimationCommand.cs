using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using DebugConsole.Interfaces;


namespace DebugConsole.Commands
{
    public class PlayAnimationCommand: ICommand<string>
    {
        private readonly Dictionary<string, AnimationCommandConfig> _animationConfigs;


        public PlayAnimationCommand(AnimationCommandConfig[] animationConfigs)
        {
            _animationConfigs = animationConfigs
                .ToDictionary(c => c.animatorStateName, c => c);
        }
        

        public string Name => "playanimation";
        public List<string> Aliases => new List<string> { "pa", "playanim" };
        public string Description => "Plays an animation on all character animators: playanimation <stateName> [duration]";

        public string DetailedHelp =>
            "playanimation <stateName> [duration]\n" +
            "    Plays animation on all characters. Duration defaults to 1.0s.\n" +
            "    Examples: playanimation jump | playanimation idle 2.5";

        public void Execute(Action<string> log, params string[] args)
        {
            if (args.Length == 0)
            {
                log("Usage: playanimation <stateName> [duration]");
                return;
            }

            var stateName = args[0];
            var duration = args.Length > 1 && float.TryParse(args[1], out var d) ? d : 1f;

            if (!_animationConfigs.TryGetValue(stateName, out var animConfig))
            {
                log($"Animation state not found: {stateName}");
                return;
            }

            var animators = CharacterAnimator.Instances;
            foreach (var charAnimator in animators)
                charAnimator.ForceAnimation(animConfig, duration);


            log($"Forced '{stateName}' on {animators.Count} CharacterAnimator(s) for {duration}s.");
        }
    }
}

