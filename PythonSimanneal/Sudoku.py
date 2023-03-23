import math
import random
from simanneal import Annealer
import numpy as np

startingSudoku = """
                   -------------------------------
| 9  0  2 | 0  0  5 | 4  0  3 | 
|         |         |         | 
| 1  0  0 | 0  6  3 | 0  2  5 | 
|         |         |         | 
| 5  0  8 | 4  0  7 | 0  6  0 | 
-------------------------------
| 0  2  6 | 3  0  9 | 0  0  1 | 
|         |         |         | 
| 0  5  7 | 0  1  0 | 2  9  0 | 
|         |         |         | 
| 0  9  0 | 6  7  0 | 5  3  0 | 
-------------------------------
| 2  4  0 | 5  3  0 | 6  0  0 | 
|         |         |         | 
| 7  0  5 | 2  0  0 | 3  0  4 | 
|         |         |         | 
| 0  8  0 | 0  4  1 | 9  5  0 | 
-------------------------------
                """

sudoku = np.array([[int(i) for i in line] for line in startingSudoku.replace("-", "").replace("|", "").replace(" ", "").split()])

def PrintSudoku(sudoku):
    print("\n")
    for i in range(len(sudoku)):
        line = ""
        if i == 3 or i == 6:
            print("---------------------")
        for j in range(len(sudoku[i])):
            if j == 3 or j == 6:
                line += "| "
            line += str(sudoku[i,j])+" "
        print(line)

def FixSudokuValues(fixed_sudoku):
    for i in range (0,9):
        for j in range (0,9):
            if fixed_sudoku[i,j] != 0:
                fixed_sudoku[i,j] = 1
    
    return(fixed_sudoku)

# Cost Function    
def CalculateNumberOfErrors(sudoku):
    numberOfErrors = 0 
    for i in range (0,9):
        numberOfErrors += CalculateNumberOfErrorsRowColumn(i ,i ,sudoku)
    return(numberOfErrors)

def CalculateNumberOfErrorsRowColumn(row, column, sudoku):
    numberOfErrors = (9 - len(np.unique(sudoku[:,column]))) + (9 - len(np.unique(sudoku[row,:])))
    return(numberOfErrors)


def CreateList3x3Blocks ():
    finalListOfBlocks = []
    for r in range (0,9):
        tmpList = []
        block1 = [i + 3*((r)%3) for i in range(0,3)]
        block2 = [i + 3*math.trunc((r)/3) for i in range(0,3)]
        for x in block1:
            for y in block2:
                tmpList.append([x,y])
        finalListOfBlocks.append(tmpList)
    return(finalListOfBlocks)

def RandomlyFill3x3Blocks(sudoku, listOfBlocks):
    for block in listOfBlocks:
        for box in block:
            if sudoku[box[0],box[1]] == 0:
                currentBlock = sudoku[block[0][0]:(block[-1][0]+1),block[0][1]:(block[-1][1]+1)]
                sudoku[box[0],box[1]] = random.choice([i for i in range(1,10) if i not in currentBlock])
    return sudoku

def SumOfOneBlock (sudoku, oneBlock):
    finalSum = 0
    for box in oneBlock:
        finalSum += sudoku[box[0], box[1]]
    return(finalSum)

def TwoRandomBoxesWithinBlock(fixedSudoku, block):
    while (1):
        firstBox = random.choice(block)
        secondBox = random.choice([box for box in block if box is not firstBox ])

        if fixedSudoku[firstBox[0], firstBox[1]] != 1 and fixedSudoku[secondBox[0], secondBox[1]] != 1:
            return([firstBox, secondBox])

def FlipBoxes(sudoku, boxesToFlip):
    proposedSudoku = np.copy(sudoku)
    placeHolder = proposedSudoku[boxesToFlip[0][0], boxesToFlip[0][1]]
    proposedSudoku[boxesToFlip[0][0], boxesToFlip[0][1]] = proposedSudoku[boxesToFlip[1][0], boxesToFlip[1][1]]
    proposedSudoku[boxesToFlip[1][0], boxesToFlip[1][1]] = placeHolder
    return (proposedSudoku)

def ProposedState (sudoku, fixedSudoku, listOfBlocks):
    randomBlock = random.choice(listOfBlocks)

    if SumOfOneBlock(fixedSudoku, randomBlock) > 6:  
        return(sudoku, 1, 1)
    boxesToFlip = TwoRandomBoxesWithinBlock(fixedSudoku, randomBlock)
    proposedSudoku = FlipBoxes(sudoku,  boxesToFlip)
    return([proposedSudoku, boxesToFlip])

class Sudoku:
    
    def __init__(self, sudoku):
        self.fixedSudoku = np.copy(sudoku)
        self.listBlocks = CreateList3x3Blocks()
        self.currentSudoku = RandomlyFill3x3Blocks(sudoku, self.listBlocks)


class SudokuProblem(Annealer):

    def __init__(self, state):
       super(SudokuProblem, self).__init__(state)
    
    def move(self):
        initial_energy = self.energy()
        newState = ProposedState(self.state.currentSudoku, self.state.fixedSudoku, self.state.listBlocks)
        self.state.currentSudoku = newState[0]

        return self.energy() - initial_energy
    
    def energy(self):
        return CalculateNumberOfErrors(self.state.currentSudoku)
    

if __name__ == '__main__':

    PrintSudoku(sudoku)
    initial_state = Sudoku(sudoku)
    SdkPb = SudokuProblem(initial_state)
    SdkPb.Tmax = 10
    SdkPb.set_schedule(SdkPb.auto(minutes=0.2))
    # since our state is just a list, slice is the fastest way to copy
    SdkPb.copy_strategy = "deepcopy"
    state, e = SdkPb.anneal()

    PrintSudoku(state.currentSudoku)
