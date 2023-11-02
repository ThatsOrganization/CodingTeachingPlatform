using System;

namespace Assets.CodeTranslation.CodeGeneration
{
    public class CodeGeneratorFactoryCustom : CodeGeneratorFactory
    {
        public override ICodeGenerator GetCodeGenerator(CodeLanguage language)
        {
            switch (language)
            {
                case CodeLanguage.CSharp:
                    return new CSharpCodeGenerator();
                case CodeLanguage.Python:
                    return new PythonCodeGenerator();
                default:
                    throw new NotSupportedException($"Language {language} is not supported in {GetType().Name}");
            }
        }
    }
}
