using Python.Deployment;
using Python.Runtime;
using Sudoku.Shared;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Python.Deployment;
using Python.Runtime;

namespace Sudoku.DlxLib;

public class DlxLibPythonSolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {
        Installer.SetupPython();
        Runtime.PythonDLL = "/nix/store/5axq6aw8j3vcs2m7gi440cwpcckl7ql9-python3-3.10.9/lib/libpython3.10.so";
        PythonEngine.Initialize();
        
        Console.WriteLine(@"===== DlxLix Python Solver =====");
        
        //System.Diagnostics.Debugger.Break();

        //{
        // create a Python scope
        using (PyModule scope = Py.CreateScope())
        {
            // convert the Cells array object to a PyObject
            PyObject pyCells = s.Cells.ToPython();

            // create a Python variable "instance"
            scope.Set("instance", pyCells);

            // run the Python script
            string code = Resources.PythonSolver_py;
            scope.Exec(code);

            //Retrieve solved Sudoku variable
            var result = scope.Get("r");
            Console.WriteLine(result);

            //Convert back to C# object
            var managedResult = result.As<int[][]>();
            //var convertesdResult = managedResult.Select(objList => objList.Select(o => (int)o).ToArray()).ToArray();
            return new Shared.SudokuGrid() { Cells = managedResult };
        }
        //}
    }
}