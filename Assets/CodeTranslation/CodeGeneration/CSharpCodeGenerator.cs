using Assets.CodeTranslation.Nodes;
using System;
using System.Linq;

namespace Assets.CodeTranslation.CodeGeneration
{
    public class CSharpCodeGenerator : ICodeGenerator
    {
        public string GenerateProgramCode(Program program, string prefix = "") => 
            $"{prefix}using System;\n" +
            $"{prefix}\n" +
            $"{prefix}namespace ConsoleApp\n" +
            $"{prefix}{{\n" +
            $"{prefix}\tclass Program\n" +
            $"{prefix}\t{{\n" +
            $"{prefix}\t\tstatic void Main(string[] args)\n" +
            $"{prefix}\t\t{{\n" +
            string.Join("", program.Statements.Select(s => $"{s.GetCodeRepresentation(this, prefix: $"{prefix}\t\t\t")}\n")) +
            $"{prefix}\t\t}}\n" +
            $"{prefix}\t}}\n" +
            $"{prefix}}}";

        public string GenerateVariableDeclarationCode(VariableDeclaration variableDeclaration, string prefix = "")
            => $"{prefix}int {variableDeclaration.Variable} = {variableDeclaration.Value};";

        public string GenerateAdditionCode(Addition addition, string prefix = "")
            => $"{prefix}{addition.Variable1} += {addition.Variable2};";

        public string GenerateSubtractionCode(Subtraction subtraction, string prefix = "")
            => $"{prefix}{subtraction.Variable1} -= {subtraction.Variable2};";

        public string GenerateMultiplicationCode(Multiplication multiplication, string prefix = "")
            => $"{prefix}{multiplication.Variable1} *= {multiplication.Variable2};";

        public string GenerateIfCode(If @if, string prefix = "") =>
            $"{prefix}if ({@if.Variable1} {GetCondition(@if.Condition)} {@if.Variable2})\n" +
            $"{prefix}{{\n" +
            string.Join("", @if.TrueStatements.Select(s => $"{s.GetCodeRepresentation(this, prefix: $"{prefix}\t")}\n")) +
            $"{prefix}}}\n" +
            $"{prefix}else\n" +
            $"{prefix}{{\n" +
            string.Join("", @if.FalseStatements.Select(s => $"{s.GetCodeRepresentation(this, prefix: $"{prefix}\t")}\n")) +
            $"{prefix}}}";

        public string GenerateWhileCode(While @while, string prefix = "") =>
            $"{prefix}while ({@while.Variable1} {GetCondition(@while.Condition, isTrue: @while.IsTrue)} {@while.Variable2})\n" +
            $"{prefix}{{\n" +
            string.Join("", @while.Statements.Select(s => $"{s.GetCodeRepresentation(this, prefix: $"{prefix}\t")}\n")) +
            $"{prefix}}}";

        public string GenerateDoWhileCode(DoWhile doWhile, string prefix = "") =>
            $"{prefix}do\n" +
            $"{prefix}{{\n" +
            string.Join("", doWhile.Statements.Select(s => $"{s.GetCodeRepresentation(this, prefix: $"{prefix}\t")}\n")) +
            $"{prefix}}}\n" +
            $"{prefix}while ({doWhile.Variable1} {GetCondition(doWhile.Condition, isTrue: doWhile.IsTrue)} {doWhile.Variable2})";

        public string GenerateOutputCode(Output output, string prefix = "")
            => $"{prefix}Console.WriteLine({output.Variable});";

        public string GenerateExceptionCode(ExceptionStatement exception, string prefix = "")
            => $"{prefix}throw new Exception();";

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
