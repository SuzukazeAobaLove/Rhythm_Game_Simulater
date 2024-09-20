using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ѭ��ֵ�ӿ�
/// </summary>
public interface ICycledValue
{
    public void Add();
    public void Sub();
    public string GetValue();
}


[Serializable]
/// <summary>
/// ѭ��ֵ����
/// </summary>
/// <typeparam name="TRealVarible">ʵ�ʱ�������</typeparam>
public class CycleValueVarible<TRealVarible> : ICycledValue where TRealVarible : IEquatable<TRealVarible>
{
    /// <summary>
    /// ʵ��ֵ���ֵ��е�����
    /// </summary>
    private int _RealIndex = 0;

    /// <summary>
    /// ���ѭ��ֵ��Ӧ��ֵ
    /// </summary>
    private Dictionary<int, Selection> _RealVaribleDict;

    /// <summary>
    /// ʵ��ֵ������ֵ�İ�
    /// </summary>
    public struct Selection
    {
        public TRealVarible RealVarible;
        public string OutPutText;
        public Selection(TRealVarible realVarible, string outPutText = null)
        {
            RealVarible = realVarible;
            OutPutText = outPutText;
        }
    }

    /// <summary>
    /// ����ʵ��ֵ
    /// </summary>
    [JsonProperty]
    public TRealVarible _Value
    {
        get { return _RealVaribleDict[_RealIndex].RealVarible; }
        set
        {
            foreach (var kvp in _RealVaribleDict)
            {
                if (kvp.Value.RealVarible.Equals(value))
                {
                    _RealIndex = kvp.Key;
                    SetEqualText();
                    return;
                }
            }

            throw new Exception("�Ƿ���CycleValueVarible����ֵ:" + typeof(TRealVarible).Name);
        }
    }

    /// <summary>
    /// ������ʾ������ֵ
    /// </summary>
    [JsonIgnore] public string _EqualText = "Undefied";
    public string GetValue() => _EqualText;

    /// <summary>
    /// Ĭ�Ϲ��캯��
    /// </summary>
    /// <param name="Dict"></param>
    public CycleValueVarible(Dictionary<int, Selection> Dict)
    {
        _RealVaribleDict = Dict;
        SetEqualText();
    }

    /// <summary>
    /// �����ı�
    /// </summary>
    private void SetEqualText()
    {
        if (_RealVaribleDict[_RealIndex].OutPutText == null) _EqualText = _RealVaribleDict[_RealIndex].RealVarible.ToString();
        else _EqualText = _RealVaribleDict[_RealIndex].OutPutText;
    }

    /// <summary>
    /// ѭ��ֵ��һ
    /// </summary>
    public void Add()
    {
        _RealIndex = (_RealIndex + 1) % _RealVaribleDict.Count;
        SetEqualText();
    }


    /// <summary>
    /// ѭ��ֵ��һ
    /// </summary>
    public void Sub()
    {
        _RealIndex = ((_RealIndex - 1) + _RealVaribleDict.Count) % _RealVaribleDict.Count;
        SetEqualText();
    }


}