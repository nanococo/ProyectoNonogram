﻿public class Cell
{

    public int XIndex;
    public int YIndex;
    
    public bool IsConfirmed;
    public bool IsDiscarded;
    public bool DemonMark;

    public string Mark; //'x' for discarded, '1' for confirmed, '0' for not processed
    
    public Cell(int xIndex, int yIndex)
    {
        
        XIndex = xIndex;
        YIndex = yIndex;
        
        IsConfirmed = false;
        IsDiscarded = false;
        DemonMark = false;
        
        Mark = "0";
        

    }

    public void Confirm()
    {
        Mark = "1";
        IsConfirmed = true;
    }

    public void Discard()
    {
        Mark = "x";
        IsDiscarded = true;
    }

    public void DeConfirm() {
        IsConfirmed = false;
        IsDiscarded = false;
        Mark = "0";
    }

    public void Undo(string originalMark, bool originalIsConfirmed, bool originalIsDiscarded) {
        Mark = originalMark;
        IsConfirmed = originalIsConfirmed;
        IsDiscarded = originalIsDiscarded;
    }

    public void UpdateWithOuterChanges(Cell outerCell)
    {
        IsConfirmed = outerCell.IsConfirmed;
        IsDiscarded = outerCell.IsDiscarded;
        Mark = outerCell.Mark;
    }

    public override string ToString()
    {
        return Mark;
    }

}
