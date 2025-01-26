using System;
using System.Collections;
using UnityEngine;

public class SpriteRendererEraser : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int traceSize = 30;
    [SerializeField] private int brushRadius;
    
    [Header("Debug stuff")]
    [SerializeField] private Texture2D maskTexture;
    [SerializeField] private Material mat;
    
    private readonly int MAIN_TX_NAME = Shader.PropertyToID("_MainTex");
    private readonly int ERASE_TX_NAME = Shader.PropertyToID("_EraseTex");
    
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public Texture2D MaskTexture => maskTexture;
    public Action OnMaskTextureReady;

    private MaterialPropertyBlock propBlock;
    
    [SerializeField] private Texture2D cleanTex;

    private void Start()
    {
        mat = new Material(Shader.Find("Shader Graphs/ErasableSprite"));
        propBlock = new MaterialPropertyBlock();
        
        maskTexture = new Texture2D(spriteRenderer.sprite.texture.width * 2, spriteRenderer.sprite.texture.height * 2, TextureFormat.RG16, false);
        
        for (int y = 0; y < maskTexture.height; y++) {
            for (int x = 0; x < maskTexture.width; x++) {
                maskTexture.SetPixel(x, y, Color.clear);
            }
        }
        maskTexture.Apply();
        cleanTex.Apply();
        
        spriteRenderer.GetPropertyBlock(propBlock);
        propBlock.SetTexture(MAIN_TX_NAME, spriteRenderer.sprite.texture);
        propBlock.SetTexture(ERASE_TX_NAME, maskTexture);
        spriteRenderer.SetPropertyBlock(propBlock);
        
        spriteRenderer.material = mat;
        
        OnMaskTextureReady?.Invoke();

        // StartCoroutine(MoveTrace());
    }

    private IEnumerator MoveTrace()
    {
        var t = 0f;
        while (t < 1) {
            yield return new WaitForSeconds(0.1f);
            t += 0.01f;

            EraseAt(0.5f, t);
        }
    }

    public int EraseAt(Vector2 pos)
    {
        var normalizedPos = new Vector2(Mathf.InverseLerp(spriteRenderer.bounds.min.x, spriteRenderer.bounds.max.x, pos.x),
            Mathf.InverseLerp(spriteRenderer.bounds.min.y, spriteRenderer.bounds.max.y, pos.y));
        return EraseAt(normalizedPos.x, normalizedPos.y);
    }

    public int EraseAt(float relX, float relY)
    {
        int erased = 0;
        int xStart = (int)(relX * maskTexture.width);
        int yStart = (int)(relY * maskTexture.height);

        for (int x = -Mathf.FloorToInt(brushRadius); x < Mathf.FloorToInt(brushRadius); x++)
        {
            for (int y = -Mathf.FloorToInt(brushRadius); y < Mathf.FloorToInt(brushRadius); y++)
            {
                if (x * x + y * y <= brushRadius * brushRadius)
                {
                    int px = xStart + x;
                    int py = yStart + y;
                    
                    if (maskTexture.GetPixel(px, py).r > 0f)
                        continue;
                    
                    erased++;
                    maskTexture.SetPixel(px, py, Color.red);
                }
            }
        }

        return erased;
    }
    
    public void UpdateTexture()
    {
        maskTexture.Apply();
        spriteRenderer.GetPropertyBlock(propBlock);
        propBlock.SetTexture(ERASE_TX_NAME, maskTexture);
        spriteRenderer.SetPropertyBlock(propBlock);
    }

    public void ResetTexture()
    {
        spriteRenderer.GetPropertyBlock(propBlock);
        propBlock.SetTexture(ERASE_TX_NAME, cleanTex);
        spriteRenderer.SetPropertyBlock(propBlock);
    }

    public void ApplyMask()
    {
        spriteRenderer.GetPropertyBlock(propBlock);
        propBlock.SetTexture(ERASE_TX_NAME, maskTexture);
        spriteRenderer.SetPropertyBlock(propBlock);
    }
}
