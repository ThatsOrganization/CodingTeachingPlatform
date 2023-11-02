using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnaryFunctionBlock : FunctionalBlock
{

    protected override void InitializeProperties()
    {
        base.InitializeProperties();

        var dataPanel = Instantiate
            (DataProvider.UnaryBlockDataPanelPrefab, new Vector3(), new Quaternion());
        var dataPanelContent = Workspace.PropertiesPanel.DataPanelContent;
        var fieldsPanel = Workspace.RunnerFieldsPanel;
        dataPanel.GetComponent<BlockDataPanelController>()
            .Initialize(dataPanelContent, this, OnOperativeFieldChanged, fieldsPanel);
    }
}