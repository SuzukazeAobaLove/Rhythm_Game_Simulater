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
    [Header("组件引用")]
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
        
        //设定模式文本
        Titles.Add("浏览设置");
        Descriptions.Add("在此修改游戏设置、浏览游玩偏好");
        TurnTo.Add(Scenes.Profile);
        
        Titles.Add("自由游玩");
        Descriptions.Add("可以按任意顺序游玩已经解锁的曲目\n没有强制顺序、血量要求");
        TurnTo.Add(Scenes.CategorySelect);

        Titles.Add("街机模式");
        Descriptions.Add("采用街机的模式进行游玩\n具有选择时间限制\n可以累计奖励区域里程");
        TurnTo.Add(Scenes.CategorySelect);

        Titles.Add("段位挑战");
        Descriptions.Add("按照设定好的顺序游玩曲目\n或者连续游玩随机挑选的曲目\n如果达成一定条件，则通过对应段位");
        TurnTo.Add(Scenes.Ranking);

        Titles.Add("数据统计");
        Descriptions.Add("浏览游玩数据统计、个人信息\n以及装扮获得、展示情况");
        TurnTo.Add(Scenes.Statistic);

        ModeTitle.text = Titles[1];
        ModeDescription.text = Descriptions[1];
       
    }

    //选择模式
    public void ChooseMode() => GameSystem.OpenScene((int)TurnTo[ModeView.CenteredPanel]);

    /// <summary>
    /// 更新中心显示
    /// </summary>
    public void UpdateCenter()
    {
        ModeTitle.text = Titles[ModeView.CenteredPanel];
        ModeDescription.text = Descriptions[ModeView.CenteredPanel];
    }

    
}
