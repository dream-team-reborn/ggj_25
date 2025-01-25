using System;
using UnityEngine;

namespace InputSystem
{
    public interface IInput
    {
        void Initialize();
        float GetInput();
    }
}

namespace InputSystem
{
    public class InputSystem : MonoBehaviour
    {
        private IInput input;
        private static InputSystem instance;
        
        public static InputSystem Instance => instance;
        public Action<float> OnInput;
        
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
            float inputVal = input.GetInput();
            
            if (inputVal > 0f)
                OnInput?.Invoke(inputVal);
        }
    }
}