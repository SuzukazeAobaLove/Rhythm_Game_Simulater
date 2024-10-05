using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ProfileController : MonoBehaviour
{
    public static ProfileController Instance;
    [Header("������ͼ���")]
    public SimpleScrollSnap ClassView;             //�����ͼ
    public SimpleScrollSnap DetailView;            //ϸ����ͼ
    
    public GameObject ClassPrefab;                 //���Ԥ�Ƽ�
    public GameObject DetailPrefab;                //ϸ��Ԥ�Ƽ�

    [Header("��ť���")]
    public Button LeftPanel;                       //ѡ��������
    public Button RightPanel;                      //ѡ���Ҳ����
    public Button LeftDetail;                      //ѡ�����ϸ��
    public Button RightDetail;                     //ѡ���Ҳ�ϸ��
    public Button AddButton;                       //��ֵ��ť
    public Button SubButton;                       //��ֵ��ť


    public UserProfile Profile;                     //��������
    private TextMeshProUGUI CurValue;               //��ʾ��ǰ�޸�ֵ
    private List<int> ClassIndex = new List<int>(); //ÿ������ʼ��ϸ������
    private List<DetailElement> DetailElements = new List<DetailElement>();
    private float Timer = 0f;

    /// <summary>
    /// ��ǰ�޸ĵ�ֵ
    /// </summary>
    private ICycledValue CurEdited;

    private void Awake()
    {
        if (!FileManager.AllLoaded) SceneManager.LoadScene(0);
        
        Instance = this;

        //ȷ�����ᱨ��
        if (!FileManager.AllLoaded) return;
        Profile = FileManager.UserProfile;

        //����Class������ÿ��Class������
        int sum = 0;
        ClassIndex.Add(sum);
        foreach (var block in Profile.ProfileBlocks)
        {
            var ClassGo = Instantiate(ClassPrefab, ClassView.Content);
            ClassGo.GetComponentInChildren<TextMeshProUGUI>().text = block.GetClassName();
            sum += block.GetDetailNum();
            ClassIndex.Add(sum);
        }

        //����ÿһ��ϸ��
        foreach (var detail in Profile.DetailList)
        {
            var DetailGo = Instantiate(DetailPrefab,DetailView.Content).GetComponent<DetailElement>();
            DetailGo.Description.text = detail.GetDescription();
            DetailGo.CurValue.text = detail.GetValue();
            DetailGo.BindCVB = detail;
            DetailElements.Add(DetailGo);
        }
    }

    void Start()
    {
        LeftPanel.onClick.AddListener(() => SwitchClass(false));
        RightPanel.onClick.AddListener(() => SwitchClass(true));
        LeftDetail.onClick.AddListener(SwitchDetail);
        RightDetail.onClick.AddListener(SwitchDetail);

        BindEdit();
    }


    /// <summary>
    /// ���沢�˳�
    /// </summary>
    public void SaveAndExit()
    {
        FileManager.SaveProfile();
        FileManager.DivideCharts();
        GameSystem.ExitScene();
    }

    public void Update()
    {
        if(Timer > 0)
        {
            Timer -= Time.deltaTime;
            if(Timer < 0)
            {
                Timer = 0;
                AddButton.gameObject.SetActive(true);
                SubButton.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// ���ĵ�ǰ�޸�ϸ��
    /// </summary>
    public void SwitchDetail()
    {
        Timer = 0.5f;
        BindEdit();
        //�ȹص�������ť
        AddButton.gameObject.SetActive(false);
        SubButton.gameObject.SetActive(false);

                
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
        CurEdited = DetailElements[DetailView.CenteredPanel].BindCVB;
        CurValue = DetailElements[DetailView.CenteredPanel].CurValue;
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
