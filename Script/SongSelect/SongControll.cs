using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongControl : MonoBehaviour
{
    public Image Cover;
    public TextMeshProUGUI FormalName;
    public TextMeshProUGUI Designer;
    public TextMeshProUGUI Composer;
    public TextMeshProUGUI BPM;
    public TextMeshProUGUI Best;
    public TextMeshProUGUI Level;

    public PartialChart Target;

    /// <summary>
    /// °ó¶¨
    /// </summary>
    /// <param name="target"></param>
    public  void Bind(PartialChart target)
    {
        Target = target;

        Composer.text = target.SongWriter;
        FormalName.text = target.FormalName;
        Designer.text = target.ChartWriter;
        BPM.text = "BPM " + target.BPM;
        Level.text = "" + (int)target.Difficulty;
        if (target.Histroy == null) Best.text = "";
        
        //Debug.Log(FileManager.ChartPath + target.InfoPath + "/cover.png");
        Cover.sprite = Resources.Load<Sprite>(FileManager.ChartPath + target.InfoPath + "/cover");
    }
    
}
