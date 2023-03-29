import numpy as np

class Dlx:
    def __init__(self, grid):
        univers = np.asarray([1, 2, 3, 4, 5, 6, 7, 8, 9])
        size = len(univers)

        row_col = np.zeros(())
        row_number = np.asarray([])
        col_number = np.asarray([])
        box_number = np.asarray([])
        
        for r in range(0, len(univers)):
            for c in range(0, len(univers)):

