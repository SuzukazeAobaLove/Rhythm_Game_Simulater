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
    [Header("滚动视图组件")]
    public SimpleScrollSnap ClassView;             //类别视图
    public SimpleScrollSnap DetailView;            //细则试图
    
    public GameObject ClassPrefab;                 //类别预制件
    public GameObject DetailPrefab;                //细则预制件

    [Header("按钮组件")]
    public Button LeftPanel;                       //选择左侧类别
    public Button RightPanel;                      //选择右侧类别
    public Button LeftDetail;                      //选择左侧细则
    public Button RightDetail;                     //选择右侧细则
    public Button AddButton;                       //加值按钮
    public Button SubButton;                       //减值按钮


    public UserProfile Profile;                     //引用设置
    private TextMeshProUGUI CurValue;               //显示当前修改值
    private List<int> ClassIndex = new List<int>(); //每个类别初始的细则索引
    private List<DetailElement> DetailElements = new List<DetailElement>();
    private float Timer = 0f;

    /// <summary>
    /// 当前修改的值
    /// </summary>
    private ICycledValue CurEdited;

    private void Awake()
    {
        if (!FileManager.AllLoaded) SceneManager.LoadScene(0);
        
        Instance = this;

        //确保不会报错
        if (!FileManager.AllLoaded) return;
        Profile = FileManager.UserProfile;

        //生成Class并计算每个Class的区间
        int sum = 0;
        ClassIndex.Add(sum);
        foreach (var block in Profile.ProfileBlocks)
        {
            var ClassGo = Instantiate(ClassPrefab, ClassView.Content);
            ClassGo.GetComponentInChildren<TextMeshProUGUI>().text = block.GetClassName();
            sum += block.GetDetailNum();
            ClassIndex.Add(sum);
        }

        //生成每一个细则
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
    /// 保存并退出
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
    /// 更改当前修改细则
    /// </summary>
    public void SwitchDetail()
    {
        Timer = 0.5f;
        BindEdit();
        //先关掉两个按钮
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
    /// 更改类显示
    /// </summary>
    public void SwitchClass(bool left)
    {
        Debug.Log("Turn To Class" + ClassView.CenteredPanel);
        //修改细则面板
        if (left) DetailView.GoToPanel(ClassIndex[ClassView.CenteredPanel]);
        else DetailView.GoToPanel(ClassIndex[ClassView.CenteredPanel + 1] - 1);
        
        //绑定最新修改值
        BindEdit();
    }

    /// <summary>
    /// 绑定要编辑的细则
    /// </summary>
    public void BindEdit()
    {
        CurEdited = DetailElements[DetailView.CenteredPanel].BindCVB;
        CurValue = DetailElements[DetailView.CenteredPanel].CurValue;
    }

    /// <summary>
    /// 按下右编辑键
    /// </summary>
    public void RightEdit()
    {
        CurEdited.Add();
        CurValue.text = CurEdited.GetValue();
    }

    /// <summary>
    /// 按下左编辑键
    /// </summary>
    public void LeftEdit()
    {
        CurEdited.Sub();
        CurValue.text = CurEdited.GetValue();
    }    
}
