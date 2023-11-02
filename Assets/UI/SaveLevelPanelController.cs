using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLevelPanelController : MonoBehaviour
{
    [SerializeField]
    GameObject nameInputObject;

    [SerializeField]
    GameObject descriptionInputObject;

    [SerializeField]
    GameObject saveButtonObject;

    [SerializeField]
    GameObject cancelButtonObject;

    InputField nameInput;

    InputField descriptionInput;

    WorkspaceController _workspace;

    void Start()
    {
        nameInput = nameInputObject.GetComponent<InputField>();

        descriptionInput = descriptionInputObject.GetComponent<InputField>();

        saveButtonObject.GetComponent<Button>().onClick.AddListener(OnSave);

        cancelButtonObject.GetComponent<Button>().onClick.AddListener(OnCancel);
    }

    public void Initialize(WorkspaceController workspace)
    {
        _workspace = workspace;
    }

    void OnSave()
    {
        WorkspaceSceneManager.CurrentLevelInfo.Save(_workspace, nameInput.text, descriptionInput.text);
        WorkspaceSceneManager.LoadMainMenu();
    }

    void OnCancel()
    {
        Destroy(gameObject);
    }
}
