using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Line : MonoBehaviour
{
    protected List<Cell> Cells;
    protected List<Clue> _clues;
    public List<int> ClueValues;
    protected readonly int _index;
    private readonly SimpleBoxes _simpleBoxesMethod;
    public readonly int Length;
    public int sumOfClues;
    

    public Line(List<int> clues, int length, int index)
    {
        ClueValues = clues;
        Length = length;
        _index = index;
        _clues = new List<Clue>();
        _simpleBoxesMethod = new SimpleBoxes();
        createCellList();
        sumCluesValues();
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
        
        //if (_simpleBoxesMethod.canDo(this))
        //{
        //    Cells = _simpleBoxesMethod.simpleBoxes(this);
        //}
        //createClues();
        //try
        //{
        //}
        //catch (Exception e)
        //{
            // ignored
        //}
        
        MathematicalApproach.mathematicalApproachMethod(ClueValues,
                                                        1,
                                                        4,
                                                        0, 
                                                        ClueValues.Count-1,
                                                        this);

    }

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
        _clues.Add(newClue);
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
        

    public List<Cell> getCells()
    {
        return Cells;
    }

    public void setCells(List<Cell> newConfig)
    {
        Cells = newConfig;
    }
    
    public List<Clue> getClues()
    {
        return _clues;
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

