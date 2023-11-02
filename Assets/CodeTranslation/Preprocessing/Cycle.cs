using System.Collections.Generic;

namespace Assets.CodeTranslation.Preprocessing
{
    public class Cycle
    {
        public IReadOnlyList<CycleItem> Items { get; set; }

        public bool IsTrue { get; set; }

        public bool IsPrecondition { get; set; }
    }
}
