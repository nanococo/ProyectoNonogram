using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Backtracking class is the main handler for all
/// backtracking code solving the nonogram
/// </summary>
public class Backtracking {

    private readonly Matrix _matrix;

    public Backtracking(Matrix matrix) => _matrix = matrix;

    public Backtracking()
    {
    }

    public bool ExecuteBacktracking() {

        for (var i = 0; i < _matrix.Rows.Count; i++) {
            if (!_matrix.Rows[i].IsCompleteBacktracking()) {
                for (var j = 0; j < _matrix.Rows[i].Cells.Count; j++) {
                    if (_matrix.Rows[i].Cells[j].Mark!="1" && _matrix.Rows[i].Cells[j].Mark!="x" && IsSafe(i, j)) {
                        //_matrix.Rows[i].Cells[j].Mark = "1";
                        _matrix.Rows[i].Cells[j].Confirm();
                        _matrix.Columns[j].Cells[i].Confirm();
                        if (ExecuteBacktracking()) {
                            return true;
                        } else {
                            //_matrix.Rows[i].Cells[j].Mark = "0";
                            _matrix.Rows[i].Cells[j].DeConfirm();
                            _matrix.Columns[j].Cells[i].DeConfirm();
                        }
                    }
                }
            }
        }
        
        var line = "";
        foreach (var matrixRow in _matrix.Rows) {
            foreach (var matrixRowCell in matrixRow.Cells) {
                line += matrixRowCell.Mark;
            }
            line += "\n";
        }
        Debug.Log(line);

        return IsFinished();
        
    }

    private bool IsFinished() {
        var isComplete = false;
        foreach (var matrixRow in _matrix.Rows) {
            isComplete = matrixRow.IsCompleteBacktracking();
        }
        return isComplete;
    }

    private int _clueIndex; //Global variable used in the is safe and valid lock methods.
    private int _globalIndex; //Global index used in the isSafe method chain.
    
    public bool IsSafe(int rowNumber, int cellNumber) {
        var originalMark = _matrix.Rows[rowNumber].Cells[cellNumber].Mark;
        var lineSafe = true;

        //------ROWS FIRST------//
        var processingLine = _matrix.Rows[rowNumber];
        processingLine.MarkCell(cellNumber);
        _globalIndex = 0;
        _clueIndex = 0;
        
        while (true)
        {
            _globalIndex = processingLine.GetNextConfirmedBlockIndex(_globalIndex);
            if (_globalIndex == -1) break;
            lineSafe = ValidBlock(_globalIndex, processingLine);
            if(!lineSafe) break;
        }
        
        processingLine.Cells[cellNumber].Undo(originalMark);
        
        //------COLUMNS SECOND------//
        processingLine = _matrix.Columns[cellNumber];
        processingLine.MarkCell(rowNumber);
        _globalIndex = 0;
        _clueIndex = 0;
        
        while (true)
        {
            _globalIndex = processingLine.GetNextConfirmedBlockIndex(_globalIndex);
            if (_globalIndex == -1) break;
            lineSafe = ValidBlock(_globalIndex, processingLine);
            if(!lineSafe) break;
        }
        
        processingLine.Cells[rowNumber].Undo(originalMark);
        
        
        return lineSafe;
    }
    
    public bool IsSafeForTesting(Line testLine)
    {
        bool lineSafe = true;
        _globalIndex = 0;
        _clueIndex = 0;
        
        while (true)
        {
            _globalIndex = testLine.GetNextConfirmedBlockIndex(_globalIndex);
            if (_globalIndex == -1) break; 
            lineSafe = ValidBlock(_globalIndex, testLine);
            if(!lineSafe) break;
        }

        return lineSafe;
    }

    public bool ValidBlock(int pBlockIndex, Line pProcessingLine)
    {
        
        while (TestBlockWithClue(pBlockIndex, pProcessingLine) == false)
        {
            if (_clueIndex == pProcessingLine.ClueValues.Count - 1) return false;
            _clueIndex++;
        }

        _clueIndex++;
        return true;

    }

    private bool TestBlockWithClue(int pBlockIndex, Line pProcessingLine)
    {
        
        ////Debug.Log("Clue: "+pProcessingLine.ClueValues[_clueIndex]);
        Debug.Log(_clueIndex);
        if (pProcessingLine.CountConfirmedBlockSize(pBlockIndex) > pProcessingLine.ClueValues[_clueIndex])
        {
            //Debug.Log(pBlockIndex);

            //Debug.Log(pProcessingLine.CountConfirmedBlockSize(pBlockIndex));
            //Debug.Log(pProcessingLine.ClueValues[_clueIndex]);
            ////Debug.Log("Block bigger than clue");
            return false;
        }
       
        int upperPtrIndex = setUpperIndex(pBlockIndex, pProcessingLine);
        int lowerPtrIndex = setLowerIndex(pProcessingLine, upperPtrIndex, pBlockIndex);
        


        if (!wasTooSmallSpace(lowerPtrIndex, upperPtrIndex, pProcessingLine))
        {
            return canEquilibrate(pBlockIndex, upperPtrIndex, lowerPtrIndex, pProcessingLine);
        }

        return false;
    }

