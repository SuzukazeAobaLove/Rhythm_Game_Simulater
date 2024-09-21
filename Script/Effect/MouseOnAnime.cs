using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class LoopAnime : MonoBehaviour
{
    public float Fps = 20f;
    public Sprite[] Frames;
    private int CurFrame;
    private float Timer;
    private Image Image;

    // Start is called before the first frame update
    void Start()
    {
        Image = GetComponent<Image>();
        Image.sprite = Frames[0];
        CurFrame = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        Timer += Time.deltaTime;

        // 检查是否需要更新帧
        if (Timer >= 1f / Fps)
        {
            // 更新当前帧
            CurFrame = (CurFrame + 1) % Frames.Length;

            // 更新材质的纹理
            Image.sprite = Frames[CurFrame];

            // 重置计时器
            Timer = 0f;
        }
        
    }

}
