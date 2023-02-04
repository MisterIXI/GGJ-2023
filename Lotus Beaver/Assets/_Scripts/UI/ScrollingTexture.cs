using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScrollingTexture : MonoBehaviour
{
    [SerializeField]private float speedX = 0.1f;
    [SerializeField]private float speedY  = 0.1f;

    private Vector2 curXY;

    // Start is called before the first frame update
    void Start()
    {
        curXY = GetComponent<RawImage>().materialForRendering.GetTextureOffset("_MainTex");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //SetTextureOffset("_MainTex", );
        curXY.x += Time.deltaTime * speedX;
        curXY.y += Time.deltaTime * speedY;
        GetComponent<RawImage>().materialForRendering.SetTextureOffset("_MainTex", curXY);
    }
}
