using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;
    
    [SerializeField] private GameObject gameVirtualCamera;
    [SerializeField] private GameObject cutsceneVirtualCamera;
    [SerializeField] private GameObject sparkles;
    [SerializeField] private SpriteRendererEraser eraser;
    
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
        eraser.ResetTexture();
        
        yield return new WaitForSeconds(5f);
        
        sparkles.SetActive(false);
        eraser.ApplyMask();

        yield return new WaitForSeconds(3f);

        callback?.Invoke(win);
    }
}
