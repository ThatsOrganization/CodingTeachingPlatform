using Assets.CodeTranslation.CodeGeneration;

namespace Assets.CodeTranslation.Nodes
{
    public abstract class UnaryOperator : IStatement
    {
        public UnaryOperator(string variable)
        {
            Variable = variable;
        }

        public string Variable { get; }

        public abstract string GetCodeRepresentation(ICodeGenerator codeGenerator, string prefix = "");
    }
}
