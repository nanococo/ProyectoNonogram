using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

/// <summary>
/// The Backtracking class is the main handler for all
/// backtracking code solving the nonogram
/// </summary>
public class Backtracking {

    private readonly Matrix _matrix;

    public Backtracking(Matrix matrix) => _matrix = matrix;
    
    private readonly Stopwatch _stopWatch = new Stopwatch();

    private bool ExecuteBacktracking() {
        for (var i = 0; i < _matrix.Rows.Count; i++) {
            if (!_matrix.Rows[i].IsCompleteBacktracking()) {
                for (var j = 0; j < _matrix.Rows[i].Cells.Count; j++) {
                    if (!_matrix.Rows[i].Cells[j].DemonMark && !_matrix.Rows[i].Cells[j].IsConfirmed && !_matrix.Rows[i].Cells[j].IsDiscarded && IsSafe(i, j)) {
                        //_matrix.Rows[i].Cells[j].Mark = "1";
                        _matrix.Rows[i].Cells[j].Confirm(true);
                        _matrix.Columns[j].Cells[i].Confirm(false);
                        _matrix.RemoveDemonMarks();
                        if (ExecuteBacktracking()) {
                            return true;
                        } else {
                            //_matrix.Rows[i].Cells[j].Mark = "0";
                            _matrix.Rows[i].Cells[j].DemonMark=true;
                            _matrix.Rows[i].Cells[j].DeConfirm(true);
                            _matrix.Columns[j].Cells[i].DemonMark=true;
                            _matrix.Columns[j].Cells[i].DeConfirm(false);
                        }
                    }
                }
            }
        }

        return IsFinished();
    }

    private bool IsFinished() {
        var isComplete = false;
        foreach (var matrixRow in _matrix.Rows) {
            if (!matrixRow.IsCompleteBacktracking()) {
                isComplete = false;
                break;
            }
            isComplete = true;
        }

        if (isComplete) {
            foreach (var matrixColumn in _matrix.Columns) {
                if (!matrixColumn.IsCompleteBacktracking()) {
                    isComplete = false;
                    break;
                }
            }    
        }

        if (isComplete) {
            Debug.Log(true);
        }
        
        return isComplete;
    }


    private bool IsSafe2(int rowIndex, int cellIndex) {
        
        //_matrix.Rows[rowIndex].Cells[cellIndex].Confirm();
        //_matrix.Rows[rowIndex].Cells[cellIndex].Undo();
        
        
        
        var blocksOnLine = 0;
        var blockCount = 0;
        var blockCounting = false;
        
        for (var i = 0; i < _matrix.Rows[rowIndex].Cells.Count; i++) {
            if (_matrix.Rows[rowIndex].Cells[i].IsConfirmed) {
                blockCount++;
            }
            else if(blockCount>0) {
                blockCount = 0;
            }
        }

        return true;
    }






    private int _clueIndex; //Global variable used in the is safe and valid lock methods.
    private int _globalIndex; //Global index used in the isSafe method chain.

    private bool IsSafe(int rowNumber, int cellNumber) {

        var originalValue = _matrix.Rows[rowNumber].Cells[cellNumber].LogicalMatrix[rowNumber, cellNumber];
        var originalMark = _matrix.Rows[rowNumber].Cells[cellNumber].Mark;
        var originalIsConfirmed = _matrix.Rows[rowNumber].Cells[cellNumber].IsConfirmed;
        var originalIsDiscarded = _matrix.Rows[rowNumber].Cells[cellNumber].IsDiscarded;
        
        var lineSafe = true;

        //------ROWS FIRST------//

        var processingLine = _matrix.Rows[rowNumber];

        lineSafe = CheckLine(processingLine, cellNumber, true);
        
        processingLine.Cells[cellNumber].Undo(originalMark, originalIsConfirmed, originalIsDiscarded,true, originalValue);
       
        if (!lineSafe) return false;
        
        //------COLUMNS SECOND------//

        processingLine = _matrix.Columns[cellNumber];
        
        lineSafe = CheckLine(processingLine, rowNumber, false);

        processingLine.Cells[rowNumber].Undo(originalMark, originalIsConfirmed, originalIsDiscarded, false, originalValue);
        
        return lineSafe;
    }

