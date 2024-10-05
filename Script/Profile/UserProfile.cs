using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using CVI = CycleValueVarible<int>;
using CVB = CycleValueVarible<bool>;
using CVS = CycleValueVarible<string>;

using Newtonsoft.Json;
using HaseMikan;

/// <summary>
/// 设置分块
/// </summary>
public interface IProfileBlock
{
    /// <summary>
    /// 该类别名称
    /// </summary>
    string GetClassName();
    
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
    /// 返回一个包含0-i的CVI字典
    /// </summary>
    /// <param name="i"></param>
    /// <param name="ifZero"></param>
    /// <returns></returns>

    static Dictionary<int,CVI.Selection> GetIntDict(int i,bool ifZero)
    {
        var Dict = new Dictionary<int, CVI.Selection>();
        for (int j = 0; j < i + (ifZero ? 1 : 0); j++)
        {
            Dict[j] = new CVI.Selection(j + (ifZero ? 0 : 1));
        }
        return Dict;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    public UserProfile()
    {
        DetailList = new List<ICycledValue>();
        ProfileBlocks = new List<IProfileBlock>();

        ProfileBlocks.Add(_Layout);
        ProfileBlocks.Add(_Volumn);
        ProfileBlocks.Add(_Score);
        ProfileBlocks.Add(_Display);


        //自动添加所有细则，把职责放到类别类里面
        foreach (var Block in ProfileBlocks) Block.BindDetailList(this);
        
    }

    [Serializable]
    
    public class Layout:IProfileBlock
    {
        
        [SerializeField] public CVI _Category = new CVI(CategoryDict,"歌曲分类方式");
        [SerializeField] public CVI _Order = new CVI(OrderDict, "歌曲排列顺序");
        
        private static Dictionary<int, CVI.Selection> CategoryDict;
        private static Dictionary<int,CVI.Selection> OrderDict;
        
        static Layout()
        {
            CategoryDict = new Dictionary<int, CVI.Selection>();
            CategoryDict[0] = new CVI.Selection(0, "流派");
            CategoryDict[1] = new CVI.Selection(1, "等级");
            CategoryDict[2] = new CVI.Selection(2, "评级");
            CategoryDict[3] = new CVI.Selection(3, "版本");
            CategoryDict[4] = new CVI.Selection(4, "全部");

            OrderDict = new Dictionary<int, CVI.Selection>();
            OrderDict[0] = new CVI.Selection(0, "默认");
            OrderDict[1] = new CVI.Selection(1, "评级");
            OrderDict[2] = new CVI.Selection(2, "等级");
        }

        void IProfileBlock.BindDetailList(UserProfile Parent)
        {
            Parent.DetailList.Add(_Category);
            Parent.DetailList.Add(_Order);
        }

        string IProfileBlock.GetClassName() => "排列";
        int IProfileBlock.GetDetailNum() => 2;
    }
    public Layout _Layout = new Layout();

    [Serializable]
    /// <summary>
    /// 计分设置
    /// </summary>
    public class Score:IProfileBlock
    {
        [SerializeField] public CVB _CriticalPerfect = new CVB(BoolDict,"得分是否区分Critical Perfect和Perfect");

        void IProfileBlock.BindDetailList(UserProfile Parent)
        {
            Parent.DetailList.Add(_CriticalPerfect);
        }
        string IProfileBlock.GetClassName() => "分数";
        int IProfileBlock.GetDetailNum() => 1;
        
    }
    public Score _Score = new Score();

    [Serializable]
    /// <summary>
    /// /// 显示设置
    /// </summary>
    public class Display:IProfileBlock
    {
        [SerializeField] public CVB _ShowLateFast = new CVB(BoolDict,"是否显示Late/Fast");

        void IProfileBlock.BindDetailList(UserProfile Parent)
        {
            Parent.DetailList.Add(_ShowLateFast);
        }
        string IProfileBlock.GetClassName() => "显示";
        int IProfileBlock.GetDetailNum() => 1;
        
    }
    public Display _Display = new Display();

    //需要音量设置
    public class Volumn : IProfileBlock
    {
        [SerializeField] public CVI _MusicVolumn = new CVI(GetIntDict(10,true),"音量值");
        public void BindDetailList(UserProfile Parent)
        {
            Parent.DetailList.Add(_MusicVolumn);
        }
        string IProfileBlock.GetClassName() => "音量";
        public int GetDetailNum() => 1;
       
    }
    public Volumn _Volumn = new Volumn();
}