using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtractionBlock : BinaryFunctionBlock
{
    public override TypeBlock BlockType => TypeBlock.Subtraction;

    public override void BlockAction(Runner runner)
    {
        OperativeField?.Sub(AccessoryField);
        runner.Move(Direction);
    }

    protected override string GetDataTip()
        => IfAccessoryField(() => $"{OperativeField.Name} - {AccessoryField.Name}");
}
