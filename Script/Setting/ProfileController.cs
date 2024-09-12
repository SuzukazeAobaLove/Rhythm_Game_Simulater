using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ProfileController : MonoBehaviour
{
    public static ProfileController Instance;

    private SimpleScrollSnap ClassView;             //类别视图
    private SimpleScrollSnap DetailView;            //细则试图
    private TextMeshProUGUI CurValue;               //显示当前修改值

    private Button LeftPanel;                       //选择左侧类别
    private Button RightPanel;                      //选择右侧类别
    private Button LeftDetail;                      //选择左侧细则
    private Button RightDetail;                     //选择右侧细则

    public UserProfile Profile;                     //引用设置
    private List<int> ClassIndex = new List<int>(); //每个类别初始的细则索引

    /// <summary>
    /// 当前修改的值
    /// </summary>
    private ICycledValue CurEdited;

    private void Awake()
    {
        Instance = this;
        
        //调试模式下确保读取设置
        if (GameSystem.DebugTry()) FileManager.LoadProfile();
        
        //标注索引
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
    /// 保存并退出
    /// </summary>
    public void SaveAndExit()
    {
        FileManager.SaveProfile();
        GameSystem.ExitScene();
    }

    /// <summary>
    /// 更改当前修改细则
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
        Debug.Log(DetailView.CenteredPanel);
        CurEdited = FileManager.UserProfile.DetailList[DetailView.CenteredPanel];
        CurValue.text = CurEdited.GetValue();
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
