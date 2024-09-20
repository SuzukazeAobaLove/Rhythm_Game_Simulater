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
    /// �ж���,�ƻ�����ɰ��ж�
    /// </summary>
    public class Judges
    {
        public enum NoteType { Tap,Hold,Touch,Slide,Break};
        public enum JudgeType {CRITICAL,PERFECT,GREAT,GOOD,MISS};
        public Dictionary<Judges, int> JudgeCount;  //�ж�����
        public Dictionary<Judges, int> JudgeWeight; //�ж�Ȩ����
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
