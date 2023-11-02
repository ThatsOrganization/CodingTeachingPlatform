using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DirectionPanelController : MonoBehaviour
{
    public static Color green = new Color(0, 255, 0);
    public static Color red = new Color(255, 0, 0);
    public static Color white = new Color(255, 255, 255);

    bool isAltDirectionEnabled;

    [SerializeField]
    GameObject buttonNorthObject;

    [SerializeField]
    GameObject buttonEastObject;

    [SerializeField]
    GameObject buttonWestObject;

    [SerializeField]
    GameObject buttonSouthObject;

    public FunctionalBlock Block { get; protected set; }

    //void Awake()
    //{
    //    var buttons = GetComponentsInChildren<Button>();
    //    foreach (var button in buttons)
    //        button?.onClick.AddListener(() => { OnDirectionButtonClicked(button, buttons); });
    //}

    //void OnDirectionButtonClicked(Button clickedButton, Button[] buttons)
    //{
    //    ChangeButtonsSelection(clickedButton, buttons);
    //}

    void ChangeButtonsSelection(Button buttonToSelect, Button[] buttons, 
        bool isFalseButton = false)
    {
        if (!buttonToSelect.interactable) return;
        buttonToSelect.interactable = false;
        var color = isFalseButton ? red : green;
        buttonToSelect.GetComponent<Image>().color = color;
        var lastSelectedButton = buttons
            .FirstOrDefault(b => b != buttonToSelect && b.GetComponent<Image>().color == color);
        if (lastSelectedButton != null)
        {
            lastSelectedButton.interactable = true;
            lastSelectedButton.GetComponent<Image>().color = white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNorthButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Block.SetDirection(TypeDirection.North);
            ChangeButtonsSelection(buttonNorthObject.GetComponent<Button>(),
                GetComponentsInChildren<Button>());
        }
        if (Input.GetMouseButtonDown(1) && Block.BlockType == TypeBlock.Condition)
        {
            var conditionBlock = Block as ConditionBlock;
            conditionBlock.SetFalseDirection(TypeDirection.North);
            ChangeButtonsSelection(buttonNorthObject.GetComponent<Button>(),
                GetComponentsInChildren<Button>(), true);
        }
    }

    public void OnEastButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Block.SetDirection(TypeDirection.East);
            ChangeButtonsSelection(buttonEastObject.GetComponent<Button>(),
                GetComponentsInChildren<Button>());
        }
        if (Input.GetMouseButtonDown(1) && Block.BlockType == TypeBlock.Condition)
        {
            var conditionBlock = Block as ConditionBlock;
            conditionBlock.SetFalseDirection(TypeDirection.East);
            ChangeButtonsSelection(buttonEastObject.GetComponent<Button>(),
                GetComponentsInChildren<Button>(), true);
        }
    }

    public void OnWestButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Block.SetDirection(TypeDirection.West);
            ChangeButtonsSelection(buttonWestObject.GetComponent<Button>(),
                GetComponentsInChildren<Button>());
        }
        if (Input.GetMouseButtonDown(1) && Block.BlockType == TypeBlock.Condition)
        {
            var conditionBlock = Block as ConditionBlock;
            conditionBlock.SetFalseDirection(TypeDirection.West);
            ChangeButtonsSelection(buttonWestObject.GetComponent<Button>(),
                GetComponentsInChildren<Button>(), true);
        }
    }

    public void OnSouthButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Block.SetDirection(TypeDirection.South);
            ChangeButtonsSelection(buttonSouthObject.GetComponent<Button>(),
                GetComponentsInChildren<Button>());
        }
        if (Input.GetMouseButtonDown(1) && Block.BlockType == TypeBlock.Condition)
        {
            var conditionBlock = Block as ConditionBlock;
            conditionBlock.SetFalseDirection(TypeDirection.South);
            ChangeButtonsSelection(buttonSouthObject.GetComponent<Button>(),
                GetComponentsInChildren<Button>(), true);
        }
    }

    public void Initialize(Transform panel, FunctionalBlock block)
    {
        gameObject.SetActive(true);

        Block = block;

        transform.SetParent(panel);
        transform.position = panel.position;
        transform.localScale = new Vector3(1, 1, 1);

        var bNorth = buttonNorthObject.GetComponent<Button>();
        bNorth?.onClick.AddListener(() => Block.SetDirection(TypeDirection.North));

        var bEast = buttonEastObject.GetComponent<Button>();
        bEast?.onClick.AddListener(() => Block.SetDirection(TypeDirection.East));

        var bWest = buttonWestObject.GetComponent<Button>();
        bWest?.onClick.AddListener(() => Block.SetDirection(TypeDirection.West));

        var bSouth = buttonSouthObject.GetComponent<Button>();
        bSouth?.onClick.AddListener(() => Block.SetDirection(TypeDirection.South));

        Button buttonToSelect;
        switch (Block.Direction)
        {
            case TypeDirection.North:
                buttonToSelect = bNorth; break;
            case TypeDirection.East:
                buttonToSelect = bEast; break;
            case TypeDirection.South:
                buttonToSelect = bSouth; break;
            case TypeDirection.West:
                buttonToSelect = bWest; break;
            default:
                buttonToSelect = bNorth; break;
        }
        var buttons = GetComponentsInChildren<Button>();
        ChangeButtonsSelection(buttonToSelect, buttons);

        if (Block.BlockType == TypeBlock.Condition)
        {
            Button falseButtonToSelect;
            var conditionBlock = Block as ConditionBlock;
            switch (conditionBlock.FalseDirection)
            {
                case TypeDirection.North:
                    falseButtonToSelect = bNorth; break;
                case TypeDirection.East:
                    falseButtonToSelect = bEast; break;
                case TypeDirection.South:
                    falseButtonToSelect = bSouth; break;
                case TypeDirection.West:
                    falseButtonToSelect = bWest; break;
                default:
                    falseButtonToSelect = bNorth; break;
            }
            ChangeButtonsSelection(falseButtonToSelect, buttons, true);
        }

        if (!Block.Interactable)
            foreach (var button in buttons)
                button.interactable = false;
    }
}
