using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public delegate void RunnerFieldChangedHandler(FieldChangedEventArgs eventArgs);

public delegate void RemoveRunnerFieldHandler(RunnerField field);

public class FieldChangedEventArgs : EventArgs
{
    public List<RunnerField> Fields { get; }
    public RunnerField ChangedField { get; }

    public FieldChangedEventArgs(List<RunnerField> fields, RunnerField changedField)
    {
        Fields = fields;
        ChangedField = changedField;
    }
}

public class RunnerFieldsPanelController : MonoBehaviour
{
    [SerializeField]
    GameObject addFieldButtonObject;

    [SerializeField]
    GameObject fieldsViewObject;

    Transform fieldsContent;

    public event RunnerFieldChangedHandler OnRunnerFieldChanged;

    public WorkspaceController Workspace { get; set; }

    public Runner Runner { get; protected set; }

    void Awake()
    {
        var addFieldButton = addFieldButtonObject.GetComponent<Button>();
        addFieldButton.onClick.AddListener(AddField);
        addFieldButton.gameObject.SetActive(WorkspaceSceneManager.IsEditMode);

        fieldsContent = fieldsViewObject.GetComponent<ScrollRect>().content;
    }

    string GetUniqueFieldName()
    {
        var name = "A";
        var letter = 'A';
        var num = 0;
        while (Runner.Fields.Any(f => f.Name == name))
        {
            letter++;
            if (letter > 'Z')
            {
                letter = 'A';
                num++;
            }
            name = "" + letter;
            if (num > 0) name += num;
        }
        return name;
    }

    public void Initialize(Runner runner, int[] fieldsValues)
    {
        Runner = runner;

        for (int i = 0; i < fieldsValues.Length; i++) AddFieldInternal(fieldsValues[i], false);
    }

    void AddField()
    {
        AddFieldInternal(0, true);
    }

    void AddFieldInternal(int value, bool removable)
    {
        var field = Instantiate(DataProvider.FieldPanelPrefab, new Vector3(), new Quaternion());
        var runnerField = field.GetComponent<RunnerField>();
        var fieldName = GetUniqueFieldName();
        runnerField.Initialize(fieldName, value, fieldsContent, RemoveField, removable);
        Runner.Fields.Add(runnerField);

        OnRunnerFieldChanged?.Invoke(new FieldChangedEventArgs(Runner.Fields, runnerField));
    }

    void RemoveField(RunnerField field)
    {
        Runner.Fields.Remove(field);
        Destroy(field.gameObject);

        OnRunnerFieldChanged?.Invoke(new FieldChangedEventArgs(Runner.Fields, field));
    }
}
