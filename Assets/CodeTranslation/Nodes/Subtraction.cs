using Assets.CodeTranslation.CodeGeneration;

namespace Assets.CodeTranslation.Nodes
{
    public class Subtraction : BinaryOperator
    {
        public Subtraction(string variable1, string variable2) : base(variable1, variable2)
        {
        }

        public override string GetCodeRepresentation(ICodeGenerator codeGenerator, string prefix = "")
            => codeGenerator.GenerateSubtractionCode(this, prefix);
    }
}
