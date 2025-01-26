using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject endGO;
    [SerializeField] private TMP_Text endText;
    [SerializeField] private TMP_Text scoreText;
    [Header("Data")] 
    [SerializeField] float winThreshold = 0.75f;
    [SerializeField] float maxScoreCorrection = 0.9f;
    [SerializeField] string winText = "You win!";
    [SerializeField] string loseText = "Sins wins!";

    private float maxScore;
    private float currentScore;

    public void OnNewGame_Signal()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Awake()
    {
        playerManager.OnPlayerInitialized += Initialize;
        playerManager.OnPlayerDied += OnPlayerDied;
        endGO.SetActive(false);
        scoreText.text = "0";
    }

    private void OnPlayerDied()
    {
        EndGame(false);
    }

    private void Initialize()
    {
        maxScore = playerManager.maskTextureSize * maxScoreCorrection;
        currentScore = 0;

        playerManager.OnErasedPixels += OnErasedPixels;
        playerManager.OnPlayerInitialized -= Initialize;
    }

    private void OnErasedPixels(int nErased)
    {
        currentScore += nErased;
        CalculatePercentage();
    }

    private void CalculatePercentage()
    {
        float percentage = currentScore / maxScore;
        //Debug.Log($"Percentage: {percentage}");

        scoreText.text = percentage < 0.01f 
            ? $"{percentage * 100:0.0}" 
            : $"{percentage * 100:0}";
        
        if (percentage > winThreshold) {
            EndGame(true);
        }
    }

    private void EndGame(bool win)
    {
        endGO.SetActive(true);
        endText.SetText(win ? winText : loseText);
        playerManager.EndGame();
    }
}