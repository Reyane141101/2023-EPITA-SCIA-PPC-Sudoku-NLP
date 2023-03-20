public class ChocoSolver {
    public static void PrintCells(int[][] cells) {
        String cellsString = "";
        for (int i = 0; i < cells.length; i++) {
            for (int j = 0; j < cells[i].length; j++) {
                cellsString += cells[i][j];
            }
        }
        System.out.println(cellsString);
    }
}