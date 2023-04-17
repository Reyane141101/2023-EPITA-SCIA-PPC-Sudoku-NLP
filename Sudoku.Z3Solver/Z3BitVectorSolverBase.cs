using Microsoft.Z3;
using Sudoku.Shared;

namespace Sudoku.Z3Solver;



// Exemple en utilisant l'API de substitution

public abstract class Z3BitVectorSolverBase : ISudokuSolver
{
	public static Context ctx = new Context();
	public static BoolExpr _GenericContraints;
	public static BitVecExpr[][] CellVariables = new BitVecExpr[9][];

	private static Sort BitVectorSort = ctx.MkBitVecSort(4);

	public static Solver _ReusableSolver;

	public Z3BitVectorSolverBase()
	{

		for (uint i = 0; i < 9; i++)
		{
			CellVariables[i] = new BitVecExpr[9];
			for (uint j = 0; j < 9; j++)
				CellVariables[i][j] = (BitVecExpr)ctx.MkConst(ctx.MkSymbol("x_" + (i + 1) + "_" + (j + 1)), BitVectorSort);
		}
	}

	public static BoolExpr GenericContraints
	{
		get
		{
			if (_GenericContraints == null)
			{
				_GenericContraints = GetGenericConstraints();
			}
			return _GenericContraints;
		}
	}

	public static Solver ReusableSolver
	{
		get
		{
			if (_ReusableSolver == null)
			{
				_ReusableSolver = MakeReusableSolver();
			}
			return _ReusableSolver;
		}
	}

	public static Solver MakeReusableSolver()
	{
		Solver s = ctx.MkSolver();
		s.Assert(GenericContraints);
		return s;
	}

	protected static BitVecExpr GetConstExpr(int value)
	{
		return (BitVecExpr)ctx.MkNumeral(value, BitVectorSort); 
	}

	public static BoolExpr GetGenericConstraints()
	{

		// each cell contains a value in {1, ..., 9}
		Expr[][] cells_c = new Expr[9][];
		for (uint i = 0; i < 9; i++)
		{
			cells_c[i] = new BoolExpr[9];
			for (uint j = 0; j < 9; j++)
				cells_c[i][j] = ctx.MkAnd(ctx.MkBVULE(GetConstExpr(1), CellVariables[i][j]),
					ctx.MkBVULE(CellVariables[i][j], GetConstExpr(9)));
		}



		// each row contains a digit at most once
		BoolExpr[] rows_c = new BoolExpr[9];
		for (uint i = 0; i < 9; i++)
			rows_c[i] = ctx.MkDistinct(CellVariables[i]);

		// each column contains a digit at most once
		BoolExpr[] cols_c = new BoolExpr[9];
		for (uint j = 0; j < 9; j++)
		{
			BitVecExpr[] column = new BitVecExpr[9];
			for (uint i = 0; i < 9; i++)
				column[i] = CellVariables[i][j];

			cols_c[j] = ctx.MkDistinct(column);
		}

		// each 3x3 square contains a digit at most once
		BoolExpr[][] sq_c = new BoolExpr[3][];
		for (uint i0 = 0; i0 < 3; i0++)
		{
			sq_c[i0] = new BoolExpr[3];
			for (uint j0 = 0; j0 < 3; j0++)
			{
				BitVecExpr[] square = new BitVecExpr[9];
				for (uint i = 0; i < 3; i++)
				for (uint j = 0; j < 3; j++)
					square[3 * i + j] = CellVariables[3 * i0 + i][3 * j0 + j];
				sq_c[i0][j0] = ctx.MkDistinct(square);
			}
		}

		BoolExpr sudoku_c = ctx.MkTrue();
		foreach (BoolExpr[] t in cells_c)
			sudoku_c = ctx.MkAnd(ctx.MkAnd(t), sudoku_c);
		sudoku_c = ctx.MkAnd(ctx.MkAnd(rows_c), sudoku_c);
		sudoku_c = ctx.MkAnd(ctx.MkAnd(cols_c), sudoku_c);
		foreach (BoolExpr[] t in sq_c)
			sudoku_c = ctx.MkAnd(ctx.MkAnd(t), sudoku_c);


		// Fin des contraintes "génériques"
		return sudoku_c;

	}

	public BoolExpr GetPuzzleConstraints(Shared.SudokuGrid grid)
	{
		BoolExpr instance_c = ctx.MkTrue();
		for (uint i = 0; i < 9; i++)
		for (uint j = 0; j < 9; j++)
			if (grid.Cells[i][j] != 0)
			{
				instance_c = ctx.MkAnd(instance_c,
					(BoolExpr)
					ctx.MkEq(CellVariables[i][j], GetConstExpr(grid.Cells[i][j])));
			}

		return instance_c;
	}

	public abstract SudokuGrid Solve(SudokuGrid s);
}