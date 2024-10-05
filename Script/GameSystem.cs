using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class GameSystem
{
    //是否以街机模式游玩
    public static bool IfArcade = false;


    private static Stack<int> _SceneOpened = new Stack<int>();
    
    static GameSystem()
    {
        Application.targetFrameRate = 60;
        //打开主界面
        _SceneOpened.Push(1);
    }


    /// <summary>
    /// 打开面板
    /// </summary>
    /// <param name="target"></param>
    public static void OpenScene(int target)
    {
        _SceneOpened.Push(target);
        SceneManager.LoadScene(target);
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    public static void ExitScene()
    {
        _SceneOpened.Pop();
        SceneManager.LoadScene(_SceneOpened.Peek());
    }

}
