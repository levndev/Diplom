using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilingTexture : MonoBehaviour
{
    enum Mode { 
        XY,
        ZY,
        ZX,
        XZ
    }

    [SerializeField] private Mode mode;

    [SerializeField] new private Renderer renderer;
    [SerializeField] private string texturePropertyName;

    private void Awake()
    {
        float x, y, z;
        x = transform.localScale.x;
        y = transform.localScale.y;
        z = transform.localScale.z;
        switch (mode)
        {
            case Mode.XY:
                renderer.material.SetTextureScale(texturePropertyName, new Vector2(x, y));
                break;
            case Mode.ZY:
                renderer.material.SetTextureScale(texturePropertyName, new Vector2(z, y));
                break;
            case Mode.ZX:
                renderer.material.SetTextureScale(texturePropertyName, new Vector2(z, x));
                break;
            case Mode.XZ:
                renderer.material.SetTextureScale(texturePropertyName, new Vector2(x, z));
                break;
            default:
                break;
        }

    }
}
