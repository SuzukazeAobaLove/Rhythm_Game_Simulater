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
    private int LastPanel = 1;
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
        VerSionText.text = "Version: " + PlayerSettings.bundleVersion;

        ExitButton = transform.Find("Exit").gameObject.GetComponent<Button>();
        ExitButton.onClick.AddListener(() => Application.Quit());

        StartButton = transform.Find("Enter").gameObject.GetComponent<Button>();
        StartButton.onClick.AddListener(ChooseMode);

        LoadingIcon = transform.Find("Loading").gameObject.GetComponent<RectTransform>();
        LoadingIcon.gameObject.SetActive(false);

        Titles.Add("浏览设置");
        Descriptions.Add("在此修改游戏设置、浏览游玩偏好");
        Titles.Add("常规游玩");
        Descriptions.Add("可以按任意顺序游玩已经解锁的曲目\n没有强制顺序、血量要求");
        Titles.Add("段位挑战");
        Descriptions.Add("按照设定好的顺序游玩曲目\n或者连续游玩随机挑选的曲目\n如果达成一定条件，则通过对应段位");
        Titles.Add("数据统计");
        Descriptions.Add("浏览游玩数据统计、个人信息\n以及装扮获得、展示情况");
        
        ModeTitle = transform.Find("ModeTitle").gameObject.GetComponent<TextMeshProUGUI>();
        ModeDescription = transform.Find("ModeDescription").gameObject.GetComponent<TextMeshProUGUI>();
        ModeTitle.text = Titles[1];
        ModeDescription.text = Descriptions[1];
    }

    //选择模式
    public void ChooseMode()
    {
        int Selected = View.CenteredPanel;
        //找到高亮物体
        GameObject Highlight = View.Content.transform.GetChild(Selected).gameObject;
        Highlight.transform.parent = this.transform;
        RectTransform HighlightRect = Highlight.GetComponent<RectTransform>();
        //高亮
        Highlight.transform.DOScale(new Vector3(2f, 2f, 2f), Time).SetEase(Ease.OutCubic);
        HighlightRect.DOAnchorPos(new Vector2(SelfRect.rect.width/2, 0), Time).SetEase(Ease.OutCubic);

        //按钮失效
        ExitButton.enabled = false;
        StartButton.enabled = false;
        View.enabled = false;

        //物体渐隐
        ExitButton.GetComponent<CanvasGroup>().DOFade(0f,Time).SetEase(Ease.OutCubic);
        StartButton.GetComponent<CanvasGroup>().DOFade(0f, Time).SetEase(Ease.OutCubic);
        View.GetComponent<CanvasGroup>().DOFade(0f, Time).SetEase(Ease.OutCubic);
        ModeDescription.GetComponent<CanvasGroup>().DOFade(0f, Time).SetEase(Ease.OutCubic);
        ModeTitle.GetComponent<CanvasGroup>().DOFade(0f, Time).SetEase(Ease.OutCubic);
        VerSionText.GetComponent<CanvasGroup>().DOFade(0f, Time).SetEase(Ease.OutCubic);

        //加载符号闪
        LoadingIcon.gameObject.SetActive(true);
        LoadingIcon.DORotate(new Vector3(0f, 0f, 360f), 2f, RotateMode.FastBeyond360)
           .SetEase(Ease.Linear) // 设置为线性缓动，使旋转速度均匀
           .SetLoops(-1, LoopType.Restart); // 设置为无限循环);

        //读取文件后转换场景
        StartCoroutine("LoadFileAsync", Selected);
    }


    void Update()
    {
        if (LastPanel != View.CenteredPanel)
        {
            LastPanel = View.CenteredPanel;
            ModeTitle.text = Titles[LastPanel];
            ModeDescription.text = Descriptions[LastPanel];
        }
    }

    //调用文件读取系统
    IEnumerator LoadFileAsync(int Mode)
    {

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(Mode + 1);
    }
}
