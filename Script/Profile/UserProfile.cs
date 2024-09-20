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
/// 设置分块
/// </summary>
public interface IProfileBlock
{
    /// <summary>
    /// 获取该块下细则数量
    /// </summary>
    int GetDetailNum();

    void BindDetailList(UserProfile Parent);
}

[Serializable]
/// <summary>
/// 用户设置，可转化为文件读写
/// </summary>
public class UserProfile
{
    /// <summary>
    /// 通用bool型表示
    /// </summary>
    private static Dictionary<int, CVB.Selection> BoolDict;
    
    /// <summary>
    /// 所有细则
    /// </summary>
    [JsonIgnore] public List<ICycledValue> DetailList;
    
    /// <summary>
    /// 所有类别
    /// </summary>
    [JsonIgnore] public List<IProfileBlock> ProfileBlocks;

    /// <summary>
    /// 初始化静态变量
    /// </summary>
    static UserProfile()
    {
        BoolDict = new Dictionary<int, CVB.Selection>();
        BoolDict[0] = new CVB.Selection(false,"关闭");
        BoolDict[1] = new CVB.Selection(true, "开启");
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    public UserProfile()
    {
        DetailList = new List<ICycledValue>();
        ProfileBlocks = new List<IProfileBlock>();

        ProfileBlocks.Add(_LongNote);
        ProfileBlocks.Add(_Score);
        ProfileBlocks.Add(_Display);


        //自动添加所有细则，把职责放到类别类里面
        foreach (var Block in ProfileBlocks) Block.BindDetailList(this);
        
    }

    [Serializable]
    /// <summary>
    /// 长条设置
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
    /// 计分设置
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
    /// /// 显示设置
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

    //需要音量设置
}