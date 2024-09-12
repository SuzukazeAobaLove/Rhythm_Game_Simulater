using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 循环值接口
/// </summary>
public interface ICycledValue
{
    public void Add();
    public void Sub();
    public string GetValue();
}


[Serializable]
/// <summary>
/// 循环值变量
/// </summary>
/// <typeparam name="TRealVarible">实际变量类型</typeparam>
public class CycleValueVarible<TRealVarible> : ICycledValue where TRealVarible : IEquatable<TRealVarible>
{
    /// <summary>
    /// 实际值在字典中的索引
    /// </summary>
    private int _RealIndex = 0;

    /// <summary>
    /// 存放循环值对应的值
    /// </summary>
    private Dictionary<int, Selection> _RealVaribleDict;

    /// <summary>
    /// 实际值和字面值的绑定
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
    /// 变量实际值
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

            throw new Exception("非法的CycleValueVarible变量值:" + typeof(TRealVarible).Name);
        }
    }

    /// <summary>
    /// 用于显示的字面值
    /// </summary>
    [JsonIgnore] public string _EqualText = "Undefied";
    public string GetValue() => _EqualText;

    /// <summary>
    /// 默认构造函数
    /// </summary>
    /// <param name="Dict"></param>
    public CycleValueVarible(Dictionary<int, Selection> Dict)
    {
        _RealVaribleDict = Dict;
        SetEqualText();
    }

    /// <summary>
    /// 设置文本
    /// </summary>
    private void SetEqualText()
    {
        if (_RealVaribleDict[_RealIndex].OutPutText == null) _EqualText = _RealVaribleDict[_RealIndex].RealVarible.ToString();
        else _EqualText = _RealVaribleDict[_RealIndex].OutPutText;
    }

    /// <summary>
    /// 循环值加一
    /// </summary>
    public void Add()
    {
        _RealIndex = (_RealIndex + 1) % _RealVaribleDict.Count;
        SetEqualText();
    }


    /// <summary>
    /// 循环值减一
    /// </summary>
    public void Sub()
    {
        _RealIndex = ((_RealIndex - 1) + _RealVaribleDict.Count) % _RealVaribleDict.Count;
        SetEqualText();
    }


}