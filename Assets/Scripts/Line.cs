using System;
using System.Collections.Generic;
using System.Linq;
using Visuals;

public class Line
{
    public int RowNumber { get; }
    public List<Cell> Cells { get; private set; }

    public int Length { get; }
    public List<int> ClueValues { get; }

    public int GreaterClueValue { get; set; }
    public List<int> SortedClueValues { get; }

    private bool _changeFlag;

    private Board _board;
    private int[,] _LogicalMatrix;


    public Line(List<int> clues, int length, int rowNumber, Board board, int[,] logicalMatrix) {
        _board = board;
        
        _changeFlag = false;
        
        ClueValues = clues;
        
        SortedClueValues = new List<int>();
        SortedClueValues = ClueValues.ToList();
        
        Length = length;
        RowNumber = rowNumber;

        _LogicalMatrix = logicalMatrix;
        
        CreateCellList();
        
        SortedClueValues = BubbleSort(SortedClueValues);
      
        SetGreaterClue();
        
    }

    private void CreateCellList()
    {
        Cells = new List<Cell>();
        for (var counter = 0; counter < Length; counter++) {
            var newCell = CreateCell(RowNumber, counter);
            Cells.Add(newCell);
        }
    }

    private Cell CreateCell(int xIndex, int yIndex)
    {
        return new Cell(xIndex, yIndex, _LogicalMatrix);
    }

    public void Refresh(Line changes) 
    {
        Cells[changes.RowNumber].UpdateWithOuterChanges(changes.Cells[RowNumber]);
    }
    
    public void AnalyzeLine(bool doDraw)
    {
        if (ClueValues[0] == 0) DiscardLeftSpaces(doDraw);
        else
        {
            DiscardCells(doDraw);
            MarkCells(doDraw);

        }
        
    }
    
    private void MarkCells(bool doDraw)
    {
        int index = 0;
        int finalIndex = Length - 1;
        if (onlyExistOneSpace())
        {
            index = GetNextIsolatedSpace(0);
            int size = CountIsolatedBlockSize(index);
            finalIndex = index + size - 1;
        }
        Cells = MathematicalApproach.MathematicalApproachMethod(ClueValues, index, finalIndex, 
            0, ClueValues.Count - 1, this, doDraw);  
        
    }

    private bool onlyExistOneSpace()
    {
        
        int index = GetNextIsolatedSpace(0);
        int size = CountIsolatedBlockSize(index);
        index += size;
        if (index < Length)
        {
            index = GetNextIsolatedSpace(index);
            if (index != -1) return false;
        }
        return true;
    }

    private void DiscardCells(bool doDraw)
    {
        DiscardIsolatedSpaces(doDraw);
        SurroundCompletedClue(doDraw);
        if (IsComplete()) DiscardLeftSpaces(doDraw);
        if (ClueValues.Count == 1) DiscardUnreachableSpaces(doDraw);
    }
    
    private void DiscardUnreachableSpaces(bool doDraw)
    {
        
        int index = GetNextConfirmedBlockIndex(0);
        if (index != -1)
        {
            int blockSize = CountConfirmedBlockSize(index);
        
            DiscardToLeft(index, blockSize, doDraw);
        
            index += blockSize;
        
            DiscardToRight(index, blockSize, doDraw);
        }
    }

    private void DiscardIsolatedSpaces(bool doDraw)
    {
        int index = 0;
        int size;
        while (index <= Length-1)
        {
            index = GetNextIsolatedSpace(index);
            if (index == -1) break;
            
            size = CountIsolatedBlockSize(index);
            
            if (IsAnEmptySpace(index))
            {
                if (SortedClueValues.Count == 0) DiscardIsolatedSpace(index, size, doDraw);
                else if (size < SortedClueValues[0]) DiscardIsolatedSpace(index, size, doDraw);
            }

            index += size;
        }
    }


    public bool IsComplete()
    {
        bool lineComplete = true;
        int index = 0;
        foreach (int clue in ClueValues)
        {
            index = GetNextConfirmedBlockIndex(index);
            if (index == -1 )
            {
                lineComplete = false;
                break;
            }

            int size = CountConfirmedBlockSize(index);
            if (size != clue)
            {
                
                lineComplete = false;
                break;
            }
            index += size;
        }

        if (lineComplete) {
        }

        return lineComplete;
    }

