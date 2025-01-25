using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuManager : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("LustScene");
        Debug.Log("Play button clicked!");
    }

    public void OnSettingsButtonClicked()
    {
        Debug.Log("Settings button clicked!");
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked!");
        Application.Quit();
    }
}
