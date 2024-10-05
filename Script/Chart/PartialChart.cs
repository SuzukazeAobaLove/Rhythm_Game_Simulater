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