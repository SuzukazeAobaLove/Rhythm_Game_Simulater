
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongElement : MonoBehaviour
{
    public Image Cover;
    public Image Panel;
    public bool Initialed = false;
    public Text FormalName;
    public Text Designer;
    public Text Composer;
    public TextMeshProUGUI BPM;
    public TextMeshProUGUI Best;
    public TextMeshProUGUI Level;

    public PartialChart Target;
    public int BingLevel;

    /// <summary>
    /// ��
    /// </summary>
    /// <param name="target"></param>
    public void Bind(PartialChart target,int Level)
    {
        Target = target;
        BingLevel = Level;
    }
    
    

    /// <summary>
    /// �ڼ����ʱ��Ż����ֵ
    /// </summary>
    public void OnEnable()
    {
        if (Initialed == false) Initialed = true;
        else UpdateData();
    }
    
    public void RewriteImage(Sprite sprite)=>Panel.sprite = sprite;
    
    /// <summary>
    /// ������ʾ����
    /// </summary>
    public void UpdateData()
    {
        Composer.text = Target.SongWriter;
        FormalName.text = Target.FormalName;
        Designer.text = Target.ChartWriter;
        BPM.text = "BPM " + Target.BPM;
        Level.text = "" + (int)Target.Difficulty[4];
        if (Target.Difficulty[4] - (int)Target.Difficulty[4] > 0.55) Level.text += "+";
        if (Target.Histroy == null) Best.text = "";
        Cover.sprite = FileManager.ReadOutPNG(FileManager.ChartPath + Target.InfoPath + "/bg.png");
    }
    
}
