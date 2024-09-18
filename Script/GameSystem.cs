using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Panel { MainMenu,Profile,SongSelect,Ranking,Statistic};
public static class GameSystem
{
    private static Stack<Panel> _PanelOpened = new Stack<Panel>();
    private static Dictionary<Panel,BasePanel> _Panels = new Dictionary<Panel,BasePanel>();

    static GameSystem()
    {
        //��������
        _PanelOpened.Push(Panel.MainMenu);
    }

    /// <summary>
    /// ���ֵ��м���
    /// </summary>
    /// <param name="Type"></param>
    /// <param name="panel"></param>
    public static void RegisterPanel(Panel Type,BasePanel panel) => _Panels[Type] = panel;
    
    /// <summary>
    /// ���ֵ��м���
    /// </summary>
    /// <param name="Type"></param>
    /// <param name="panel"></param>
    public static void UnRegisterPanel(Panel Type) => _Panels[Type] = null;

    /// <summary>
    /// �Ƿ��ǵ�ǰ���
    /// </summary>
    /// <param name="Type"></param>
    /// <returns></returns>
    public static bool IsCurPanel(Panel Type) => _PanelOpened.Peek() == Type;

    /// <summary>
    /// �����
    /// </summary>
    /// <param name="target"></param>
    public static void OpenPanel(Panel target)
    {
        _Panels[_PanelOpened.Peek()].gameObject.SetActive(false);
        _PanelOpened.Push(target);
        _Panels[target].OnSceneOpened();
        _Panels[_PanelOpened.Peek()].gameObject.SetActive(true);
    }

    /// <summary>
    /// �ر����
    /// </summary>
    public static void ClosePanel()
    {
        _Panels[_PanelOpened.Peek()].gameObject.SetActive(false);
        _PanelOpened.Pop();
        _Panels[_PanelOpened.Peek()].gameObject.SetActive(true);
    }

    //public static bool DebugTry() => _PanelOpened.Count == 1;
}
