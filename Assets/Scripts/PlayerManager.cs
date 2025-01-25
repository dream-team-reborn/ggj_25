using Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private SpriteEraser spriteEraser;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private byte health;
    [SerializeField] private SpriteRendererEraser eraser;
    [SerializeField] private GameObject spray;

    private Vector3 startingPos;
    private Vector3 lastFramePos;
    private Rigidbody2D rb;
    
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        InputSystem.InputSystem.Instance.OnMove += OnMove;
        InputSystem.InputSystem.Instance.OnRotate += OnRotate;
        
        startingPos = transform.position;
        lastFramePos = transform.position;

        spray.GetComponent<Animator>().SetTrigger("spawn");
    }

    private void OnDestroy()
    {
        InputSystem.InputSystem.Instance.OnMove -= OnMove;
        InputSystem.InputSystem.Instance.OnRotate -= OnRotate;

        spray.GetComponent<Animator>().SetTrigger("spawn");
    }

    private void OnMove(float obj)
    {
        rb.AddForce(transform.up * speed, ForceMode2D.Impulse);

        return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(spriteEraser.RectTransform, screenPos, Camera.main, out var localPos);
        Vector2 normalizedPos = Rect.PointToNormalized(spriteEraser.RectTransform.rect, localPos);
        
        spriteEraser.EraseAt(normalizedPos.x, normalizedPos.y);
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
            transform.position = startingPos;
            transform.rotation = Quaternion.identity;
            virtualCamera.transform.rotation = Quaternion.identity;
            rb.velocity = Vector2.zero;
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
                eraser.EraseAt(pos);
            }
            
            eraser.UpdateTexture();
            lastFramePos = transform.position;
        }
    }
}
