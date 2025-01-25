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

    private void Start()
    {
        mat = new Material(Shader.Find("Shader Graphs/ErasableSprite"));
        
        var scale = spriteRenderer.transform.localScale;
        maskTexture = new Texture2D(spriteRenderer.sprite.texture.width * 2, spriteRenderer.sprite.texture.height * 2, TextureFormat.RG16, false);

        for (int y = 0; y < maskTexture.height; y++) {
            for (int x = 0; x < maskTexture.width; x++) {
                maskTexture.SetPixel(x, y, Color.clear);
            }
        }
        maskTexture.Apply();
        
        mat.SetTexture(MAIN_TX_NAME, spriteRenderer.sprite.texture);
        mat.SetTexture(ERASE_TX_NAME, maskTexture);
        
        spriteRenderer.material = mat;

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

    public void EraseAt(Vector2 pos)
    {
        var normalizedPos = new Vector2(Mathf.InverseLerp(spriteRenderer.bounds.min.x, spriteRenderer.bounds.max.x, pos.x),
            Mathf.InverseLerp(spriteRenderer.bounds.min.y, spriteRenderer.bounds.max.y, pos.y));
        EraseAt(normalizedPos.x, normalizedPos.y);
    }

    public void EraseAt(float relX, float relY)
    {
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

                    maskTexture.SetPixel(px, py, Color.red);
                }
            }
        }
    }
    
    public void UpdateTexture()
    {
        maskTexture.Apply();
        mat.SetTexture(ERASE_TX_NAME, maskTexture);
    }
}
