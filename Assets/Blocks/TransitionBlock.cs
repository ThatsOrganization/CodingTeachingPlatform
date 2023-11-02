public class TransitionBlock : FunctionalBlock
{
    public override TypeBlock BlockType => TypeBlock.Transition;

    public override void BlockAction(Runner runner)
    {
        runner.Move(Direction);
    }
}
