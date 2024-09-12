using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class SongSelect : MonoBehaviour
{
    public List<PartialChart> ChartInfos;
    public GameObject Prefab;
    public Transform Content;
    private AudioSource AudioSource;
    public SimpleScrollSnap SongView;
    public Button StartButton;
    public Button LeftSongButton;
    public Button RightSongButton;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();

        //调试模式下确保读取设置
        if (GameSystem.DebugTry()) FileManager.LoadChartInfo();

        //创建并绑定元素
        foreach(var Chart in FileManager.ChartInfos)
        {
            var Element = Instantiate(Prefab, Content);
            Element.GetComponent<SongControl>().Bind(Chart);
        }

        ChartInfos = FileManager.ChartInfos;
    }
    void Start()
    {
        LeftSongButton.onClick.AddListener(SwitchSong);
        RightSongButton.onClick.AddListener(SwitchSong);

        SwitchSong();
    }

    /// <summary>
    /// 切换当前音频预览
    /// </summary>
    private void SwitchSong()
    {
        AudioSource.Stop();
        StartCoroutine(FileManager.ReadOutMP3(FileManager.ChartPath + FileManager.ChartInfos[SongView.CenteredPanel].InfoPath + "/music.mp3", AudioSource));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
