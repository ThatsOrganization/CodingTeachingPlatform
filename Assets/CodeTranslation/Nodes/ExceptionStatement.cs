using Assets.CodeTranslation.CodeGeneration;
using Assets.CodeTranslation.Nodes;

public class ExceptionStatement : IStatement
{
    public string GetCodeRepresentation(ICodeGenerator codeGenerator, string prefix = "")
        => codeGenerator.GenerateExceptionCode(this, prefix);
}