    public bool IsCompleteBacktracking() {

        var cluesValueAddition = 0;
        var totalCluesFound = 0;
        
        foreach (var clueValue in ClueValues) {
            cluesValueAddition += clueValue;
        }
        
        foreach (var cell in Cells) {
            if (cell.Mark=="1") {
                totalCluesFound++;
            }
        }

        if (totalCluesFound==cluesValueAddition) {
            return true;
        }
        return false;
    }

    //Auxiliary Methods
    private void DiscardToLeft(int pIndex, int pBlockSize, bool doDraw) 
    
    {

        int maxReach = ClueValues[0] - pBlockSize;
        pIndex -= maxReach;
        pIndex--;

        while (pIndex >= 0)
        {
            DiscardCell(pIndex, doDraw);
            
            pIndex--;
            
        }

    }
    
    private void DiscardToRight(int pIndex, int pBlockSize, bool doDraw)
    {
        
        int maxReach = ClueValues[ClueValues.Count-1] - pBlockSize;
        pIndex += maxReach;

        while (pIndex < Length )
        {
            DiscardCell(pIndex, doDraw);
            pIndex++;
        }
        
    }

    private void MarkCellFromIndex(int pIndex, int pQuantity, bool toLeft, bool doDraw)
    { 
        while (pQuantity > 0)
        {
            Cells[pIndex].Confirm(doDraw);
            if (toLeft) pIndex--;
            else pIndex++;
            pQuantity--;
            SetChangeFlag(true);
        }
    }
    
    private int GetNextIsolatedSpace(int pCurrentIndex)
    {

        int internalIndex = pCurrentIndex;
        
        while (Cells[internalIndex].IsDiscarded)
        {
            internalIndex++;
            if (internalIndex == Length) return -1;
        }

        return internalIndex;
        
    }

    private int CountIsolatedBlockSize(int pBlockIndex)
    {
        
        int size = 0;
        int internalIndex = pBlockIndex;
        
        while (!Cells[internalIndex].IsDiscarded)
        {
            size++;
            internalIndex++;
            if (internalIndex == Length) return size;
        }

        return size;
        
    }
  
    private bool IsAnEmptySpace(int pBlockIndex)
    {
        
        int internalIndex = pBlockIndex;
        while (!Cells[internalIndex].IsDiscarded)
        {
            if (Cells[internalIndex].IsConfirmed) return false;
            internalIndex++;
            if (internalIndex == Length) return true;
        }

        return true;
        
    }

    private bool IsCompleteSpace(int pBlockIndex)
    {
        
        int internalIndex = pBlockIndex;
        while (!Cells[internalIndex].IsDiscarded)
        {
            if (!Cells[internalIndex].IsConfirmed) return false;
            internalIndex++;
            if (internalIndex == Length) return true;
        }

        return true;
        
    }
    
    private void DiscardIsolatedSpace(int pBlockIndex, int pSize, bool doDraw)
    {

        int finalIndex = pBlockIndex + pSize;
        while (pBlockIndex < finalIndex)
        {
            DiscardCell(pBlockIndex, doDraw);
            pBlockIndex++;
        }

    }
    
    
    public int CountConfirmedBlockSize(int pBlockIndex)
    {
        int confirmedBlockSize = 0;
        
        while (Cells[pBlockIndex].IsConfirmed)
        {
            confirmedBlockSize++;
            if (pBlockIndex == Length - 1) break;
            pBlockIndex++;
        }
        
        return confirmedBlockSize;
    }

    public int GetNextConfirmedBlockIndex(int pCurrentIndex)
    {
        if (pCurrentIndex >= Length) return -1;
        while (!Cells[pCurrentIndex].IsConfirmed)
        {
            if (pCurrentIndex == Length - 1) return -1;
            pCurrentIndex++;
        }
        
        int nextConfirmedBlockIndex = pCurrentIndex;
        
        return nextConfirmedBlockIndex;
    }

