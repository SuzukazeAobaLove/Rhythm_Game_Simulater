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
    

    public string FormalName = "";
    public int BPM = 0;
    public int ID = 0;
    public List<float> Difficulty = new List<float>();
    public string SongWriter = "";
    public string ChartWriter = "";
    public string Collection = "";
    public string Version = "";
    
    public string InfoPath = null;
    public bool IfUtage = false;
    
}