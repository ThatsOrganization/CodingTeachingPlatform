using Assets.CodeTranslation.Nodes;
using System;
using System.Linq;

namespace Assets.CodeTranslation.CodeGeneration
{
    public class PythonCodeGenerator : ICodeGenerator
    {
        public string GenerateProgramCode(Program program, string prefix = "")
            => string.Join("\n", program.Statements.Select(s => $"{s.GetCodeRepresentation(this, prefix)}"));

        public string GenerateVariableDeclarationCode(VariableDeclaration variableDeclaration, string prefix = "")
            => $"{prefix}{variableDeclaration.Variable} = {variableDeclaration.Value}";

        public string GenerateAdditionCode(Addition addition, string prefix = "")
            => $"{prefix}{addition.Variable1} += {addition.Variable2}";

        public string GenerateSubtractionCode(Subtraction subtraction, string prefix = "")
            => $"{prefix}{subtraction.Variable1} -= {subtraction.Variable2}";

        public string GenerateMultiplicationCode(Multiplication multiplication, string prefix = "")
            => $"{prefix}{multiplication.Variable1} *= {multiplication.Variable2}";

        public string GenerateIfCode(If @if, string prefix = "") =>
            $"{prefix}if {@if.Variable1} {GetCondition(@if.Condition)} {@if.Variable2}:\n" +
            string.Join("", @if.TrueStatements.Select(s => $"{s.GetCodeRepresentation(this, prefix: $"{prefix}\t")}\n")) +
            $"{prefix}else:\n" +
            string.Join("\n", @if.FalseStatements.Select(s => $"{s.GetCodeRepresentation(this, prefix: $"{prefix}\t")}"));

        public string GenerateWhileCode(While @while, string prefix = "") =>
            $"{prefix}while {@while.Variable1} {GetCondition(@while.Condition, isTrue: @while.IsTrue)} {@while.Variable2}:\n" +
            string.Join("\n", @while.Statements.Select(s => $"{s.GetCodeRepresentation(this, prefix: $"{prefix}\t")}"));

        public string GenerateDoWhileCode(DoWhile doWhile, string prefix = "") =>
            $"{prefix}while True:\n" +
            string.Join("\n", doWhile.Statements.Select(s => $"{s.GetCodeRepresentation(this, prefix: $"{prefix}\t")}")) +
            $"{prefix}\tif not {doWhile.Variable1} {GetCondition(doWhile.Condition, isTrue: doWhile.IsTrue)} {doWhile.Variable2}:" +
            $"{prefix}\t\tbreak";

        public string GenerateOutputCode(Output output, string prefix = "")
            => $"{prefix}print({output.Variable})";

        public string GenerateExceptionCode(ExceptionStatement exception, string prefix = "")
            => $"{prefix}raise Exception()";

        private string GetCondition(TypeCondition condition, bool isTrue = true)
        {
            switch (condition)
            {
                case TypeCondition.Greater:
                    return isTrue ? ">" : "<=";
                case TypeCondition.GreaterEqual:
                    return isTrue ? ">=" : "<";
                case TypeCondition.Equal:
                    return isTrue ? "==" : "!=";
                case TypeCondition.LessEqual:
                    return isTrue ? "<=" : ">";
                case TypeCondition.Less:
                    return isTrue ? "<" : ">=";
                default:
                    throw new NotSupportedException($"Condition {condition} is not supported in {GetType().Name}");
            }
        }
    }
}
