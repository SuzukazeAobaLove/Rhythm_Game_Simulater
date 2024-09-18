using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingTransition : MonoBehaviour
{

    public TextMeshProUGUI LoadingText;
    public RectTransform LoadingIcon;

    void Start()
    {
        //加载符号闪
        LoadingIcon.DORotate(new Vector3(0f, 0f, 360f), 2f, RotateMode.FastBeyond360)
           .SetEase(Ease.Linear) // 设置为线性缓动，使旋转速度均匀
           .SetLoops(-1, LoopType.Restart); // 设置为无限循环);

        //协程处理
        StartCoroutine(PreLoad());
    }

    /// <summary>
    /// 预加载
    /// </summary>
    /// <returns></returns>
    public IEnumerator PreLoad()
    {
        yield return null;

        LoadingText.text = "加载设置中...";
        if (!FileManager.ProfileLoaded) FileManager.LoadProfile();
        if(FileManager.PlayBackground == null) StartCoroutine(FileManager.LoadPlayBackGround());
        //yield return new WaitForSeconds(2f);

        LoadingText.text = "加载谱面中...";
        if (!FileManager.ChartLoaded) FileManager.LoadChartInfo();

        //结束设置
        LoadingIcon.gameObject.SetActive(false);
        LoadingText.gameObject.SetActive(false);

        FileManager.AllLoaded = true;
        SceneManager.LoadScene(1);
        yield break;
    }
}
