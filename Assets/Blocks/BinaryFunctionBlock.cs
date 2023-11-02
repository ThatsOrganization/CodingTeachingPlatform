using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BinaryFunctionBlock : FunctionalBlock
{
    public RunnerField AccessoryField { get; protected set; }

    public override void Restore(BlockInfo info)
    {
        base.Restore(info);
        OnAccessoryFieldChanged(Workspace.RunnerFieldsPanel.Runner.Fields, info.AccessoryFieldIndex);
    }

    protected override void InitializeProperties()
    {
        base.InitializeProperties();
        
        var dataPanel = Instantiate
            (DataProvider.BinaryBlockDataPanelPrefab, new Vector3(), new Quaternion());
        var dataPanelContent = Workspace.PropertiesPanel.DataPanelContent;
        var fieldsPanel = Workspace.RunnerFieldsPanel;
        dataPanel.GetComponent<BinaryBlockDataPanelController>()
            .Initialize(dataPanelContent, this, OnOperativeFieldChanged, OnAccessoryFieldChanged, fieldsPanel);
    }

    public override void Initialize(Transform content, GridCell cell, 
        WorkspaceController workspace, bool stationary = false)
    {
        base.Initialize(content, cell, workspace, stationary);

        AccessoryField = Workspace.RunnerFieldsPanel.Runner.Fields[1];
        ShowDataTipIfNeeded();
    }

    protected void OnAccessoryFieldChanged(List<RunnerField> fields, int index)
    {
        AccessoryField = fields[index];
        ShowDataTipIfNeeded();
    }

    protected T IfAccessoryField<T>(Func<T> func) => AccessoryField is null ? default : func();
}
