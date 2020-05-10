public class Cell
{
    private readonly int _xIndex;
    private readonly int _yIndex;
    
    public bool IsConfirmed;
    public bool IsDiscarded;
    public bool DemonMark;

    public readonly int[,] LogicalMatrix;

    public string Mark; //'x' for discarded, '1' for confirmed, '0' for not processed
    
    public Cell(int xIndex, int yIndex, int[,] logicalMatrix) {
        _xIndex = xIndex;
        _yIndex = yIndex;
        
        IsConfirmed = false;
        IsDiscarded = false;
        DemonMark = false;

        LogicalMatrix = logicalMatrix;
        
        Mark = "0";
        

    }

    public void Confirm(bool doDraw) {

        if (doDraw) {
            LogicalMatrix[_xIndex, _yIndex] = 2;
        }
        else {
            LogicalMatrix[_yIndex, _xIndex] = 2;
        }
        Mark = "1";
        IsConfirmed = true;
    }

    public void Discard(bool doDraw)
    {
        if (doDraw) {
            LogicalMatrix[_xIndex, _yIndex] = 3;
        }else {
            LogicalMatrix[_yIndex, _xIndex] = 3;
        }
        Mark = "x";
        IsDiscarded = true;
    }

    public void DeConfirm(bool doDraw) {
        if (doDraw) {
            LogicalMatrix[_xIndex, _yIndex] = 1;
        }else {
            LogicalMatrix[_yIndex, _xIndex] = 1;
        }
        IsConfirmed = false;
        IsDiscarded = false;
        Mark = "0";
    }

    public void Undo(string originalMark, bool originalIsConfirmed, bool originalIsDiscarded, bool doDraw, int originalValue) {
        if (doDraw) {
            LogicalMatrix[_xIndex, _yIndex] = originalValue;
        }else {
            LogicalMatrix[_yIndex, _xIndex] = originalValue;
        }
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