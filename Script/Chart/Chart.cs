using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HaseMikan;

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
    
    [JsonConverter(typeof(StringEnumConverter))]
    [SerializeField] public Block Rail_;          //����
    [JsonConverter(typeof(StringEnumConverter))]
    [SerializeField] public DetailNoteType Type_;       //����
    [SerializeField] public bool Ex_ = false;     //�Ƿ��������
    [SerializeField] public bool Break_ = false;  //�Ƿ�Ϊ����
    [SerializeField] public bool Each_ = false;     //�Ƿ�Ϊ��Ѻ
    [SerializeField] public double ExactTime_;    //����ʱ��

    /// <summary>
    /// ��������note
    /// </summary>
    /// <param name="Token"></param>
    /// <returns></returns>
    public static void ParseNote(Chart target,string token, double exTime,bool ifEach)
    {
        //�����һλ����ĸ����ΪTouch����TouchHold
        if (char.IsLetter(token[0]))
        {
            //�������h��ΪTouchHold
            if (token.Contains("h"))
            {
                Note note = new Note();
                note.Type_ = DetailNoteType.TouchHold;
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
                note.Type_ = DetailNoteType.Touch;
                note.ExactTime_ = exTime;
                note.Each_ = ifEach;
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
                note.Type_ = DetailNoteType.Hold;
                note.ExactTime_ = exTime;
                note.Each_ = ifEach;
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
                note.Type_ = DetailNoteType.Slide_Tap;
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
                    snote.Type_ = DetailNoteType.Slide_Track;
                    snote.ExactTime_ = exTime;
                    snote.Each_ = ifEach;
                    if(slide.Substring(3).Contains("b")) snote.Break_ = true;
                    snote.Rail_ = (Block)(token[0] - '1');
                    target.AddNote(snote);
                }
            }
            //ʣ�µ�ֻ������Tap
            else
            {
                Note note = new Note();
                note.Type_ = DetailNoteType.Tap;
                note.ExactTime_ = exTime;
                note.Each_ = ifEach;
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