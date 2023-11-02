using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;

public class BinaryBlockDataPanelController : BlockDataPanelController
{
    [SerializeField]
    GameObject accessoryFieldCbObject;

    Dropdown accessoryFieldCb;

    FieldChangedHandler onAccessoryFieldChanged;

    protected override void Awake()
    {
        base.Awake();

        accessoryFieldCb = accessoryFieldCbObject.GetComponent<Dropdown>();
    }

    public void Initialize(Transform panel, FunctionalBlock block, FieldChangedHandler operativeFieldChanged,
        FieldChangedHandler accessoryFieldChanged, RunnerFieldsPanelController fieldsPanel)
    {
        Initialize(panel, block, operativeFieldChanged, fieldsPanel);

        onAccessoryFieldChanged = accessoryFieldChanged;

        accessoryFieldCb.onValueChanged.AddListener(OnAccessoryFieldChanged);

        accessoryFieldCb.interactable = Block.Interactable;
    }

    protected override void UpdateFieldsOptions()
    {
        base.UpdateFieldsOptions();

        var options = runnerFields.Select(f => new OptionData(f.Name)).ToList();

        accessoryFieldCb.options = options;
        var selectedAccessoryField = runnerFields.IndexOf((Block as BinaryFunctionBlock).AccessoryField);
        if (selectedAccessoryField < 0) selectedAccessoryField = 0;
        accessoryFieldCb.value = selectedAccessoryField;
        OnAccessoryFieldChanged(selectedAccessoryField);
    }

    void OnAccessoryFieldChanged(int index)
    {
        if (index < 0 || index >= runnerFields.Count) return;
        onAccessoryFieldChanged?.Invoke(runnerFields, index);
    }
}
