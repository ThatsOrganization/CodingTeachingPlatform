using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RunnerField : MonoBehaviour
{

    string _name;
    int _value;

    public event RemoveRunnerFieldHandler OnRemoveRunnerField;

    [SerializeField]
    GameObject nameComponentObject;

    [SerializeField]
    GameObject valueComponentObject;

    [SerializeField]
    GameObject removeButtonObject;

    Text _nameComponent;
    InputField _valueComponent;
    Button _removeButton;

    public Text NameComponent => _nameComponent;

    public InputField ValueComponent => _valueComponent;

    public Button RemoveButton => _removeButton;

    public string Name => _name;

    public int Value => _value;

    void Awake()
    {
        _nameComponent = nameComponentObject.GetComponent<Text>();
        _valueComponent = valueComponentObject.GetComponent<InputField>();
        _removeButton = removeButtonObject.GetComponent<Button>();

        _valueComponent.textComponent.fontSize = _nameComponent.fontSize;
        _valueComponent.onEndEdit.AddListener(OnValueChanged);
        _valueComponent.interactable = WorkspaceSceneManager.IsEditMode;

        _removeButton.onClick.AddListener(RemoveField);
    }

    void RemoveField()
    {
        OnRemoveRunnerField?.Invoke(this);
    }

    void OnValueChanged(string newVal)
    {
        int newValue = Value;
        if (int.TryParse(newVal, out newValue))
            SetValue(newValue);
        else
            SetValue(Value);
    }

    public void Initialize(string name, int value, Transform content, RemoveRunnerFieldHandler removeAction, bool removable)
    {
        transform.SetParent(content);
        transform.localPosition = new Vector3();
        transform.localScale = new Vector3(1, 1, 1);
        _removeButton.gameObject.SetActive(removable);
        OnRemoveRunnerField += removeAction;
        SetName(name);
        SetValue(value);
    }

    public void Add(RunnerField field)
    {
        SetValue(Value + field.Value);
    }

    public void Sub(RunnerField field)
    {
        SetValue(Value - field.Value);
    }

    public void Multi(RunnerField field)
    {
        SetValue(Value * field.Value);
    }

    public void Div(RunnerField field)
    {
        SetValue(Value / field.Value);
    }

    public void Mod(RunnerField field)
    {
        SetValue(Value % field.Value);
    }

    void SetValue(int val)
    {
        _value = val;
        var valString = val.ToString();
        ValueComponent.text = valString;
        ValueComponent.textComponent.fontSize = NameComponent.fontSize - (valString.Length - 1);
    }

    void SetName(string name)
    {
        _name = name;
        NameComponent.text = Name;
    }

    public override string ToString()
    {
        return Name;
    }

}
