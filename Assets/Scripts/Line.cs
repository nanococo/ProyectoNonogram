using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

public class Line
{
    private readonly int _index;
    private List<Cell> _cells;

    private readonly int _length;
    private readonly List<int> _clueValues;
    
    private int _greaterClueValue;
    private readonly List<int> _sortedClueValues;

    private bool _changeFlag;
 
    public Line(List<int> clues, int length, int index)
    {
        _changeFlag = false;
        
        _clueValues = clues;
        
        _sortedClueValues = new List<int>();
        _sortedClueValues = _clueValues.ToList();
        
        _length = length;
        _index = index;
        
        CreateCellList();
        
        _sortedClueValues = BubbleSort(_sortedClueValues);
      
        SetGreaterClue();
        
    }

    private void CreateCellList()
    {
        _cells = new List<Cell>();
        for (int counter = 0; counter < _length; counter++)
        {
            Cell newCell = CreateCell(_index, counter);
            _cells.Add(newCell);
        }
    }

    private static Cell CreateCell(int xIndex, int yIndex)
    {
        return new Cell(xIndex, yIndex);
    }

    public void Refresh(Line changes) 
    {
        _cells[changes._index].UpdateWithOuterChanges(changes._cells[_index]);
    }
    
    public void AnalyzeLine()
    {
        if (_clueValues[0] == 0) discardLeftSpaces();
        else
        {
            DiscardCells();
            MarkCells();

        }
        
    }
    
    private void MarkCells()
    {
        int index = 0;
        int finalIndex = _length - 1;
        if (onlyExistOneSpace())
        {
            index = GetNextIsolatedSpace(0);
            int size = CountIsolatedBlockSize(index);
            finalIndex = index + size - 1;
        }
        _cells = MathematicalApproach.MathematicalApproachMethod(_clueValues, index, finalIndex, 
            0, _clueValues.Count - 1, this);  
        
    }

    private bool onlyExistOneSpace()
    {
        
        int index = GetNextIsolatedSpace(0);
        int size = CountIsolatedBlockSize(index);
        index += size;
        if (index < _length)
        {
            index = GetNextIsolatedSpace(index);
            if (index != -1) return false;
        }
        return true;
    }

    private void DiscardCells()
    {
        DiscardIsolatedSpaces();
        SurroundCompletedClue();
        if (IsComplete()) discardLeftSpaces();
        if (_clueValues.Count == 1) DiscardUnreachableSpaces();
    }
    
    private void DiscardUnreachableSpaces()
    {
        
        int index = GetNextConfirmedBlockIndex(0);
        if (index != -1)
        {
            int blockSize = CountConfirmedBlockSize(index);
        
            DiscardToLeft(index, blockSize);
        
            index += blockSize;
        
            DiscardToRight(index, blockSize);
        }
    }

    private void DiscardIsolatedSpaces()
    {
        int index = 0;
        int size;
        while (index <= _length-1)
        {
            index = GetNextIsolatedSpace(index);
            if (index == -1) break;
            
            size = CountIsolatedBlockSize(index);
            
            if (IsAnEmptySpace(index))
            {
                if (_sortedClueValues.Count == 0) DiscardIsolatedSpace(index, size);
                else if (size < _sortedClueValues[0]) DiscardIsolatedSpace(index, size);
            }

            index += size;
        }
    }


    private bool IsComplete()
    {
        bool lineComplete = true;
        int index = 0;
        foreach (int clue in _clueValues)
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

        return lineComplete;
    }
    
    //Auxiliary Methods
    private void DiscardToLeft(int pIndex, int pBlockSize) 
    
    {

        int maxReach = _clueValues[0] - pBlockSize;
        pIndex -= maxReach;
        pIndex--;

        while (pIndex >= 0)
        {
            DiscardCell(pIndex);
            
            pIndex--;
            
        }

    }
    
    private void DiscardToRight(int pIndex, int pBlockSize)
    {
        
        int maxReach = _clueValues[_clueValues.Count-1] - pBlockSize;
        pIndex += maxReach;

        while (pIndex < _length )
        {
            DiscardCell(pIndex);
            pIndex++;
        }
        
    }

    private void MarkCellFromIndex(int pIndex, int pQuantity, bool toLeft)
    { 
        while (pQuantity > 0)
        {
            _cells[pIndex].Confirm();
            if (toLeft) pIndex--;
            else pIndex++;
            pQuantity--;
            SetChangeFlag(true);
        }
    }
    
    private int GetNextIsolatedSpace(int pCurrentIndex)
    {

        int internalIndex = pCurrentIndex;
        
        while (_cells[internalIndex].IsDiscarded)
        {
            internalIndex++;
            if (internalIndex == _length) return -1;
        }

        return internalIndex;
        
    }

