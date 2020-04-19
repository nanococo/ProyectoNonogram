using System.Collections.Generic;

public static class MathematicalApproach
{

    public static List<Cell> MathematicalApproachMethod(List<int> pClueValues, int pListStartIndex, int pListEndIndex,
        int pClueStartIndex, int pClueEndIndex, Line pLine)
    {
        int size = pListEndIndex - pListStartIndex + 1;
        int clueSpanning = GetCluesSpanning(pClueValues, pClueStartIndex, pClueEndIndex);
        int markDeterminator = size - clueSpanning;

        while (pClueStartIndex <= pClueEndIndex)
        {

            int quantity = pClueValues[pClueStartIndex] - markDeterminator;
            pListStartIndex += pClueValues[pClueStartIndex] - 1;
            pLine = BackMarkCells(pListStartIndex, quantity, pLine);

            pClueStartIndex++;
            pListStartIndex += 2;
            
        }

        return pLine.GetCells();
    }

    
    //Backmarks the number of cells indicated in the 
    private static Line BackMarkCells(int pIndex, int pQuantity, Line pLine)
    {
        int internalIndex = pIndex;
        int markedCells = 0;
        
        if (pQuantity < 0) pQuantity = 0;
        
        while (markedCells != pQuantity)
        {
            pLine.MarkCell(internalIndex);
            markedCells += 1;
            
            internalIndex -= 1;
        }

        return pLine;
    }
    
    
    //Recursive method that get the sum of the clue values inside the indexes and the spaces between them
    private static int GetCluesSpanning(List<int> pClueValues, int pClueStartIndex, int pListEndIndex)
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
