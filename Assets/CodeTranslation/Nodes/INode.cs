using Assets.CodeTranslation.CodeGeneration;

namespace Assets.CodeTranslation.Nodes
{
    public interface INode
    {
        string GetCodeRepresentation(ICodeGenerator codeGenerator, string prefix = "");
    }
}
