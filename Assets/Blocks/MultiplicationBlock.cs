using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplicationBlock : BinaryFunctionBlock
{
    public override TypeBlock BlockType => TypeBlock.Multiplication;

    public override void BlockAction(Runner runner)
    {
        OperativeField?.Multi(AccessoryField);
        runner?.Move(Direction);
    }

    protected override string GetDataTip()
        => IfAccessoryField(() => $"{OperativeField.Name} * {AccessoryField.Name}");
}
