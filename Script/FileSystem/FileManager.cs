using DG.Tweening.Plugins.Core.PathCore;
using Newtonsoft.Json;
using NLayer;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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

    #region 文件状态
    public static bool AllLoaded = false;
    public static bool ProfileLoaded = false;
    public static bool ChartLoaded = false;
    #endregion

    #region 游玩部分
    
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
        
        //游玩部分解析
        CurLoadChart.Notes_ = new List<Chart.Note>();
        
        //切割文本
        string[] Elements = File.ReadAllText(ChartPath + chart.InfoPath + "/data.txt").Split("$");


        foreach (string Element in Elements)
        {
            if (!Element.Contains("inote ")) continue;

            double AlignTime = 0;
            string[] Eq = Element.Split("=");
            if (Eq[0].Contains('5'))
            {
                //按Bpm分段
                string[] Bpms = Eq[1].Split("(");
                
                foreach(string Bpm in Bpms)
                {
                    //解析Bpm值
                    int bpm = int.Parse(Bpm.Substring(0, Bpm.IndexOf(")")));
                    
                    //按切分音分段
                    string[] Divs = Bpm.Split("{");
                    foreach(string Div in Divs)
                    {
                        //解析切分音值
                        int div = int.Parse(Div.Substring(0, Div.IndexOf("}")));
                        double delta = 60 / bpm * 4 / div;
                        
                        //读至末尾
                        int index = 0;
                        while (index < Div.Length)
                        {
                            //逗号则加时
                            if (Div[index] == ',') AlignTime += delta;
                            else
                            {
                                //否则先将整体提取
                                string Token = "";
                                while(index < Div.Length && Div[index] != ',')
                                {
                                    Token += Div[index];
                                    index++;
                                }

                            }
                        }
                        
                    }
                }
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
        ProfileLoaded = true;
    }

    /// <summary>
    /// 保存设置
    /// </summary>
    public static void SaveProfile()
    {
        File.WriteAllText(SettingFilePath,JsonConvert.SerializeObject(UserProfile));
    }

    #endregion

    #region 谱面信息部分
    /// <summary>
    /// 所有谱面的信息
    /// </summary>
    public static List<PartialChart> ChartInfos;
    /// <summary>
    /// 从缓存中获取的临时字典
    /// </summary>
    private static Dictionary<string, PartialChart> CacheTemp;
    
    /// <summary>
    /// 加载谱面信息
    /// </summary>
    public static void LoadChartInfo()
    {
        ChartInfos = new List<PartialChart>();
        CacheTemp = new Dictionary<string, PartialChart>();

        //路径不存在那么一定没有谱面
        if (!Directory.Exists(ChartPath))
        {
            Directory.CreateDirectory(ChartPath);
        }
        //如果没有缓存文件，只能全量扫描
        else if (!File.Exists(CachePath))
        {
            TraverseFolders(ChartPath, false);
            SaveCacheFile();
        }
        //如果存在，首先读入缓存
        else
        {
            LoadCacheFile();
            //再进行遍历
            TraverseFolders(ChartPath, true);
            SaveCacheFile();
            CacheTemp.Clear();

        }
        
        ChartLoaded = true;
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
    private static void TraverseFolders(string folderPath,bool ifCache)
    {
        string[] subfolders = Directory.GetDirectories(folderPath);

        //如果是叶文件夹，则开始解析
        if(subfolders.Length == 0)
        {
            //首先进行重命名
            if(File.Exists(folderPath + "/maidata.txt")) File.Move(folderPath + "/maidata.txt", folderPath + "/data.txt");
            if (File.Exists(folderPath + "/track.mp3")) File.Move(folderPath + "/track.mp3", folderPath + "/music.mp3");
            if (File.Exists(folderPath + "/bg.png")) File.Move(folderPath + "/bg.png", folderPath + "/cover.png");

            //如果有缓存
            if(ifCache)
            {
                string RelativePath = folderPath.Substring(folderPath.IndexOf(ChartPath) + ChartPath.Length);
                //Debug.Log(RelativePath);

                
                if (!File.Exists(folderPath + "/data.txt")) return;

                //如果有缓存文件中有写
                if (CacheTemp.ContainsKey(RelativePath))
                {
                    //如果一致则转移
                    if (CacheTemp[RelativePath].LastEditTime == File.GetLastWriteTime(folderPath + "/data.txts").ToString())
                    {
                        ChartInfos.Add(CacheTemp[RelativePath]);
                        CacheTemp.Remove(RelativePath);
                        return;
                    }
                    //否则移除，选择读取文件
                    else CacheTemp.Remove(RelativePath);
                }
            }

            
            if (!File.Exists(folderPath + "/music.mp3")) return;
            if (!File.Exists(folderPath + "/cover.png")) return;
            

            //解析文件
            PartialChart chart = ParseFile(folderPath);
            if(chart != null) ChartInfos.Add(chart);
            
                    
        }
        //否则递归
        else
        {
            foreach(string subfolder in subfolders) TraverseFolders(subfolder,ifCache);
        }
    }

    /// <summary>
    /// 从文件中解析出PartialChart
    /// </summary>
    /// <returns>一个谱面信息，可能是null</returns>
    private static PartialChart ParseFile(string folderPath)
    {
        PartialChart chart = new PartialChart();

        //切割文本
        string[] Elements = Regex.Split(File.ReadAllText(folderPath + "/data.txt"), "\r?\n");

        //填入路径信息
        chart.InfoPath = folderPath.Substring(folderPath.IndexOf(ChartPath) + ChartPath.Length);
        chart.LastEditTime = File.GetLastWriteTime(folderPath + "/data.txt").ToString();

        //在文本中寻找属性
        int CollectProperty = 0;        
        foreach (string Element in Elements)
        {
            if (Element.Length == 0 || Element[0] != '&') continue;

            //提取Token
            string[] Tokens = Element.Split('=');
            Tokens[0] = Tokens[0].Substring(1);
            if(Tokens.Length != 2) continue;

            //处理等级信息
            if (Tokens[0].IndexOf("lv_")!= -1)
            {
                if (chart.Difficulty == 0f) CollectProperty++;
                //Debug.Log(Tokens[0].Substring(3));
                chart.Difficulty = float.Parse(Tokens[0].Substring(3));
                continue;
            }

            //Note序列开始标志着属性信息结束
            if (Tokens[0].IndexOf("inote_") != -1) break;

            //Token比对
            switch (Tokens[0])
            {
                case "title":
                    {
                        if (chart.FormalName == null) CollectProperty++;
                        chart.FormalName = Tokens[1];
                        break;
                    }
                case "shortid":
                case "id":
                    {
                        if (chart.ID == 0) CollectProperty++;
                        chart.ID = int.Parse(Tokens[1]);
                        break;
                    }
                case "artist":
                    {
                        if (chart.SongWriter == null) CollectProperty++;
                        chart.SongWriter = Tokens[1];
                        break;
                    }
                case "des":
                case "designer":
                    {
                        if (chart.ChartWriter == null) CollectProperty++;
                        chart.ChartWriter = Tokens[1];
                        break;
                    }
                case "wholebpm":
                case "bpm":
                    {
                        if(chart.BPM == 0) CollectProperty++;
                        chart.BPM = int.Parse(Tokens[1]);
                        break;
                    }
                case "genre":
                case "collection":
                    {
                        if (chart.Collection == null) CollectProperty++;
                        chart.Collection = Tokens[1];
                        break;
                    }
            }
        }
        //如果属性数量不够
        if(CollectProperty < 7)return null;
        else return chart;
    }

    /// <summary>
    /// 读取缓存信息文件
    /// </summary>
    private static void LoadCacheFile()
    {
        string[] Element = Regex.Split(File.ReadAllText(CachePath), "\r?\n");
        //放到临时字典中去
        foreach (var Text in Element)
        {
            PartialChart Chart = PartialChart.ParseText(Text);
            if(Chart != null) CacheTemp[Chart.InfoPath] = Chart;
        }
    }


    /// <summary>
    /// 保存缓存信息文件
    /// </summary>
    private static void SaveCacheFile()
    {
        StreamWriter writer = new StreamWriter(CachePath);
        foreach(var chart in ChartInfos) writer.WriteLine(chart.GetWriteText());
        writer.Close();
        
    }

    #endregion
}
