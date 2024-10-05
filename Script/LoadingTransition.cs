using DG.Tweening;
using HaseMikan;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingTransition : MonoBehaviour
{

   

    void Start()
    {
        PreLoad();
    }

    /// <summary>
    /// ‘§º”‘ÿ
    /// </summary>
    /// <returns></returns>
    public void PreLoad()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        FileManager.LoadChartInfo();
        
        FileManager.LoadProfile();

        FileManager.DivideCharts();

        FileManager.AllLoaded = true;

        SceneManager.LoadScene(1);
        
        sw.Stop();
        FileManager.LoadDelay = sw.ElapsedMilliseconds;
    }
}
