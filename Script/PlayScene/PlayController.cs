using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayController : MonoBehaviour
{
    public Image BackGround;
    public Sprite refs;

    void Awake()
    {
        if (!FileManager.AllLoaded) SceneManager.LoadScene(0);
    }
    // Start is called before the first frame update
    void Start()
    {
        refs = FileManager.PlayBackground;
        BackGround.sprite = FileManager.PlayBackground;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
