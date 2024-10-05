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
/// 单个Note
/// </summary>
public class Note
{
    
    [JsonConverter(typeof(StringEnumConverter))]
    [SerializeField] public Block Rail_;          //分区
    [JsonConverter(typeof(StringEnumConverter))]
    [SerializeField] public DetailNoteType Type_;       //类型
    [SerializeField] public bool Ex_ = false;     //是否带保护套
    [SerializeField] public bool Break_ = false;  //是否为绝赞
    [SerializeField] public bool Each_ = false;     //是否为多押
    [SerializeField] public double ExactTime_;    //正解时间

    /// <summary>
    /// 解析单个note
    /// </summary>
    /// <param name="Token"></param>
    /// <returns></returns>
    public static void ParseNote(Chart target,string token, double exTime,bool ifEach)
    {
        //如果第一位是字母，则为Touch或者TouchHold
        if (char.IsLetter(token[0]))
        {
            //如果存在h则为TouchHold
            if (token.Contains("h"))
            {
                Note note = new Note();
                note.Type_ = DetailNoteType.TouchHold;
                note.ExactTime_ = exTime;
                
                if (Enum.TryParse(token.Substring(0, token.IndexOf("C") == -1 ? 2 : 1), false, out note.Rail_))
                {
                    //NOTE: 解析时值表达式
                    target.AddNote(note);
                }
            }

            //否则是Touch
            else
            {
                Note note = new Note();
                note.Type_ = DetailNoteType.Touch;
                note.ExactTime_ = exTime;
                note.Each_ = ifEach;
                if (Enum.TryParse(token.Substring(0, token.IndexOf("C") == -1 ? 2 : 1), false, out note.Rail_))
                {
                    //NOTE: 烟花效果
                    target.AddNote(note);
                }


            }
        }
        else
        {
            //如过包含h，那么一定是Hold
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
            //如果包含时值表达式，那么一定是Slide
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

                //处理同起点星星
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
            //剩下的只可能是Tap
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