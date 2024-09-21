using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class Chart
{
    public PartialChart Info_;
    public Judges Judge_;
    public List<Note> Notes_;
    public AudioClip Music_;
    //public Sprite Cover_;
    
    public void AddNote(Note note)
    {
        Judge_.StaticAdd(note);
        Notes_.Add(note); 
    }
}

[Serializable]
/// <summary>
/// ����Note
/// </summary>
public class Note
{
    /// <summary>
    /// Note��������ö��
    /// </summary>
    public enum NoteType
    {
        Tap, Hold, Slide_Tap, Slide_Track, Wifi_Slide, Touch, TouchHold
    };

    /// <summary>
    /// �������
    /// </summary>
    public enum Block
    {
        A1, A2, A3, A4, A5, A6, A7, A8,
        B1, B2, B3, B4, B5, B6, B7, B8,
        C,
        D1 = 24, D2, D3, D4, D5, D6, D7, D8,
        E1, E2, E3, E4, E5, E6, E7, E8
    };
    [JsonConverter(typeof(StringEnumConverter))]
    [SerializeField] public Block Rail_;          //����
    [JsonConverter(typeof(StringEnumConverter))]
    [SerializeField] public NoteType Type_;       //����
    [SerializeField] public bool Ex_ = false;     //�Ƿ��������
    [SerializeField] public bool Break_ = false;  //�Ƿ�Ϊ����
    [SerializeField] public double ExactTime_;    //����ʱ��

    /// <summary>
    /// ��������note
    /// </summary>
    /// <param name="Token"></param>
    /// <returns></returns>
    public static void ParseNote(Chart target,string token, double exTime)
    {
        //�����һλ����ĸ����ΪTouch����TouchHold
        if (char.IsLetter(token[0]))
        {
            //�������h��ΪTouchHold
            if (token.Contains("h"))
            {
                Note note = new Note();
                note.Type_ = NoteType.TouchHold;
                note.ExactTime_ = exTime;
                if (Enum.TryParse(token.Substring(0, token.IndexOf("C") == -1 ? 2 : 1), false, out note.Rail_))
                {
                    //NOTE: ����ʱֵ���ʽ
                    target.AddNote(note);
                }
            }

            //������Touch
            else
            {
                Note note = new Note();
                note.Type_ = NoteType.Touch;
                note.ExactTime_ = exTime;
                if (Enum.TryParse(token.Substring(0, token.IndexOf("C") == -1 ? 2 : 1), false, out note.Rail_))
                {
                    //NOTE: �̻�Ч��
                    target.AddNote(note);
                }


            }
        }
        else
        {
            //�������h����ôһ����Hold
            if (token.Contains("h"))
            {
                Note note = new Note();
                note.Type_ = NoteType.Hold;
                note.ExactTime_ = exTime;
                if (token.Contains("b")) note.Break_ = true;
                if (token.Contains("x")) note.Ex_ = true;

                if (token[0] >= '1' && token[0] <= '8')
                {
                    note.Rail_ = (Block)(token[0] - '1');
                    target.AddNote(note);
                }
            }
            //�������ʱֵ���ʽ����ôһ����Slide
            else if (token.Contains("["))
            {
                Note note = new Note();
                note.Type_ = NoteType.Slide_Tap;
                note.ExactTime_ = exTime;
                if (token.Substring(0, 3).Contains("b")) note.Break_ = true;
                if (token.Substring(0, 3).Contains("x")) note.Ex_ = true;
                if (token[0] >= '1' && token[0] <= '8')
                {
                    note.Rail_ = (Block)(token[0] - '1');
                    target.AddNote(note);
                }
                else return;

                //����ͬ�������
                string[] each = token.Split('*');
                foreach(var slide in each)
                {
                    Note snote = new Note();
                    snote.Type_ = NoteType.Slide_Track;
                    snote.ExactTime_ = exTime;
                    if(slide.Substring(3).Contains("b")) snote.Break_ = true;
                    snote.Rail_ = (Block)(token[0] - '1');
                    target.AddNote(snote);
                }
            }
            //ʣ�µ�ֻ������Tap
            else
            {
                Note note = new Note();
                note.Type_ = NoteType.Tap;
                note.ExactTime_ = exTime;
                if(token.Contains("b")) note.Break_ = true;
                if(token.Contains("x")) note.Ex_ = true;

                if (token[0] >= '1' && token[0] <= '8')
                {
                    note.Rail_ = (Block)(token[0] - '1');
                    target.AddNote(note);
                }
            }
        }
        
    }

}