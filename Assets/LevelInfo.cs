using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class LevelInfo
{
    string _name;
    string _description;

    int[] _fieldsValues;
    
    Dictionary<TypeBlock, int> _avaliableBlocks;
    
    List<BlockInfo> _blocks;

    public string Name => _name;
    public string Description => _description;

    public int[] FieldsValues => _fieldsValues;

    public Dictionary<TypeBlock, int> AvailableBlocks => _avaliableBlocks;

    public List<BlockInfo> Blocks => _blocks;

    public LevelInfo()
    {
        _fieldsValues = new int[] { 0, 0 };

        _blocks = new List<BlockInfo>()
        {
            new BlockInfo(TypeBlock.Start, TypeDirection.North),
            new BlockInfo(TypeBlock.Finish, TypeDirection.North),
            new BlockInfo(TypeBlock.Storage, TypeDirection.North),
        };

        var blocksTypes = Resources.LoadAll("Blocks")
            .Where(b =>
            {
                if (b is GameObject blockPrefab)
                {
                    var funcBlock = blockPrefab.GetComponent<FunctionalBlock>();
                    if (funcBlock == null) return false;
                    return funcBlock.BlockType != TypeBlock.Start &&
                    funcBlock.BlockType != TypeBlock.Finish &&
                    funcBlock.BlockType != TypeBlock.Storage;
                }
                else
                    return false;
            })
            .Select(b => (b as GameObject).GetComponent<FunctionalBlock>().BlockType).ToList();

        _avaliableBlocks = new Dictionary<TypeBlock, int>();

        foreach (var blockType in blocksTypes)
            _avaliableBlocks[blockType] = 0;
    }

    public void Save(WorkspaceController workspace, string name, string description)
    {
        if (name == "") name = "level";
        if (description == "") description = "description";

        _name = name;
        _description = description;

        _fieldsValues = workspace.RunnerFieldsPanel.Runner.Fields.Select(f => f.Value).ToArray();

        foreach (var initializer in workspace.BlockPanel.BlockInitializers)
            _avaliableBlocks[initializer.BlockPrefab.GetComponent<FunctionalBlock>().BlockType] = initializer.BlocksCount;

        _blocks = new List<BlockInfo>();
        for (int i = 0; i < workspace.GridPlane.GridCells.Length; i++)
            for (int j = 0; j < workspace.GridPlane.GridCells[0].Length; j++)
            {
                var block = workspace.GridPlane.GridCells[i][j].Block;

                if (block == FunctionalBlock.EmptyBlock) continue;

                var operativeFieldIndex = workspace.RunnerFieldsPanel.Runner.Fields.IndexOf(block.OperativeField);
                var accessoryFieldIndex = 1;
                if (block is BinaryFunctionBlock binaryBlock)
                    accessoryFieldIndex = workspace.RunnerFieldsPanel.Runner.Fields.IndexOf(binaryBlock.AccessoryField);
                var falseDirection = TypeDirection.West;
                if (block is ConditionBlock conditionBlock)
                    falseDirection = conditionBlock.FalseDirection;
                var additionalInfo = new Dictionary<TypeInfo, object>();
                if (block is FinishBlock finishBlock)
                    additionalInfo[TypeInfo.Values] = finishBlock.Values;
                _blocks.Add(new BlockInfo(block.BlockType, block.Direction, falseDirection, 
                    block.X, block.Y, operativeFieldIndex, accessoryFieldIndex, additionalInfo));
            }

        var serializer = new BinaryFormatter();
        var filePath = string.Format("levels/{0}.lvl", Name).Replace(' ', '_');
        var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        serializer.Serialize(stream, this);
        stream.Close();
    }
}
