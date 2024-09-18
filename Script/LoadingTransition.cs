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
        //���ط�����
        LoadingIcon.DORotate(new Vector3(0f, 0f, 360f), 2f, RotateMode.FastBeyond360)
           .SetEase(Ease.Linear) // ����Ϊ���Ի�����ʹ��ת�ٶȾ���
           .SetLoops(-1, LoopType.Restart); // ����Ϊ����ѭ��);

        //Э�̴���
        StartCoroutine(PreLoad());
    }

    /// <summary>
    /// Ԥ����
    /// </summary>
    /// <returns></returns>
    public IEnumerator PreLoad()
    {
        yield return null;

        LoadingText.text = "����������...";
        if (!FileManager.ProfileLoaded) FileManager.LoadProfile();
        if(FileManager.PlayBackground == null) StartCoroutine(FileManager.LoadPlayBackGround());
        //yield return new WaitForSeconds(2f);

        LoadingText.text = "����������...";
        if (!FileManager.ChartLoaded) FileManager.LoadChartInfo();

        //��������
        LoadingIcon.gameObject.SetActive(false);
        LoadingText.gameObject.SetActive(false);

        FileManager.AllLoaded = true;
        SceneManager.LoadScene(1);
        yield break;
    }
}
