using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HaseMikan
{
    /// <summary>
    /// 场景编号
    /// </summary>
    public enum Scenes { MainMenu = 1, Profile, CategorySelect, SongSelect, Region_Partner, Ranking, Statistic, SongPlay };
    
    /// <summary>
    /// 歌曲分类模式
    /// </summary>
    public enum CategoryType { Collection, Level, Grade, Version, All }
    
    /// <summary>
    /// 歌曲显示顺序
    /// </summary>
    public enum DisplayOrderType { Id, Achievement, Level }

    /// <summary>
    /// Note基础类型枚举
    /// </summary>
    public enum DetailNoteType
    {
        Tap, Hold, Slide_Tap, Slide_Track, Touch, TouchHold
    };

    /// <summary>
    /// Note分区编号
    /// </summary>
    public enum Block
    {
        A1, A2, A3, A4, A5, A6, A7, A8,
        B1, B2, B3, B4, B5, B6, B7, B8,
        C,
        D1 = 24, D2, D3, D4, D5, D6, D7, D8,
        E1, E2, E3, E4, E5, E6, E7, E8
    };

    /// <summary>
    /// Note种类
    /// </summary>
    public enum NoteType { Tap, Hold, Slide, Touch, Break };

    /// <summary>
    /// 具体种类
    /// </summary>
    public enum DetailJudge
    {
        CRITICAL_PERFECT,
        F_PERFECT_1, L_PERECT_1, F_PERFECT_2, L_PERFECT_2,
        F_GREAT_1, L_GREAT_1, F_GREAT_2, L_GREAT_2, F_GREAT_3, L_GREAT_3,
        F_GOOD, L_GOOD,
        MISS
    };

    public enum JudgeType
    {
        CRITICAL_PERFECT, PERFECT, GREAT, GOOD, MISS
    };
}