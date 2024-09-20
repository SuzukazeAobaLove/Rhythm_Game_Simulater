using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chart
{
    public PartialChart Info_;
    public List<Note> Notes_;
    public AudioClip Music_;
    public Sprite Cover_;

    /// <summary>
    /// 单个Note
    /// </summary>
    public struct Note
    {
        /// <summary>
        /// Note类型枚举
        /// </summary>
        public enum NoteType {Tap,Ex_Tap,Long,Break};

        public NoteType Type;       //类型
        public double ExactTime;    //正解时间

        public Note(NoteType type,double exactTime)
        {
            Type = type;
            ExactTime = exactTime;
        }
    }

    
}
