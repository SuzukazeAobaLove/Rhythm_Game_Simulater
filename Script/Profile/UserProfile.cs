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
/// ���÷ֿ�
/// </summary>
public interface IProfileBlock
{
    /// <summary>
    /// ���������
    /// </summary>
    string GetClassName();
    
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
    /// ����һ������0-i��CVI�ֵ�
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
    /// ���캯��
    /// </summary>
    public UserProfile()
    {
        DetailList = new List<ICycledValue>();
        ProfileBlocks = new List<IProfileBlock>();

        ProfileBlocks.Add(_Layout);
        ProfileBlocks.Add(_Volumn);
        ProfileBlocks.Add(_Score);
        ProfileBlocks.Add(_Display);


        //�Զ��������ϸ�򣬰�ְ��ŵ����������
        foreach (var Block in ProfileBlocks) Block.BindDetailList(this);
        
    }

    [Serializable]
    
    public class Layout:IProfileBlock
    {
        
        [SerializeField] public CVI _Category = new CVI(CategoryDict,"�������෽ʽ");
        [SerializeField] public CVI _Order = new CVI(OrderDict, "��������˳��");
        
        private static Dictionary<int, CVI.Selection> CategoryDict;
        private static Dictionary<int,CVI.Selection> OrderDict;
        
        static Layout()
        {
            CategoryDict = new Dictionary<int, CVI.Selection>();
            CategoryDict[0] = new CVI.Selection(0, "����");
            CategoryDict[1] = new CVI.Selection(1, "�ȼ�");
            CategoryDict[2] = new CVI.Selection(2, "����");
            CategoryDict[3] = new CVI.Selection(3, "�汾");
            CategoryDict[4] = new CVI.Selection(4, "ȫ��");

            OrderDict = new Dictionary<int, CVI.Selection>();
            OrderDict[0] = new CVI.Selection(0, "Ĭ��");
            OrderDict[1] = new CVI.Selection(1, "����");
            OrderDict[2] = new CVI.Selection(2, "�ȼ�");
        }

        void IProfileBlock.BindDetailList(UserProfile Parent)
        {
            Parent.DetailList.Add(_Category);
            Parent.DetailList.Add(_Order);
        }

        string IProfileBlock.GetClassName() => "����";
        int IProfileBlock.GetDetailNum() => 2;
    }
    public Layout _Layout = new Layout();

    [Serializable]
    /// <summary>
    /// �Ʒ�����
    /// </summary>
    public class Score:IProfileBlock
    {
        [SerializeField] public CVB _CriticalPerfect = new CVB(BoolDict,"�÷��Ƿ�����Critical Perfect��Perfect");

        void IProfileBlock.BindDetailList(UserProfile Parent)
        {
            Parent.DetailList.Add(_CriticalPerfect);
        }
        string IProfileBlock.GetClassName() => "����";
        int IProfileBlock.GetDetailNum() => 1;
        
    }
    public Score _Score = new Score();

    [Serializable]
    /// <summary>
    /// /// ��ʾ����
    /// </summary>
    public class Display:IProfileBlock
    {
        [SerializeField] public CVB _ShowLateFast = new CVB(BoolDict,"�Ƿ���ʾLate/Fast");

        void IProfileBlock.BindDetailList(UserProfile Parent)
        {
            Parent.DetailList.Add(_ShowLateFast);
        }
        string IProfileBlock.GetClassName() => "��ʾ";
        int IProfileBlock.GetDetailNum() => 1;
        
    }
    public Display _Display = new Display();

    //��Ҫ��������
    public class Volumn : IProfileBlock
    {
        [SerializeField] public CVI _MusicVolumn = new CVI(GetIntDict(10,true),"����ֵ");
        public void BindDetailList(UserProfile Parent)
        {
            Parent.DetailList.Add(_MusicVolumn);
        }
        string IProfileBlock.GetClassName() => "����";
        public int GetDetailNum() => 1;
       
    }
    public Volumn _Volumn = new Volumn();
}