    private bool IsBasicSafe(int rowNumber, int cellNumber) {

        bool retVal;
        
        var originalValue = _matrix.Rows[rowNumber].Cells[cellNumber].LogicalMatrix[rowNumber, cellNumber];
        var originalMark = _matrix.Rows[rowNumber].Cells[cellNumber].Mark;
        var originalIsConfirmed = _matrix.Rows[rowNumber].Cells[cellNumber].IsConfirmed;
        var originalIsDiscarded = _matrix.Rows[rowNumber].Cells[cellNumber].IsDiscarded;
        
        var processingLine = _matrix.Rows[rowNumber];
        processingLine.MarkCell(cellNumber, true);

        retVal = processingLine.IsBasicSafe();
        processingLine.Cells[cellNumber].Undo(originalMark, originalIsConfirmed, originalIsDiscarded,true, originalValue);
        
        if (!retVal) return false;
        
        processingLine = _matrix.Columns[cellNumber];
        retVal = processingLine.IsBasicSafe();
        processingLine.Cells[rowNumber].Undo(originalMark, originalIsConfirmed, originalIsDiscarded, false, originalValue);
        
        
        // var originalMark = _matrix.Rows[rowNumber].Cells[cellNumber].Mark;
        // var originalMarkC = _matrix.Columns[cellNumber].Cells[rowNumber].Mark;
        //
        // var retVal = false;
        //
        // _matrix.Rows[rowNumber].Cells[cellNumber].Mark = "1";
        // _matrix.Columns[cellNumber].Cells[rowNumber].Mark = "1";
        //
        // if (_matrix.Rows[rowNumber].IsBasicSafe() && _matrix.Columns[cellNumber].IsBasicSafe()) {
        //     retVal = true;
        // }
        //
        // _matrix.Rows[rowNumber].Cells[cellNumber].Mark = originalMark;
        // _matrix.Columns[cellNumber].Cells[rowNumber].Mark = originalMarkC;

        return retVal;
    }

    private bool CheckLine(Line processingLine, int rowNumber, bool doDraw){
        var lineSafe = true;
        
        processingLine.MarkCell(rowNumber, doDraw);
        
        //Debug.Log(listToString(processingLine.Cells));
        
        _globalIndex = 0;
        _clueIndex = 0;

        while (true)
        {
            _globalIndex = processingLine.GetNextConfirmedBlockIndex(_globalIndex);
            if (_globalIndex == -1) break;
            lineSafe = ValidBlock(_globalIndex, processingLine);
            if(!lineSafe) break;
        }
        
        return lineSafe;
    }
    
    

    public bool ValidBlock(int pBlockIndex, Line pProcessingLine)
    {

        if (_clueIndex >= pProcessingLine.ClueValues.Count) return false;

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
        
        //Debug.Log("Clue size: "+pProcessingLine.ClueValues.Count);
        //Debug.Log("Clue index: "+_clueIndex);
        if (pProcessingLine.CountConfirmedBlockSize(pBlockIndex) > pProcessingLine.ClueValues[_clueIndex])
        {
            //Debug.Log(pBlockIndex);

            //Debug.Log(pProcessingLine.CountConfirmedBlockSize(pBlockIndex));
            //Debug.Log(pProcessingLine.ClueValues[_clueIndex]);
            ////Debug.Log("Block bigger than clue");
            return false;
        }
       
        int upperPtrIndex = SetUpperIndex(pBlockIndex, pProcessingLine);
        int lowerPtrIndex = SetLowerIndex(pProcessingLine, upperPtrIndex, pBlockIndex);
        
        

        if (!wasTooSmallSpace(lowerPtrIndex, upperPtrIndex, pProcessingLine))
        {
            return CanEquilibrate(pBlockIndex, upperPtrIndex, lowerPtrIndex, pProcessingLine);
        }

        return false;
    }
   
