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
    private List<int> SortedClueValues;

    public Line(List<int> clues, int length, int index)
    {
        ClueValues = clues;
        
        SortedClueValues = new List<int>();
        SortedClueValues = ClueValues.ToList();
        
        Length = length;
        _index = index;
        
        createCellList();
        
        SortedClueValues = bubbleSort(SortedClueValues);
      
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

        Cells = MathematicalApproach.mathematicalApproachMethod(
             ClueValues,
            0,
            Length - 1,
            0,
            ClueValues.Count - 1,
            this);
        
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

    public int countConfirmedBlockSize(int pBlockIndex)
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

    public int getNextConfirmedBlockIndex(int pCurrentIndex)
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

    public void discardLeftSpaces()
    {
        foreach (Cell cell in Cells)
        {
            if (!cell.IsConfirmed) cell.discard();
        }
    }

    public void discardCells()
    {
        
    }

    public void surroundCompletedClue()
    {
        
        int index = 0;
        while (true)
        {
            index = getNextConfirmedBlockIndex(index);
            if (index == -1) return;
            int size = countConfirmedBlockSize(index);
            if (size == _greaterClueValue)
            {
                
                discardBlockBoundaries(index-1, size);
                if (SortedClueValues.Count == 0) return;
                SortedClueValues.RemoveAt(SortedClueValues.Count-1);
                setGreaterClue();
            }
            index += size;

        }
        
        
    }

    public void discardBlockBoundaries(int pIndex, int pSize)
    {
        if (pIndex < 0) return;
        Cells[pIndex].discard();
        pIndex += pSize + 1;
        if (pIndex >= Length) return;
        Cells[pIndex].discard();
    }
    
    public List<Cell> getCells()
    {
        return Cells;
    }

    public void setGreaterClue()
    {
        if (SortedClueValues.Count == 0) return;
        _greaterClueValue = SortedClueValues[SortedClueValues.Count - 1];   
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

