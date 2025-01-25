using UnityEngine;
using UnityEngine.UI;

public class SpriteEraser : MonoBehaviour
{
    public Texture2D eraseTexture;
    public float brushSize = 50f;
    public Texture2D nero;
    
    private Texture2D _eraseTexInstance;
    private RectTransform _rectTransform;
    private SpriteRenderer _image;
    private Vector2 _prevMousePos;

    public RectTransform RectTransform => _rectTransform;
    
    void Start()
    {
        _image = GetComponent<SpriteRenderer>();
        _rectTransform = GetComponent<RectTransform>();

        // Create an instance of erase texture
        // _eraseTexInstance = new Texture2D(eraseTexture.width, eraseTexture.height, TextureFormat.RGBA32, false);
        _eraseTexInstance = new Texture2D(nero.width, nero.height, nero.format, false);
        Graphics.CopyTexture(nero, _eraseTexInstance);
        _image.material.SetTexture("_EraseTex", _eraseTexInstance);
    }

    void Update()
    {
        // if (Input.GetMouseButton(0))
        // {
        //     Vector2 localMousePos;
        //     RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, Input.mousePosition, null, out localMousePos);
        //     Vector2 normalizedPos = Rect.PointToNormalized(_rectTransform.rect, localMousePos);
        //
        //     int x = (int)(normalizedPos.x * _eraseTexInstance.width);
        //     int y = (int)(normalizedPos.y * _eraseTexInstance.height);
        //
        //     EraseAt(x, y);
        // }
    }

    public void EraseAt(float xf, float yf)
    {
        int x = (int)(xf * _eraseTexInstance.width);
        int y = (int)(yf * _eraseTexInstance.height);
        
        int radius = (int)brushSize / 2;
        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                if (x + i < 0 || x + i >= _eraseTexInstance.width || y + j < 0 || y + j >= _eraseTexInstance.height)
                    continue;

                _eraseTexInstance.SetPixel(x + i, y + j, new Color(1, 0,0,0));
            }
        }
        _eraseTexInstance.Apply();
    }
}