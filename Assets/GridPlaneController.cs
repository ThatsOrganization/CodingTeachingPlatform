using Assets.CodeTranslation.Preprocessing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridPlaneController : MonoBehaviour
{
    [SerializeField]
    private GridAlgorithmPreprocessor _algorithmPreprocessor;

    private IReadOnlyList<Cycle> _cycles;

    private FunctionalBlock _selectedBlock;

    private readonly HashSet<CycleItem> _outlinedItems = new HashSet<CycleItem>();

    public GridCell[][] GridCells { get; set; }

    public GridCell GridCellAdditional { get; set; }

    public WorkspaceController Workspace { get; set; }

    public IReadOnlyList<Cycle> Cycles => _cycles;

    public int FreeCellsCount => GridCells.Sum(rowCells => rowCells.Count(cell => cell.Block == FunctionalBlock.EmptyBlock));

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if ((hits = Physics.RaycastAll(ray))
                .Any(h => h.transform.CompareTag("FunctionalBlock")))
            {
                var hit = Physics.RaycastAll(ray).FirstOrDefault(h => h.transform.CompareTag("FunctionalBlock"));
                var block = hit.transform.GetComponent<FunctionalBlock>();
                if (block == FunctionalBlock.EmptyBlock) return;
                if (_selectedBlock != FunctionalBlock.EmptyBlock)
                {
                    _selectedBlock.DisableOutline();
                }
                _selectedBlock = block;
                UpdateCycleOutlining();
                _selectedBlock.EnableSelectionOutline();
                _selectedBlock.StartDragging();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_selectedBlock != FunctionalBlock.EmptyBlock)
            {
                _selectedBlock.StopDragging();
            }
        }
    }

    public void UpdateCycles()
    {
        _cycles = _algorithmPreprocessor.FindCycles(this);
        UpdateCycleOutlining();
    }

    public bool IsInRange(int x, int y)
    {
        return IsInXRange(x) && IsInYRange(y);
    }

    public bool IsInXRange(int x)
    {
        return x >= 0 && x < GridCells.Length;
    }

    public bool IsInYRange(int y)
    {
        return y >= 0 && y < GridCells[0].Length;
    }

    public List<GridCell> GetFreeCells(int count)
    {
        var freeCells = new List<GridCell>();
        for (int i = 0; i < GridCells.Length; i++)
            for (int j = 0; j < GridCells.FirstOrDefault()?.Length; j++)
                if (GridCells[i][j].Block == FunctionalBlock.EmptyBlock)
                {
                    freeCells.Add(GridCells[i][j]);
                    if (freeCells.Count == count) return freeCells;
                }
        return freeCells;
    }

    public GridCell GetFreeCell()
    {
        if (FreeCellsCount > 0) return GetFreeCells(1).First();
        return null;
    }

    public T GetBlock<T>() where T : FunctionalBlock
    {
        return GetBlocksInternal<T>().FirstOrDefault();
    }

    public IReadOnlyList<T> GetBlocks<T>() where T : FunctionalBlock
    {
        return GetBlocksInternal<T>().ToList();
    }

    public FunctionalBlock GetBlockByType(TypeBlock type)
    {
        return GetBlocksByTypeInternal(type).FirstOrDefault();
    }

    public IReadOnlyList<FunctionalBlock> GetBlocksByType(TypeBlock type)
    {
        return GetBlocksByTypeInternal(type).ToList();
    }

    private IEnumerable<FunctionalBlock> GetBlocksByTypeInternal(TypeBlock type)
    {
        return GridCells
            .SelectMany(cells => cells)
            .Where(cell => cell.Block != FunctionalBlock.EmptyBlock && cell.Block.BlockType == type)
            .Select(cell => cell.Block);
    }

    private IEnumerable<T> GetBlocksInternal<T>() where T : FunctionalBlock
    {
        return GridCells
            .SelectMany(cells => cells)
            .Select(cell => cell.Block)
            .OfType<T>();
    }

    private void UpdateCycleOutlining()
    {
        foreach (var block in _outlinedItems
            .Select(item => GridCells[item.X][item.Y].Block)
            .Where(block => block != FunctionalBlock.EmptyBlock))
        {
            block.DisableOutline();
        }
        _outlinedItems.Clear();
        if (_selectedBlock == FunctionalBlock.EmptyBlock)
        {
            return;
        }
        foreach (var cycle in _cycles
            .Where(cycle => cycle.Items.Any(item => _selectedBlock.X == item.X && _selectedBlock.Y == item.Y)))
        {
            var conditionItem = cycle.Items[0];
            if ((_selectedBlock.X != conditionItem.X || _selectedBlock.Y != conditionItem.Y) &&
                GridCells[conditionItem.X][conditionItem.Y].Block is ConditionBlock conditionBlock)
            {
                conditionBlock.EnableCycleConditionOutline();
                _outlinedItems.Add(conditionItem);
            }
            foreach (var item in cycle.Items
                .Skip(1)
                .Where(item => _selectedBlock.X != item.X || _selectedBlock.Y != item.Y))
            {
                var block = GridCells[item.X][item.Y].Block;
                if (block != FunctionalBlock.EmptyBlock)
                {
                    if (cycle.IsTrue)
                    {
                        block.EnableTrueCycleOutline();
                    }
                    else
                    {
                        block.EnableFalseCycleOutline();
                    }
                    _outlinedItems.Add(item);
                }
            }
        }
    }
}
