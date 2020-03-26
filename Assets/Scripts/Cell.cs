using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{

    public int XIndex;
    public int YIndex;
    
    public bool hasRowClueAssigned;
    public bool hasColumnClueAssigned;

    public Clue ColumnCluePartOf;
    public Clue RowCluePartOf;

    public bool IsConfirmed;
    public bool IsDiscarded;

    public bool IsRowLowerLimit;
    public bool IsRowUpperLimit;
    
    public bool IsColumnLowerLimit;
    public bool IsColumnUpperLimit;
    
    public string Mark; //'x' for discarded, '1' for confirmed, '0' for not processed

    public int DistanceToRightBorder;
    public int DistanceToLeftBorder;

    public Cell(int xIndex, int yIndex)
    {
        
        XIndex = xIndex;
        YIndex = yIndex;
        
        IsConfirmed = false;
        IsDiscarded = false;
        
        Mark = "0";
        
        

        hasRowClueAssigned = false;
        hasColumnClueAssigned = false;

        IsColumnLowerLimit = false;
        IsColumnUpperLimit = false;

        IsRowLowerLimit = false;
        IsRowUpperLimit = false;

    }

    public void setLimitFlags(bool isRow, int length)
    {
        if (isRow)
        {
            if (XIndex == 0) IsRowLowerLimit = true;
            if (XIndex == length - 1) IsRowUpperLimit = true;
        }
        else
        {
            if (YIndex == 0) IsColumnLowerLimit = true;
            if (YIndex == length - 1) IsColumnUpperLimit = true;
        }
    }

    public void confirm()
    {
        Mark = "1";
        IsConfirmed = true;
    }

    public void discard()
    {
        Mark = "x";
        IsDiscarded = true;
    }

    public void setPartOfRowClue(Clue rowClue)
    {
        RowCluePartOf = rowClue;
        hasRowClueAssigned = true;
    }
    public void setPartOfColumnClue(Clue columnClue)
    {
        ColumnCluePartOf = columnClue;
        hasColumnClueAssigned = true;

    }

    public void updateWithOuterChanges(Cell outerCell)
    {
        IsConfirmed = outerCell.IsConfirmed;
        IsDiscarded = outerCell.IsDiscarded;
        Mark = outerCell.Mark;
    }

}
