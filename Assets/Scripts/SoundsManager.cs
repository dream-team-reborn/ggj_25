using System;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    [Serializable]
    public class Audio
    {
        public AudioClip Clip;
        public float Volume = 1f;
    }
    
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private float movingThreshold = 0.5f;
    [Header("Audio Clips")]
    [SerializeField] private Audio playerCollidedClip;
    [SerializeField] private Audio playerMovingClip;
    [SerializeField] private Audio playerSpawnedClip;
    [SerializeField] private Audio playerEnemyHitClip;
    [SerializeField] private Audio sprayClip;
    
    private AudioSource audioSource;

    private void Awake()
    {
        playerManager.OnPlayerCollided += OnPlayerCollided;
        playerManager.OnPlayerMoving += OnPlayerMoving;
        playerManager.OnPlayerSpawned += OnPlayerSpawned;
        playerManager.OnSprayed += OnSprayed;
        audioSource = playerManager.AudioSource;
    }

    private void PlayAudio(Audio a)
    {
        if (a == null || a.Clip == null)
            return;
        
        audioSource.clip = a.Clip;
        audioSource.volume = a.Volume;
        
        audioSource.Play();
    }

    private void OnPlayerMoving(float vel)
    {
        if (vel > movingThreshold && !audioSource.isPlaying)
            PlayAudio(playerMovingClip);
    }

    private void OnPlayerSpawned()
    {
        PlayAudio(playerSpawnedClip);
    }
    
    private void OnSprayed()
    {
        PlayAudio(sprayClip);
    }

    private void OnPlayerCollided(CollisionType obj)
    {
        switch (obj) {
            case CollisionType.Enemy:
                PlayAudio(playerEnemyHitClip);
                break;
            case CollisionType.Generic:
            case CollisionType.BouncingWall:
                PlayAudio(playerCollidedClip);
                break;
            default:
                PlayAudio(playerCollidedClip);
                break;
        } 
    }
}