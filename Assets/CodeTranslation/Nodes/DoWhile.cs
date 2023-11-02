using Assets.CodeTranslation.CodeGeneration;
using Assets.CodeTranslation.Nodes;
using System.Collections.Generic;
using System.Linq;

public class DoWhile : BinaryOperator
{
    public DoWhile(string variable1, string variable2, TypeCondition condition, bool isTrue, IEnumerable<IStatement> statements) : base(variable1, variable2)
    {
        Condition = condition;
        IsTrue = isTrue;
        Statements = statements.ToList();
    }

    public TypeCondition Condition { get; }

    public bool IsTrue { get; }

    public IReadOnlyList<IStatement> Statements { get; }

    public override string GetCodeRepresentation(ICodeGenerator codeGenerator, string prefix = "")
        => codeGenerator.GenerateDoWhileCode(this, prefix);
}
