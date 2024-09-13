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
    [Header("组件引用")]
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
        
        //设定模式文本
        Titles.Add("浏览设置");
        Descriptions.Add("在此修改游戏设置、浏览游玩偏好");
        Titles.Add("常规游玩");
        Descriptions.Add("可以按任意顺序游玩已经解锁的曲目\n没有强制顺序、血量要求");
        Titles.Add("段位挑战");
        Descriptions.Add("按照设定好的顺序游玩曲目\n或者连续游玩随机挑选的曲目\n如果达成一定条件，则通过对应段位");
        Titles.Add("数据统计");
        Descriptions.Add("浏览游玩数据统计、个人信息\n以及装扮获得、展示情况");
        
        ModeTitle.text = Titles[1];
        ModeDescription.text = Descriptions[1];

        
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
        if(!FileManager.ProfileLoaded) FileManager.LoadProfile();
        //yield return new WaitForSeconds(2f);

        LoadingText.text = "加载谱面中...";
        if(!FileManager.ChartLoaded) FileManager.LoadChartInfo();

        //结束设置
        LoadingIcon.gameObject.SetActive(false);
        LoadingText.gameObject.SetActive(false);
        yield break;
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

        

        //读取文件后转换场景
        StartCoroutine("LoadFileAsync", Selected);
    }

    /// <summary>
    /// 更新中心显示
    /// </summary>
    public void UpdateCenter()
    {
        ModeTitle.text = Titles[View.CenteredPanel];
        ModeDescription.text = Descriptions[View.CenteredPanel];
    }

    //调用文件读取系统
    IEnumerator LoadFileAsync(int Mode)
    {
        yield return new WaitForSeconds(2f);

        GameSystem.OpenScene(Mode + 1);
    }
}
