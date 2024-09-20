using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using CVI = CycleValueVarible<int>;
using CVB = CycleValueVarible<bool>;
using CVS = CycleValueVarible<string>;
using Newtonsoft.Json;

/// <summary>
/// ���÷ֿ�
/// </summary>
public interface IProfileBlock
{
    /// <summary>
    /// ��ȡ�ÿ���ϸ������
    /// </summary>
    int GetDetailNum();

    void BindDetailList(UserProfile Parent);
}

[Serializable]
/// <summary>
/// �û����ã���ת��Ϊ�ļ���д
/// </summary>
public class UserProfile
{
    /// <summary>
    /// ͨ��bool�ͱ�ʾ
    /// </summary>
    private static Dictionary<int, CVB.Selection> BoolDict;
    
    /// <summary>
    /// ����ϸ��
    /// </summary>
    [JsonIgnore] public List<ICycledValue> DetailList;
    
    /// <summary>
    /// �������
    /// </summary>
    [JsonIgnore] public List<IProfileBlock> ProfileBlocks;

    /// <summary>
    /// ��ʼ����̬����
    /// </summary>
    static UserProfile()
    {
        BoolDict = new Dictionary<int, CVB.Selection>();
        BoolDict[0] = new CVB.Selection(false,"�ر�");
        BoolDict[1] = new CVB.Selection(true, "����");
    }

    /// <summary>
    /// ���캯��
    /// </summary>
    public UserProfile()
    {
        DetailList = new List<ICycledValue>();
        ProfileBlocks = new List<IProfileBlock>();

        ProfileBlocks.Add(_LongNote);
        ProfileBlocks.Add(_Score);
        ProfileBlocks.Add(_Display);


        //�Զ��������ϸ�򣬰�ְ��ŵ����������
        foreach (var Block in ProfileBlocks) Block.BindDetailList(this);
        
    }

    [Serializable]
    /// <summary>
    /// ��������
    /// </summary>
    public class LongNote:IProfileBlock
    {
        public CVB _HoldCalculate = new CVB(BoolDict);
        public CVB _EndJudge = new CVB(BoolDict);
        public CVB _HoldJudge = new CVB(BoolDict);

        void IProfileBlock.BindDetailList(UserProfile Parent)
        {
            Parent.DetailList.Add(_HoldCalculate);
            Parent.DetailList.Add(_EndJudge);
            Parent.DetailList.Add(_HoldJudge);
        }

        int IProfileBlock.GetDetailNum() => 3;
        
    }
    public LongNote _LongNote = new LongNote();

    [Serializable]
    /// <summary>
    /// �Ʒ�����
    /// </summary>
    public class Score:IProfileBlock
    {
        [SerializeField] public CVB _CriticalPerfect = new CVB(BoolDict);

        void IProfileBlock.BindDetailList(UserProfile Parent)
        {
            Parent.DetailList.Add(_CriticalPerfect);
        }

        int IProfileBlock.GetDetailNum() => 1;
        
    }
    public Score _Score = new Score();

    [Serializable]
    /// <summary>
    /// /// ��ʾ����
    /// </summary>
    public class Display:IProfileBlock
    {
        [SerializeField] public CVB _ShowLateFast = new CVB(BoolDict);

        void IProfileBlock.BindDetailList(UserProfile Parent)
        {
            Parent.DetailList.Add(_ShowLateFast);
        }

        int IProfileBlock.GetDetailNum() => 1;
        
    }
    public Display _Display = new Display();

    //��Ҫ��������
}