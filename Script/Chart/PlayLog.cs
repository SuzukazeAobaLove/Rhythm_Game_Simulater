using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ֻ��������֮��Ż����������,�����浥����ţ�ͨ��ID����ȡ
/// </summary>
public class PlayLog 
{
    /// <summary>
    /// �������
    /// </summary>
    public int PlayCount;

    /// <summary>
    /// һ���ж������ƫ���ӳٱ�����ms��λ��
    /// </summary>
    public struct JudgeResult
    {
        public int Offset;
        public int Count;
    }
    public List<JudgeResult> BestResults;


}
