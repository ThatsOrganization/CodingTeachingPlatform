using Assets.CodeTranslation.CodeGeneration;

namespace Assets.CodeTranslation.Nodes
{
    public abstract class BinaryOperator : IStatement
    {
        public BinaryOperator(string variable1, string variable2)
        {
            Variable1 = variable1;
            Variable2 = variable2;
        }

        public string Variable1 { get; }

        public string Variable2 { get; }

        public abstract string GetCodeRepresentation(ICodeGenerator codeGenerator, string prefix = "");
    }
}
