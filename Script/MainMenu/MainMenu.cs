using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum Scenes { MainMenu,Setting,NormalPlay,Ranking};
public class MainMenu : MonoBehaviour
{
    [Header("�������")]
    public SimpleScrollSnap View;
    public TextMeshProUGUI VerSionText;
    public TextMeshProUGUI LoadingText;
    public TextMeshProUGUI ModeTitle;
    public TextMeshProUGUI ModeDescription;
    public Button ExitButton;
    public Button StartButton;
    public Button LeftButton;
    public Button RightButton;
    private RectTransform SelfRect;
    public RectTransform LoadingIcon;

    public float Time = 1.5f;
    private List<string> Titles = new List<string>();
    private List<string> Descriptions = new List<string>();

    void Start()
    {
        
        View = GetComponentInChildren<SimpleScrollSnap>();
        SelfRect = GetComponent<RectTransform>();

        VerSionText.text = "Version: " + Application.version;
        StartButton.onClick.AddListener(ChooseMode);
        ExitButton.onClick.AddListener(Application.Quit);
        LeftButton.onClick.AddListener(UpdateCenter);
        RightButton.onClick.AddListener(UpdateCenter);
        
        //�趨ģʽ�ı�
        Titles.Add("�������");
        Descriptions.Add("�ڴ��޸���Ϸ���á��������ƫ��");
        Titles.Add("��������");
        Descriptions.Add("���԰�����˳�������Ѿ���������Ŀ\nû��ǿ��˳��Ѫ��Ҫ��");
        Titles.Add("��λ��ս");
        Descriptions.Add("�����趨�õ�˳��������Ŀ\n�����������������ѡ����Ŀ\n������һ����������ͨ����Ӧ��λ");
        Titles.Add("����ͳ��");
        Descriptions.Add("�����������ͳ�ơ�������Ϣ\n�Լ�װ���á�չʾ���");
        
        ModeTitle.text = Titles[1];
        ModeDescription.text = Descriptions[1];

        
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
        if(!FileManager.ProfileLoaded) FileManager.LoadProfile();
        //yield return new WaitForSeconds(2f);

        LoadingText.text = "����������...";
        if(!FileManager.ChartLoaded) FileManager.LoadChartInfo();

        //��������
        LoadingIcon.gameObject.SetActive(false);
        LoadingText.gameObject.SetActive(false);
        yield break;
    }

    //ѡ��ģʽ
    public void ChooseMode()
    {
        int Selected = View.CenteredPanel;
        //�ҵ���������
        GameObject Highlight = View.Content.transform.GetChild(Selected).gameObject;
        Highlight.transform.parent = this.transform;
        RectTransform HighlightRect = Highlight.GetComponent<RectTransform>();
        //����
        Highlight.transform.DOScale(new Vector3(2f, 2f, 2f), Time).SetEase(Ease.OutCubic);
        HighlightRect.DOAnchorPos(new Vector2(SelfRect.rect.width/2, 0), Time).SetEase(Ease.OutCubic);

        //��ťʧЧ
        ExitButton.enabled = false;
        StartButton.enabled = false;
        View.enabled = false;

        //���彥��
        ExitButton.GetComponent<CanvasGroup>().DOFade(0f,Time).SetEase(Ease.OutCubic);
        StartButton.GetComponent<CanvasGroup>().DOFade(0f, Time).SetEase(Ease.OutCubic);
        View.GetComponent<CanvasGroup>().DOFade(0f, Time).SetEase(Ease.OutCubic);
        ModeDescription.GetComponent<CanvasGroup>().DOFade(0f, Time).SetEase(Ease.OutCubic);
        ModeTitle.GetComponent<CanvasGroup>().DOFade(0f, Time).SetEase(Ease.OutCubic);
        VerSionText.GetComponent<CanvasGroup>().DOFade(0f, Time).SetEase(Ease.OutCubic);

        

        //��ȡ�ļ���ת������
        StartCoroutine("LoadFileAsync", Selected);
    }

    /// <summary>
    /// ����������ʾ
    /// </summary>
    public void UpdateCenter()
    {
        ModeTitle.text = Titles[View.CenteredPanel];
        ModeDescription.text = Descriptions[View.CenteredPanel];
    }

    //�����ļ���ȡϵͳ
    IEnumerator LoadFileAsync(int Mode)
    {
        yield return new WaitForSeconds(2f);

        GameSystem.OpenScene(Mode + 1);
    }
}
