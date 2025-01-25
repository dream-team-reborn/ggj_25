using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Image player;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private SpriteEraser spriteEraser;

    private Transform playerTrns;
    
    public void Start()
    {
        InputSystem.InputSystem.Instance.OnMove += OnMove;
        InputSystem.InputSystem.Instance.OnRotate += OnRotate;
        playerTrns = player.transform;
    }

    private void OnDestroy()
    {
        InputSystem.InputSystem.Instance.OnMove -= OnMove;
        InputSystem.InputSystem.Instance.OnRotate -= OnRotate;
    }

    private void OnMove(float obj)
    {
        Debug.Log("MOVING");
        playerTrns.Translate(playerTrns.up * (speed * Time.deltaTime));

        Vector3 screenPos = Camera.main.WorldToScreenPoint(playerTrns.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(spriteEraser.RectTransform, screenPos, Camera.main, out var localPos);
        Vector2 normalizedPos = Rect.PointToNormalized(spriteEraser.RectTransform.rect, localPos);
        
        spriteEraser.EraseAt(normalizedPos.x, normalizedPos.y);
    }
    
    private void OnRotate(sbyte dir)
    {
        playerTrns.Rotate((Vector3.forward * (dir * 100 * Time.deltaTime)));
    }
}
