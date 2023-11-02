using Assets.CodeTranslation.CodeGeneration;
using System.Collections.Generic;
using System.Linq;

namespace Assets.CodeTranslation.Nodes
{
    public class Program : INode
    {
        public Program(IEnumerable<IStatement> statements)
        {
            Statements = statements.ToList();
        }

        public IReadOnlyList<IStatement> Statements { get; }

        public string GetCodeRepresentation(ICodeGenerator codeGenerator, string prefix = "")
            => codeGenerator.GenerateProgramCode(this, prefix);
    }
}
