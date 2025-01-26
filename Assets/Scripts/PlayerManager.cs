using System;
using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float stopThreshold = 20f;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private byte health;
    [SerializeField] private SpriteRendererEraser eraser;
    [SerializeField] private GameObject spray;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthIcon;
    [SerializeField] private Sprite[] healthSprites; // ordered from the least filled
    [SerializeField] private float winThreshold = 0.75f;
    
    public float maskTextureSize => (float)eraser.MaskTexture.width * eraser.MaskTexture.height;
    public Action<int> OnErasedPixels;
    public Action OnPlayerInitialized;
    public Action OnPlayerDied;

    private Vector3 startingPos;
    private Vector3 lastFramePos;
    private Rigidbody2D rb;

    private bool isFreezed;

    public void Awake()
    {
        eraser.OnMaskTextureReady += () => OnPlayerInitialized?.Invoke();
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        InputSystem.InputSystem.Instance.OnMove += OnMove;
        InputSystem.InputSystem.Instance.OnRotate += OnRotate;
        
        startingPos = transform.position;
        lastFramePos = transform.position;

        spray.GetComponent<Animator>().SetTrigger("spawn");
        healthText.SetText(health.ToString());
    }

    private void OnDestroy()
    {
        InputSystem.InputSystem.Instance.OnMove -= OnMove;
        InputSystem.InputSystem.Instance.OnRotate -= OnRotate;
    }

    private void OnMove(float obj)
    {
        if (isFreezed)
            return;
        
        if (rb.velocity.magnitude > stopThreshold)
            return;
        
        rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }
    
    private void OnRotate(sbyte dir)
    {
        if (isFreezed)
            return;
        
        transform.Rotate((Vector3.forward * (dir * rotationSpeed * Time.deltaTime)));
        rb.velocity = transform.up * rb.velocity.magnitude;
        virtualCamera.transform.eulerAngles = transform.eulerAngles;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GetComponent<Animator>().SetTrigger("explode");
            
            if (health == 0)
            {
                OnPlayerDied?.Invoke();
                return;
            }
            
            isFreezed = true;
            rb.velocity = Vector2.zero;
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        
        health--;
        healthText.SetText(health.ToString());
        healthIcon.sprite = healthSprites[Mathf.Min(health, healthSprites.Length - 1)];
        
        transform.position = startingPos;
        transform.rotation = Quaternion.identity;
        virtualCamera.transform.rotation = Quaternion.identity;
        lastFramePos = transform.position;

        yield return new WaitForSeconds(0.5f);
        
        spray.GetComponent<Animator>().SetTrigger("spawn");
        
        yield return new WaitForSeconds(0.2f);
        
        GetComponent<Animator>().SetTrigger("spawn");

        yield return new WaitForSeconds(1f);
        isFreezed = false;
    }

    private Vector3 newPos, newDir;
    
    private void Update()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            newPos = transform.position;
            newDir = newPos - lastFramePos;
            int ticks = Mathf.CeilToInt(0.005f * rb.velocity.magnitude);
            var erasedPixels = 0;
            
            for (int i = 0; i < ticks; i++)
            {
                var pos = lastFramePos + newDir * i / ticks;
                erasedPixels += eraser.EraseAt(pos);
            }
            
            OnErasedPixels?.Invoke(erasedPixels);
            
            eraser.UpdateTexture();
            lastFramePos = transform.position;
        }
    }

    public void EndGame()
    {
        rb.velocity = Vector2.zero;
        isFreezed = true;
    }
}
