using Assets.CodeTranslation.CodeGeneration;
using System.Collections.Generic;
using System.Linq;

namespace Assets.CodeTranslation.Nodes
{
    public class If : BinaryOperator
    {
        public If(
            string variable1,
            string variable2,
            TypeCondition condition,
            IEnumerable<IStatement> trueStatements,
            IEnumerable<IStatement> falseStatements)
            : base(variable1, variable2)
        {
            Condition = condition;
            TrueStatements = trueStatements.ToList();
            FalseStatements = falseStatements.ToList();
        }

        public TypeCondition Condition { get; }

        public IReadOnlyList<IStatement> TrueStatements { get; }

        public IReadOnlyList<IStatement> FalseStatements { get; }

        public override string GetCodeRepresentation(ICodeGenerator codeGenerator, string prefix = "")
            => codeGenerator.GenerateIfCode(this, prefix);
    }
}
