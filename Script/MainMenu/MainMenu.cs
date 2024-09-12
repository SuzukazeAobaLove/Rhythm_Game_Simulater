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
    private SimpleScrollSnap View;
    private TextMeshProUGUI VerSionText;
    private TextMeshProUGUI ModeTitle;
    private TextMeshProUGUI ModeDescription;
    private Button ExitButton;
    private Button StartButton;
    private Button LeftButton;
    private Button RightButton;
    private RectTransform SelfRect;
    private RectTransform LoadingIcon;

    public float Time = 1.5f;
    List<string> Titles = new List<string>();
    List<string> Descriptions = new List<string>();
    void Start()
    {
        
        View = GetComponentInChildren<SimpleScrollSnap>();
        SelfRect = GetComponent<RectTransform>();

        VerSionText = transform.Find("Version").gameObject.GetComponent<TextMeshProUGUI>();
        VerSionText.text = "Version: " + Application.version;

        ExitButton = transform.Find("Exit").gameObject.GetComponent<Button>();
        ExitButton.onClick.AddListener(() => Application.Quit());

        LeftButton = transform.Find("ModeSelect").Find("LeftButton").gameObject.GetComponent<Button>();
        LeftButton.onClick.AddListener(() => UpdateCenter());
        
        RightButton = transform.Find("ModeSelect").Find("RightButton").gameObject.GetComponent<Button>();
        RightButton.onClick.AddListener(() => UpdateCenter());

        StartButton = transform.Find("Enter").gameObject.GetComponent<Button>();
        StartButton.onClick.AddListener(ChooseMode);

        LoadingIcon = transform.Find("Loading").gameObject.GetComponent<RectTransform>();
        LoadingIcon.gameObject.SetActive(false);

        Titles.Add("�������");
        Descriptions.Add("�ڴ��޸���Ϸ���á��������ƫ��");
        Titles.Add("��������");
        Descriptions.Add("���԰�����˳�������Ѿ���������Ŀ\nû��ǿ��˳��Ѫ��Ҫ��");
        Titles.Add("��λ��ս");
        Descriptions.Add("�����趨�õ�˳��������Ŀ\n�����������������ѡ����Ŀ\n������һ����������ͨ����Ӧ��λ");
        Titles.Add("����ͳ��");
        Descriptions.Add("�����������ͳ�ơ�������Ϣ\n�Լ�װ���á�չʾ���");
        
        ModeTitle = transform.Find("ModeTitle").gameObject.GetComponent<TextMeshProUGUI>();
        ModeDescription = transform.Find("ModeDescription").gameObject.GetComponent<TextMeshProUGUI>();
        ModeTitle.text = Titles[1];
        ModeDescription.text = Descriptions[1];
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

        //���ط�����
        LoadingIcon.gameObject.SetActive(true);
        LoadingIcon.DORotate(new Vector3(0f, 0f, 360f), 2f, RotateMode.FastBeyond360)
           .SetEase(Ease.Linear) // ����Ϊ���Ի�����ʹ��ת�ٶȾ���
           .SetLoops(-1, LoopType.Restart); // ����Ϊ����ѭ��);

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
        Mode += 1;
        switch((Scenes)Mode)
        {
            case Scenes.Setting:
                {
                    FileManager.LoadProfile();
                    break;
                }
            case Scenes.NormalPlay:
                {
                    FileManager.LoadChartInfo();
                    break;
                }
        }
        yield return new WaitForSeconds(2f);

        GameSystem.OpenScene(Mode);
    }
}
