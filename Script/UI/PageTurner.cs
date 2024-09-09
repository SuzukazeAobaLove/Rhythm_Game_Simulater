using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class PageTurner : MonoBehaviour
{
    private ScrollSnap ScrollSnap;
    // Start is called before the first frame update
    void Start()
    {
        ScrollSnap = GetComponent<ScrollSnap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextPage()
    {
        ScrollSnap.NextScreen();
    }

    // ∑≠µΩ…œ“ª“≥
    public void PreviousPage()
    {
        ScrollSnap.PreviousScreen();
    }
}