    private bool wasTooSmallSpace(int pLowerIndex, int pUpperIndex, Line pProcessingLine)
    {
        if (pLowerIndex < 0) return true;
        
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
    
    private bool CanEquilibrate(int pBlockIndex, int upperPtrIndex, int lowerPtrIndex, Line pProcessingLine)
    {
        
        int neededDistanceToRight = GetNeededDistanceToRight(pProcessingLine.ClueValues);
        int neededDistanceToLeft = GetNeededDistanceToLeft(pProcessingLine.ClueValues);

        int distanceToRight = this.distanceToRight(pProcessingLine, upperPtrIndex);
        int distanceToLeft = this.distanceToLeft(lowerPtrIndex);
        
        //Debug.Log("Upper: "+upperPtrIndex);
        //Debug.Log("Lower: "+lowerPtrIndex);
        
        while (NextCellMarked(upperPtrIndex, pProcessingLine) ||
               !IsValidLeftDistance(distanceToLeft, neededDistanceToLeft) ||
               !IsValidRightDistance(distanceToRight, neededDistanceToRight))
        {
            if (!CanMoveBackwards(lowerPtrIndex, pProcessingLine))
            {
                //Debug.Log("Couldn't equilibrate");
                return false;
            }

            lowerPtrIndex--;
            upperPtrIndex--;
            
            distanceToRight = this.distanceToRight(pProcessingLine, upperPtrIndex);
            distanceToLeft = this.distanceToLeft(lowerPtrIndex);
            
            //Debug.Log("Upper: "+upperPtrIndex);
            //Debug.Log("Lower: "+lowerPtrIndex);

            if (upperPtrIndex < pBlockIndex)
            {
                
                return false;
            }
        }
        //Debug.Log("Returned True");
        _globalIndex = upperPtrIndex + 1;
        return true;
    }

    private int SetUpperIndex(int pBlockIndex, Line pProcessingLine)
    {
        var blockSize = pProcessingLine.CountConfirmedBlockSize(pBlockIndex);
        var upperIndex = pBlockIndex + blockSize;
        
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

    private int SetLowerIndex(Line pProcessingLine, int pUpperIndex, int pBlockIndex)
    {
        var indexesDistance = pUpperIndex - pBlockIndex + 1;
        var cellsToMove = pProcessingLine.ClueValues[_clueIndex] - indexesDistance;
        
        //Debug.Log(pBlockIndex);
        //Debug.Log(cellsToMove);
        
        return pBlockIndex - cellsToMove;
    }

    private static bool IsValidRightDistance(int pDistanceToRight, int pNeededDistanceToRight)
    {
        //Debug.Log(pDistanceToRight);
        //if(pDistanceToRight < pNeededDistanceToRight) Debug.Log("Invalid right distance");
        return pDistanceToRight >= pNeededDistanceToRight;
    }

    private static bool IsValidLeftDistance(int pDistanceToLeft, int pNeededDistanceToLeft)
    {
        //if(pDistanceToLeft < pNeededDistanceToLeft) Debug.Log("Invalid left distance");
        return pDistanceToLeft >= pNeededDistanceToLeft;
    }
    

    private static bool NextCellMarked(int pUpperPtrIndex, Line pLine)
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

    private static bool CanMoveBackwards(int pLowerPtrIndex, Line pLine)
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
    
    private int GetNeededDistanceToRight(List<int> pClueValues)
    {
        var index = _clueIndex + 1;
        var neededDistance = 0;
        while (index < pClueValues.Count)
        {
            neededDistance += 1 + pClueValues[index];
            index++;
        }
        return neededDistance;
    }
    
    private int GetNeededDistanceToLeft(List<int> pClueValues)
    {
        var index = _clueIndex - 1;
        var neededDistance = 0;
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
    
    public string listToString<T>(List<T> list)
    {
        string result = "";
        foreach (var element in list)
        {
            result += element + ",";
        }

        result += "";
        return result;
    }
    
    public bool IsSafeForTesting(Line testLine)
    {
        var lineSafe = true;
        _globalIndex = 0;
        _clueIndex = 0;
        
        while (true)
        {
            _globalIndex = testLine.GetNextConfirmedBlockIndex(_globalIndex);
            if (_globalIndex == -1) break; 
            lineSafe = ValidBlock(_globalIndex, testLine);
            if(!lineSafe) break;
        }
        //Debug.Log(lineSafe);
        return lineSafe;
    }

    public void StartBacktracking() {
        _stopWatch.Start();
        Debug.Log(ExecuteBacktracking());
        _stopWatch.Stop();
        var ts = _stopWatch.Elapsed;
        Debug.Log((float) ts.TotalMinutes);
        
        Debug.Log("-----------------------------------------------");
        Debug.Log("Rows");
        Debug.Log(_matrix.listToString(_matrix.Rows));
    }
}