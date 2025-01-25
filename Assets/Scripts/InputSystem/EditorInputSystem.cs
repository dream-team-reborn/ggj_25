using UnityEngine;

namespace InputSystem
{
    public class EditorInputSystem : IInput
    {
        private bool isDragging;
        private Vector3 startTouchPos;
        private Vector3 lastTouchPos;

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
                lastTouchPos = startTouchPos;
                isDragging = true;
            }
                
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
            
            if (!isDragging)
                return 0;

            sbyte returnVal = 0;
            var diff = lastTouchPos - Input.mousePosition;

            if (!(diff.magnitude < 0.07f))
                returnVal = diff.x > 0 ? (sbyte)1 : (sbyte)-1;

            lastTouchPos = Input.mousePosition;

            return returnVal;
        }
    }
}