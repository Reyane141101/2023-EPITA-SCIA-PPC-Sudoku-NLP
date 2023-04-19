package sudoku.chocosolver;

import java.util.ArrayList;

import org.chocosolver.solver.Model;
import org.chocosolver.solver.Solution;
import org.chocosolver.solver.search.strategy.Search;
import org.chocosolver.solver.variables.IntVar;
import org.chocosolver.util.tools.ArrayUtils;



public class SudokuCP extends Object {

    private Model model;
    private int[][] sudoku;
    private int[] validEntries = {1,2,3,4,5,6,7,8,9};
    private int side ;
    private IntVar[][] grid;

    public SudokuCP(int[][] sudoku) {
        this.sudoku = sudoku;

        // New model
        model = new Model();

        // Dimensions
        final int dimensionality = 3;
        side = dimensionality * dimensionality;

        // Initialize the grid
        grid = model.intVarMatrix(side, side, 1, validEntries.length);

        // Fill cells from initialized grid
        for (int i = 0; i < side; i++) {
            for (int j = 0; j < side; j++) {
                int c = sudoku[i][j];
                if (c != 0) {
                    model.arithm(grid[i][j], "=", c).post();
                }
            }
        }

        // Constraint on rows and columns
        for (int i = 0; i < side; i++) {
            IntVar[] row = new IntVar[side];
            IntVar[] column = new IntVar[side];
            for (int j = 0; j < side; j++) {
                row[j] = grid[i][j];
                column[j] = grid[j][i];
            }
            model.allDifferent(row).post();
            model.allDifferent(column).post();
        }

        // Constraint on sections
        for (int i = 0; i < dimensionality; i++) {
            for (int j = 0; j < dimensionality; j++) {
                IntVar[] section = new IntVar[side];
                int idx = 0;
                for (int xoff = 0; xoff < dimensionality; xoff++) {
                    for (int yoff = 0; yoff < dimensionality; yoff++) {
                        section[idx++] = grid[i * dimensionality + xoff][j * dimensionality + yoff];
                    }
                }
                model.allDifferent(section).post();
            }
        }
    }

    private boolean buildSolution() {
        boolean solved = model.getSolver().solve();

        if (!solved) {
            return false;
        }

        // Get the solution
        ArrayList<ArrayList<Integer>> sol = new ArrayList<ArrayList<Integer>>(side);
        for (int i = 0; i < side; i++) {
            ArrayList<Integer> row = new ArrayList<Integer>();
            for (int j = 0; j < side; j++) {
                row.add(grid[i][j].getValue() - 1);
            }
            sol.add(row);
        }

        // Search for more solutions
        boolean moreSolutions = model.getSolver().solve();
        if (moreSolutions) {
            return false;
        }
        
        for (int i = 0; i <side; i++) {
            for (int j = 0; j < side; j++) {
                this.sudoku[i][j] = validEntries[sol.get(i).get(j)];
            }
        }
        return true;
    }

    public boolean solveDefault() {
        return buildSolution();
    }

    public boolean solveInputOrderLB() {
        model.getSolver().setSearch(Search.inputOrderLBSearch(ArrayUtils.flatten(grid)));

        return buildSolution();
    }

    public boolean solveRandom() {
        model.getSolver().setSearch(Search.randomSearch(ArrayUtils.flatten(grid), side));

        return buildSolution();
    }

    public boolean solveMinDomLB() {
        model.getSolver().setSearch(Search.minDomLBSearch(ArrayUtils.flatten(grid)));

        return buildSolution();
    }

    public boolean solveActivityBased() {
        model.getSolver().setSearch(Search.activityBasedSearch(ArrayUtils.flatten(grid)));

        return buildSolution();
    }

    public boolean solveDomOverWDeg() {
        model.getSolver().setSearch(Search.domOverWDegSearch(ArrayUtils.flatten(grid)));

        return buildSolution();
    }

}