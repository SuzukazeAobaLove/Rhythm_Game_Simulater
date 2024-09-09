using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 文件管理器，负责读谱面，段位配置，读写设置，统计数据
/// </summary>
public static class FileManager
{
    private static string ProjectPath = Application.persistentDataPath;
    private static string SettingPath = ProjectPath + "/Setting/";
    private static string StatisticsPath = ProjectPath + "/Statistics/";
    private static string ChartPath = ProjectPath + "/Charts/";
    private static string RankPath = ProjectPath + "/RankLists/";

    public static List<UserProfile> UserProfiles;
    public static void ReadProfile()
    {
        UserProfiles = new List<UserProfile>();
        if (Directory.Exists(SettingPath))
        {

            foreach (string file in Directory.EnumerateFiles(SettingPath, "*", SearchOption.AllDirectories))
            {
                string JsonS = File.ReadAllText(file);
                Debug.Log(JsonS + "\n");
                UserProfiles.Add(JsonConvert.DeserializeObject<UserProfile>(JsonS));
            }
        }
        else
        {
            Directory.CreateDirectory(SettingPath);
        }
    }

}
