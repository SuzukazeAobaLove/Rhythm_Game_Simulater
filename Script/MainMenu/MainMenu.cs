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
    public SimpleScrollSnap ModeView;
    public TextMeshProUGUI VerSionText;
    
    public TextMeshProUGUI ModeTitle;
    public TextMeshProUGUI ModeDescription;
    public Button ExitButton;
    public Button StartButton;
    public Button LeftButton;
    public Button RightButton;
    private RectTransform SelfRect;

    public float Time = 1.5f;
    private List<string> Titles = new List<string>();
    private List<string> Descriptions = new List<string>();
    private List<Panel> Panels = new List<Panel>();

    private void Awake()
    {
        if (!FileManager.AllLoaded) SceneManager.LoadScene(0);
    }

     void Start()
    {
        
        ModeView = GetComponentInChildren<SimpleScrollSnap>();
        SelfRect = GetComponent<RectTransform>();

        VerSionText.text = "Version: " + Application.version;
        StartButton.onClick.AddListener(ChooseMode);
        ExitButton.onClick.AddListener(Application.Quit);
        LeftButton.onClick.AddListener(UpdateCenter);
        RightButton.onClick.AddListener(UpdateCenter);
        
        //�趨ģʽ�ı�
        Titles.Add("�������");
        Descriptions.Add("�ڴ��޸���Ϸ���á��������ƫ��");
        Panels.Add(Panel.Profile);
        
        Titles.Add("��������");
        Descriptions.Add("���԰�����˳�������Ѿ���������Ŀ\nû��ǿ��˳��Ѫ��Ҫ��");
        Panels.Add(Panel.SongSelect);
        
        Titles.Add("�ֻ�ģʽ");
        Descriptions.Add("���ýֻ���ģʽ��������\n����ѡ��ʱ������\n�����ۼƽ����������");
        Panels.Add(Panel.Arcade);

        Titles.Add("��λ��ս");
        Descriptions.Add("�����趨�õ�˳��������Ŀ\n�����������������ѡ����Ŀ\n������һ����������ͨ����Ӧ��λ");
        Panels.Add(Panel.Ranking);

        Titles.Add("����ͳ��");
        Descriptions.Add("�����������ͳ�ơ�������Ϣ\n�Լ�װ���á�չʾ���");
        Panels.Add(Panel.Statistic);

        ModeTitle.text = Titles[1];
        ModeDescription.text = Descriptions[1];
       
    }

    //ѡ��ģʽ
    public void ChooseMode() => GameSystem.OpenScene((int)Panels[ModeView.CenteredPanel]);
        /*
        
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

        */

    /// <summary>
    /// ����������ʾ
    /// </summary>
    public void UpdateCenter()
    {
        ModeTitle.text = Titles[ModeView.CenteredPanel];
        ModeDescription.text = Descriptions[ModeView.CenteredPanel];
    }

    
}
