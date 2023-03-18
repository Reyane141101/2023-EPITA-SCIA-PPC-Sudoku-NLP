using System.Diagnostics.CodeAnalysis;
using SimnOpt.Heuristics.SimAn;
using Sudoku.Shared;

namespace Sudoku.SimulatedAnnealing;

/// <summary>
/// SA solver using SimulatedAnnealingDotNet library 
/// </summary>
public class SimulatedAnnealingDotNetOptimizedSolver: ISudokuSolver
{
    public SASudokuGrid GenerateNeighbour(ISimAnSolution baseSolution)
    {
        if (!(baseSolution is SASudokuGrid sudokuSol))
            return null;
        // generate a copy of the existing assignment
        SASudokuGrid neighbor = new SASudokuGrid();
        neighbor.Sudoku = sudokuSol.Sudoku.CloneSudoku();
        neighbor.CreateNewState();
        neighbor.CalculateFitness();
        return neighbor;
    }
    
    public SASudokuGrid GenerateStartSolution()
    {
        SASudokuGrid startSolution = new SASudokuGrid();
        startSolution.FillEmptyValues();
        startSolution.CalculateFitness();
        return startSolution;
    }

    public SudokuGrid Solve(SudokuGrid s)
    {
        SASudokuGrid.Original = s.CloneSudoku();
        
        SimAnHeurParams saParams = new SimAnHeurParams(defaultStepSize: 1, defaultCoolDown: 0.9, startTemp: 20);
        saParams.GenerateNeighborSolution = GenerateNeighbour;
        saParams.MaxIter = (int)Math.Pow(10,6);
        //optimize
        ISimAnSolution startSolution = GenerateStartSolution();
        SimAnHeuristic heuristic = new SimAnHeuristic(saParams, startSolution);
        SimAnOutput output = heuristic.Minimize();

        Console.WriteLine(output.ToString());
        return (output.BestSolution as SASudokuGrid)?.Sudoku;
    }
}