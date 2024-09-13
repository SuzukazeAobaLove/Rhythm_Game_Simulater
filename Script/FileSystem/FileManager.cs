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

    #region �ļ�״̬
    public static bool ProfileLoaded = false;
    public static bool ChartLoaded = false;
    #endregion
    #region �ⲿ��ȡ����
    /// <summary>
    /// ���ⲿ��ȡpng�ļ�
    /// </summary>
    /// <param name="path">·��</param>
    /// <param name="holder">������</param>
    /// <returns></returns>
    public static IEnumerator ReadOutPNG(string path,Image holder)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(path))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) UnityEngine.Debug.LogError("Error loading audio: " + www.error);

            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            // ���� Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            //����Sprite
            holder.sprite = sprite;
        }
        
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
        ProfileLoaded = true;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public static void SaveProfile()
    {
        File.WriteAllText(SettingFilePath,JsonConvert.SerializeObject(UserProfile));
    }

    #endregion

    #region ���沿��
    /// <summary>
    /// �����������Ϣ
    /// </summary>
    public static List<PartialChart> ChartInfos;
    /// <summary>
    /// �ӻ����л�ȡ����ʱ�ֵ�
    /// </summary>
    private static Dictionary<string, PartialChart> CacheTemp;
    /// <summary>
    /// ��ǰ��ȫ���ص�����
    /// </summary>
    public static Chart CurLoadChart;
    
    /// <summary>
    /// ����������Ϣ
    /// </summary>
    public static void LoadChartInfo()
    {
        ChartInfos = new List<PartialChart>();
        CacheTemp = new Dictionary<string, PartialChart>();

        //·����������ôһ��û������
        if (!Directory.Exists(ChartPath))
        {
            Directory.CreateDirectory(ChartPath);
        }
        //���û�л����ļ���ֻ��ȫ��ɨ��
        else if (!File.Exists(CachePath))
        {
            TraverseFolders(ChartPath, false);
            SaveCacheFile();
        }
        //������ڣ����ȶ��뻺��
        else
        {
            LoadCacheFile();
            //�ٽ��б���
            TraverseFolders(ChartPath, true);
            SaveCacheFile();
            CacheTemp.Clear();

        }
        
        ChartLoaded = true;
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
    private static void TraverseFolders(string folderPath,bool ifCache)
    {
        string[] subfolders = Directory.GetDirectories(folderPath);

        //�����Ҷ�ļ��У���ʼ����
        if(subfolders.Length == 0)
        {
            //����л���
            if(ifCache)
            {
                string RelativePath = folderPath.Substring(folderPath.IndexOf(ChartPath) + ChartPath.Length);
                //Debug.Log(RelativePath);

                if (!File.Exists(folderPath + "/data.txt")) return;

                //����л����ļ�����д
                if (CacheTemp.ContainsKey(RelativePath))
                {
                    //���һ����ת��
                    if (CacheTemp[RelativePath].LastEditTime == File.GetLastWriteTime(folderPath + "/data.txts").ToString())
                    {
                        ChartInfos.Add(CacheTemp[RelativePath]);
                        CacheTemp.Remove(RelativePath);
                        return;
                    }
                    //�����Ƴ���ѡ���ȡ�ļ�
                    else CacheTemp.Remove(RelativePath);
                }
            }
            
            if (!File.Exists(folderPath + "/music.mp3")) return;
            if (!File.Exists(folderPath + "/cover.png")) return;
            

            //�����ļ�
            PartialChart chart = ParseFile(folderPath);
            if(chart != null) ChartInfos.Add(chart);
            
                    
        }
        //����ݹ�
        else
        {
            foreach(string subfolder in subfolders) TraverseFolders(subfolder,ifCache);
        }
    }

    /// <summary>
    /// ���ļ��н�����PartialChart
    /// </summary>
    /// <returns>һ��������Ϣ��������null</returns>
    private static PartialChart ParseFile(string folderPath)
    {
        PartialChart chart = new PartialChart();

        //�и��ı�
        string[] Elements = Regex.Split(File.ReadAllText(folderPath + "/data.txt"), "\r?\n");

        //����·����Ϣ
        chart.InfoPath = folderPath.Substring(folderPath.IndexOf(ChartPath) + ChartPath.Length);
        chart.LastEditTime = File.GetLastWriteTime(folderPath + "/data.txt").ToString();

        //���ı���Ѱ������
        int CollectProperty = 0;        
        foreach (string Element in Elements)
        {
            if (Element.Length == 0 || Element[0] != '&') continue;

            //��ȡToken
            string[] Tokens = Element.Split('=');
            Tokens[0] = Tokens[0].Substring(1);
            if(Tokens.Length != 2) continue;

            //����ȼ���Ϣ
            if (Tokens[0].IndexOf("lv_")!= -1)
            {
                if (chart.Difficulty == 0f) CollectProperty++;
                //Debug.Log(Tokens[0].Substring(3));
                chart.Difficulty = float.Parse(Tokens[0].Substring(3));
                continue;
            }

            //Note���п�ʼ��־��������Ϣ����
            if (Tokens[0].IndexOf("inote_") != -1) break;

            //Token�ȶ�
            switch (Tokens[0])
            {
                case "title":
                    {
                        if (chart.FormalName == null) CollectProperty++;
                        chart.FormalName = Tokens[1];
                        break;
                    }
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
                case "designer":
                    {
                        if (chart.ChartWriter == null) CollectProperty++;
                        chart.ChartWriter = Tokens[1];
                        break;
                    }
                case "bpm":
                    {
                        if(chart.BPM == 0) CollectProperty++;
                        chart.BPM = int.Parse(Tokens[1]);
                        break;
                    }
                case "collection":
                    {
                        if (chart.Collection == null) CollectProperty++;
                        chart.Collection = Tokens[1];
                        break;
                    }
            }
        }
        //���������������
        if(CollectProperty != 7)return null;
        else return chart;
    }

    /// <summary>
    /// ��ȡ������Ϣ�ļ�
    /// </summary>
    private static void LoadCacheFile()
    {
        string[] Element = Regex.Split(File.ReadAllText(CachePath), "\r?\n");
        //�ŵ���ʱ�ֵ���ȥ
        foreach (var Text in Element)
        {
            PartialChart Chart = PartialChart.ParseText(Text);
            if(Chart != null) CacheTemp[Chart.InfoPath] = Chart;
        }
    }


    /// <summary>
    /// ���滺����Ϣ�ļ�
    /// </summary>
    private static void SaveCacheFile()
    {
        StreamWriter writer = new StreamWriter(CachePath);
        foreach(var chart in ChartInfos) writer.WriteLine(chart.GetWriteText());
        writer.Close();
        
    }

    /// <summary>
    /// ����ĳ�������ȫ����Ϣ
    /// </summary>
    /// <param name="Index"></param>
    public static void LoadFullChart(int Index)
    {

    }

    #endregion
}
