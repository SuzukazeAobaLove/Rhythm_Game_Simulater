using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
/// <summary>
/// 判定器,计划加入旧版判定
/// </summary>
public class Judges
{
    /// <summary>
    /// Note种类
    /// </summary>
    public enum NoteType { Tap, Hold, Slide, Touch, Break };
    
    /// <summary>
    /// 具体种类
    /// </summary>
    public enum JudgeType 
    {
        CRITICAL_PERFECT, 
        F_PERFECT_1,L_PERECT_1,F_PERFECT_2,L_PERFECT_2,
        F_GREAT_1,L_GREAT_1,F_GREAT_2,L_GREAT_2,F_GREAT_3,L_GREAT_3,
        F_GOOD,L_GOOD,
        MISS
    };
    
    [Header("静态量")]
    public int TotalCount = 0;
    public int MaxScore = 0;
    public int ExtraScore = 0;
    public List<int> RespectiveCount = new List<int> { 0, 0, 0, 0, 0 };

    [Header("运行时量")]
    public int CurScore = 0;
    public int CurExtraScore = 0;
    public Dictionary<Tuple<NoteType, Judges>, int> JudgeCount;
    public int MaXCombo = 0;
    public int CurCombo = 0;
    public int DxScore = 0;
    public int FastCount = 0;
    public int LateCount = 0;

    /// <summary>
    /// 用这个进行一次判定
    /// </summary>
    /// <param name="Note"></param>
    /// <param name="DeltaTime"></param>
    public void AddJudge(Note Note,double DeltaTime)
    {

    }

    /// <summary>
    /// 计算理论值
    /// </summary>
    /// <param name="Note"></param>
    public void StaticAdd(Note Note)
    {
        TotalCount++;
        if(Note.Break_) 
        {
            RespectiveCount[(int)NoteType.Break]++;
            MaxScore += 2500;
            ExtraScore += 500;
        }
        else
        {
            switch(Note.Type_)
            {
                case Note.NoteType.Tap:
                case Note.NoteType.Slide_Tap:
                    RespectiveCount[(int)NoteType.Tap]++;
                    MaxScore += 500;
                    break;
                case Note.NoteType.Touch:
                    RespectiveCount[(int)NoteType.Touch]++;
                    MaxScore += 500;
                    break;
                case Note.NoteType.Hold:
                case Note.NoteType.TouchHold:
                    RespectiveCount[(int)NoteType.Hold]++;
                    MaxScore += 1000;
                    break;
                case Note.NoteType.Slide_Track:
                    RespectiveCount[(int)NoteType.Slide]++;
                    MaxScore += 1500;
                    break;
            }
        }
    }
}