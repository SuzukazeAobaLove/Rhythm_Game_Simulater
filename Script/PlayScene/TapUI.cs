using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapUI : MonoBehaviour
{
    private int Rail;
    private Vector2 Angle;
    private RectTransform RectT;
    public Image Image;
    private Note BindNote;
    public float DisTime = 0;
    private float Speed;

    // Start is called before the first frame update
    void Start()
    {
        Speed = PlayController.Instance.Speed;
        
        RectT = GetComponent<RectTransform>();   
        //Image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        DisTime += Time.deltaTime;
        RectT.localPosition = Angle * DisTime / Speed * 18 * 650f;
        if(DisTime / Speed * 18 > 1) Destroy(gameObject);
    }

    /// <summary>
    /// 重置Tap
    /// </summary>
    public void Restart(Note note,float alignTime)
    {
        
        //计算移动方向
        Rail = (int) note.Rail_;
        Angle = new Vector2(Mathf.Cos((157.5f - Rail * 45) * Mathf.Deg2Rad), Mathf.Sin((157.5f - Rail * 45) * Mathf.Deg2Rad));

        if (note.Break_) Image.color = Color.red;
        else if (note.Each_) Image.color = Color.yellow;
        else Image.color = new Color(1, 0.5808624f, 0.9751914f, 0.572549f);

        //重置状态
        DisTime = (float)(PlayController.Instance.Speed / 18f - note.ExactTime_ + alignTime);
        //RectT.localPosition = Vector3.zero;
        
        //激活物体
        gameObject.SetActive(true);
    }
}
