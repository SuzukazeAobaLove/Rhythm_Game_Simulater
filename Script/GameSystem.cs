using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class GameSystem
{
    //�Ƿ��Խֻ�ģʽ����
    public static bool IfArcade = false;


    private static Stack<int> _SceneOpened = new Stack<int>();
    
    static GameSystem()
    {
        Application.targetFrameRate = 60;
        //��������
        _SceneOpened.Push(1);
    }


    /// <summary>
    /// �����
    /// </summary>
    /// <param name="target"></param>
    public static void OpenScene(int target)
    {
        _SceneOpened.Push(target);
        SceneManager.LoadScene(target);
    }

    /// <summary>
    /// �ر����
    /// </summary>
    public static void ExitScene()
    {
        _SceneOpened.Pop();
        SceneManager.LoadScene(_SceneOpened.Peek());
    }

}
