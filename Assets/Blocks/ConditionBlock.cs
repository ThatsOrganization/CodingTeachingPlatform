using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionBlock : BinaryFunctionBlock
{
    CompareActionHandler compareAction = (a, b) => a.Value >= b.Value;

    Sprite compareActionIcon;

    public override TypeBlock BlockType => TypeBlock.Condition;

    public TypeDirection FalseDirection { get; protected set; } = TypeDirection.West;

    public TypeCondition Condition { get; private set; } = TypeCondition.GreaterEqual;

    public override void Restore(BlockInfo info)
    {
        base.Restore(info);
        SetFalseDirection(info.FalseDirection);
    }

    public override void Initialize(Transform content, GridCell cell, WorkspaceController workspace, bool stationary = false)
    {
        base.Initialize(content, cell, workspace, stationary);
        SetFalseDirection(FalseDirection);
    }

    void Awake()
    {
        compareActionIcon = DataProvider.CompareActionIconButtonPrefab.GetComponent<Image>().sprite;
    }

    public override void BlockAction(Runner runner)
    {
        if (compareAction.Invoke(OperativeField, AccessoryField)) runner.Move(Direction);
        else runner.Move(FalseDirection);
    }

    public override void InitializeActionContent(Transform content)
    {
        var compareActionContent = Instantiate(DataProvider.CompareActionIconButtonPrefab, new Vector3(), new Quaternion());
        compareActionContent.GetComponent<CompareActionIconButtonController>().Initialize(content, this, compareActionIcon, OnCompareActionChosen);
    }

    void OnCompareActionChosen(CompareActionHandler action, Sprite actionIcon, TypeCondition condition)
    {
        compareAction = action;
        compareActionIcon = actionIcon;
        _actionSprite.sprite = actionIcon;
        Condition = condition;
        ShowDataTipIfNeeded();
    }

    public void SetFalseDirection(TypeDirection direction)
    {
        if (_arrowSprites.TryGetValue(FalseDirection, out var oldArrowSprite) && oldArrowSprite)
        {
            oldArrowSprite.enabled = false;
            oldArrowSprite.color = DirectionPanelController.white;
        }

        if (_arrowSprites.TryGetValue(direction, out var newArrowSprite) && newArrowSprite)
        {
            newArrowSprite.color = DirectionPanelController.red;
            newArrowSprite.enabled = true;
        }

        FalseDirection = direction;

        Workspace.GridPlane.UpdateCycles();
    }

    public void EnableCycleConditionOutline()
    {
        _outlinable.OutlineParameters.Color = Color.yellow;
        _outlinable.enabled = true;
    }

    protected override string GetDataTip()
        => IfAccessoryField(() => $"{OperativeField.Name} {GetCondition(Condition)} {AccessoryField.Name}");

    private string GetCondition(TypeCondition condition)
    {
        switch (condition)
        {
            case TypeCondition.Greater:
                return ">";
            case TypeCondition.GreaterEqual:
                return ">=";
            case TypeCondition.Equal:
                return "=";
            case TypeCondition.LessEqual:
                return "<=";
            case TypeCondition.Less:
                return "<";
            default:
                throw new NotSupportedException($"Condition {condition} is not supported in {GetType().Name}");
        }
    }
}
