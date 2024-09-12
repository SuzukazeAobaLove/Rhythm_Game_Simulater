using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSystem
{
    private static Stack<int> _SceneOpened = new Stack<int>();

    static GameSystem()
    {
        _SceneOpened.Push(0);
    }

    /// <summary>
    /// 打开场景
    /// </summary>
    /// <param name="target"></param>
    public static void OpenScene(int target)
    {
        _SceneOpened.Push(target);
        SceneManager.LoadScene(target);
    }

    /// <summary>
    /// 场景回退
    /// </summary>
    public static void ExitScene()
    {
        _SceneOpened.Pop();
        SceneManager.LoadScene(_SceneOpened.Peek());
    }

    public static bool DebugTry() => _SceneOpened.Count == 1;
}
