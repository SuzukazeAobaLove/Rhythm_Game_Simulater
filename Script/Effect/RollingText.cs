using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RollingText : MonoBehaviour
{
    private ScrollRect Rect;
    private TextMeshProUGUI Text;
    private bool IfRolling;
    
    void Start()
    {
        Rect = GetComponent<ScrollRect>();
        Text = GetComponentInChildren<TextMeshProUGUI>();
        Text.rectTransform.sizeDelta = new Vector2(Text.text.Length * 30f,Text.rectTransform.rect.height);
        if(Text.text.Length > 20) IfRolling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(IfRolling)
        {
            Rect.horizontalNormalizedPosition += 0.08f * Time.deltaTime;
            if(Rect.horizontalNormalizedPosition >= 1f) Rect.horizontalNormalizedPosition = 0f; 
        }
        
    }


}
