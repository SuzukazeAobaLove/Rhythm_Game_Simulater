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
/// �ļ�����������������棬��λ���ã���д���ã�ͳ������
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

    #region ��Ϸ״̬
    public static bool AllLoaded = false;
    public static CategoryType CategoryType;
    public static float MaxLevel = 0;
    public static string SelectCategory;
    #endregion

    #region ���沿��
    /// <summary>
    /// ������
    /// </summary>
    public static Dictionary<string,List<PartialChart>> CategoryResult = new Dictionary<string,List<PartialChart>>();
    
    /// <summary>
    /// ��ǰ��ȫ���ص�����
    /// </summary>
    public static Chart CurLoadChart;

    /// <summary>
    /// �����������沿��
    /// </summary>
    /// <param name="chart"></param>
    /// <returns></returns>
    public static void LoadChartFile(PartialChart chart)
    {
        CurLoadChart = new Chart();
        
        CurLoadChart.Info_ = chart;
        CurLoadChart.Music_ = ReadOutMP3(ChartPath + chart.InfoPath + "/music.mp3");
        CurLoadChart.Judge_ = new Judges();

        //���沿�ֽ���
        CurLoadChart.Notes_ = new List<Note>();
        
        //�и��ı�
        string[] Elements = File.ReadAllText(ChartPath + chart.InfoPath + "/data.txt").Split("&");

        //��λ��������
        foreach (string Element in Elements)
        {
            if (!Element.Contains("inote")) continue;

            double AlignTime = 0;
            string[] Eq = Element.Split("=");

            //��ǰ����������
            if (Eq[0].Contains('5'))
            {
                //��Bpm�ֶ�
                string[] Bpms = Eq[1].Split("(");
                
                foreach(string Bpm in Bpms)
                {
                    if (!Bpm.Contains(")")) continue;

                    //����Bpmֵ
                    int bpm = int.Parse(Bpm.Substring(0, Bpm.IndexOf(")")));
                    
                    //���з����ֶ�
                    string[] Divs = Bpm.Split("{");
                    for(int i = 0;i < Divs.Length;++i)
                    {
                        string Div = Divs[i];
                        if (!Div.Contains("}")) continue;

                        //�����з���ֵ
                        int div = int.Parse(Div.Substring(0, Div.IndexOf("}")));

                        //ȥ����ͷ
                        Div = Div.Remove(0, Div.IndexOf("}") + 1);

                        double delta = 60.0 / bpm * 4.0 / div;

                        //����ĩβ
                        int index = 0;
                        while (index < Div.Length)
                        {
                            //�������ʱ
                            if (Div[index] == ',' || Div[index] == '��')
                            {
                                AlignTime += delta;
                                index++;
                            }
                            else
                            {
                                //�����Ƚ�������ȡ
                                int stpos = index;
                                while (index < Div.Length && Div[index] != ',') index++;
                                string Tokens = Div.Substring(stpos, index - stpos);

                                //�и��Ѻ
                                foreach (var Token in Tokens.Split("/"))
                                {
                                    //Debug.Log(Token);
                                    if (Token.IndexOfAny(new[] { '1', '2', '3', '4', '5', '6', '7', '8' }) == -1) continue;
                                    
                                    //����Note
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
    /// ��������
    /// </summary>
    public static void DivideCharts()
    {

        CategoryType = (CategoryType)UserProfile._Layout._Category._Value;
        List<PartialChart> ChartList = ChartInfos;
        CategoryResult.Clear();
        CategoryResult["��᳡"] = new List<PartialChart>();

        //�ж���������
        switch (CategoryType)
        {
            case CategoryType.Collection:
                {
                    foreach (var Chart in ChartList)
                    {
                        if (Chart.IfUtage)
                        {
                            CategoryResult["��᳡"].Add(Chart);
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
                            CategoryResult["��᳡"].Add(Chart);
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
            //δʵ�ֵĹ���
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
                            CategoryResult["��᳡"].Add(Chart);
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
                            CategoryResult["��᳡"].Add(Chart);
                            continue;
                        }
                        CategoryResult["All"].Add(Chart);
                    }
                    break;
                }
        }
    }
    #endregion

    #region �ⲿ��ȡ����
    /// <summary>
    /// ���ⲿ��ȡpng�ļ���Image
    /// </summary>
    /// <param name="path">·��</param>
    /// <param name="holder">������</param>
    /// <returns></returns>
    public static Sprite ReadOutPNG(string path)
    {
        byte[] imageData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2); // ����һ�� Texture2D ����
        texture.LoadImage(imageData); // ���ֽ��������ͼƬ����
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        
    }  

    /// <summary>
    /// ��ȡ�ⲿMP3
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
                                        //����Ļص������٣�����Ļص������߲��ԣ����㲻ҪҲ�����������飡
                                        position => { mpegFile = new MpegFile(path); }
                                      );
        return audioClip;
    }
    #endregion

    #region ���ò���
    /// <summary>
    /// ȫ�����ö���
    /// </summary>
    public static UserProfile UserProfile;

    /// <summary>
    /// ��ȡ����
    /// </summary>
    public static void LoadProfile()
    {
        UserProfile = new UserProfile();
        //������ô��ڣ����ȡ
        if (File.Exists(SettingFilePath))
        {
            string JsonS = File.ReadAllText(SettingFilePath);
            //Debug.Log(JsonS + "\n");
            UserProfile = JsonConvert.DeserializeObject<UserProfile>(JsonS);
        }
        //������д��Ĭ��ֵ
        else
        {
            if(!Directory.Exists(SettingFolderPath)) Directory.CreateDirectory(SettingFolderPath);
            File.WriteAllText(SettingFilePath,JsonConvert.SerializeObject(UserProfile));
        }
        
    }

    /// <summary>
    /// ��������
    /// </summary>
    public static void SaveProfile()
    {
        File.WriteAllText(SettingFilePath,JsonConvert.SerializeObject(UserProfile,Formatting.Indented));
    }

    #endregion

    #region ������Ϣ����
    /// <summary>
    /// �����������Ϣ
    /// </summary>
    public static List<PartialChart> ChartInfos;
    
    /// <summary>
    /// ��ȡ�ӳ�
    /// </summary>
    public static float LoadDelay = 0f;
    /// <summary>
    /// ����������Ϣ
    /// </summary>
    public static void LoadChartInfo()
    {
        ChartInfos = new List<PartialChart>();
        
        //·����������ôһ��û������
        if (!Directory.Exists(ChartPath)) Directory.CreateDirectory(ChartPath);   
        //ȫ��ɨ��
        else TraverseFolders(ChartPath);
        
        
        //�������¼
        //BindPlayHistory();
    }

    /// <summary>
    /// �������¼
    /// </summary>
    private static void BindPlayHistory()
    {

    }

    /// <summary>
    /// �����ļ���
    /// </summary>
    private static void TraverseFolders(string folderPath)
    {
        string[] subfolders = Directory.GetDirectories(folderPath);

        //�����Ҷ�ļ��У���ʼ����
        if(subfolders.Length == 0)
        {
            if (!File.Exists(folderPath + "/track.mp3")) return;
            if (!File.Exists(folderPath + "/bg.png")) return;
            
            //�����ļ�
            PartialChart chart = ParseFile(folderPath);
            if(chart != null) ChartInfos.Add(chart);
        }
        //����ݹ�
        else
        {
            //Parallel.ForEach( subfolders,item => TraverseFolders(item));
            foreach(string subfolder in subfolders) TraverseFolders(subfolder);
        }
    }

    /// <summary>
    /// ���ļ��н�����PartialChart
    /// </summary>
    /// <returns>һ��������Ϣ��������null</returns>
    private static PartialChart ParseFile(string folderPath)
    {
        PartialChart chart = new PartialChart();
        chart.Difficulty.AddRange(new List<float>{ 0,0,0,0,0,0,0,0});
        
        //����·����Ϣ
        chart.InfoPath = folderPath.Substring(folderPath.IndexOf(ChartPath) + ChartPath.Length);
        
        
        //���ı���Ѱ������
        int CollectProperty = 0;
        bool IfValidDiff = false;

        
        using(StreamReader sr = new StreamReader(folderPath + "/maidata.txt"))
         {
            //��ȡ
            while (!sr.EndOfStream)
            {
                string Line = sr.ReadLine();
                
                //��ʽ��׼
                if(Line.Length == 0) continue;
                if (Line.Contains("inote")) break;
                if (Line[0] != '&') continue;

                //�и��ֵ
                Line = Line.Remove(0, 1);
                string[] Token = Line.Split("=");
                if (Token.Length != 2) continue;

                //�����Ѷ�
                if (Token[0].IndexOf("lv") != -1)
                {
                    if (Token[1].Length == 0) continue;

                    //�������
                    if (Token[1].Last() == '?')
                    {
                        Token[1] = Token[1].Remove(Token[1].Length - 1);
                        chart.IfUtage = true;
                    }

                    //�Է�����
                    try { chart.Difficulty[int.Parse(Token[0].Substring(3)) - 1] = float.Parse(Token[1]); }
                    catch { continue; }
                    
                    IfValidDiff = true;
                    MaxLevel = Mathf.Max(MaxLevel, float.Parse(Token[1]));

                    continue;
                }

                //Token�ȶ�
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
        
        //���������������
        if(!IfValidDiff)return null;
        else return chart;
    }

    #endregion
}
