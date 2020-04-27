public class Cell
{

    public int XIndex;
    public int YIndex;
    
    public bool IsConfirmed;
    public bool IsDiscarded;

    public string Mark; //'x' for discarded, '1' for confirmed, '0' for not processed
    
    public Cell(int xIndex, int yIndex)
    {
        
        XIndex = xIndex;
        YIndex = yIndex;
        
        IsConfirmed = false;
        IsDiscarded = false;
        
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

    public void UpdateWithOuterChanges(Cell outerCell)
    {
        IsConfirmed = outerCell.IsConfirmed;
        IsDiscarded = outerCell.IsDiscarded;
        Mark = outerCell.Mark;
    }

}
