using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleBoxes
{
    
    public bool canDo(Line lineBeingAnalyzed)
    {

        return lineBeingAnalyzed.sumOfClues > lineBeingAnalyzed.Length / 2;

    }
 
    public List<Cell> simpleBoxes([NotNull] Line lineBeingAnalyzed)
    {
       
        
        List<int> clues = lineBeingAnalyzed.ClueValues;
        int length = lineBeingAnalyzed.Length;
        
        List<int> minCase = getMinCase(clues, length);
        List<int> maxCase = getMaxCase(clues, length);
    
        List<int> simpleBoxesResult = commonConfirmedCellsBetweenLines(minCase, maxCase);

        lineBeingAnalyzed.setCells(updateLine(lineBeingAnalyzed, simpleBoxesResult));
        
        return lineBeingAnalyzed.getCells();
    
    }

    public List<Cell> updateLine(Line lineBeingAnalyzed, List<int> simpleBoxesResult)
    {
        
        for (int index = 0; index < lineBeingAnalyzed.Length; index++)
        {
            if (simpleBoxesResult[index] == 1) lineBeingAnalyzed.getCells()[index].confirm();
        }

        return lineBeingAnalyzed.getCells();
    }
    
    private List<int> getMinCase(List<int> clues, int lineSize) {
        List<int> minCase = getMinClueDistribution(clues);
        
        while(minCase.Count < lineSize){
            minCase.Add(9);
        }
        
        return minCase;
    }
    
    private List<int> getMaxCase(List<int> clues, int lineSize) {
        List<int> maxCase = getMinClueDistribution(clues);
        
        while(maxCase.Count < lineSize){
            maxCase.Insert(0,9);
        }
        
        return maxCase;
    }
    
    private List<int> getMinClueDistribution(List<int> clues){
        
        List<int> minClueDistribution = new List<int> {};
        int cluesQuantity = clues.Count;
        
        for (int clueIndex = 0; clueIndex < cluesQuantity; clueIndex++){
            
            for(int clueCounter = 0; clueCounter < clues[clueIndex]; clueCounter++){
                minClueDistribution.Add(1);
            }
            if(clueIndex < clues.Count-1){
                minClueDistribution.Add(9);
            }    
        
        }
        return minClueDistribution;
    }
    
    private List<int> commonConfirmedCellsBetweenLines(List<int> lineA, List<int> lineB){
    
        List<int> commonConfirmedCells = new List<int> {};
    
        for (int index = 0; index < lineA.Count; index++){
            
            if (lineA[index] == 1 && lineB[index] == 1){
                commonConfirmedCells.Add(1);
            }
            else{
                commonConfirmedCells.Add(9);
            }
    
        }
        return commonConfirmedCells;
       
    }


}
