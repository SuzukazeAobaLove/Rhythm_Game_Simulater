using DG.Tweening.Plugins.Core.PathCore;
using HaseMikan;
using Newtonsoft.Json;
using NLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 文件管理器，负责读谱面，段位配置，读写设置，统计数据
/// </summary>
public static class FileManager
{
    private static string ProjectPath = Application.persistentDataPath;
    private static string SettingFolderPath = ProjectPath + "/Setting/";
    private static string SettingFilePath = ProjectPath + "/Setting/Profile.json";
    private static string StatisticsPath = ProjectPath + "/Statistics/";
    public static string ChartPath = ProjectPath + "/Charts/";
    private static string CachePath = ProjectPath + "/Charts/log.txt";
    private static string RankPath = ProjectPath + "/RankLists/";

    #region 游戏状态
    public static bool AllLoaded = false;
    public static CategoryType CategoryType;
    public static float MaxLevel = 0;
    public static string SelectCategory;
    #endregion

    #region 游玩部分
    /// <summary>
    /// 分类结果
    /// </summary>
    public static Dictionary<string,List<PartialChart>> CategoryResult = new Dictionary<string,List<PartialChart>>();
    
    /// <summary>
    /// 当前完全加载的谱面
    /// </summary>
    public static Chart CurLoadChart;

    /// <summary>
    /// 解析谱面游玩部分
    /// </summary>
    /// <param name="chart"></param>
    /// <returns></returns>
    public static void LoadChartFile(PartialChart chart)
    {
        CurLoadChart = new Chart();
        
        CurLoadChart.Info_ = chart;
        CurLoadChart.Music_ = ReadOutMP3(ChartPath + chart.InfoPath + "/music.mp3");
        CurLoadChart.Judge_ = new Judges();

        //游玩部分解析
        CurLoadChart.Notes_ = new List<Note>();
        
        //切割文本
        string[] Elements = File.ReadAllText(ChartPath + chart.InfoPath + "/data.txt").Split("&");

        //定位谱面内容
        foreach (string Element in Elements)
        {
            if (!Element.Contains("inote")) continue;

            double AlignTime = 0;
            string[] Eq = Element.Split("=");

            //当前仅加载紫谱
            if (Eq[0].Contains('5'))
            {
                //按Bpm分段
                string[] Bpms = Eq[1].Split("(");
                
                foreach(string Bpm in Bpms)
                {
                    if (!Bpm.Contains(")")) continue;

                    //解析Bpm值
                    int bpm = int.Parse(Bpm.Substring(0, Bpm.IndexOf(")")));
                    
                    //按切分音分段
                    string[] Divs = Bpm.Split("{");
                    for(int i = 0;i < Divs.Length;++i)
                    {
                        string Div = Divs[i];
                        if (!Div.Contains("}")) continue;

                        //解析切分音值
                        int div = int.Parse(Div.Substring(0, Div.IndexOf("}")));

                        //去掉开头
                        Div = Div.Remove(0, Div.IndexOf("}") + 1);

                        double delta = 60.0 / bpm * 4.0 / div;

                        //读至末尾
                        int index = 0;
                        while (index < Div.Length)
                        {
                            //逗号则加时
                            if (Div[index] == ',' || Div[index] == '，')
                            {
                                AlignTime += delta;
                                index++;
                            }
                            else
                            {
                                //否则先将整体提取
                                int stpos = index;
                                while (index < Div.Length && Div[index] != ',') index++;
                                string Tokens = Div.Substring(stpos, index - stpos);

                                //切割多押
                                foreach (var Token in Tokens.Split("/"))
                                {
                                    //Debug.Log(Token);
                                    if (Token.IndexOfAny(new[] { '1', '2', '3', '4', '5', '6', '7', '8' }) == -1) continue;
                                    
                                    //解析Note
                                    Note.ParseNote(CurLoadChart,Token,AlignTime,Tokens.Length > 1);
                                }
                            }
                        }
                        
                    }
                }
            }

        }
        File.WriteAllText(ChartPath + "/read.txt", JsonConvert.SerializeObject(CurLoadChart,Formatting.Indented));
    }

