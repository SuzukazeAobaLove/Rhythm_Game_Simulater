using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayController : MonoBehaviour
{

    public Image Cover;
    public JudgeBoard JudgeBoard;
    public Chart CurPlayChart;
    public RectTransform[] Points;
    void Awake()
    {
        if (!FileManager.AllLoaded) SceneManager.LoadScene(0);
    }
    
    void Start()
    {
        CurPlayChart = FileManager.CurLoadChart;
        Cover.sprite = FileManager.ReadOutPNG(FileManager.ChartPath + FileManager.CurLoadChart.Info_.InfoPath + "/cover.png");
        SetPointPos();
    }


    void Update()
    { 
        
    }

    /// <summary>
    /// …Ë÷√µ„Œª
    /// </summary>
    public void SetPointPos()
    {
        for(int x = 0;x < 8;++x)
        {
            Points[x].localPosition = new Vector3(Mathf.Cos((22.5f + x *  45) * Mathf.Deg2Rad), Mathf.Sin((22.5f + x * 45) * Mathf.Deg2Rad), 0) * 650f;
        }
    }
}
