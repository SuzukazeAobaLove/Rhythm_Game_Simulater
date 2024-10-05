using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HaseMikan;

[Serializable]
/// <summary>
/// 判定器,计划加入旧版判定
/// </summary>
public class Judges
{
    

    [Header("静态量")]
    public int TotalCount = 0;
    public int MaxScore = 0;
    public int ExtraScore = 0;
    public List<int> RespectiveCount = new List<int> { 0, 0, 0, 0, 0 };

    [Header("运行时量")]
    public int CurScore = 0;
    public int CurExtraScore = 0;
    public DEnumArray<NoteType,JudgeType,int> JudgeCount = new DEnumArray<NoteType, JudgeType, int>();
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
    public void AddJudge(Note Note,DetailJudge Result)
    {
        if(Note.Break_)
        {
            switch(Result)
            {
                case DetailJudge.CRITICAL_PERFECT:
                    CurScore += 2500;
                    CurExtraScore += 500;
                    JudgeCount[NoteType.Break, JudgeType.CRITICAL_PERFECT]++;
                    break;
                case DetailJudge.F_PERFECT_1:
                case DetailJudge.L_PERECT_1:
                    CurScore += 2500;
                    CurExtraScore += 375;
                    JudgeCount[NoteType.Break, JudgeType.PERFECT]++;
                    break;
                case DetailJudge.F_PERFECT_2:
                case DetailJudge.L_PERFECT_2:
                    CurScore += 2500;
                    CurExtraScore += 250;
                    JudgeCount[NoteType.Break, JudgeType.PERFECT]++;
                    break;
                case DetailJudge.F_GREAT_1:
                case DetailJudge.L_GREAT_1:
                    CurScore += 2000;
                    CurExtraScore += 200;
                    JudgeCount[NoteType.Break,JudgeType.GREAT]++;
                    break;
                case DetailJudge.F_GREAT_2:
                case DetailJudge.L_GREAT_2:
                    CurScore += 1500;
                    CurExtraScore += 200;
                    JudgeCount[NoteType.Break, JudgeType.GREAT]++;
                    break;
                case DetailJudge.F_GREAT_3:
                case DetailJudge.L_GREAT_3:
                    CurScore += 1250;
                    CurExtraScore += 200;
                    JudgeCount[NoteType.Break, JudgeType.GREAT]++;
                    break;
                case DetailJudge.F_GOOD:
                case DetailJudge.L_GOOD:
                    CurScore += 1000;
                    CurExtraScore += 150;
                    JudgeCount[NoteType.Break, JudgeType.GOOD]++;
                    break;
                    
            }
        }
        else
        {
            NoteType noteType = NoteType.Tap;
            JudgeType judgeType = JudgeType.PERFECT;
            int baseMultiple = 500;
            switch(Note.Type_)
            {
                case DetailNoteType.Touch:
                    noteType = NoteType.Touch;
                    break;
                case DetailNoteType.Hold:
                case DetailNoteType.TouchHold:
                    baseMultiple *= 2;
                    noteType = NoteType.Hold;
                    break;
                case DetailNoteType.Slide_Track:
                    baseMultiple *= 3;
                    noteType = NoteType.Slide;
                    break;
            }
            switch (Result)
            {
                case DetailJudge.CRITICAL_PERFECT:
                    judgeType = JudgeType.CRITICAL_PERFECT;
                    break;
                case DetailJudge.F_GREAT_1:
                case DetailJudge.L_GREAT_1:
                case DetailJudge.F_GREAT_2:
                case DetailJudge.L_GREAT_2:
                case DetailJudge.F_GREAT_3:
                case DetailJudge.L_GREAT_3:
                    baseMultiple = baseMultiple * 4 / 5;
                    judgeType = JudgeType.GREAT;
                    break;
                case DetailJudge.F_GOOD:
                case DetailJudge.L_GOOD:
                    baseMultiple = baseMultiple / 2;
                    judgeType = JudgeType.GOOD;
                    break;
                case DetailJudge.MISS:
                    baseMultiple = 0;
                    judgeType = JudgeType.MISS;
                    break;
            }
            CurScore += baseMultiple;
            JudgeCount[noteType, judgeType]++;
        }
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
                case DetailNoteType.Tap:
                case DetailNoteType.Slide_Tap:
                    RespectiveCount[(int)NoteType.Tap]++;
                    MaxScore += 500;
                    break;
                case DetailNoteType.Touch:
                    RespectiveCount[(int)NoteType.Touch]++;
                    MaxScore += 500;
                    break;
                case DetailNoteType.Hold:
                case DetailNoteType.TouchHold:
                    RespectiveCount[(int)NoteType.Hold]++;
                    MaxScore += 1000;
                    break;
                case DetailNoteType.Slide_Track:
                    RespectiveCount[(int)NoteType.Slide]++;
                    MaxScore += 1500;
                    break;
            }
        }
    }
}