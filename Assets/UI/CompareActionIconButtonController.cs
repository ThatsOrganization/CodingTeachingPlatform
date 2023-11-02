using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate bool CompareActionHandler(RunnerField operative, RunnerField accessory);

public delegate void CompareActionChosenHandler(CompareActionHandler compareAction, Sprite compareActionIcon, TypeCondition condition);

public class CompareActionIconButtonController : MonoBehaviour
{
    [SerializeField]
    GameObject compareActionIconsView;

    CompareActionChosenHandler onActionChosen;

    Button chooseActionButton;

    void Awake()
    {
        chooseActionButton = transform.GetComponent<Button>();
        chooseActionButton.onClick.AddListener(OnChooseActionButtonClick);

        compareActionIconsView.GetComponent<CompareActionIconsViewController>().Initialize(OnCompareActionChosen);
    }

    void OnChooseActionButtonClick()
    {
        compareActionIconsView.SetActive(true);
    }

    public void Initialize(Transform content, FunctionalBlock block, Sprite currentCompareActionIcon, CompareActionChosenHandler actionChosenHandler)
    {
        transform.SetParent(content, false);

        onActionChosen = actionChosenHandler;

        transform.GetComponent<Image>().sprite = currentCompareActionIcon;

        chooseActionButton.interactable = block.Interactable;
    }

    void OnCompareActionChosen(CompareActionHandler action, Sprite actionIcon, TypeCondition condition)
    {
        transform.GetComponent<Image>().sprite = actionIcon;
        onActionChosen?.Invoke(action, actionIcon, condition);
    }
}
