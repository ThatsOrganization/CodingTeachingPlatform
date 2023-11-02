using Assets.CodeTranslation.Nodes;

namespace Assets.CodeTranslation.CodeGeneration
{
    public interface ICodeGenerator
    {
        string GenerateProgramCode(Program program, string prefix = "");

        string GenerateVariableDeclarationCode(VariableDeclaration variableDeclaration, string prefix = "");

        string GenerateAdditionCode(Addition addition, string prefix = "");

        string GenerateSubtractionCode(Subtraction subtraction, string prefix = "");

        string GenerateMultiplicationCode(Multiplication multiplication, string prefix = "");

        string GenerateIfCode(If @if, string prefix = "");

        string GenerateWhileCode(While @while, string prefix = "");

        string GenerateDoWhileCode(DoWhile doWhile, string prefix = "");

        string GenerateOutputCode(Output output, string prefix = "");

        string GenerateExceptionCode(ExceptionStatement exception, string prefix = "");
    }
}
