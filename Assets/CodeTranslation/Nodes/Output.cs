using Assets.CodeTranslation.CodeGeneration;

namespace Assets.CodeTranslation.Nodes
{
    public class Output : UnaryOperator
    {
        public Output(string variable) : base(variable)
        {
        }

        public override string GetCodeRepresentation(ICodeGenerator codeGenerator, string prefix = "")
            => codeGenerator.GenerateOutputCode(this, prefix);
    }
}
