using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetsAnimCamera : MonoBehaviour
{
    void Start()
    {
        Anim = transform.GetComponent<Animator>();
        travel = 0;
        selector = 0;
        StonesButtons[0].GetComponent<Button>().onClick.AddListener(PlayClick);
        StonesButtons[1].GetComponent<Button>().onClick.AddListener(CreateClick);
        StonesButtons[2].GetComponent<Button>().onClick.AddListener(LogOutClick);
        StonesButtons[3].GetComponent<Button>().onClick.AddListener(ExitClick);
        //Buttons[0].onClick.AddListener(delegate { selector = 1; travel = 1; Debug.Log("Button1 is clicked");});
    }

    private void FixedUpdate()
    {
          Anim.SetFloat("Selector", selector);
          Anim.SetFloat("AnyTravel", travel, 0.5F, Time.deltaTime);
        if (Anim.GetFloat("AnyTravel") > 0.5F) { travel = 0; Anim.SetFloat("AnyTravel", travel); }
     }

    float travel;
    float selector;
    [SerializeField]
    Transform []StonesButtons;
    [SerializeField]
    Transform ExitButton;
    Animator Anim;
    // 1 - Any - Selector,  3 Any - None, 5 Selector - Any, 6 - Selector - Start

    public void Action(float Selection)
    {
        selector = Selection; travel = 1; Debug.Log("Button is clicked");
    }

    void PlayClick()
    {
        selector = 1;
        travel = 1;
    }

    void CreateClick()
    {
        //selector = 1;
        //travel = 1;
        var level = new LevelInfo();
        WorkspaceSceneManager.LoadWorkspaceScene(level, true);
    }

    void LogOutClick()
    {
        ExitButton.GetComponent<ExitButtonController>().OpenExit();
        selector = 3;
        travel = 1;
        //etc
    }

    void ExitClick()
    {
        ExitButton.GetComponent<ExitButtonController>().OpenExit();
        selector = 3;
        travel = 1;
    }
}
