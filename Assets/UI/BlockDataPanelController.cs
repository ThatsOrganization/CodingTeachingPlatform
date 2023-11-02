using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public delegate void FieldChangedHandler(List<RunnerField> fields, int index);

public class BlockDataPanelController : MonoBehaviour
{
    [SerializeField]
    GameObject operativeFieldCbObject;

    [SerializeField]
    GameObject actionContentObject;

    Dropdown operativeFieldCb;

    FieldChangedHandler onOperativeFieldChanged;

    protected List<RunnerField> runnerFields;

    public FunctionalBlock Block { get; protected set; }

    protected virtual void Awake()
    {
        operativeFieldCb = operativeFieldCbObject.GetComponent<Dropdown>();
    }

    public void Initialize(Transform panel, FunctionalBlock block, FieldChangedHandler operativeFieldChanged,
        RunnerFieldsPanelController fieldsPanel)
    {
        gameObject.SetActive(true);

        Block = block;

        block.InitializeActionContent(actionContentObject.transform);

        onOperativeFieldChanged = operativeFieldChanged;

        transform.SetParent(panel);
        transform.position = panel.position;
        transform.localScale = new Vector3(1, 1, 1);

        runnerFields = fieldsPanel.Runner?.Fields ?? new List<RunnerField>();
        fieldsPanel.OnRunnerFieldChanged += UpdateFieldsData;

        operativeFieldCb.onValueChanged.AddListener(OnOperativeFieldChanged);

        UpdateFieldsOptions();

        operativeFieldCb.interactable = Block.Interactable;
    }

    void UpdateFieldsData(FieldChangedEventArgs eventArgs)
    {
        if (eventArgs.ChangedField == null) return;
        UpdateFieldsOptions();
    }

    protected virtual void UpdateFieldsOptions()
    {
        var options = runnerFields.Select(f => new OptionData(f.Name)).ToList();

        operativeFieldCb.options = options;
        var selectedOperativeField = runnerFields.IndexOf(Block.OperativeField);
        if (selectedOperativeField < 0) selectedOperativeField = 0;
        operativeFieldCb.value = selectedOperativeField;
        OnOperativeFieldChanged(selectedOperativeField);
    }

    void OnOperativeFieldChanged(int index)
    {
        if (index < 0 || index >= runnerFields.Count) return;
        onOperativeFieldChanged?.Invoke(runnerFields, index);
    }
}
