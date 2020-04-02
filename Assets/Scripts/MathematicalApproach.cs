using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathematicalApproach
{

    public static void mathematicalApproachMethod(List<int> pClueValues,
                                                  int pListStartIndex,
                                                  int pListEndIndex,
                                                  int pClueStartIndex,
                                                  int pClueEndIndex,
                                                  Line pLine)
    {
        int size = pListEndIndex - pListStartIndex + 1;
        int clueSpannig = getCluesSpanning(pClueValues, pClueStartIndex, pClueEndIndex);

        int markDeterminator = size - clueSpannig;
        
        while (pClueStartIndex < pListEndIndex)
        {
            int quantity = pClueValues[pClueStartIndex] - markDeterminator;
            pListStartIndex += pClueValues[pClueStartIndex] - 1;
            
            backMarkCells(pListStartIndex, quantity, pLine);
            
            pClueStartIndex++;
            pListStartIndex++;
            
        }

    }

    public static void backMarkCells(int pIndex, int pQuantity, Line pLine)
    {
        int internalIndex = pIndex;
        int markedCells = 0;
        
        while (markedCells != pQuantity)
        {
            pLine.getCells()[internalIndex].confirm();
            markedCells += 1;
            
            internalIndex -= 1;
        }
    }
    
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
