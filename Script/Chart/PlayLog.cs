using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 只有在游玩之后才会产生的数据,和谱面单独存放，通过ID来获取
/// </summary>
public class PlayLog 
{
    /// <summary>
    /// 游玩次数
    /// </summary>
    public int PlayCount;

    /// <summary>
    /// 一种判定结果，偏差延迟保留到ms个位数
    /// </summary>
    public struct JudgeResult
    {
        public int Offset;
        public int Count;
    }
    public List<JudgeResult> BestResults;


}
