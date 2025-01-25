using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private SpriteEraser spriteEraser;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    
    private Rigidbody2D rb;
    
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        InputSystem.InputSystem.Instance.OnMove += OnMove;
        InputSystem.InputSystem.Instance.OnRotate += OnRotate;
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
}
