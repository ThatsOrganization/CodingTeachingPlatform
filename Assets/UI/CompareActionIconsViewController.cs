using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompareActionIconsViewController : MonoBehaviour
{
    [SerializeField]
    GameObject greaterEqualButtonObject;

    [SerializeField]
    GameObject greaterButtonObject;

    [SerializeField]
    GameObject equalButtonObject;

    [SerializeField]
    GameObject lessButtonObject;

    [SerializeField]
    GameObject lessEqualButtonObject;

    CompareActionChosenHandler onActionChosen;

    public void Initialize(CompareActionChosenHandler actionChosenHandler)
    {
        onActionChosen = actionChosenHandler;

        greaterEqualButtonObject.GetComponent<Button>().onClick.AddListener(OnGreaterEqualButtonClick);

        greaterButtonObject.GetComponent<Button>().onClick.AddListener(OnGreaterButtonClick);

        equalButtonObject.GetComponent<Button>().onClick.AddListener(OnEqualButtonClick);

        lessButtonObject.GetComponent<Button>().onClick.AddListener(OnLessButtonClick);

        lessEqualButtonObject.GetComponent<Button>().onClick.AddListener(OnLessEqualButtonClick);

        gameObject.SetActive(false);
    }

    void OnGreaterEqualButtonClick()
    {
        onActionChosen?.Invoke((a, b) => a.Value >= b.Value, greaterEqualButtonObject.GetComponent<Image>().sprite, TypeCondition.GreaterEqual);
        gameObject.SetActive(false);
    }

    void OnGreaterButtonClick()
    {
        onActionChosen?.Invoke((a, b) => a.Value > b.Value, greaterButtonObject.GetComponent<Image>().sprite, TypeCondition.Greater);
        gameObject.SetActive(false);
    }

    void OnEqualButtonClick()
    {
        onActionChosen?.Invoke((a, b) => a.Value == b.Value, equalButtonObject.GetComponent<Image>().sprite, TypeCondition.Equal);
        gameObject.SetActive(false);
    }

    void OnLessButtonClick()
    {
        onActionChosen?.Invoke((a, b) => a.Value < b.Value, lessButtonObject.GetComponent<Image>().sprite, TypeCondition.Less);
        gameObject.SetActive(false);
    }

    void OnLessEqualButtonClick()
    {
        onActionChosen?.Invoke((a, b) => a.Value <= b.Value, lessEqualButtonObject.GetComponent<Image>().sprite, TypeCondition.LessEqual);
        gameObject.SetActive(false);
    }
}
