
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RigidbodyMic : MonoBehaviour
{
    public float threshold = 0.1f;
    public float forceAmount = 10f;
    public TMP_Text dbgText;
    public TMP_InputField inputField;
    public Image circle;

    private AudioClip microphoneClip;
    private string microphoneName;
    private int sampleRate = 44100;
    private bool isMicrophoneInitialized = false;

    void Start()
    {
        if (Microphone.devices.Length > 0) {
            microphoneName = Microphone.devices[0];
            microphoneClip = Microphone.Start(microphoneName, true, 10, sampleRate);
            isMicrophoneInitialized = true;
        }
        else {
            Debug.LogError("No microphone found!");
        }
        
        inputField.text = threshold.ToString(CultureInfo.InvariantCulture);
    }

    void Update()
    {
        if (isMicrophoneInitialized && microphoneClip != null) {
            float[] samples = new float[256];
            int position = Microphone.GetPosition(microphoneName) - samples.Length + 1;
            if (position < 0) return;

            microphoneClip.GetData(samples, position);
            float averageVolume = GetAverageVolume(samples);

            if (averageVolume > threshold) {
                ApplyForce(averageVolume);
            }
            else {
                circle.color = Color.white;
            }
        }
    }

    float GetAverageVolume(float[] samples)
    {
        float sum = 0f;
        foreach (var sample in samples) {
            sum += Mathf.Abs(sample);
        }

        return sum / samples.Length;
    }

    void ApplyForce(float avg)
    {
        //rb.AddForce(Vector3.up * forceAmount, ForceMode.Impulse);
        dbgText.text = $"Force applied! F={avg}";
        circle.color = Color.red;
    }

    public void OnThresholdChanged(string str)
    {
        if (float.TryParse(str, out var val)) {
            threshold = val;
        }
    }
}