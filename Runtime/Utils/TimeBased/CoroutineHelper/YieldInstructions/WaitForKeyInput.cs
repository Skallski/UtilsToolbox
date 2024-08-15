using UnityEngine;

namespace UtilsToolbox.Utils.TimeBased.CoroutineHelper.YieldInstructions
{
    public sealed class WaitForKeyInput : CustomYieldInstruction
    {
        public enum InputType
        {
            Press,
            Hold,
            Release
        }

        private readonly KeyCode _keyCode;
        private readonly InputType _inputType;
        
        public override bool keepWaiting
        {
            get
            {
                return _inputType switch
                {
                    InputType.Press => !Input.GetKeyDown(_keyCode),
                    InputType.Hold => !Input.GetKey(_keyCode),
                    InputType.Release => !Input.GetKeyUp(_keyCode),
                    _ => !Input.GetKeyDown(_keyCode)
                };
            }
        }

        /// <summary>
        /// Creates a new yield-instruction
        /// </summary>
        /// <param name="keyCode">  </param>
        /// <param name="inputType">  </param>
        public WaitForKeyInput(KeyCode keyCode, InputType inputType)
        {
            _keyCode = keyCode;
            _inputType = inputType;
        }
    }
}