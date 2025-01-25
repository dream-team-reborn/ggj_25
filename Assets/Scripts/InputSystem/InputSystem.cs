using System;
using UnityEngine;

namespace InputSystem
{
    public interface IInput
    {
        void Initialize();
        float GetMovementInput();
        sbyte GetRotationInput();
    }
}

namespace InputSystem
{
    public class InputSystem : MonoBehaviour
    {
        private IInput input;
        private static InputSystem instance;
        
        public static InputSystem Instance => instance;
        public Action<float> OnMove;
        public Action<sbyte> OnRotate;
        
        private void Awake()
        {
            instance = this;
            
            #if UNITY_EDITOR
            input = new EditorInputSystem();
            #else
            input = new MobileInputSystem();
            #endif
        }
        
        private void Start()
        {
            input.Initialize();
        }
        
        private void Update()
        {
            float inputVal = input.GetMovementInput();
            sbyte rotationInput = input.GetRotationInput();
            
            if (inputVal > 0f)
                OnMove?.Invoke(inputVal);
            
            if (rotationInput != 0)
                OnRotate?.Invoke(rotationInput);
        }
    }
}