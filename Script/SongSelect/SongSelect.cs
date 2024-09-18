using DanielLochner.Assets.SimpleScrollSnap;
using HaseMikan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongSelect: BasePanel
{
    public List<PartialChart> ChartInfos;
    public GameObject Prefab;
    public Transform Content;

    private AudioSource AudioSource;
    public SimpleScrollSnap SongView;

    public Button StartButton;
    public Button LeftSongButton;
    public Button RightSongButton;

    private PartialChart PreSong;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {   
        CreateSongElement(); 
        
        Type = Panel.SongSelect;
        
        base.Start();
    }

    public override void OnSceneOpened()
    {
        SwitchSong();
        PreSong = FileManager.ChartInfos[SongView.CenteredPanel];
    }

    /// <summary>
    /// ��������Ԫ��
    /// </summary>
    private void CreateSongElement()
    {
        //�ɲ�����������
        ChartInfos = FileManager.ChartInfos;
        
        //��������Ԫ��
            foreach (var Chart in FileManager.ChartInfos)
            {
                var Element = Instantiate(Prefab, Content);
                Element.GetComponent<SongElement>().Bind(Chart);
                Element.GetComponent<SongElement>().UpdateData();
            }

        //������ֱ�Ӱ�
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
    /// �л���ǰ��ƵԤ��
    /// </summary>
    private void SwitchSong()
    {
        PreSong = FileManager.ChartInfos[SongView.CenteredPanel];
        AudioSource.Stop();
        AudioSource.clip = FileManager.ReadOutMP3(FileManager.ChartPath + PreSong.InfoPath + "/music.mp3");
        AudioSource.Play();
    }

    /// <summary>
    /// �󶨸����ذ�ť
    /// </summary>
    public void ReturnButton() => GameSystem.ClosePanel();

    /// <summary>
    /// ��ʼ���水ť
    /// </summary>
    public void StartPlayButton()
    {
        SceneManager.LoadScene(2);
    }

    // Update is called once per frame
    void Update()
    {
        if(PreSong != FileManager.ChartInfos[SongView.CenteredPanel]) SwitchSong();
        
    }
}
