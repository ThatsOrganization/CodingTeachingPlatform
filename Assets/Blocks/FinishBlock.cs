using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBlock : FunctionalBlock
{
    List<int> _values = new List<int>();

    public List<int> Values => _values;

    public override TypeBlock BlockType => TypeBlock.Finish;

    public override void BlockAction(Runner runner)
    {
        var storage = Workspace.GridPlane.GetBlockByType(TypeBlock.Storage) as StorageBlock;

        var equal = true;
        if (_values.Count == storage.Values.Count)
        {
            for (int i = 0; i < _values.Count; i++)
                if (_values[i] != storage.Values[i])
                {
                    equal = false;
                    break;
                }
        }
        else equal = false;

        if (equal)
        {
            runner.Finish();
        }
        else
        {
            runner.Restart();
        }
    }

    public override void Restore(BlockInfo info)
    {
        base.Restore(info);

        if (info.AdditionalInfo.ContainsKey(TypeInfo.Values))
            _values = info.AdditionalInfo[TypeInfo.Values] as List<int>;
    }

    protected override void InitializeProperties()
    {
        base.InitializeProperties();

        var directionPanelContent = Workspace.PropertiesPanel.DirectionPanelContent;
        for (int i = 0; i < directionPanelContent.childCount; i++)
        {
            var child = directionPanelContent.GetChild(i).gameObject;
            if (child != null) Destroy(child);
        }

        if (WorkspaceSceneManager.IsEditMode)
        {
            var dataPanelContent = Workspace.PropertiesPanel.DataPanelContent;
            var inputPanel = Instantiate(DataProvider.InputPanelPrefab);
            inputPanel.GetComponent<InputPanelController>()
                .Initialize(dataPanelContent, _values, OnValuesChanged);
        }
    }

    void OnValuesChanged(List<int> values)
    {
        _values = values;
    }
}
