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

        Titles.Add("�������");
        Descriptions.Add("�ڴ��޸���Ϸ���á��������ƫ��");
        Titles.Add("��������");
        Descriptions.Add("���԰�����˳�������Ѿ���������Ŀ\nû��ǿ��˳��Ѫ��Ҫ��");
        Titles.Add("��λ��ս");
        Descriptions.Add("�����趨�õ�˳��������Ŀ\n�����������������ѡ����Ŀ\n������һ����������ͨ����Ӧ��λ");

        
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