    private bool wasTooSmallSpace(int pLowerIndex, int pUpperIndex, Line pProcessingLine)
    {
        while (pLowerIndex < pUpperIndex)
        {
            if (pProcessingLine.Cells[pLowerIndex].IsDiscarded)
            {
                //Debug.Log("Too Small Space");
                return true;
            }
            pLowerIndex++;
        }

        return false;
    }
    
    private bool canEquilibrate(int pBlockIndex, int upperPtrIndex, int lowerPtrIndex, Line pProcessingLine)
    {
        
        int neededDistanceToRight = getNeededDistanceToRight(pProcessingLine.ClueValues);
        int neededDistanceToLeft = getNeededDistanceToLeft(pProcessingLine.ClueValues);

        int distanceToRight = this.distanceToRight(pProcessingLine, upperPtrIndex);
        int distanceToLeft = this.distanceToLeft(lowerPtrIndex);
        
        //Debug.Log("Upper: "+upperPtrIndex);
        //Debug.Log("Lower: "+lowerPtrIndex);
        
        while (nextCellMarked(upperPtrIndex, pProcessingLine) ||
               !isValidLeftDistance(distanceToLeft, neededDistanceToLeft) ||
               !isValidRightDistance(distanceToRight, neededDistanceToRight))
        {
            if (!canMoveBackwards(lowerPtrIndex, pProcessingLine))
            {
                //Debug.Log("Couldn't equilibrate");
                return false;
            }

            lowerPtrIndex--;
            upperPtrIndex--;
            
            distanceToRight = this.distanceToRight(pProcessingLine, upperPtrIndex);
            distanceToLeft = this.distanceToLeft(lowerPtrIndex);
            
            ////Debug.Log("Upper: "+upperPtrIndex);
            ////Debug.Log("Lower: "+lowerPtrIndex);

            if (upperPtrIndex < pBlockIndex)
            {
                
                return false;
            }
        }
        //Debug.Log("Returned True");
        _globalIndex = upperPtrIndex + 1;
        return true;
    }

    private int setUpperIndex(int pBlockIndex, Line pProcessingLine)
    {
        int blockSize = pProcessingLine.CountConfirmedBlockSize(pBlockIndex);
        int upperIndex = pBlockIndex + blockSize;
        
        try
        {
            while (!pProcessingLine.Cells[upperIndex].IsDiscarded && 
                   upperIndex - pBlockIndex + 1 <= pProcessingLine.ClueValues[_clueIndex])
            {
                upperIndex++;
            }

            upperIndex--;
            return upperIndex;
        }
        catch (ArgumentOutOfRangeException e)
        {
            return pProcessingLine.Length - 1;
        }
        
    }

    private int setLowerIndex(Line pProcessingLine, int pUpperIndex, int pBlockIndex)
    {
        int indexesDistance = pUpperIndex - pBlockIndex + 1;
        int cellsToMove = pProcessingLine.ClueValues[_clueIndex] - indexesDistance;
        return pBlockIndex - cellsToMove;
    }

    private bool isValidRightDistance(int pDistanceToRight, int pNeededDistanceToRight)
    {
        //Debug.Log(pDistanceToRight);
        //if(pDistanceToRight < pNeededDistanceToRight) //Debug.Log("Invalid right distance");
        return pDistanceToRight >= pNeededDistanceToRight;
    }

    private bool isValidLeftDistance(int pDistanceToLeft, int pNeededDistanceToLeft)
    {
        //if(pDistanceToLeft < pNeededDistanceToLeft) //Debug.Log("Invalid left distance");
        return pDistanceToLeft >= pNeededDistanceToLeft;
    }
    

    private bool nextCellMarked(int pUpperPtrIndex, Line pLine)
    {
        try
        {

            if (pLine.Cells[pUpperPtrIndex + 1].IsConfirmed)
            {
                //Debug.Log(pUpperPtrIndex + 1);

                //Debug.Log("Next cell marked");
            }
            return pLine.Cells[pUpperPtrIndex + 1].IsConfirmed;
        }
        catch (ArgumentOutOfRangeException e)
        {
            return false;
        }
    }

    private bool canMoveBackwards(int pLowerPtrIndex, Line pLine)
    {
        try
        {
            if(pLine.Cells[pLowerPtrIndex - 1].IsDiscarded) return false;
            return true;
        }
        catch (ArgumentOutOfRangeException e)
        {
            return false;
        }
    }
    
    private int getNeededDistanceToRight(List<int> pClueValues)
    {
        int index = _clueIndex + 1;
        int neededDistance = 0;
        while (index < pClueValues.Count)
        {
            neededDistance += 1 + pClueValues[index];
            index++;
        }
        return neededDistance;
    }
    
    private int getNeededDistanceToLeft(List<int> pClueValues)
    {
        int index = _clueIndex - 1;
        int neededDistance = 0;
        while (index >= 0)
        {
            neededDistance += 1 + pClueValues[index];
            index--;
        }
        return neededDistance;
    }

    public int distanceToRight(Line pLine, int pUpperPtrIndex)
    {
        return pLine.Length - (pUpperPtrIndex + 1);
    }

     
    public int distanceToLeft(int pLowerPtrIndex)
    {
        return pLowerPtrIndex;
    }
}