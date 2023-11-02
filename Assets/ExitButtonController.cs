using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("UI Return").Find("Canvas").Find("Button").GetComponent<Button>().onClick.AddListener(ReturnClick);
        transform.Find("UI Right").Find("Canvas").Find("Button").GetComponent<Button>().onClick.AddListener(CheckClick);
    }
    void ReturnClick()
    {
        transform.GetComponent<Animator>().Play("Auto Exit");
        MainCamera.GetComponent<LetsAnimCamera>().Action(0);
    }
    public void OpenExit()
    {
        transform.GetComponent<Animator>().Play("Auto Open");
    }
    void CheckClick()
    {
        //код выхода из игры
        Application.Quit();
    }
    [SerializeField]
    Transform MainCamera;
    // Update is called once per frame
}
