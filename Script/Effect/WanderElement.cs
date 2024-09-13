using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WanderElement : MonoBehaviour
{
    private RectTransform UIElement; // Ҫ�ƶ��� UI Ԫ��


    private Vector2 CurDirection = new Vector2(101, -62);
    //private float Speed = 20f;

    void Start()
    {
        UIElement = GetComponent<RectTransform>();

        // ��ʼ�ƶ�
        MoveElement();
    }


    void MoveElement()
    {
        //CurDirection = -CurDirection;

        UIElement.DOAnchorPos(CurDirection, 1f).SetLoops(-1, LoopType.Yoyo);
    }

}
