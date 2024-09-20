using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingTransition : MonoBehaviour
{

   

    void Start()
    {
        StartCoroutine(PreLoad());
    }

    /// <summary>
    /// ‘§º”‘ÿ
    /// </summary>
    /// <returns></returns>
    public IEnumerator PreLoad()
    {
        yield return null;

        if (!FileManager.ProfileLoaded) FileManager.LoadProfile();

        if (!FileManager.ChartLoaded) FileManager.LoadChartInfo();

        FileManager.AllLoaded = true;

        SceneManager.LoadScene(1);
        yield break;
    }
}