    private int CountIsolatedBlockSize(int pBlockIndex)
    {
        
        int size = 0;
        int internalIndex = pBlockIndex;
        
        while (!_cells[internalIndex].IsDiscarded)
        {
            size++;
            internalIndex++;
            if (internalIndex == _length) return size;
        }

        return size;
        
    }
  
    private bool IsAnEmptySpace(int pBlockIndex)
    {
        
        int internalIndex = pBlockIndex;
        while (!_cells[internalIndex].IsDiscarded)
        {
            if (_cells[internalIndex].IsConfirmed) return false;
            internalIndex++;
            if (internalIndex == _length) return true;
        }

        return true;
        
    }

    private bool IsCompleteSpace(int pBlockIndex)
    {
        
        int internalIndex = pBlockIndex;
        while (!_cells[internalIndex].IsDiscarded)
        {
            if (!_cells[internalIndex].IsConfirmed) return false;
            internalIndex++;
            if (internalIndex == _length) return true;
        }

        return true;
        
    }
    
    private void DiscardIsolatedSpace(int pBlockIndex, int pSize)
    {

        int finalIndex = pBlockIndex + pSize;
        while (pBlockIndex < finalIndex)
        {
            DiscardCell(pBlockIndex);
            pBlockIndex++;
        }

    }
    
    
    private int CountConfirmedBlockSize(int pBlockIndex)
    {
        int confirmedBlockSize = 0;
        
        while (_cells[pBlockIndex].IsConfirmed)
        {
            confirmedBlockSize++;
            if (pBlockIndex == _length - 1) break;
            pBlockIndex++;
        }
        
        return confirmedBlockSize;
    }

    private int GetNextConfirmedBlockIndex(int pCurrentIndex)
    {
        if (pCurrentIndex >= _length) return -1;
        while (!_cells[pCurrentIndex].IsConfirmed)
        {
            if (pCurrentIndex == _length - 1) return -1;
            pCurrentIndex++;
        }
        
        int nextConfirmedBlockIndex = pCurrentIndex;
        
        return nextConfirmedBlockIndex;
    }

    private void discardLeftSpaces()
    {
        foreach (Cell cell in _cells)
        {
            if (!cell.IsConfirmed && !cell.IsDiscarded)
            {
                cell.Discard();
                SetChangeFlag(true);
            }
        }
    }

    private void SurroundCompletedClue()
    {
        int index = 0;
        int size;
        while (true)
        {
            index = GetNextConfirmedBlockIndex(index);
            if(index == -1) break;
            size = CountConfirmedBlockSize(index);
            if (size == _greaterClueValue && !IsAlreadySurrounded(index-1, size))
            {
                
                DiscardBlockBoundaries(index-1, size);
                _sortedClueValues.RemoveAt(_sortedClueValues.Count-1);
                SetGreaterClue();
            }

            index += size;
        }  
      
    }

    private void DiscardBlockBoundaries(int pIndex, int pSize)
    {
        try
        {
            DiscardCell(pIndex);
        }
        catch (ArgumentOutOfRangeException outOfRangeException)
        {}

        try
        {
            pIndex += pSize + 1;
            DiscardCell(pIndex);
        } 
        catch (ArgumentOutOfRangeException outOfRangeException)
        {}
    }

    private bool IsAlreadySurrounded(int pIndex, int pSize)
    {
     
        if (pIndex < 0 && pIndex + pSize + 1 >= _length) return true;
        
        if (pIndex < 0) return _cells[pIndex + pSize + 1].IsDiscarded;
        
        if (pIndex + pSize + 1 >= _length) return _cells[pIndex].IsDiscarded;
        
        else return _cells[pIndex].IsDiscarded && _cells[pIndex + pSize + 1].IsDiscarded;
        
    }
    
    
    public void SetGreaterClue()
    {
        if (_sortedClueValues.Count == 0) return;
        _greaterClueValue = _sortedClueValues[_sortedClueValues.Count - 1];   
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

    public bool isBroken() // establishes if the configuration of the line is impossible due to the configuration of the given clues
    {
        return false;
    }
    
    //420 line remains untouched
    
    public bool validBlock(int pBlockIndex, int pClueIndex)
    {
        
        return false;    
    }

    public int distanceToRight(int pUpperPtrIndex)
    {
        return _length - (pUpperPtrIndex + 1);
    }

    public int distanceToLeft(int pLowerPtrIndex)
    {
        return pLowerPtrIndex;
    } 

    public void MarkCell(int pIndex)
    {
        if (!_cells[pIndex].IsConfirmed)
        {
            _cells[pIndex].Confirm();
            SetChangeFlag(true);
        }
    }
    
    public void DiscardCell(int pIndex)
    {
        if (!_cells[pIndex].IsDiscarded)
        {
            _cells[pIndex].Discard();
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
        return _cells;
    }
    
    public override string ToString() { 
        string test = "|";
        foreach (var cell in _cells)
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

