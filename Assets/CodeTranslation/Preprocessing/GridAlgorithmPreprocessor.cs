using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.CodeTranslation.Preprocessing
{
    public class GridAlgorithmPreprocessor : MonoBehaviour
    {
        public IReadOnlyList<Cycle> FindCycles(GridPlaneController grid)
        {
            var topologicalOrder = TopologicalSort(grid);

            var conditionBlocks = grid.GetBlocks<ConditionBlock>();
            List<List<int>> used = null;
            FunctionalBlock endBlock;
            int cycleNum;
            int? startTopologicalOrder;
            int endTopologicalOrder;

            void ResetData(FunctionalBlock newEndBlock)
            {
                if (used == null)
                {
                    used = grid.GridCells.Select(row => Enumerable.Repeat(0, row.Length).ToList()).ToList();
                }
                else
                {
                    foreach (var list in used)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            list[i] = 0;
                        }
                    }
                }

                endBlock = newEndBlock;
                cycleNum = 1;
                startTopologicalOrder = null;
                endTopologicalOrder = topologicalOrder[endBlock.X][endBlock.Y];
            }

            bool DFS(int x, int y, List<Stack<CycleItem>> cycles)
            {
                if (endBlock.X == x && endBlock.Y == y)
                {
                    cycleNum++;
                    return true;
                }
                var block = grid.GridCells[x][y].Block;
                if (block == FunctionalBlock.EmptyBlock || block.BlockType == TypeBlock.Finish)
                {
                    return false;
                }
                if (block.BlockType != TypeBlock.Transition)
                {
                    if (!startTopologicalOrder.HasValue)
                    {
                        startTopologicalOrder = topologicalOrder[x][y];
                    }
                    if ((startTopologicalOrder.Value - endTopologicalOrder) *
                        (topologicalOrder[x][y] - endTopologicalOrder) < 0)
                    {
                        return false;
                    }
                }
                used[x][y] = cycleNum;
                var foundCycles = false;
                int xNext, yNext;
                List<Stack<CycleItem>> additionalCycles;
                if (block is ConditionBlock conditionBlock)
                {
                    xNext = x + conditionBlock.FalseDirection.GetXOffset();
                    yNext = y + conditionBlock.FalseDirection.GetYOffset();
                    if (grid.IsInRange(xNext, yNext) &&
                        used[xNext][yNext] < cycleNum &&
                        DFS(xNext, yNext, additionalCycles = cycles
                            .Select(cycle => new Stack<CycleItem>(cycle))
                            .ToList())
                        )
                    {
                        foundCycles = true;
                        foreach (var cycle in additionalCycles)
                        {
                            cycle.Push(new CycleItem { X = x, Y = y });
                        }
                    }
                    else
                    {
                        additionalCycles = new List<Stack<CycleItem>>();
                    }
                    used[x][y] = cycleNum;
                }
                else
                {
                    additionalCycles = new List<Stack<CycleItem>>();
                }
                xNext = x + block.Direction.GetXOffset();
                yNext = y + block.Direction.GetYOffset();
                if (grid.IsInRange(xNext, yNext) && used[xNext][yNext] < cycleNum && DFS(xNext, yNext, cycles))
                {
                    foundCycles = true;
                    foreach (var cycle in cycles)
                    {
                        cycle.Push(new CycleItem { X = x, Y = y });
                    }
                }
                cycles.AddRange(additionalCycles);
                return foundCycles;
            }

            var allCycles = new List<Cycle>();

            foreach (var block in conditionBlocks)
            {
                ResetData(block);
                var trueCycles = new List<Stack<CycleItem>> { new Stack<CycleItem>() };
                var xTrue = block.X + block.Direction.GetXOffset();
                var yTrue = block.Y + block.Direction.GetYOffset();
                var foundTrueCycles = grid.IsInRange(xTrue, yTrue) && DFS(xTrue, yTrue, trueCycles);

                ResetData(block);
                var falseCycles = new List<Stack<CycleItem>> { new Stack<CycleItem>() };
                var xFalse = block.X + block.FalseDirection.GetXOffset();
                var yFalse = block.Y + block.FalseDirection.GetYOffset();
                var foundFalseCycles = grid.IsInRange(xFalse, yFalse) && DFS(xFalse, yFalse, falseCycles);

                if (foundTrueCycles ^ foundFalseCycles)
                {
                    var foundCycles = foundTrueCycles ? trueCycles : falseCycles;
                    foreach (var stack in foundCycles)
                    {
                        var items = new List<CycleItem> { new CycleItem { X = block.X, Y = block.Y } };
                        while (stack.Count > 0)
                        {
                            items.Add(stack.Pop());
                        }
                        var secondIndex = items.FindIndex(
                            1,
                            item => grid.GridCells[item.X][item.Y].Block.BlockType != TypeBlock.Transition);
                        bool isPrecondition;
                        if (secondIndex < 0)
                        {
                            isPrecondition = true;
                        }
                        else
                        {
                            var firstOrder = topologicalOrder[items[0].X][items[0].Y];
                            var secondOrder = topologicalOrder[items[secondIndex].X][items[secondIndex].Y];
                            isPrecondition = firstOrder < secondOrder;
                        }
                        allCycles.Add(new Cycle
                        {
                            Items = items,
                            IsTrue = foundTrueCycles,
                            IsPrecondition = isPrecondition
                        });
                    }
                }
            }

            return allCycles;
        }

        private IReadOnlyList<IReadOnlyList<int>> TopologicalSort(GridPlaneController grid)
        {
            var order = grid.GridCells.Select(row => Enumerable.Repeat(0, row.Length).ToList()).ToList();
            var queue = new Queue<(int x, int y)>();

            void DFS(int x, int y)
            {
                var block = grid.GridCells[x][y].Block;
                if (block == FunctionalBlock.EmptyBlock)
                {
                    return;
                }
                order[x][y] = -1;
                if (block.BlockType != TypeBlock.Finish)
                {
                    var xNext = x + block.Direction.GetXOffset();
                    var yNext = y + block.Direction.GetYOffset();
                    if (grid.IsInRange(xNext, yNext) && order[xNext][yNext] == 0)
                    {
                        DFS(xNext, yNext);
                    }
                    if (block is ConditionBlock conditionBlock)
                    {
                        xNext = x + conditionBlock.FalseDirection.GetXOffset();
                        yNext = y + conditionBlock.FalseDirection.GetYOffset();
                        if (grid.IsInRange(xNext, yNext) && order[xNext][yNext] == 0)
                        {
                            DFS(xNext, yNext);
                        }
                    }
                }
                queue.Enqueue((x, y));
            }

            var startBlock = grid.GetBlockByType(TypeBlock.Start);
            DFS(startBlock.X, startBlock.Y);
            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();
                order[x][y] = queue.Count + 1;
            }

            return order;
        }
    }
}
