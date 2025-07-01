using UnityEngine;
using System;
using System.Collections.Generic;


namespace DebugConsole.Commands
{
    [CreateAssetMenu(
        fileName = "AnimationCommandConfig",
        menuName = "Debug/Animation Command Config",
        order = 1)]
    public class AnimationCommandConfig : ScriptableObject
    {
        [Tooltip("Exact name of the state within the Animator")]
        public string animatorStateName;

        [Tooltip("Parameters to override when the animation is executed")]
        public List<AnimationParameter> parameters = new();
    
        [Tooltip("Parameters applied when the animation finishes")]
        public List<AnimationParameter> exitParameters = new();

        [Serializable]
        public class AnimationParameter
        {
            public AnimatorControllerParameterType type;
            public string name;
            public bool boolValue;
            public float floatValue;
        }
    }
}

