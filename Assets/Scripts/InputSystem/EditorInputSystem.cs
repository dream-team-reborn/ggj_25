using UnityEngine;

namespace InputSystem
{
    public class EditorInputSystem : IInput
    {
        public void Initialize()
        {
            
        }

        public float GetMovementInput()
        {
            return Input.GetKey(KeyCode.Space) ? 1f : 0f;
        }
        
        public sbyte GetRotationInput()
        {
            return (sbyte)(Input.GetKey(KeyCode.A) ? 1 : Input.GetKey(KeyCode.D) ? -1 : 0);
        }
    }
}