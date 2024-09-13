using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelect:MonoBehaviour
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

        //����ģʽ��ȷ����ȡ����
        if (GameSystem.DebugTry()) FileManager.LoadChartInfo();

        
        //��������Ԫ��
        foreach(var Chart in FileManager.ChartInfos)
        {
            var Element = Instantiate(Prefab, Content);
            Element.GetComponent<SongElement>().Bind(Chart);
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
    /// �л���ǰ��ƵԤ��
    /// </summary>
    private void SwitchSong()
    {
        AudioSource.Stop();
        AudioSource.clip = FileManager.ReadOutMP3(FileManager.ChartPath + FileManager.ChartInfos[SongView.CenteredPanel].InfoPath + "/music.mp3");
        AudioSource.Play();
    }

    /// <summary>
    /// �󶨸����ذ�ť
    /// </summary>
    public void ReturnButton() => GameSystem.ExitScene();


    // Update is called once per frame
    void Update()
    {
        
    }
}
