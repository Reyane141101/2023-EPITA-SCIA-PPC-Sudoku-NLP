from sklearn.model_selection import ParameterGrid
import numpy as np
import pandas as pd



class DlxCustom:
    matrix = np.asarray([])
    solved = np.asarray([])
    
    def __init__(self, grid):
        solved = np.asarray(grid)
        digits = [1, 2, 3, 4, 5, 6, 7, 8, 9]
        rows = 9
        cols = 9
        boxes = 9
        possibilities = rows * cols * digits

        rows_cols_constraint = rows * cols
        rows_digits_constraint = rows * len(digits)
        cols_digits_constraint = cols * len(digits)
        boxes_digits_constraint = boxes * len(digits)

        matrix = np.zeros((possibilities,
                    rows_cols_constraint + cols_digits_constraint + rows_digits_constraint + boxes_digits_constraint))

        selfmatrix = []

        for r in range(0, len(digits)):
            for c in range(0, len(digits)):
                digit = grid[r][c]

                #if digit != 0:
                #    possibility = r * digits + c + digit
                #    matrix[possibility][81 * 0 + r * digits + c] = 1                   # rows_cols
                #    matrix[possibility][81 + 1 + r * digits + digit] = 1               # rows_digits
                #    matrix[possibility][81 * 2 + c * digits + digit] = 1               # cols_numbers
                #    matrix[possibility][int(81 * 3 + (r + c / 3) + digit)] = 1         # boxes_numbers


def solve():
    print("I'm solving the sudoku...")
    solver = DlxCustom(instance)
    return instance


r = solve()