    /// <summary>
    /// 分类谱面
    /// </summary>
    public static void DivideCharts()
    {

        CategoryType = (CategoryType)UserProfile._Layout._Category._Value;
        List<PartialChart> ChartList = ChartInfos;
        CategoryResult.Clear();
        CategoryResult["宴会场"] = new List<PartialChart>();

        //判断排列类型
        switch (CategoryType)
        {
            case CategoryType.Collection:
                {
                    foreach (var Chart in ChartList)
                    {
                        if (Chart.IfUtage)
                        {
                            CategoryResult["宴会场"].Add(Chart);
                            continue;
                        }
                        if (!CategoryResult.ContainsKey(Chart.Collection))
                            CategoryResult[Chart.Collection] = new List<PartialChart>();
                        CategoryResult[Chart.Collection].Add(Chart);
                    }
                    break;
                }
            case CategoryType.Level:
                {
                    foreach (var Chart in ChartList)
                    {
                        if (Chart.IfUtage)
                        {
                            CategoryResult["宴会场"].Add(Chart);
                            continue;
                        }
                        foreach (var Diff in Chart.Difficulty)
                        {
                            if (Diff == 0f) continue;
                            string Diffstr = ((int)Diff).ToString();
                            if (Diff > 6f && Diff - (int)Diff > 0.5f) Diffstr += "+";

                            if (!CategoryResult.ContainsKey(Diffstr))
                                CategoryResult[Diffstr] = new List<PartialChart>();
                            CategoryResult[Diffstr].Add(Chart);
                        }

                    }
                    break;
                }
            //未实现的功能
            case CategoryType.Grade:
                {
                    break;
                }
            case CategoryType.Version:
                {
                    foreach (var Chart in ChartList)
                    {
                        if (Chart.IfUtage)
                        {
                            CategoryResult["宴会场"].Add(Chart);
                            continue;
                        }
                        if (!CategoryResult.ContainsKey(Chart.Version))
                            CategoryResult[Chart.Version] = new List<PartialChart>();

                        CategoryResult[Chart.Version].Add(Chart);
                    }
                    break;
                }
            case CategoryType.All:
                {
                    CategoryResult["All"] = new List<PartialChart>();
                    foreach (var Chart in ChartList)
                    {
                        if (Chart.IfUtage)
                        {
                            CategoryResult["宴会场"].Add(Chart);
                            continue;
                        }
                        CategoryResult["All"].Add(Chart);
                    }
                    break;
                }
        }
    }
    #endregion

