using System.Collections;
using UnityEngine;

public class SpriteRendererEraser : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int traceSize = 30;
    [Header("Debug stuff")]
    [SerializeField] private Texture2D maskTexture;
    [SerializeField] private Material mat;
    
    private readonly int MAIN_TX_NAME = Shader.PropertyToID("_MainTex");
    private readonly int ERASE_TX_NAME = Shader.PropertyToID("_EraseTex");

    private void Start()
    {
        mat = new Material(Shader.Find("Shader Graphs/ErasableSprite"));
        
        var scale = spriteRenderer.transform.localScale;
        maskTexture = new Texture2D((int)scale.x, (int)scale.y, TextureFormat.RG16, false);

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

    public void EraseAt(float relX, float relY)
    {
        int xStart = Mathf.Clamp((int)(relX * maskTexture.width) - traceSize / 2, 0, maskTexture.width - traceSize);
        int yStart = Mathf.Clamp((int)(relY * maskTexture.height) - traceSize / 2, 0, maskTexture.height - traceSize);

        for (int y = 0; y < traceSize; y++) {
            for (int x = 0; x < traceSize; x++) {
                int px = xStart + x;
                int py = yStart + y;
                maskTexture.SetPixel(px, py, Color.red);
            }
        }

        maskTexture.Apply();
        mat.SetTexture(ERASE_TX_NAME, maskTexture);
    }
}
