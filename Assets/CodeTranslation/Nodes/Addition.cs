using Assets.CodeTranslation.CodeGeneration;

namespace Assets.CodeTranslation.Nodes
{
    public class Addition : BinaryOperator
    {
        public Addition(string variable1, string variable2) : base(variable1, variable2)
        {
        }

        public override string GetCodeRepresentation(ICodeGenerator codeGenerator, string prefix = "")
            => codeGenerator.GenerateAdditionCode(this, prefix);
    }
}
