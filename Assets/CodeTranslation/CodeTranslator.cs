using Assets.CodeTranslation.CodeGeneration;
using Assets.CodeTranslation.Nodes;
using Assets.CodeTranslation.Preprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.CodeTranslation
{
    public class CodeTranslator : MonoBehaviour
    {
        [SerializeField]
        private CodeGeneratorFactory _codeGeneratorFactory;

        public bool TryBuildSyntaxTree(
            GridPlaneController grid,
            IReadOnlyList<Cycle> cycles,
            Runner runner,
            out Program program)
        {
            var ignorePostconditionCycle = false;

            bool TryGetPostconditionCycle(
                FunctionalBlock block,
                out Cycle postconditionCycle,
                out ConditionBlock postconditionBlock)
            {
                postconditionCycle = null;
                postconditionBlock = null;

                if (ignorePostconditionCycle)
                {
                    ignorePostconditionCycle = false;
                    return false;
                }

                if (!(cycles.FirstOrDefault(cycle =>
                {
                    if (cycle.IsPrecondition)
                    {
                        return false;
                    }
                    var firstItem = cycle.Items
                        .Skip(1)
                        .First(item => grid.GridCells[item.X][item.Y].Block.BlockType != TypeBlock.Transition);
                    return firstItem.X == block.X && firstItem.Y == block.Y;
                }) is Cycle postcondCycle))
                {
                    return false;
                }

                var conditionItem = postcondCycle.Items.First();
                if (!(grid.GridCells[conditionItem.X][conditionItem.Y].Block is ConditionBlock postcondBlock))
                {
                    return false;
                }

                postconditionCycle = postcondCycle;
                postconditionBlock = postcondBlock;

                return true;
            }

            bool TryGetPreconditionCycle(ConditionBlock block, out Cycle preconditionCycle)
            {
                if (!(cycles.FirstOrDefault(cycle =>
                {
                    if (!cycle.IsPrecondition)
                    {
                        return false;
                    }
                    var firstItem = cycle.Items.First();
                    return firstItem.X == block.X && firstItem.Y == block.Y;
                }) is Cycle precondCycle))
                {
                    preconditionCycle = null;
                    return false;
                }
                preconditionCycle = precondCycle;
                return true;
            }

            var used = grid.GridCells.Select(row => Enumerable.Repeat(false, row.Length).ToList()).ToList();

            bool TryGetStatements(int xStart, int yStart, int xEnd, int yEnd, out List<IStatement> statements)
                => TryGetStatementsWithCoords(xStart, yStart, xEnd, yEnd, out statements, out _, out _, out _);

            bool TryGetStatementsWithCoords(
                int xStart,
                int yStart,
                int xEnd,
                int yEnd,
                out List<IStatement> statements,
                out List<(int x, int y)> coords,
                out int x,
                out int y)
            {
                x = xStart;
                y = yStart;
                int xPrev, yPrev;
                var outStatements = new List<IStatement>();
                var outCoords = new List<(int, int)>();
                statements = outStatements;
                coords = outCoords;

                void AddStatement(IStatement statement)
                {
                    outStatements.Add(statement);
                    outCoords.Add((xPrev, yPrev));
                }

                while (x != xEnd || y != yEnd)
                {
                    FunctionalBlock block;
                    if (!grid.IsInRange(x, y) || (block = grid.GridCells[x][y].Block) == FunctionalBlock.EmptyBlock)
                    {
                        xPrev = x;
                        yPrev = x;
                        AddStatement(new ExceptionStatement());
                        return true;
                    }
                    if (block.BlockType == TypeBlock.Finish)
                    {
                        return false;
                    }
                    if (used[x][y])
                    {
                        return false;
                    }
                    xPrev = x;
                    yPrev = y;
                    if (TryGetPostconditionCycle(block, out var postconditionCycle, out var postconditionBlock))
                    {
                        ignorePostconditionCycle = true;
                        if (!TryGetStatements(
                            x,
                            y,
                            postconditionBlock.X,
                            postconditionBlock.Y,
                            out var cycleStatements))
                        {
                            return false;
                        }
                        var nextDirection = postconditionCycle.IsTrue ?
                            postconditionBlock.FalseDirection :
                            postconditionBlock.Direction;
                        AddStatement(new DoWhile(
                            postconditionBlock.OperativeField.Name,
                            postconditionBlock.AccessoryField.Name,
                            postconditionBlock.Condition,
                            postconditionCycle.IsTrue,
                            cycleStatements.Where(NotEmpty)));
                        x = postconditionBlock.X + nextDirection.GetXOffset();
                        y = postconditionBlock.Y + nextDirection.GetYOffset();
                    }
                    else if (block is ConditionBlock conditionBlock)
                    {
                        if (TryGetPreconditionCycle(conditionBlock, out var preconditionCycle))
                        {
                            var direction = preconditionCycle.IsTrue ?
                                conditionBlock.Direction :
                                conditionBlock.FalseDirection;
                            if (!TryGetStatements(
                                x + direction.GetXOffset(),
                                y + direction.GetYOffset(),
                                x,
                                y,
                                out var cycleStatements))
                            {
                                return false;
                            }
                            var nextDirection = preconditionCycle.IsTrue ?
                                conditionBlock.FalseDirection :
                                conditionBlock.Direction;
                            AddStatement(new While(
                                conditionBlock.OperativeField.Name,
                                conditionBlock.AccessoryField.Name,
                                conditionBlock.Condition,
                                preconditionCycle.IsTrue,
                                cycleStatements.Where(NotEmpty)));
                            x += nextDirection.GetXOffset();
                            y += nextDirection.GetYOffset();
                        }
                        else
                        {
                            if (!TryGetStatementsWithCoords(
                                conditionBlock.X + conditionBlock.Direction.GetXOffset(),
                                conditionBlock.Y + conditionBlock.Direction.GetYOffset(),
                                xEnd,
                                yEnd,
                                out var trueStatements,
                                out var trueCoords,
                                out _,
                                out _))
                            {
                                return false;
                            }
                            TryGetStatementsWithCoords(
                                conditionBlock.X + conditionBlock.FalseDirection.GetXOffset(),
                                conditionBlock.Y + conditionBlock.FalseDirection.GetYOffset(),
                                xEnd,
                                yEnd,
                                out var falseStatements,
                                out _,
                                out var xStop,
                                out var yStop);
                            var commonIndex = trueCoords.IndexOf((xStop, yStop));
                            if (commonIndex < 0)
                            {
                                return false;
                            }
                            foreach (var (xUsed, yUsed) in trueCoords.Skip(commonIndex))
                            {
                                used[xUsed][yUsed] = false;
                            }
                            AddStatement(new If(
                                conditionBlock.OperativeField.Name,
                                conditionBlock.AccessoryField.Name,
                                conditionBlock.Condition,
                                trueStatements.Take(commonIndex).Where(NotEmpty),
                                falseStatements.Where(NotEmpty)));
                            x = xStop;
                            y = yStop;
                        }
                    }
                    else
                    {
                        switch (block)
                        {
                            case AdditionBlock b:
                                AddStatement(new Addition(b.OperativeField.Name, b.AccessoryField.Name));
                                break;
                            case SubtractionBlock b:
                                AddStatement(new Subtraction(b.OperativeField.Name, b.AccessoryField.Name));
                                break;
                            case MultiplicationBlock b:
                                AddStatement(new Multiplication(b.OperativeField.Name, b.AccessoryField.Name));
                                break;
                            case StorageBlock b:
                                AddStatement(new Output(b.OperativeField.Name));
                                break;
                            default:
                                AddStatement(new EmptyStatement());
                                break;
                        }
                        x += block.Direction.GetXOffset();
                        y += block.Direction.GetYOffset();
                    }
                    used[xPrev][yPrev] = true;
                }
                return true;
            }

            var startBlock = grid.GetBlockByType(TypeBlock.Start);
            var finishBlock = grid.GetBlockByType(TypeBlock.Finish);

            if (!TryGetStatements(startBlock.X, startBlock.Y, finishBlock.X, finishBlock.Y, out var blockStatements))
            {
                program = null;
                return false;
            }

            var variableStatements = runner.Fields.Select(field => new VariableDeclaration(field.Name, field.Value));

            program = new Program(variableStatements.Concat(blockStatements.Where(NotEmpty)));

            return true;
        }

        public string GenerateCode(Program program, CodeLanguage language)
        {
            var codeGenerator = _codeGeneratorFactory.GetCodeGenerator(language);
            var code = program.GetCodeRepresentation(codeGenerator);

            return code;
        }

        private static bool NotEmpty(IStatement statement) => !(statement is EmptyStatement);

        private class EmptyStatement : IStatement
        {
            public string GetCodeRepresentation(ICodeGenerator codeGenerator, string prefix = "")
            {
                throw new InvalidOperationException($"Trying to get code representation of {GetType().Name}");
            }
        }
    }
}
