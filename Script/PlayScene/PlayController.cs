using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HaseMikan;

public class PlayController : MonoBehaviour
{
    public static PlayController Instance;

    public GameObject TapPrefab;
    public RectTransform NoteCollection;
    public float RealCircleSize;
    public Image Cover;
    public JudgeBoard JudgeBoard;
    public Chart CurPlayChart;
    public RectTransform[] Points;
    private AudioSource Player;

    private double GameStep = 0;
    private double TargetPlayTime;
    private bool IfStarted = false;
    public float Speed = 0.5f;
    private double PreRender = 0.5f;

    void Awake()
    {
        Instance = this;
        if (!FileManager.AllLoaded) SceneManager.LoadScene(0);
        Player = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        CurPlayChart = FileManager.CurLoadChart;
        
        //��ȡ������Ϣ    
        Player.clip = CurPlayChart.Music_;
        Cover.sprite = FileManager.ReadOutPNG(FileManager.ChartPath + FileManager.CurLoadChart.Info_.InfoPath + "/cover.png");
        
        //����UI
        SetPointPos();
        RealCircleSize = (Cover.rectTransform.rect.width + Cover.rectTransform.rect.height) / 2;

        //�趨Ԥ�ƿ�ʼ����ʱ��
        TargetPlayTime = AudioSettings.dspTime + 2;
        Player.PlayScheduled(TargetPlayTime);
    }


    void FixedUpdate()
    {
        
        if(IfStarted) GameStepOut();
        else if(AudioSettings.dspTime > TargetPlayTime)
        {
            IfStarted = true;
            GameStep += AudioSettings.dspTime - TargetPlayTime;
        }
    }

    /// <summary>
    /// ��ʼ����
    /// </summary>
    public void GameSetUp()
    {
        

    }

    /// <summary>
    /// ��Ϸ���Ȳ���
    /// </summary>
    public void GameStepOut()
    {
        GameStep += Time.fixedDeltaTime;
        for (int i = 0; i < CurPlayChart.Notes_.Count; i++)
        {
            if (CurPlayChart.Notes_[i].ExactTime_ - GameStep > Speed / 18) break;
            else
            {
                var Note = CurPlayChart.Notes_[i];
                //�Ƴ�
                CurPlayChart.Notes_.RemoveAt(i);

                if(Note.Type_ == DetailNoteType.Tap)
                {
                    var Go = Instantiate(TapPrefab, NoteCollection);
                    Go.GetComponent<TapUI>().Restart(Note,(float)GameStep);
                }
            }
        }
    }

    /// <summary>
    /// ���õ�λ
    /// </summary>
    public void SetPointPos()
    {
        for(int x = 0;x < 8;++x)
        {
            Points[x].localPosition = new Vector3(Mathf.Cos((22.5f + x *  45) * Mathf.Deg2Rad), Mathf.Sin((22.5f + x * 45) * Mathf.Deg2Rad), 0) * 690f;
        }
    }
}
