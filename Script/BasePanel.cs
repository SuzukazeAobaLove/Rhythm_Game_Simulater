using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePanel : MonoBehaviour,IDisposable
{
    protected Panel Type;

    protected virtual void Start()
    {
        GameSystem.RegisterPanel(Type, this);
        if(!GameSystem.IsCurPanel(Type)) gameObject.SetActive(false);
    }

    /// <summary>
    /// ÇÐ»»³¡¾°´¥·¢º¯Êý
    /// </summary>
    public virtual void OnSceneOpened() { }
    public void Dispose()=>GameSystem.UnRegisterPanel(Type);

}
