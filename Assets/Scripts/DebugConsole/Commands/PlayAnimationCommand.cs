using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using DebugConsole.Interfaces;

namespace DebugConsole.Commands
{
    public class PlayAnimationCommand: ICommand<string>
    {
        private readonly Dictionary<string, AnimationClip> _clipsByName;

        public PlayAnimationCommand(AnimationClip[] clips)
        {
            _clipsByName = clips.ToDictionary(c => c.name, c => c);
        }

        public string Name => "playanimation";
        public List<string> Aliases => new List<string>{ "pa", "playanim" };
        public string Description => "Plays an animation on all animators: playanimation <clipName>";

        public void Execute(Action<string> log, params string[] args)
        {
            if (args.Length == 0)
            {
                log("Usage: playanimation <clipName>");
                return;
            }

            var clipName = args[0];
            if (!_clipsByName.TryGetValue(clipName, out var clip))
            {
                log($"Animation not found: {clipName}");
                return;
            }

            var animators = GameObject.FindObjectsByType<Animator>(FindObjectsSortMode.None);            foreach (var animator in animators)
                animator.Play(clip.name);

            log($"Played '{clipName}' on {animators.Length} Animator(s).");
        }
    }
}

