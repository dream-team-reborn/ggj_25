using UnityEngine;

namespace InputSystem
{
    public class EditorInputSystem : IInput
    {
        private bool isDragging;
        private Vector3 startTouchPos;

        public void Initialize()
        {
            
        }

        public float GetMovementInput()
        {
            return Input.GetKey(KeyCode.Space) ? 1f : 0f;
        }
        
        public sbyte GetRotationInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPos = Input.mousePosition;
                isDragging = true;
            }
                
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
            
            if (!isDragging)
                return 0;
            
            return startTouchPos.x - Input.mousePosition.x > 0 ? (sbyte)1 : (sbyte)-1;
        }
    }
}