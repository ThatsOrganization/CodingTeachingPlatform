using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
//не обращай внимание на ошибки, textmeshpro ппц глючный
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class SelectorLevelScript : MonoBehaviour
{
    List<LevelInfo> levels;
    int pageNum;
    int levelNum;

    bool isPageChanging;

    // Start is called before the first frame update
    void Start()
    {

        Screens = new Transform[ScreenCount];
        Transform[] ScreenButtons = new Transform[ScreenCount];
        Transform scr = transform.Find("Screens");
        
        for (int i = 0; i < Screens.Length; i++)
        {
            Screens[i] = scr.GetChild(i);
           ScreenButtons[i] = Screens[i].Find("Screen").Find("Canvas").Find("Button");
        }
        ScreenButtons[0].GetComponent<Button>().onClick.AddListener(delegate { LetsSelect(0); });//такое в цикл не загнать, глюки(
        ScreenButtons[1].GetComponent<Button>().onClick.AddListener(delegate { LetsSelect(1); });
        ScreenButtons[2].GetComponent<Button>().onClick.AddListener(delegate { LetsSelect(2); });
        ScreenButtons[3].GetComponent<Button>().onClick.AddListener(delegate { LetsSelect(3); });
        ScreenButtons[4].GetComponent<Button>().onClick.AddListener(delegate { LetsSelect(4); });
        ScreenButtons[5].GetComponent<Button>().onClick.AddListener(delegate { LetsSelect(5); });
        for (int i = 0; i < Screens.Length; i++)
        {
            SetUnglesColor(activecolor, i);
        }

        Arrows[0] = transform.Find("Arrows").GetChild(0).Find("Canvas").Find("Button");
        Arrows[0].GetComponent<Button>().onClick.AddListener(delegate { ClickArrow(0); });

        Arrows[1] = transform.Find("Arrows").GetChild(1).Find("Canvas").Find("Button");
        Arrows[1].GetComponent<Button>().onClick.AddListener(delegate { ClickArrow(1); });

        Labels[0] = transform.Find("UI Label Pages").Find("Text");//штука типа "1/2" (то бишь страницы уровней, первая из двух)
        Labels[1] = transform.Find("UI Level Name Label").Find("Text");//имя уровня, ~18-19 символов
        Labels[2] = transform.Find("UI Big Plate").Find("Text"); //панель с описанием уровня. текст должен быть ~20 символов в строке, 4 строчки

        startLevelButton = transform.Find("UI Long Button Start").Find("Canvas").Find("Button").GetComponent<Button>();
        startLevelButton.onClick.AddListener(OnStartLevel);

        //пример изменения текста. Не страшно что ругается
        //Labels[0].GetComponent<TextMeshPro>().text = "new text";

        transform.Find("UI Return").Find("Canvas").Find("Button").GetComponent<Button>().onClick.AddListener(ReturnButtonClick);
        transform.Find("UI Return Start").Find("Canvas").Find("Button").GetComponent<Button>().onClick.AddListener(ReturnStartButtonClick);

        pageNum = 0;

        levels = new List<LevelInfo>();
        var dir = new DirectoryInfo("levels");
        var serializer = new BinaryFormatter();
        foreach (var file in dir.GetFiles("*.lvl"))
        {
            using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                var level = serializer.Deserialize(stream) as LevelInfo;
                if (level != null)
                    levels.Add(level);
            }
        }
    }

    void OnStartLevel()
    {
        if (levelNum < levels.Count)
            WorkspaceSceneManager.LoadWorkspaceScene(levels[levelNum], false);
    }
    
    byte[] activecolor = new byte[] { 50, 200, 50, 255 };
    byte[] selectcolor = new byte[] { 255, 255, 255, 255 };
    byte[] closedcolor = new byte[] { 200, 50, 50, 255 };
    LinkedList<byte> Numbers = new LinkedList<byte>();

    void SetUnglesColor(byte[]colors, int i)
    {
        //Debug.Log("i = " + i);
        Transform[] Ungles = new Transform[4];
        Ungles[0] = Screens[i].Find("Ungle Select RU");
        Ungles[1] = Screens[i].Find("Ungle Select RD");
        Ungles[2] = Screens[i].Find("Ungle Select LU");
        Ungles[3] = Screens[i].Find("Ungle Select LD");
        for (int j = 0; j < 4; j++)
        {
            Material Mat = Ungles[j].Find("default").GetComponent<MeshRenderer>().material;
            Mat.SetColor("_EmissionColor", new Color32(colors[0], colors[1], colors[2], colors[3]));
        }
    }
    void PlaySelect(int i)
    {
        if (oldscreen != -1)
        {
            Animator oldAnim = Screens[oldscreen].GetComponent<Animator>();
            oldAnim.SetFloat("Untravel", 1);
            oldAnim.SetFloat("Travel", 0);
        }
        Animator Anim = Screens[i].GetComponent<Animator>();
        Anim.SetFloat("Untravel", 0);
        Anim.SetFloat("Travel", 1);
        oldscreen = i;
    }
    int oldscreen = -1;
    void ClickArrow(int i)//метод нажатия стрелок
    {
        if (i == 0)//левая
        {
            Debug.Log("Im Left Arrow!");
            if (pageNum == 0) return;
            pageNum--;
            isPageChanging = true;
            LetsSelect(0);
        }
        else//правая
        {
            Debug.Log("Im Right Arrow!");
            if ((pageNum + 1) * 6 >= levels.Count) return;
            pageNum++;
            isPageChanging = true;
            LetsSelect(0);
        }
    }
    public void WorkOut()
    {
        SetUnglesColor(activecolor, oldscreen);
        Animator Anim = Screens[oldscreen].GetComponent<Animator>();
        Anim.SetFloat("Untravel", 0);
    }
    [SerializeField]
    GameObject MainCamera;
    [SerializeField]
    AnimationClip ReturnButtonOpen;
    [SerializeField]
    AnimationClip ReturnButtonExit;

    public void LetsSelect(int i)
    {
        levelNum = 6 * pageNum + i;

        if (levelNum < levels.Count)
        {
            Labels[1].GetComponent<TextMeshPro>().text = levels[levelNum].Name;
            Labels[2].GetComponent<TextMeshPro>().text = levels[levelNum].Description;
        }
        else
        {
            Labels[1].GetComponent<TextMeshPro>().text = "Empty";
            Labels[2].GetComponent<TextMeshPro>().text = "Empty";
        }

        Debug.Log("Let Select " + i);
        if (oldscreen == i && !isPageChanging)
        {
            MainCamera.GetComponent<LetsAnimCamera>().Action(6);
            transform.Find("UI Return Start").GetComponent<PlayMe>().Play(ReturnButtonOpen);

            return;
        }
        isPageChanging = false;
        if (oldscreen != -1) {
        SetUnglesColor(activecolor, oldscreen); }
        SetUnglesColor(selectcolor, i);
        PlaySelect(i);
    }
    void ReturnStartButtonClick()
    {
        MainCamera.GetComponent<LetsAnimCamera>().Action(0);
        transform.Find("UI Return Start").GetComponent<PlayMe>().Play(ReturnButtonExit);
    }
    void ReturnButtonClick()
    {
        MainCamera.GetComponent<LetsAnimCamera>().Action(5);
        if (oldscreen != -1)
        {
            Debug.Log("Nooooo");
            SetUnglesColor(activecolor, oldscreen);
            Animator Anim = Screens[oldscreen].GetComponent<Animator>();
            Anim.SetFloat("Untravel", 1);
            Anim.SetFloat("Travel", 0);
        }
        oldscreen = -1;
    }
    const int ScreenCount = 6;
    Transform[] Screens;
    Transform[] Arrows = new Transform[2];
    Transform[] Labels = new Transform[3];

    Button startLevelButton;

    // Update is called once per frame
    void Update()
    {
        
    }
}
