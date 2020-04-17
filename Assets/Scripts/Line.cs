using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Line
{
    protected int _index;
    protected List<Cell> Cells;
    
    public int Length;
    public List<int> ClueValues;
    
    private int _greaterClueValue;
    private List<int> _sortedClueValues;

    private bool changeFlag;
 
    public Line(List<int> clues, int length, int index)
    {
        changeFlag = false;
        
        ClueValues = clues;
        
        _sortedClueValues = new List<int>();
        _sortedClueValues = ClueValues.ToList();
        
        Length = length;
        _index = index;
        
        createCellList();
        
        _sortedClueValues = bubbleSort(_sortedClueValues);
      
        setGreaterClue();
        
    }

    private void createCellList()
    {
        Cells = new List<Cell>();
        for (int counter = 0; counter < Length; counter++)
        {
            Cell newCell = createCell(_index, counter);
            Cells.Add(newCell);
        }
    }

    private Cell createCell(int xIndex, int yIndex)
    {
        return new Cell(xIndex, yIndex);
    }

    public void refresh(Line changes) 
    {
        Cells[changes._index].updateWithOuterChanges(changes.Cells[_index]);
    }
    
    public void analyzeLine()
    {
        if (ClueValues[0] == 0) discardLeftSpaces();
        else
        {
            //Debug.Log("INDEX: "+ _index);
            discardCells();
            markCells();

        }
        
    }
    
    private void markCells()
    {
        int index = 0;
        int finalIndex = Length - 1;
        if (onlyExistOneSpace())
        {
            index = getNextIsolatedSpace(0);
            int size = countIsolatedBlockSize(index);
            finalIndex = index + size - 1;
        }
        Cells = MathematicalApproach.mathematicalApproachMethod(ClueValues, index, finalIndex, 
            0, ClueValues.Count - 1, this);  
        
    }

    private bool onlyExistOneSpace()
    {
        
        int index = getNextIsolatedSpace(0);
        int size = countIsolatedBlockSize(index);
        index += size;
        if (index < Length)
        {
            index = getNextIsolatedSpace(index);
            if (index != -1) return false;
        }
        return true;
    }

    private void discardCells()
    {
        discardIsolatedSpaces();
        surroundCompletedClue();
        if (isComplete()) discardLeftSpaces();
        if (ClueValues.Count == 1) discardUnreachableSpaces();
    }
    
    private void discardUnreachableSpaces()
    {
        
        int index = getNextConfirmedBlockIndex(0);
        if (index != -1)
        {
            int blockSize = countConfirmedBlockSize(index);
        
            discardToLeft(index, blockSize);
        
            index += blockSize;
        
            discardToRight(index, blockSize);
        }
    }

    private void discardIsolatedSpaces()
    {
        int index = 0;
        int size;
        while (index <= Length-1)
        {
            index = getNextIsolatedSpace(index);
            if (index == -1) break;
            
            size = countIsolatedBlockSize(index);
            
            if (isAnEmptySpace(index))
            {
                if (_sortedClueValues.Count == 0) discardIsolatedSpace(index, size);
                else if (size < _sortedClueValues[0]) discardIsolatedSpace(index, size);
            }

            index += size;
        }
    }
    
    
    public bool isComplete()
    {
        bool lineComplete = true;
        int index = 0;
        foreach (int clue in ClueValues)
        {
            index = getNextConfirmedBlockIndex(index);
            if (index == -1 )
            {
                lineComplete = false;
                break;
            }

            int size = countConfirmedBlockSize(index);
            if (size != clue)
            {
                
                lineComplete = false;
                break;
            }
            index += size;
        }

        return lineComplete;
    }
    
    //Auxiliary Methods
    private void discardToLeft(int pIndex, int pBlockSize)
    {

        int maxReach = ClueValues[0] - pBlockSize;
        pIndex -= maxReach;
        pIndex--;

        while (pIndex >= 0)
        {
            discardCell(pIndex);
            
            pIndex--;
            
        }

    }
    
    private void discardToRight(int pIndex, int pBlockSize)
    {
        
        int maxReach = ClueValues[ClueValues.Count-1] - pBlockSize;
        pIndex += maxReach;

        while (pIndex < Length )
        {
            discardCell(pIndex);
            pIndex++;
        }
        
    }

    private void markCellFromIndex(int pIndex, int pQuantity, bool toLeft)
    { 
        while (pQuantity > 0)
        {
            Cells[pIndex].confirm();
            if (toLeft) pIndex--;
            else pIndex++;
            pQuantity--;
            setChangeFlag(true);
        }
    }
    
    private int getNextIsolatedSpace(int pCurrentIndex)
    {

        int internalIndex = pCurrentIndex;
        
        while (Cells[internalIndex].IsDiscarded)
        {
            internalIndex++;
            if (internalIndex == Length) return -1;
        }

        return internalIndex;
        
    }

    private int countIsolatedBlockSize(int pBlockIndex)
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
  
    private bool isAnEmptySpace(int pBlockIndex)
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

    private bool isCompleteSpace(int pBlockIndex)
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
    
    private void discardIsolatedSpace(int pBlockIndex, int pSize)
    {

        int finalIndex = pBlockIndex + pSize;
        while (pBlockIndex < finalIndex)
        {
            discardCell(pBlockIndex);
            pBlockIndex++;
        }

    }
    
    
    private int countConfirmedBlockSize(int pBlockIndex)
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

    private int getNextConfirmedBlockIndex(int pCurrentIndex)
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

    private void discardLeftSpaces()
    {
        foreach (Cell cell in Cells)
        {
            if (!cell.IsConfirmed && !cell.IsDiscarded)
            {
                cell.discard();
                setChangeFlag(true);
            }
        }
    }

    private void surroundCompletedClue()
    {
        int index = 0;
        int size;
        while (true)
        {
            index = getNextConfirmedBlockIndex(index);
            if(index == -1) break;
            size = countConfirmedBlockSize(index);
            if (size == _greaterClueValue && !isAlreadySurrounded(index-1, size))
            {
                
                discardBlockBoundaries(index-1, size);
                _sortedClueValues.RemoveAt(_sortedClueValues.Count-1);
                setGreaterClue();
            }

            index += size;
        }
      
    }

    private void discardBlockBoundaries(int pIndex, int pSize)
    {
        try
        {
            discardCell(pIndex);
        }
        catch (ArgumentOutOfRangeException outOfRangeException)
        {}

        try
        {
            pIndex += pSize + 1;
            discardCell(pIndex);
        } 
        catch (ArgumentOutOfRangeException outOfRangeException)
        {}
    }

    private bool isAlreadySurrounded(int pIndex, int pSize)
    {
     
        if (pIndex < 0 && pIndex + pSize + 1 >= Length) return true;
        
        if (pIndex < 0) return Cells[pIndex + pSize + 1].IsDiscarded;
        
        if (pIndex + pSize + 1 >= Length) return Cells[pIndex].IsDiscarded;
        
        else return Cells[pIndex].IsDiscarded && Cells[pIndex + pSize + 1].IsDiscarded;
        
    }
    
    
    public void setGreaterClue()
    {
        if (_sortedClueValues.Count == 0) return;
        _greaterClueValue = _sortedClueValues[_sortedClueValues.Count - 1];   
    }
    
    public List<int> bubbleSort(List<int> list)
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

    public void markCell(int pIndex)
    {
        if (!Cells[pIndex].IsConfirmed)
        {
            Cells[pIndex].confirm();
            setChangeFlag(true);
        }
    }
    
    public void discardCell(int pIndex)
    {
        if (!Cells[pIndex].IsDiscarded)
        {
            Cells[pIndex].discard();
            setChangeFlag(true);
        }
    }
    
    //Auxiliary Methods
    
    public void setChangeFlag(bool pValue)
    {
        changeFlag = pValue;
    }

    public bool wasChanged()
    {
        return changeFlag;
    }
    
    public List<Cell> getCells()
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


}

