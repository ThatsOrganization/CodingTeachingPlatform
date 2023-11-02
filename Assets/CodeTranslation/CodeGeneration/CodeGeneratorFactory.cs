using UnityEngine;

namespace Assets.CodeTranslation.CodeGeneration
{
    public abstract class CodeGeneratorFactory : MonoBehaviour
    {
        public abstract ICodeGenerator GetCodeGenerator(CodeLanguage language);
    }
}
