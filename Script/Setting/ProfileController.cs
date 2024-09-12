using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ProfileController : MonoBehaviour
{
    public static ProfileController Instance;

    private SimpleScrollSnap ClassView;             //�����ͼ
    private SimpleScrollSnap DetailView;            //ϸ����ͼ
    private TextMeshProUGUI CurValue;               //��ʾ��ǰ�޸�ֵ

    private Button LeftPanel;                       //ѡ��������
    private Button RightPanel;                      //ѡ���Ҳ����
    private Button LeftDetail;                      //ѡ�����ϸ��
    private Button RightDetail;                     //ѡ���Ҳ�ϸ��

    public UserProfile Profile;                     //��������
    private List<int> ClassIndex = new List<int>(); //ÿ������ʼ��ϸ������

    /// <summary>
    /// ��ǰ�޸ĵ�ֵ
    /// </summary>
    private ICycledValue CurEdited;

    private void Awake()
    {
        Instance = this;
        
        //����ģʽ��ȷ����ȡ����
        if (GameSystem.DebugTry()) FileManager.LoadProfile();
        
        //��ע����
        int sum = 0;
        ClassIndex.Add(sum);
        foreach(var block in FileManager.UserProfile.ProfileBlocks)
        {
            sum += block.GetDetailNum();
            ClassIndex.Add(sum);
        }

        
    }

    void Start()
    {
        Profile = FileManager.UserProfile;

        CurValue = transform.Find("CurValue").gameObject.GetComponent<TextMeshProUGUI>();

        ClassView = transform.Find("ClassSelect").gameObject.GetComponent<SimpleScrollSnap>();
        DetailView = transform.Find("DetailSelect").gameObject.GetComponent<SimpleScrollSnap>();

        LeftPanel = ClassView.transform.Find("LeftButton").gameObject.GetComponent<Button>();
        LeftPanel.onClick.AddListener(() => SwitchClass(false));

        RightPanel = ClassView.transform.Find("RightButton").gameObject.GetComponent<Button>();
        RightPanel.onClick.AddListener(() => SwitchClass(true));

        LeftDetail = DetailView.transform.Find("LeftButton").gameObject.GetComponent<Button>();
        LeftDetail.onClick.AddListener(() => SwitchDetail());

        RightDetail = DetailView.transform.Find("RightButton").gameObject.GetComponent<Button>();
        RightDetail.onClick.AddListener(() => SwitchDetail());
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
