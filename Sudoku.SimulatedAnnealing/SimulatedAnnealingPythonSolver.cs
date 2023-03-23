using System;
using Python.Runtime;
using Sudoku.Shared;

namespace Sudoku.SimulatedAnnealing;

public class SimulatedAnnealingPythonSolver:PythonSolverBase
{
    public override SudokuGrid Solve(SudokuGrid s)
    {
        PythonEngine.Initialize();

        //System.Diagnostics.Debugger.Break();

        //For some reason, the Benchmark runner won't manage to get the mutex whereas individual execution doesn't cause issues
        // create a Python scope
        using (PyModule scope = Py.CreateScope())
        {
            // convert the Cells array object to a PyObject
            PyObject pyCells = ConverterExtension.ToPython(s.Cells);

            // create a Python variable "instance"
            scope.Set("instance", pyCells);
            // run the Python script
            string code = ResourceSA.Sudoku_py;
            scope.Exec(code);

            //Retrieve solve function from the module
            dynamic solver = scope.Get("solve");
            
            // execute the function and get the result
            PyObject result = solver();
            
            //Convert back to C# object
            int[][] managedResult = ConvertToTwoDimensionalArray(result.As<int[]>());
            
            return new Shared.SudokuGrid() { Cells = managedResult };
        }
        

    }
    
    private int[][] ConvertToTwoDimensionalArray(int[] oneDimensionalArray)
    {
        int[][] twoDimensionalArray = new int[9][];
        int index = 0;
        for (int row = 0; row < 9; row++)
        {
            twoDimensionalArray[row] = new int[9];
            for (int col = 0; col < 9; col++)
            {
                twoDimensionalArray[row][col] = oneDimensionalArray[index];
                index++;
            }
        }
        return twoDimensionalArray;
    }

    protected override void InitializePythonComponents()
    {
        //declare your pip packages here
        InstallPipModule("numpy");
        InstallPipModule("simanneal");
        base.InitializePythonComponents();
    }
}