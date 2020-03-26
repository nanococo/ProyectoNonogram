using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : Line
{
    public Column(List<int> clues, int length, int index) : base(clues, length, index)
    {
    }
    
    protected override void createCellList()
    {
        Cells = new List<Cell>();
        for (int counter = 0; counter < Length; counter++)
        {
            Cell newCell = createCell(_index, counter);
            newCell.setLimitFlags(false, Length);
            Cells.Add(newCell);
        }
    }

    
    protected override void createClues()
    {
        
        for (int index = 0; index < Length; index++)
        {
            if (Cells[index].IsConfirmed && !Cells[index].hasColumnClueAssigned)
            {
                index = createClue(index);
            }
            
        }

        
    }

    protected override void assignCluesToCells()
    {
        
    }
    
}
