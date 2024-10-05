using DanielLochner.Assets.SimpleScrollSnap;
using HaseMikan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CategorySelectPanel : MonoBehaviour
{
    private SimpleScrollSnap _CategoryView;
    public GameObject _CategoryPrefab;


    private void Awake()
    {
        if (!FileManager.AllLoaded) SceneManager.LoadScene(0);
    }

    void Start()
    {
        _CategoryView = GetComponentInChildren<SimpleScrollSnap>();

        ArrangeCategory();
        
    }

    /// <summary>
    /// 初始化类别
    /// </summary>
    private void ArrangeCategory()
    {
        var CateResult = FileManager.CategoryResult;

        if (FileManager.CategoryType == CategoryType.Level)
        {
            for (int i = 1; i < (int)FileManager.MaxLevel + 1; ++i)
            {
                if (CateResult.ContainsKey(i.ToString()))
                {
                    var CateGo = Instantiate(_CategoryPrefab, _CategoryView.Content).GetComponent<CategoryElement>();
                    CateGo.NameText = i.ToString();
                    CateGo.Name.text = i.ToString();
                    CateGo.Count.text += CateResult[i.ToString()].Count;
                }
                if (i > 6 && CateResult.ContainsKey(i.ToString() + "+"))
                {
                    var CateGo = Instantiate(_CategoryPrefab, _CategoryView.Content).GetComponent<CategoryElement>();
                    CateGo.NameText = i.ToString() + "+";
                    CateGo.Name.text = i.ToString() + "+";
                    CateGo.Count.text += CateResult[i.ToString() + "+"].Count;
                }
            }
            
        }
        else
        {
            //根据键名生成
            foreach (var keys in FileManager.CategoryResult)
            {
                var CateGo = Instantiate(_CategoryPrefab, _CategoryView.Content).GetComponent<CategoryElement>();
                CateGo.NameText = keys.Key;
                CateGo.Name.text = keys.Key;
                CateGo.Count.text += keys.Value.Count;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 按下离开按钮
    /// </summary>
    public void OnExitButtonClick()
    {
        
        GameSystem.ExitScene();
    }

    /// <summary>
    /// 按下继续按钮
    /// </summary>
    public void OnNextButtonClick()
    {
        FileManager.SelectCategory = _CategoryView.Content.transform.GetChild(_CategoryView.CenteredPanel)
            .GetComponent<CategoryElement>().NameText;
        GameSystem.OpenScene((int)Scenes.SongSelect);
    }
    
}
