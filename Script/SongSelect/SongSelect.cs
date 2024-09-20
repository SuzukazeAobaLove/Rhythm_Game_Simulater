using DanielLochner.Assets.SimpleScrollSnap;
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
    public SimpleScrollSnap SongView;
    [Header("按钮")]
    public Button StartButton;
    public Button LeftSongButton;
    public Button RightSongButton;

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
    public void ReturnButton() => GameSystem.ExitScene();

    /// <summary>
    /// 开始游玩按钮
    /// </summary>
    public void StartPlayButton()
    {
        FileManager.LoadChartFile(PreSong);
        GameSystem.OpenScene((int)Panel.PlaySong);
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
