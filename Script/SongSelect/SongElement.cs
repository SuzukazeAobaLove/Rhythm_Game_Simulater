
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongElement : MonoBehaviour
{
    public Image Cover;
    public bool Initialed = false;
    public Text FormalName;
    public Text Designer;
    public Text Composer;
    public TextMeshProUGUI BPM;
    public TextMeshProUGUI Best;
    public TextMeshProUGUI Level;

    public PartialChart Target;

    /// <summary>
    /// 绑定
    /// </summary>
    /// <param name="target"></param>
    public void Bind(PartialChart target) => Target = target;
    

    /// <summary>
    /// 在激活的时候才会更新值
    /// </summary>
    public void OnEnable()
    {
        if (Initialed == false) Initialed = true;
        else UpdateData();
    }
    
    /// <summary>
    /// 更新显示数据
    /// </summary>
    public void UpdateData()
    {
        Composer.text = Target.SongWriter;
        FormalName.text = Target.FormalName;
        Designer.text = Target.ChartWriter;
        BPM.text = "BPM " + Target.BPM;
        Level.text = "" + (int)Target.Difficulty;
        if (Target.Histroy == null) Best.text = "";
        Cover.sprite = FileManager.ReadOutPNG(FileManager.ChartPath + Target.InfoPath + "/cover.png");
    }
    
}
