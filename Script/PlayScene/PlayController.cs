using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayController : MonoBehaviour
{

    public Image Cover;
     
    /// <summary>
    /// 判定器,计划加入旧版判定
    /// </summary>
    public class Judges
    {
        public enum NoteType { Tap,Hold,Touch,Slide,Break};
        public enum JudgeType {CRITICAL,PERFECT,GREAT,GOOD,MISS};
        public Dictionary<Judges, int> JudgeCount;  //判定数量
        public Dictionary<Judges, int> JudgeWeight; //判定权重数
        public int MaXCombo = 0;
        public int DxScore = 0;
        public int TotalCount = 0;
    }

    void Awake()
    {
        if (!FileManager.AllLoaded) SceneManager.LoadScene(0);
    }
    
    void Start()
    {
        Cover.sprite = FileManager.ReadOutPNG(FileManager.ChartPath + FileManager.CurLoadChart.Info_.InfoPath + "/cover.png");
    }


    void Update()
    { 
        
    }
}
