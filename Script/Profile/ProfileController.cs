using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ProfileController : MonoBehaviour
{
    public static ProfileController Instance;

    public SimpleScrollSnap ClassView;             //�����ͼ
    public SimpleScrollSnap DetailView;            //ϸ����ͼ
    public TextMeshProUGUI CurValue;               //��ʾ��ǰ�޸�ֵ

    public Button LeftPanel;                       //ѡ��������
    public Button RightPanel;                      //ѡ���Ҳ����
    public Button LeftDetail;                      //ѡ�����ϸ��
    public Button RightDetail;                     //ѡ���Ҳ�ϸ��

    public UserProfile Profile;                     //��������
    private List<int> ClassIndex = new List<int>(); //ÿ������ʼ��ϸ������

    /// <summary>
    /// ��ǰ�޸ĵ�ֵ
    /// </summary>
    private ICycledValue CurEdited;

    private void Awake()
    {
        if (!FileManager.AllLoaded) SceneManager.LoadScene(0);
        Instance = this;
    }

    void Start()
    {
        //��ע����
        int sum = 0;
        ClassIndex.Add(sum);
        foreach (var block in FileManager.UserProfile.ProfileBlocks)
        {
            sum += block.GetDetailNum();
            ClassIndex.Add(sum);
        }

        Profile = FileManager.UserProfile;

        LeftPanel.onClick.AddListener(() => SwitchClass(false));
        RightPanel.onClick.AddListener(() => SwitchClass(true));
        LeftDetail.onClick.AddListener(() => SwitchDetail());
        RightDetail.onClick.AddListener(() => SwitchDetail());

        BindEdit();
    }


    /// <summary>
    /// ���沢�˳�
    /// </summary>
    public void SaveAndExit()
    {
        FileManager.SaveProfile();
        GameSystem.ExitScene();
    }

    /// <summary>
    /// ���ĵ�ǰ�޸�ϸ��
    /// </summary>
    public void SwitchDetail()
    {

        BindEdit();
        for (int i = 0; i < ClassIndex.Count; i++)
        {
            if(DetailView.CenteredPanel >= ClassIndex[i] && DetailView.CenteredPanel < ClassIndex[i+1])
            {
                ClassView.GoToPanel(i);
                break;
            }
        }
    }

    /// <summary>
    /// ��������ʾ
    /// </summary>
    public void SwitchClass(bool left)
    {
        Debug.Log("Turn To Class" + ClassView.CenteredPanel);
        //�޸�ϸ�����
        if (left) DetailView.GoToPanel(ClassIndex[ClassView.CenteredPanel]);
        else DetailView.GoToPanel(ClassIndex[ClassView.CenteredPanel + 1] - 1);
        
        //�������޸�ֵ
        BindEdit();
    }

    /// <summary>
    /// ��Ҫ�༭��ϸ��
    /// </summary>
    public void BindEdit()
    {
        Debug.Log(DetailView.CenteredPanel);
        CurEdited = FileManager.UserProfile.DetailList[DetailView.CenteredPanel];
        CurValue.text = CurEdited.GetValue();
    }

    /// <summary>
    /// �����ұ༭��
    /// </summary>
    public void RightEdit()
    {
        CurEdited.Add();
        CurValue.text = CurEdited.GetValue();
    }

    /// <summary>
    /// ������༭��
    /// </summary>
    public void LeftEdit()
    {
        CurEdited.Sub();
        CurValue.text = CurEdited.GetValue();
    }    
}
