using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathematicalApproach
{

    public static List<Cell> mathematicalApproachMethod(List<int> pClueValues, int pListStartIndex, int pListEndIndex,
        int pClueStartIndex, int pClueEndIndex, Line pLine)
    {
        int size = pListEndIndex - pListStartIndex + 1;
        int clueSpannig = getCluesSpanning(pClueValues, pClueStartIndex, pClueEndIndex);
        int markDeterminator = size - clueSpannig;

        while (pClueStartIndex <= pClueEndIndex)
        {

            int quantity = pClueValues[pClueStartIndex] - markDeterminator;
            pListStartIndex += pClueValues[pClueStartIndex] - 1;
            pLine = backMarkCells(pListStartIndex, quantity, pLine);

            pClueStartIndex++;
            pListStartIndex += 2;
            
        }

        return pLine.getCells();
    }

    
    //Backmarks the number of cells indicated in the 
    public static Line backMarkCells(int pIndex, int pQuantity, Line pLine)
    {
        int internalIndex = pIndex;
        int markedCells = 0;
        
        if (pQuantity < 0) pQuantity = 0;
        
        while (markedCells != pQuantity)
        {
            pLine.getCells()[internalIndex].confirm();
            markedCells += 1;
            
            internalIndex -= 1;
        }

        return pLine;
    }
    
    
    //Recursive method that get the sum of the clue values inside the indexes and the spaces between them
    private static int getCluesSpanning(List<int> pClueValues, int pClueStartIndex, int pListEndIndex)
    {
        return getCluesSpanning_aux(pClueStartIndex, pListEndIndex, 0, pClueValues);
    } 
    private static int getCluesSpanning_aux(int pIndex, int pEndIndex, int pSpanning, List<int> pClueValues)
    {
        if (pIndex == pEndIndex)
        {
            return pSpanning + pClueValues[pIndex];
        }
        int newIndex = pIndex + 1;
        return getCluesSpanning_aux(newIndex, pEndIndex, pSpanning + pClueValues[pIndex] + 1, pClueValues);
    } 
}
