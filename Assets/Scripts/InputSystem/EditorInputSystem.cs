using UnityEngine;

namespace InputSystem
{
    public class EditorInputSystem : IInput
    {
        public void Initialize()
        {
            
        }

        public float GetInput()
        {
            return Input.GetKey(KeyCode.Space) ? 1f : 0f;
        }
    }
}