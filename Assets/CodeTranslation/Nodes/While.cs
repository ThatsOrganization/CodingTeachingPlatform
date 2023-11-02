using Assets.CodeTranslation.CodeGeneration;
using System.Collections.Generic;
using System.Linq;

namespace Assets.CodeTranslation.Nodes
{
    public class While : BinaryOperator
    {
        public While(string variable1, string variable2, TypeCondition condition, bool isTrue, IEnumerable<IStatement> statements) : base(variable1, variable2)
        {
            Condition = condition;
            IsTrue = isTrue;
            Statements = statements.ToList();
        }

        public TypeCondition Condition { get; }

        public bool IsTrue { get; }

        public IReadOnlyList<IStatement> Statements { get; }

        public override string GetCodeRepresentation(ICodeGenerator codeGenerator, string prefix = "")
            => codeGenerator.GenerateWhileCode(this, prefix);
    }
}
