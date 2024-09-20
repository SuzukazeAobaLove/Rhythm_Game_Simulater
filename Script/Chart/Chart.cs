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
    /// ����Note
    /// </summary>
    public struct Note
    {
        /// <summary>
        /// Note����ö��
        /// </summary>
        public enum NoteType {Tap,Ex_Tap,Long,Break};

        public NoteType Type;       //����
        public double ExactTime;    //����ʱ��

        public Note(NoteType type,double exactTime)
        {
            Type = type;
            ExactTime = exactTime;
        }
    }

    
}
