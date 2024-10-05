using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using HaseMikan;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("�������")]
    private SimpleScrollSnap ModeView;
    public TextMeshProUGUI VerSionText;
    public TextMeshProUGUI DelayText;

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
    private List<Scenes> TurnTo = new List<Scenes>();

    private void Awake()
    {
        if (!FileManager.AllLoaded) SceneManager.LoadScene(0);
    }

     void Start()
    {
        
        ModeView = GetComponentInChildren<SimpleScrollSnap>();
        SelfRect = GetComponent<RectTransform>();

        VerSionText.text = "Version: " + Application.version;
        DelayText.text += $"{FileManager.LoadDelay} ms";
        
        StartButton.onClick.AddListener(ChooseMode);
        ExitButton.onClick.AddListener(Application.Quit);
        LeftButton.onClick.AddListener(UpdateCenter);
        RightButton.onClick.AddListener(UpdateCenter);
        
        //�趨ģʽ�ı�
        Titles.Add("�������");
        Descriptions.Add("�ڴ��޸���Ϸ���á��������ƫ��");
        TurnTo.Add(Scenes.Profile);
        
        Titles.Add("��������");
        Descriptions.Add("���԰�����˳�������Ѿ���������Ŀ\nû��ǿ��˳��Ѫ��Ҫ��");
        TurnTo.Add(Scenes.CategorySelect);

        Titles.Add("�ֻ�ģʽ");
        Descriptions.Add("���ýֻ���ģʽ��������\n����ѡ��ʱ������\n�����ۼƽ����������");
        TurnTo.Add(Scenes.CategorySelect);

        Titles.Add("��λ��ս");
        Descriptions.Add("�����趨�õ�˳��������Ŀ\n�����������������ѡ����Ŀ\n������һ����������ͨ����Ӧ��λ");
        TurnTo.Add(Scenes.Ranking);

        Titles.Add("����ͳ��");
        Descriptions.Add("�����������ͳ�ơ�������Ϣ\n�Լ�װ���á�չʾ���");
        TurnTo.Add(Scenes.Statistic);

        ModeTitle.text = Titles[1];
        ModeDescription.text = Descriptions[1];
       
    }

    //ѡ��ģʽ
    public void ChooseMode() => GameSystem.OpenScene((int)TurnTo[ModeView.CenteredPanel]);

    /// <summary>
    /// ����������ʾ
    /// </summary>
    public void UpdateCenter()
    {
        ModeTitle.text = Titles[ModeView.CenteredPanel];
        ModeDescription.text = Descriptions[ModeView.CenteredPanel];
    }

    
}
