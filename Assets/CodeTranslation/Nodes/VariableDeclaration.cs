using Assets.CodeTranslation.CodeGeneration;

namespace Assets.CodeTranslation.Nodes
{
    public class VariableDeclaration : UnaryOperator
    {
        public VariableDeclaration(string variable, int value) : base(variable)
        {
            Value = value;
        }

        public int Value { get; }

        public override string GetCodeRepresentation(ICodeGenerator codeGenerator, string prefix = "")
            => codeGenerator.GenerateVariableDeclarationCode(this, prefix);
    }
}
