using UnityEngine;

namespace InputSystem
{
    public class MobileInputSystem : IInput
    {
        private const float threshold = 0.25f;
        
        private AudioClip microphoneClip;
        private string microphoneName;
        private int sampleRate = 44100;
        private bool isMicrophoneInitialized = false;
        
        private bool isDragging;
        private bool isInputEnabled;
        private bool hasMoved;

        private Vector3 startTouchPos;
        private Vector3 lastTouchPos;

        private float startDragTime;
        private float maxDragTime = 200.0f;
        private float minDelta = 0.05f;
        
        public void Initialize()
        {
            if (Microphone.devices.Length > 0) {
                microphoneName = Microphone.devices[0];
                microphoneClip = Microphone.Start(microphoneName, true, 10, sampleRate);
                isMicrophoneInitialized = true;
            }
            else {
                Debug.LogError("No microphone found!");
            }
        }

        public float GetMovementInput()
        {
            if (!isMicrophoneInitialized || microphoneClip == null)
                return 0f;
            
            float[] samples = new float[256];
            int position = Microphone.GetPosition(microphoneName) - samples.Length + 1;
            if (position < 0)
                return 0f;

            microphoneClip.GetData(samples, position);
            float averageVolume = GetAverageVolume(samples);

            if (averageVolume > threshold) 
                return 1f;

            return 0f;
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
                
            // if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            // {
            //     OnInputPressed(touch.position);
            // }
            
            if (!isDragging)
                return 0;
            
            sbyte returnVal = 0;
            var diff = lastTouchPos - Input.mousePosition;

            if (!(diff.magnitude < 0.07f))
                returnVal = diff.x > 0 ? (sbyte)1 : (sbyte)-1;

            lastTouchPos = Input.mousePosition;

            return returnVal;
        }
        
        private float GetAverageVolume(float[] samples)
        {
            float sum = 0f;
            foreach (var sample in samples) {
                sum += Mathf.Abs(sample);
            }

            return sum / samples.Length;
        }
    }
}