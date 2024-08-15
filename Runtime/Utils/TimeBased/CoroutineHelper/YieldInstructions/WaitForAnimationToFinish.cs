using UnityEngine;

namespace UtilsToolbox.Utils.TimeBased.CoroutineHelper.YieldInstructions
{
    public class WaitForAnimationToFinish : CustomYieldInstruction
    {
        private readonly Animator _animator;
        private readonly string _animationName;
        private readonly int _animationHash;
        private readonly int _layerIndex;

        private AnimatorStateInfo StateInfo => _animator.GetCurrentAnimatorStateInfo(_layerIndex);
        
        private bool CorrectAnimationIsPlaying
        {
            get
            {
                if (_animationName != string.Empty)
                {
                    return StateInfo.IsName(_animationName);
                }

                if (_animationHash != 0)
                {
                    return StateInfo.fullPathHash == _animationHash;
                }

                return false;
            }
        }

        private bool AnimationIsDone => StateInfo.normalizedTime >= 1;

        public override bool keepWaiting => CorrectAnimationIsPlaying && !AnimationIsDone;

        /// <summary>
        /// Creates a new yield-instruction
        /// </summary>
        /// <param name="animator"> animator to track </param>
        /// <param name="animationName"> name of the animation </param>
        /// <param name="layerIndex"> layer the animation is playing on </param>
        public WaitForAnimationToFinish(Animator animator, string animationName, int layerIndex = 0)
        {
            _animator = animator;
            _layerIndex = layerIndex;
            _animationName = animationName;
        }
        
        /// <summary>
        /// Creates a new yield-instruction
        /// </summary>
        /// <param name="animator"> animator to track </param>
        /// <param name="animationHash"> hash of the animation </param>
        /// <param name="layerIndex"> layer the animation is playing on </param>
        public WaitForAnimationToFinish(Animator animator, int animationHash, int layerIndex = 0)
        {
            _animator = animator;
            _layerIndex = layerIndex;
            _animationHash = animationHash;
        }
    }
}