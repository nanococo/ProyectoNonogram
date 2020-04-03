using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Line : MonoBehaviour
{
    protected List<Cell> Cells;
    //protected List<Clue> _clues;
    public List<int> ClueValues;
    protected readonly int _index;
    private readonly SimpleBoxes _simpleBoxesMethod;
    public readonly int Length;
    public int sumOfClues;

    private int greaterClueValue;
    

    public Line(List<int> clues, int length, int index)
    {
        ClueValues = clues;
        Length = length;
        _index = index;
        //_clues = new List<Clue>();
        _simpleBoxesMethod = new SimpleBoxes();
        createCellList();
        sumCluesValues();
        setGreaterClue();
    }

    private void sumCluesValues()
    {
        foreach (var clue in ClueValues)
        {
            sumOfClues += clue;
        }
    }

    protected abstract void createCellList();

    protected Cell createCell(int xIndex, int yIndex)
    {
        return new Cell(xIndex, yIndex);
    }

    public void refresh(Line changes)
    {
        Cells[changes._index].updateWithOuterChanges(changes.Cells[_index]);
    }
    
    
    public void analyzeLine() {

        Cells = MathematicalApproach.mathematicalApproachMethod(ClueValues,
                                                        0,
                                                        5,
                                                        0, 
                                                        ClueValues.Count-1,
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
            if (countConfirmedBlockSize(index) != clue)
            {
                
                lineComplete = false;
                break;
            }
            index += countConfirmedBlockSize(index);
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

    }
    
    
    
    /*
    public abstract void createClues();
    protected abstract void assignCluesToCells();

    
    protected int createClue(int index)
    {
        int minIndex = index;
        int maxIndex = index;

        while (Cells[index].IsConfirmed)
        {
            maxIndex = index;
            index++;
            if (index == Length) break;
        }

        Clue newClue = new Clue(minIndex, maxIndex, ClueValues);
        newClue.calcDistanceToRightBoundary(this);
        newClue.calcDistanceToLeftBoundary(this);
        //_clues.Add(newClue);
        Debug.Log("NewClue: " +
                  "|BlockSize:" + newClue.BlockSize +
                  "|minIndex:" + minIndex +
                  "|maxIndex:" + maxIndex);

        return index;
    }
    

    public int calcSumToRight(int index, int blockSize)
    {
        int sumOfRestOfValuesAndSpaces = sumFromAnIndexWithSpaces(index + 1);
        int leftFromBlock = ClueValues[index] - blockSize;
        return leftFromBlock + sumOfRestOfValuesAndSpaces;
    }

    public int calcSumToLeft(int index, int blockSize)
    {
        int sumOfRestOfValuesAndSpaces = sumFromAnIndexWithSpacesBackwards(index - 1);
        int leftFromBlock = ClueValues[index] - blockSize;
        return leftFromBlock + sumOfRestOfValuesAndSpaces;
    }

    private int sumFromAnIndexWithSpaces(int index)
    {
        return sumFromAnIndexWithSpaces_aux(index, 0);
    }

    private int sumFromAnIndexWithSpaces_aux(int index, int result) //TODO check if that +1 is right
    {
        if (index == ClueValues.Count)
        {
            return result;
        }

        int newIndex = index+1;
        return sumFromAnIndexWithSpaces_aux(newIndex, result + 1 + ClueValues[index]);
    }
    
    private int sumFromAnIndexWithSpacesBackwards(int index)
    {
        return sumFromAnIndexWithSpacesBackwards_aux(index, 0);
    }
    
    private int sumFromAnIndexWithSpacesBackwards_aux(int index, int result) //TODO check if that +1 is right
    {
        if (index < 0)
        {
            return result;
        }
        int newIndex = index-1;
        return sumFromAnIndexWithSpacesBackwards_aux(newIndex, result + 1 + ClueValues[index]);
    }
        public List<Clue> getClues()
    {
        return _clues;
    }
    */

    public List<Cell> getCells()
    {
        return Cells;
    }

    public void setCells(List<Cell> newConfig)
    {
        Cells = newConfig;
    }

    public void setGreaterClue()
    {
        greaterClueValue = findGreaterClue();
    }

    public int findGreaterClue()
    {
        return findGreaterClue_aux(0, ClueValues[0]);
    }

    public int findGreaterClue_aux(int index, int greaterClue)
    {
        if (index == Length - 1) return greaterClue;
        
        int newIndex = index + 1;
        
        if (ClueValues[index] > greaterClue) return findGreaterClue_aux(newIndex, ClueValues[index]);
        return findGreaterClue_aux(newIndex, greaterClue);
    }
    
    public override string ToString() { //TODO
        string test = "[";
        foreach (var cell in Cells)
        {
            test += cell.Mark + ",";
        }

        test += "]";
        return test;
    }

    public void setClues()
    {
        
    }


}

