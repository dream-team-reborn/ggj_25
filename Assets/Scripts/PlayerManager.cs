using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private SpriteEraser spriteEraser;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private byte health;

    private Vector3 startingPos;
    private Rigidbody2D rb;
    
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        InputSystem.InputSystem.Instance.OnMove += OnMove;
        InputSystem.InputSystem.Instance.OnRotate += OnRotate;
        
        startingPos = transform.position;
    }

    private void OnDestroy()
    {
        InputSystem.InputSystem.Instance.OnMove -= OnMove;
        InputSystem.InputSystem.Instance.OnRotate -= OnRotate;
    }

    private void OnMove(float obj)
    {
        rb.AddForce(transform.up * speed, ForceMode2D.Force);

        return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(spriteEraser.RectTransform, screenPos, Camera.main, out var localPos);
        Vector2 normalizedPos = Rect.PointToNormalized(spriteEraser.RectTransform.rect, localPos);
        
        spriteEraser.EraseAt(normalizedPos.x, normalizedPos.y);
    }
    
    private void OnRotate(sbyte dir)
    {
        transform.Rotate((Vector3.forward * (dir * 100 * Time.deltaTime)));
        virtualCamera.transform.Rotate((Vector3.forward * (dir * 100 * Time.deltaTime)));
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
        }
    }
}
