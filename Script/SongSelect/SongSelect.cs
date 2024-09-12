using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelect : MonoBehaviour
{
    public List<PartialChart> ChartInfos;
    public GameObject Prefab;
    public Transform Content;
    private AudioSource AudioSource;
    public SimpleScrollSnap SongView;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
