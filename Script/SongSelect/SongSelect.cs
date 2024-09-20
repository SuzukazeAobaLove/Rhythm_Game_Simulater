using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongSelect: MonoBehaviour
{
    [Header("����Ԫ��")]
    public List<PartialChart> ChartInfos;
    public GameObject Prefab;
    public Transform Content;
    
    private AudioSource AudioSource;
    public SimpleScrollSnap SongView;
    [Header("��ť")]
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
    /// ��������Ԫ��
    /// </summary>
    private void CreateSongElement()
    {
        //�ɲ�����������
        ChartInfos = FileManager.ChartInfos;
        if(ChartInfos.Count == 0 )
        {
            IfNoChart = true;
            return;
        }

        //��������Ԫ��
        foreach (var Chart in ChartInfos)
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
        PreSong = ChartInfos[SongView.CenteredPanel];
        AudioSource.Stop();
        AudioSource.clip = FileManager.ReadOutMP3(FileManager.ChartPath + PreSong.InfoPath + "/music.mp3");
        AudioSource.Play();
    }

    /// <summary>
    /// �󶨸����ذ�ť
    /// </summary>
    public void ReturnButton() => GameSystem.ExitScene();

    /// <summary>
    /// ��ʼ���水ť
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
