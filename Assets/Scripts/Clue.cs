using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue
{

    public List<int> PossibleInitialValues;
    
    public int InitialValue;
    public int LeftValue;
    public int BlockSize;
    public int MinIndex;
    public int MaxIndex;
    public int DistanceToRightBoundary;
    public int DistanceToLeftBoundary;

    public Clue(int minIndex, int maxIndex, List<int> possibleInitialValues)
    {
        PossibleInitialValues = possibleInitialValues;
        MaxIndex = maxIndex;
        MinIndex = minIndex;
        DistanceToRightBoundary = 0;
        DistanceToLeftBoundary = 0;
        calcBlockSize();
    }

    public void updateClue()
    {
        updateCompleteness();
        if (PossibleInitialValues.Count != 1) return;
        InitialValue = PossibleInitialValues[0];
        LeftValue = InitialValue - BlockSize;
    }

    private void updateCompleteness()
    {
        LeftValue--;
    }


    private void calcBlockSize()
    {
        BlockSize = MaxIndex - MinIndex + 1;
    }

    public void calcDistanceToRightBoundary(Line line)
    {
        for (int index = MaxIndex; index < line.Length; index++)
        {
            
            if (line.getCells()[index].IsConfirmed || line.getCells()[index].IsDiscarded)
            {
                index = line.Length;
            }

            DistanceToRightBoundary++;
        }
    }

    public void calcDistanceToLeftBoundary(Line line)
    {
       
        

        for (int index = MinIndex; index >= 0; index--)
        {
            
            if (line.getCells()[index].IsConfirmed || line.getCells()[index].IsDiscarded)
            {
                index = -1;
            }
            DistanceToLeftBoundary++;
            
        }
    }

    public override string ToString()
    {
        return BlockSize.ToString();
    }
}
