﻿using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Row : Line
{
    public Row(List<int> clues, int length, int index) : base(clues, length, index)
    {
    }

    protected override void createCellList()
    {
        Cells = new List<Cell>();
        for (int counter = 0; counter < Length; counter++)
        {
            Cell newCell = createCell(_index, counter);
            newCell.setLimitFlags(true, Length);
            Cells.Add(newCell);
        }
    }

    /*
    public override void createClues()
    {
        for (int index = 0; index < Length; index++)
        {
            if (Cells[index].IsConfirmed && !Cells[index].hasRowClueAssigned)
            {
                index = createClue(index);
            }
        }
    }

    protected override void assignCluesToCells()
    {
        
    }
    */

}
