using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionBlock : BinaryFunctionBlock
{
    public override TypeBlock BlockType => TypeBlock.Addition;

    public override void BlockAction(Runner runner)
    {
        OperativeField?.Add(AccessoryField);
        runner.Move(Direction);
    }

    protected override string GetDataTip()
        => IfAccessoryField(() => $"{OperativeField.Name} + {AccessoryField.Name}");
}
