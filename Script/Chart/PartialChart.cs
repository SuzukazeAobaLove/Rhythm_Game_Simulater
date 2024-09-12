using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
/// <summary>
/// 选择界面需要用到的信息
/// </summary>
public class PartialChart
{
    /// <summary>
    /// 无需写入文件的数据
    /// </summary>
    public PlayLog Histroy = null;


    /// <summary>
    /// 需要写入文件的数据
    /// </summary>
    

    public string FormalName = null;
    public int BPM = 0;
    public int ID = 0;
    public float Difficulty = 0f;
    public string SongWriter = null;
    public string ChartWriter = null;
    public string Collection = null;
    
    public string LastEditTime = null;
    public string InfoPath = null;

    /// <summary>
    /// 将对象转化为写入缓存文件的字符串
    /// </summary>
    /// <returns></returns>
    public string GetWriteText()
    {
        string Text = "";
        Text += $"{ID}|{FormalName}|{Collection}|{BPM}|{SongWriter}|{ChartWriter}|{Difficulty.ToString("F1")}|{InfoPath}|{LastEditTime}";
        return Text;
    }

    /// <summary>
    /// 将缓存中的字符串转化为对象
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static PartialChart ParseText(string text)
    {
        //切割获取所有元素
        PartialChart ChartInfo = new PartialChart();
        if (text == "") return null;

        string[] Element = text.Split('|');

        ChartInfo.ID = int.Parse(Element[0]);
        ChartInfo.FormalName = Element[1];
        ChartInfo.Collection = Element[2];
        ChartInfo.BPM = int.Parse(Element[3]);
        ChartInfo.SongWriter = Element[4];
        ChartInfo.ChartWriter = Element[5];
        ChartInfo.Difficulty = float.Parse(Element[6]);
        ChartInfo.InfoPath = Element[7];
        ChartInfo.LastEditTime = Element[8];
        return ChartInfo;
    }
}