    private void DiscardLeftSpaces(bool doDraw)
    {
        foreach (Cell cell in Cells)
        {
            if (!cell.IsConfirmed && !cell.IsDiscarded)
            {
                cell.Discard(doDraw);
                SetChangeFlag(true);
            }
        }
    }

    private void SurroundCompletedClue(bool doDraw)
    {
        int index = 0;
        int size;
        while (true)
        {
            index = GetNextConfirmedBlockIndex(index);
            if(index == -1) break;
            size = CountConfirmedBlockSize(index);
            if (size == GreaterClueValue && !IsAlreadySurrounded(index-1, size))
            {
                
                DiscardBlockBoundaries(index-1, size, doDraw);
                SortedClueValues.RemoveAt(SortedClueValues.Count-1);
                SetGreaterClue();
            }

            index += size;
        }  
      
    }

    private void DiscardBlockBoundaries(int pIndex, int pSize, bool doDraw)
    {
        try
        {
            DiscardCell(pIndex, doDraw);
        }
        catch (ArgumentOutOfRangeException outOfRangeException)
        {}

        try
        {
            pIndex += pSize + 1;
            DiscardCell(pIndex, doDraw);
        } 
        catch (ArgumentOutOfRangeException outOfRangeException)
        {}
    }

    private bool IsAlreadySurrounded(int pIndex, int pSize)
    {
     
        if (pIndex < 0 && pIndex + pSize + 1 >= Length) return true;
        
        if (pIndex < 0) return Cells[pIndex + pSize + 1].IsDiscarded;
        
        if (pIndex + pSize + 1 >= Length) return Cells[pIndex].IsDiscarded;
        
        else return Cells[pIndex].IsDiscarded && Cells[pIndex + pSize + 1].IsDiscarded;
        
    }
    
    
    public void SetGreaterClue()
    {
        if (SortedClueValues.Count == 0) return;
        GreaterClueValue = SortedClueValues[SortedClueValues.Count - 1];   
    }
    
    public List<int> BubbleSort(List<int> list)
    {
        
        int temp;
        List<int> internalList = list;

        for (int j = 0; j <= internalList.Count - 2; j++)
        {
            for (int i = 0; i <= internalList.Count - 2; i++)
            {
                //Debug.Log(listToString(backupList));
                if (internalList[i] > internalList[i + 1])
                {
                    temp = internalList[i + 1];
                    internalList[i + 1] = internalList[i];
                    internalList[i] = temp;
                }
            }
        }
        return internalList;

    }

     

    public void MarkCell(int pIndex, bool doDraw)
    {
        if (!Cells[pIndex].IsConfirmed)
        {
            Cells[pIndex].Confirm(doDraw);
            SetChangeFlag(true);
        }
    }
    
    public void DiscardCell(int pIndex, bool doDraw)
    {
        if (!Cells[pIndex].IsDiscarded)
        {
            Cells[pIndex].Discard(doDraw);
            SetChangeFlag(true);
        }
    }
    
    //Auxiliary Methods
    
    public void SetChangeFlag(bool pValue)
    {
        _changeFlag = pValue;
    }

    public bool WasChanged()
    {
        return _changeFlag;
    }
    
    public List<Cell> GetCells()
    {
        return Cells;
    }
    
    public override string ToString() { 
        string test = "|";
        foreach (var cell in Cells)
        {
            test += cell.Mark + "|";
        }
        
        return test;
        
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


    public bool IsBasicSafe() {
        var block = false;
        var totalCluesFound = 0;
        var blockSize = 0;
        var cluesValueAddition = ClueValues.Sum();

        foreach (var cell in Cells) {
            if (cell.Mark == "1") {
                block = true;
                totalCluesFound++;
                blockSize++;    
                
            } else if (block) {
                if (blockSize>GreaterClueValue) {
                    return false;
                }

                blockSize = 0;
                block = false;
            }
        }
        if (blockSize>GreaterClueValue) {
            return false;
        }

        if (totalCluesFound > cluesValueAddition) {
            return false;
        }

        return true;

    }
}

