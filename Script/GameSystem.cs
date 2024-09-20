using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Panel { MainMenu = 1,Profile,SongSelect,Arcade,Ranking,Statistic,PlaySong};
public static class GameSystem
{

    private static Stack<int> _SceneOpened = new Stack<int>();
    
    static GameSystem()
    {
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
