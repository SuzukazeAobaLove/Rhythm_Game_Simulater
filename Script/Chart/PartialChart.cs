using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
/// <summary>
/// ѡ�������Ҫ�õ�����Ϣ
/// </summary>
public class PartialChart
{
    /// <summary>
    /// ����д���ļ�������
    /// </summary>
    public PlayLog Histroy = null;


    /// <summary>
    /// ��Ҫд���ļ�������
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
    /// ������ת��Ϊд�뻺���ļ����ַ���
    /// </summary>
    /// <returns></returns>
    public string GetWriteText()
    {
        string Text = "";
        Text += $"{ID}|{FormalName}|{Collection}|{BPM}|{SongWriter}|{ChartWriter}|{Difficulty.ToString("F1")}|{InfoPath}|{LastEditTime}";
        return Text;
    }

    /// <summary>
    /// �������е��ַ���ת��Ϊ����
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static PartialChart ParseText(string text)
    {
        //�и��ȡ����Ԫ��
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
