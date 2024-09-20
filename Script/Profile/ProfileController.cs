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

    public SimpleScrollSnap ClassView;             //类别视图
    public SimpleScrollSnap DetailView;            //细则试图
    public TextMeshProUGUI CurValue;               //显示当前修改值

    public Button LeftPanel;                       //选择左侧类别
    public Button RightPanel;                      //选择右侧类别
    public Button LeftDetail;                      //选择左侧细则
    public Button RightDetail;                     //选择右侧细则

    public UserProfile Profile;                     //引用设置
    private List<int> ClassIndex = new List<int>(); //每个类别初始的细则索引

    /// <summary>
    /// 当前修改的值
    /// </summary>
    private ICycledValue CurEdited;

    private void Awake()
    {
        if (!FileManager.AllLoaded) SceneManager.LoadScene(0);
        Instance = this;
    }

    void Start()
    {
        //标注索引
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
