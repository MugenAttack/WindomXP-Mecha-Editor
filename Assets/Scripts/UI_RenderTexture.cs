using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RenderTexture : MonoBehaviour
{
    public RenderTexture m_Texture;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 size = rectTransform.sizeDelta;
        m_Texture.Release();
        m_Texture.width = (int)(size.x * 1920);
        m_Texture.height = (int)(size.y * 1080);
    }
}
