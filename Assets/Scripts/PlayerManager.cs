using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float stopThreshold = 20f;
    [SerializeField] private SpriteEraser spriteEraser;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private byte health;
    [SerializeField] private SpriteRendererEraser eraser;
    [SerializeField] private GameObject spray;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthIcon;
    [SerializeField] private Sprite[] healthSprites; // ordered from the least filled

    private Vector3 startingPos;
    private Vector3 lastFramePos;
    private Rigidbody2D rb;
    
    private int erasedPixels;
    
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
        if (rb.velocity.magnitude > stopThreshold)
            return;
        
        rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }
    
    private void OnRotate(sbyte dir)
    {
        transform.Rotate((Vector3.forward * (dir * rotationSpeed * Time.deltaTime)));
        rb.velocity = transform.up * rb.velocity.magnitude;
        virtualCamera.transform.eulerAngles = transform.eulerAngles;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (health == 0)
            {
                Destroy(gameObject);
                return;
            }
            
            health--;
            healthText.SetText(health.ToString());
            healthIcon.sprite = healthSprites[Mathf.Min(health, healthSprites.Length - 1)];
            spray.GetComponent<Animator>().SetTrigger("spawn");
            GetComponent<Animator>().SetTrigger("spawn");
            transform.position = startingPos;
            transform.rotation = Quaternion.identity;
            virtualCamera.transform.rotation = Quaternion.identity;
            rb.velocity = Vector2.zero;
            lastFramePos = transform.position;
        }
    }

    private Vector3 newPos, newDir;
    
    private void Update()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            newPos = transform.position;
            newDir = newPos - lastFramePos;
            int ticks = Mathf.CeilToInt(0.005f * rb.velocity.magnitude);
            
            for (int i = 0; i < ticks; i++)
            {
                var pos = lastFramePos + newDir * i / ticks;
                erasedPixels += eraser.EraseAt(pos);
            }
            
            float size = (float)eraser.MaskTexture.width * eraser.MaskTexture.height;
            float percentage = erasedPixels / size;
            Debug.Log($"Percentage: {percentage}");
            if (percentage > 0.75f)
            {
                Debug.LogError("You win!");
            }
            
            eraser.UpdateTexture();
            lastFramePos = transform.position;
        }
    }
}