    #region 外部读取部分
    /// <summary>
    /// 从外部读取png文件至Image
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="holder">返回至</param>
    /// <returns></returns>
    public static Sprite ReadOutPNG(string path)
    {
        byte[] imageData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2); // 创建一个 Texture2D 对象
        texture.LoadImage(imageData); // 从字节数组加载图片数据
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        
    }  

    /// <summary>
    /// 读取外部MP3
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static AudioClip ReadOutMP3(string path)
    {
        string filename = System.IO.Path.GetFileNameWithoutExtension(path);

        MpegFile mpegFile = new MpegFile(path);

        // assign samples into AudioClip
        AudioClip audioClip = AudioClip.Create(filename,
                                        (int)(mpegFile.Length / sizeof(float) / mpegFile.Channels),
                                        mpegFile.Channels,
                                        mpegFile.SampleRate,
                                        true,
                                        data => { int actualReadCount = mpegFile.ReadSamples(data, 0, data.Length); },
                                        //上面的回调不能少，下面的回调经笔者测试，就算不要也能正常搞事情！
                                        position => { mpegFile = new MpegFile(path); }
                                      );
        return audioClip;
    }
    #endregion

    #region 设置部分
    /// <summary>
    /// 全局设置对象
    /// </summary>
    public static UserProfile UserProfile;

    /// <summary>
    /// 读取设置
    /// </summary>
    public static void LoadProfile()
    {
        UserProfile = new UserProfile();
        //如果设置存在，则读取
        if (File.Exists(SettingFilePath))
        {
            string JsonS = File.ReadAllText(SettingFilePath);
            //Debug.Log(JsonS + "\n");
            UserProfile = JsonConvert.DeserializeObject<UserProfile>(JsonS);
        }
        //否则则写入默认值
        else
        {
            if(!Directory.Exists(SettingFolderPath)) Directory.CreateDirectory(SettingFolderPath);
            File.WriteAllText(SettingFilePath,JsonConvert.SerializeObject(UserProfile));
        }
        
    }

    /// <summary>
    /// 保存设置
    /// </summary>
    public static void SaveProfile()
    {
        File.WriteAllText(SettingFilePath,JsonConvert.SerializeObject(UserProfile,Formatting.Indented));
    }

    #endregion

    #region 谱面信息部分
    /// <summary>
    /// 所有谱面的信息
    /// </summary>
    public static List<PartialChart> ChartInfos;
    
    /// <summary>
    /// 读取延迟
    /// </summary>
    public static float LoadDelay = 0f;
    /// <summary>
    /// 加载谱面信息
    /// </summary>
    public static void LoadChartInfo()
    {
        ChartInfos = new List<PartialChart>();
        
        //路径不存在那么一定没有谱面
        if (!Directory.Exists(ChartPath)) Directory.CreateDirectory(ChartPath);   
        //全量扫描
        else TraverseFolders(ChartPath);
        
        
        //绑定游玩记录
        //BindPlayHistory();
    }

    /// <summary>
    /// 绑定游玩记录
    /// </summary>
    private static void BindPlayHistory()
    {

    }

    /// <summary>
    /// 遍历文件夹
    /// </summary>
    private static void TraverseFolders(string folderPath)
    {
        string[] subfolders = Directory.GetDirectories(folderPath);

        //如果是叶文件夹，则开始解析
        if(subfolders.Length == 0)
        {
            if (!File.Exists(folderPath + "/track.mp3")) return;
            if (!File.Exists(folderPath + "/bg.png")) return;
            
            //解析文件
            PartialChart chart = ParseFile(folderPath);
            if(chart != null) ChartInfos.Add(chart);
        }
        //否则递归
        else
        {
            //Parallel.ForEach( subfolders,item => TraverseFolders(item));
            foreach(string subfolder in subfolders) TraverseFolders(subfolder);
        }
    }

    /// <summary>
    /// 从文件中解析出PartialChart
    /// </summary>
    /// <returns>一个谱面信息，可能是null</returns>
    private static PartialChart ParseFile(string folderPath)
    {
        PartialChart chart = new PartialChart();
        chart.Difficulty.AddRange(new List<float>{ 0,0,0,0,0,0,0,0});
        
        //填入路径信息
        chart.InfoPath = folderPath.Substring(folderPath.IndexOf(ChartPath) + ChartPath.Length);
        
        
        //在文本中寻找属性
        int CollectProperty = 0;
        bool IfValidDiff = false;

        
        using(StreamReader sr = new StreamReader(folderPath + "/maidata.txt"))
         {
            //读取
            while (!sr.EndOfStream)
            {
                string Line = sr.ReadLine();
                
                //格式基准
                if(Line.Length == 0) continue;
                if (Line.Contains("inote")) break;
                if (Line[0] != '&') continue;

                //切割键值
                Line = Line.Remove(0, 1);
                string[] Token = Line.Split("=");
                if (Token.Length != 2) continue;

                //解析难度
                if (Token[0].IndexOf("lv") != -1)
                {
                    if (Token[1].Length == 0) continue;

                    //标记宴谱
                    if (Token[1].Last() == '?')
                    {
                        Token[1] = Token[1].Remove(Token[1].Length - 1);
                        chart.IfUtage = true;
                    }

                    //以防报错
                    try { chart.Difficulty[int.Parse(Token[0].Substring(3)) - 1] = float.Parse(Token[1]); }
                    catch { continue; }
                    
                    IfValidDiff = true;
                    MaxLevel = Mathf.Max(MaxLevel, float.Parse(Token[1]));

                    continue;
                }

                //Token比对
                switch (Token[0])
                {
                    case "title":
                        {
                            if (chart.FormalName == null) CollectProperty++;
                            chart.FormalName = Token[1];
                            break;
                        }
                    case "shortid":
                        {
                            if (chart.ID == 0) CollectProperty++;
                            try { chart.ID = int.Parse(Token[1]); }
                            catch { }
                            break;
                        }
                    case "artist":
                        {
                            if (chart.SongWriter == null) CollectProperty++;
                            chart.SongWriter = Token[1];
                            break;
                        }
                    case "des":
                        {
                            if (chart.ChartWriter == null) CollectProperty++;
                            chart.ChartWriter = Token[1];
                            break;
                        }
                    case "wholebpm":
                        {
                            if (chart.BPM == 0) CollectProperty++;
                            try { chart.BPM = int.Parse(Token[1]); }
                            catch { }
                            break;
                        }
                    case "genre":
                        {
                            if (chart.Collection == null) CollectProperty++;
                            chart.Collection = Token[1];
                            break;
                        }
                    case "version":
                        {
                            if (chart.Version == null) CollectProperty++;
                            chart.Version = Token[1];
                            break;
                        }
                }
            }
        }
        
        //如果属性数量不够
        if(!IfValidDiff)return null;
        else return chart;
    }

    #endregion
}
