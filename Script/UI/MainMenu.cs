using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private SimpleScrollSnap View;
    private TextMeshProUGUI VerSionText;
    private TextMeshProUGUI ModeTitle;
    private TextMeshProUGUI ModeDescription;
    private Button ExitButton;
    private int LastPanel = 1;

    List<string> Titles = new List<string>();
    List<string> Descriptions = new List<string>();
    void Start()
    {
        View = GetComponentInChildren<SimpleScrollSnap>();

        VerSionText = transform.Find("Version").gameObject.GetComponent<TextMeshProUGUI>();
        VerSionText.text = "Version: " + PlayerSettings.bundleVersion;

        ExitButton = transform.Find("Exit").gameObject.GetComponent<Button>();
        ExitButton.onClick.AddListener(() => Application.Quit());

        Titles.Add("浏览设置");
        Descriptions.Add("在此修改游戏设置、浏览游玩偏好");
        Titles.Add("常规游玩");
        Descriptions.Add("可以按任意顺序游玩已经解锁的曲目\n没有强制顺序、血量要求");
        Titles.Add("段位挑战");
        Descriptions.Add("按照设定好的顺序游玩曲目\n或者连续游玩随机挑选的曲目\n如果达成一定条件，则通过对应段位");

        
        ModeTitle = transform.Find("ModeTitle").gameObject.GetComponent<TextMeshProUGUI>();
        ModeDescription = transform.Find("ModeDescription").gameObject.GetComponent<TextMeshProUGUI>();
        ModeTitle.text = Titles[1];
        ModeDescription.text = Descriptions[1];
    }

   
    void Update()
    {
        if (LastPanel != View.CenteredPanel)
        {
            LastPanel = View.CenteredPanel;
            ModeTitle.text = Titles[LastPanel];
            ModeDescription.text = Descriptions[LastPanel];
        }
    }
}
