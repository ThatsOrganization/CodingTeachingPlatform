using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBlock : FunctionalBlock
{
    public override TypeBlock BlockType => TypeBlock.Start;

    public override void BlockAction(Runner runner)
    {
        runner.Move(Direction);
    }


}
