using System;
using System.Collections;
using System.Collections.Generic;
using CandyCoded.HapticFeedback;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;
    
    [SerializeField] private GameObject gameVirtualCamera;
    [SerializeField] private GameObject cutsceneVirtualCamera;
    [SerializeField] private GameObject sparkles;
    [SerializeField] private SpriteRendererEraser eraser;
    [SerializeField] private GameObject dirtyImage;
    [SerializeField] private GameObject obstaclesHolder;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartCutscene(Action<bool> callback, bool win)
    {
        StartCoroutine(EndCutsceneDelayed(callback, win));
    }
    
    private IEnumerator EndCutsceneDelayed(Action<bool> callback, bool win)
    {
        gameVirtualCamera.SetActive(false);
        cutsceneVirtualCamera.SetActive(true);
        dirtyImage.SetActive(true);
        for (int i = 0; i < obstaclesHolder.transform.childCount; i++)
        {
            obstaclesHolder.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        yield return new WaitForSeconds(5f);
        
        sparkles.SetActive(true);
        dirtyImage.SetActive(false);
        
        HapticFeedback.LightFeedback();

        yield return new WaitForSeconds(3f);

        callback?.Invoke(win);
    }
}
