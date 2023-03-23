import java.util.ArrayList;

import org.chocosolver.solver.Model;

public class SudokuCP extends Object {
    public static void PrintCells(int[][] cells) {
        String cellsString = "";
        for (int i = 0; i < cells.length; i++) {
            for (int j = 0; j < cells[i].length; j++) {
                cellsString += cells[i][j];
            }
        }
        System.out.println(cellsString);
    }

    private Model model;
    private int[][] sudoku;
    private int[][] validEntries = {1,2,3,4,5,6,7,8,9};

    public SudokuCP(int[][] sudoku) {
        this.sudoku = sudoku;
        model = new Model();

        final var dimensionality = 3;
        var side = dimensionality * dimensionality;

        var vs = model.intVarMatrix(side, side, 1, validEntries.length);

        for (int i = 0; i < side; i++) {
            for (int j = 0; j < side; j++) {
                var c = sudoku[i][j];
                if (c != 0) {
                    model.arithm(vs[i][j], "=", c).post();
                }
            }
        }

        for (int i = 0; i < side; i++) {
            var row = new IntVar[side];
            var column = new IntVar[side];
            for (int j = 0; j < side; j++) {
                row[j] = vs[i][j];
                column[j] = vs[j][i];
            }
            model.allDifferent(row).post();
            model.allDifferent(column).post();
        }

        for (int i = 0; i < dimensionality; i++) {
            for (int j = 0; j < dimensionality; j++) {
                var section = new IntVar[side];
                int idx = 0;
                for (int xoff = 0; xoff < dimensionality; xoff++) {
                    for (int yoff = 0; yoff < dimensionality; yoff++) {
                        section[idx++] = vs[i * dimensionality + xoff][j * dimensionality + yoff];
                    }
                }
                model.allDifferent(section).post();
            }
        }
    }

    public boolean solve() {
        var solved = model.getSolver().solve();
        if (!solved) {
            return false;
        }

        var sol = new ArrayList<ArrayList<Integer>>(side);
        for (int i = 0; i < side; i++) {
            var row = new ArrayList<Integer>();
            for (int j = 0; j < side; j++) {
                row.add(vs[i][j] - 1);
            }
            sol.add(row);
        }

        var moreSolutions = model.getSolver().solve();
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
}