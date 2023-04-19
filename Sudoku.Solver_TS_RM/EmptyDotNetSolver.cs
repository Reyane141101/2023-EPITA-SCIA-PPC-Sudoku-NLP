using Python.Deployment;
using Python.Runtime;
using Sudoku.Shared;

namespace Sudoku.DemoSolver
{
	public class EmptyDotNetSolver:ISudokuSolver
	{
		public SudokuGrid Solve(SudokuGrid s)
		{
			Installer.SetupPython();
			Runtime.PythonDLL = "/nix/store/hhk4wr7hwry854sq69chmrjqyi964p7y-python3-3.10.9/lib/libpython3.so";
			PythonEngine.Initialize();
			
			using (PyModule scope = Py.CreateScope())
         	{
         		// convert the Cells array object to a PyObject
         		PyObject pyCells = s.Cells.ToPython();
 
         		// create a Python variable "instance"
         		scope.Set("instance", pyCells);
 
         		// run the Python script
         		string code = Resource1.EmptyPythonSolver_py;
         		scope.Exec(code);
 
         		//Retrieve solved Sudoku variable
         		var result = scope.Get("r");
 
         		//Convert back to C# object
         		var managedResult = result.As<int[][]>();
         		//var convertesdResult = managedResult.Select(objList => objList.Select(o => (int)o).ToArray()).ToArray();
         		return new Shared.SudokuGrid() { Cells = managedResult };
         	}
		}
	}
}