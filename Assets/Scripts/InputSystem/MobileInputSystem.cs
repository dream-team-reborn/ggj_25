using UnityEngine;

namespace InputSystem
{
    public class MobileInputSystem : IInput
    {
        private const float threshold = 0.1f;
        
        private AudioClip microphoneClip;
        private string microphoneName;
        private int sampleRate = 44100;
        private bool isMicrophoneInitialized = false;
        
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
            return 0;
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