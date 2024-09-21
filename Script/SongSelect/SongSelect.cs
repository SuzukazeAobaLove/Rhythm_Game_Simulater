using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using HaseMikan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongSelect: MonoBehaviour
{
    [Header("生成元素")]
    public List<PartialChart> ChartInfos;
    public GameObject Prefab;
    public Transform Content;
    
    private AudioSource AudioSource;

    [Header("滚动视图")]
    public SimpleScrollSnap CategoryView;
    public SimpleScrollSnap SongView;
    [Header("按钮")]
    public Button ReturnButton;
    public Button StartButton;
    public Button LeftSongButton;
    public Button RightSongButton;

    [Header("动画参数")]
    public float TransitTime = 2.5f;
    public CanvasGroup BlackBackground;

    private PartialChart PreSong;
    private bool IfNoChart = false;
    private float TimeSinceSwitch = 0f;

    private void Awake()
    {
        if (!FileManager.AllLoaded) SceneManager.LoadScene(0);
        AudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {   
        CreateSongElement();
        ReturnButton.onClick.AddListener(ReturnButtonClick);
        StartButton.onClick.AddListener(StartPlayButton);
        LeftSongButton.onClick.AddListener(() => TimeSinceSwitch = 0);
        RightSongButton.onClick.AddListener(() => TimeSinceSwitch = 0);
        if(FileManager.ChartInfos.Count > 0) SwitchSong();
    }


    /// <summary>
    /// 创建歌曲元素
    /// </summary>
    private void CreateSongElement()
    {
        //可操作性在这里
        ChartInfos = FileManager.ChartInfos;
        if(ChartInfos.Count == 0 )
        {
            IfNoChart = true;
            return;
        }

        //创建并绑定元素
        foreach (var Chart in ChartInfos)
        {
            var Element = Instantiate(Prefab, Content);
            Element.GetComponent<SongElement>().Bind(Chart);
            Element.GetComponent<SongElement>().UpdateData();
        }

        //数量少直接绑定
        if (ChartInfos.Count < 30)
        {
            
        }
        else
        {
            /*for(int i = 0;i < 30;++i)
            {
                var Element = Instantiate(Prefab, Content);
                Element.GetComponent<SongElement>().Bind(ChartInfos[i]);
                Element.GetComponent<SongElement>().UpdateData();
            }*/
        }
    }

    /// <summary>
    /// 切换当前音频预览
    /// </summary>
    private void SwitchSong()
    {
        PreSong = ChartInfos[SongView.CenteredPanel];
        AudioSource.Stop();
        AudioSource.clip = FileManager.ReadOutMP3(FileManager.ChartPath + PreSong.InfoPath + "/music.mp3");
        AudioSource.Play();
    }

    /// <summary>
    /// 绑定给返回按钮
    /// </summary>
    public void ReturnButtonClick() => GameSystem.ExitScene();

    /// <summary>
    /// 开始游玩按钮
    /// </summary>
    public void StartPlayButton()
    {
        FileManager.LoadChartFile(PreSong);
        
        //找到高亮物体
        GameObject Highlight = SongView.Content.transform.GetChild(SongView.CenteredPanel).gameObject;
        Highlight.transform.parent = this.transform;
        RectTransform HighlightRect = Highlight.GetComponent<RectTransform>();
        //高亮
        HighlightRect.DOScale(new Vector3(1.5f, 1.5f, 1.5f), TransitTime).SetEase(Ease.OutCubic);
        HighlightRect.DOAnchorPos(new Vector2(GetComponent<RectTransform>().rect.width/2, 0), TransitTime).SetEase(Ease.OutCubic);

        //按钮失效
        ReturnButton.enabled = false;
        StartButton.enabled = false;
        SongView.enabled = false;

        //物体渐隐
        ReturnButton.GetComponent<CanvasGroup>().DOFade(0f,TransitTime - 1.0f).SetEase(Ease.OutCubic);
        StartButton.GetComponent<CanvasGroup>().DOFade(0f, TransitTime - 1.0f).SetEase(Ease.OutCubic);
        SongView.GetComponent<CanvasGroup>().DOFade(0f, TransitTime - 1.0f).SetEase(Ease.OutCubic);
        CategoryView.GetComponent<CanvasGroup>().DOFade(0f, TransitTime - 1.0f).SetEase(Ease.OutCubic);

        //打开黑背景
        BlackBackground.DOFade(1f, TransitTime).SetEase(Ease.OutCubic);

        //延时关闭音乐
        LinerClock.InsertDelayAction(1.0f, () => AudioSource.Stop());
        //延时加载
        LinerClock.InsertDelayAction(TransitTime + 3.0f,() => GameSystem.OpenScene((int)Panel.PlaySong));
    }

    void Update()
    {
        if (!IfNoChart)
        {
            if (TimeSinceSwitch <= 0.5f) TimeSinceSwitch += Time.deltaTime;
            else if(PreSong != ChartInfos[SongView.CenteredPanel]) SwitchSong();
        } 
    }
}
