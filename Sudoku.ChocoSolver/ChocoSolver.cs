using Sudoku.Shared;
using sudoku.chocosolver;

namespace Sudoku.ChocoSolver;

// Je vous remets les commentaires d'une conversation Discord: J'imagine que comme dans l'exemple du cours on doit pouvoir paramétrer tout un tas de combinaisons d'heuristiques (j'ai vu passer du code en ce sens) , d'inference et sans doute le back jumping. l'exemple suivant (https://github.com/chocoteam/samples/blob/master/src/main/java/org/chocosolver/samples/integer/Sudoku.java) a l'air aussi de configurer directement l'exploration. Et puis il y a la question de la reutilisabilité comme par la supstitution des cellules du masque par des constantes que vos camarades testent dans Z3.




public class ChocoSolver : ISudokuSolver
{
	public SudokuGrid Solve(SudokuGrid s)
	{
		var toSolve = s.CloneSudoku();
		var javaSolver = new sudoku.chocosolver.SudokuCP(toSolve.Cells);
		

		Console.WriteLine("Choose a strategy :\n1 - Default\n2 - InputOrderLB\n3 - MinDomLB\n4 - ActivityBased\n5 - DomOverWDeg");
		string strategie = Console.ReadLine();
		
		switch (strategie)
		{
			case "1":
				javaSolver.solveDefault();
				break;
			
			case "2":
				javaSolver.solveInputOrderLB();
				break;

			case "3":
				javaSolver.solveMinDomLB();
				break;
			
			case "4":
				javaSolver.solveActivityBased();
				break;
			
			case "5":
				javaSolver.solveDomOverWDeg();
				break;

			default:
				Console.WriteLine("Please select a valid strategy.");
				break;
		}

		return toSolve;
	}
}
