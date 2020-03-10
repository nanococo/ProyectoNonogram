using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nonogramSolver : MonoBehaviour {
    private int height;
    private int length;
    private List<List<int>> baseNonogram = new List<List<int>>();
    private List<List<int>> X = new List<List<int>>();
    private List<List<int>> Y = new List<List<int>>();
    private List<int> columnEasyAccessList = new List<int>();
    private bool skip = false;

    public GameObject cell;


    // Start is called before the first frame update
    void Start() {
        createCluesMatrix();
        createLogicalMatrix();


        Board board = Board.MakeFooObject(this.height, this.length);

        Debug.Log(this.length);
        board.draw(cell);
        solvePuzzle();
        Debug.Log("X-CLUES");
        printMatrix(X);
        Debug.Log("X-CLUES");
        Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%");
        Debug.Log("Y-CLUES");
        printMatrix(Y);
        Debug.Log("Y-CLUES");
        Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%");
        Debug.Log("MATRIX");
        printMatrix(baseNonogram);
        Debug.Log("MATRIX");
        // Debug.Log("Min and max cases");
        // printList(getMinCase(Y[5],baseNonogram[0].Count));
        // printList(getMaxCase(Y[5],baseNonogram[0].Count));
        // Debug.Log("Result");
        // printList(simpleBoxes(baseNonogram[0], Y[5]));

    }

    // Update is called once per frame
    void Update() {

    }

    void createLogicalMatrix() {
        for (int i = 0; i < this.height; i++) {
            List<int> row = new List<int>();
            for (int j = 0; j < this.length; j++) {

                row.Add(9);

            }
            baseNonogram.Add(row);
        }
    }

    void createCluesMatrix() {
        string[] lines = System.IO.File.ReadAllLines(@"Assets\Scripts\input.txt");
        bool rows = true;
        foreach (string line in lines) {
            if (!skip) {
                skip = true;
                string[] words = line.Split(',');
                this.length = int.Parse(words[0].Trim());
                this.height = int.Parse(words[1].Trim());
            }
            else {
                if (!line.Contains("FILAS")) {
                    if (line.Contains("COLUMNAS")) {
                        rows = false;
                    }
                    else {  
                        string[] words = line.Split(',');

                        List<int> listToAdd = new List<int>();
                        //listToAdd.Add(0); //Initial Zero for completenes of row.
                        foreach (string number in words) {
                            listToAdd.Add(int.Parse(number.Trim()));

                        }
                        

                        if (rows) {
                            X.Add(listToAdd);
                        }
                        else {
                            Y.Add(listToAdd);
                        }
                    }
                }
            }
        }
    }

    void solvePuzzle(){
        
        Debug.Log("Rows");
        goThroughRows();
        Debug.Log("Collumns");
        goThroughCollumns();
       

    }

    void goThroughRows(){
        for(int index = 0; index < height; index++){
            analyzeLine(true, index);
        }
    }
    
    
    void goThroughCollumns(){

        for(int index = 0; index < length; index++){
            analyzeLine(false, index);
        }
    
    }

    void analyzeLine(bool isRow, int index) {

        List<int> lineBeingAnalyzed;
        List<int> clues;
        if (isRow) {
            lineBeingAnalyzed = this.baseNonogram[index];
            clues = getCluesByIndex(index, true);

        } else {
            setColumnAccessList(index);
            lineBeingAnalyzed = columnEasyAccessList;
            clues = getCluesByIndex(index, false);


        }

        if (isUpToSimpleBoxes(lineBeingAnalyzed, clues)) {
            lineBeingAnalyzed = simpleBoxes(lineBeingAnalyzed, clues);
            printList(lineBeingAnalyzed);
            copyResultIntoNonogram(isRow, index, lineBeingAnalyzed);
        }
        
    }

    void copyResultIntoNonogram(bool isRow, int index, List<int> lineProcessed){

        if(isRow){

            baseNonogram[index] = lineProcessed;

        } else{

            copyResultIntoCollumn(index, lineProcessed);

        }

    }

    void copyResultIntoCollumn(int index, List<int> lineProcessed){
        
        for(int nonogramIndex = 0; nonogramIndex < baseNonogram.Count; nonogramIndex++){
            baseNonogram[nonogramIndex][index] = lineProcessed[nonogramIndex];
        }

    }

   
    List<int> simpleBoxes(List<int> lineBeingAnalyzed, List<int> clues) {

        int lineSize = lineBeingAnalyzed.Count;

        List<int> minCase = getMinCase(clues, lineSize);
        List<int> maxCase = getMaxCase(clues, lineSize);

        List<int> simpleBoxesResult = commonConfirmedCellsBetweenLines(minCase, maxCase);

        return simpleBoxesResult;
    
    }



    List<int> getMinCase(List<int> clues, int lineSize) {

        List<int> minCase = getMinClueDistribution(clues, lineSize);

        while(minCase.Count < lineSize){
            minCase.Add(9);
        }
        return minCase;
    
    }

    List<int> getMaxCase(List<int> clues, int lineSize) {

        List<int> maxCase = getMinClueDistribution(clues, lineSize);
        int maxSapcesBeforeClues = getMaxSpacesBeforeClues(clues, lineSize);

        
        while(maxCase.Count < lineSize){
            maxCase.Insert(0,9);
            maxSapcesBeforeClues--;
        }

       return maxCase;


    void markCell(int xIndex, int yIndex, int oneOrTwo) { //2 marks discarded, 1 marks confirmed
        this.baseNonogram[xIndex][yIndex] = oneOrTwo;
    }


    List<int> getMinClueDistribution(List<int> clues, int lineSize){
        
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

    bool isUpToSimpleBoxes(List<int> lineBeingAnalyzed, List<int> clues) {
        
        int sumOfClues = addListElements(clues);
        if (sumOfClues > (lineBeingAnalyzed.Count / 2) && isAnEmptyLine(lineBeingAnalyzed)) { 
            return true;
        }
        else return false;
    
    }

    int getMaxSpacesBeforeClues(List<int> clues, int listSize){

        int sumOfClues = addListElements(clues);
        int conditionSum = sumOfClues + getObligatorySpacesBetweenClues(clues);
        int maxSapcesBeforeClues = listSize - conditionSum;
        
        return maxSapcesBeforeClues;

    }

    int addListElements(List<int> list) {
        int sum = 0;        
        foreach (int element in list){
            sum += element;
        }
        return sum;
    }




    int getObligatorySpacesBetweenClues(List<int> clues) {
        return clues.Count - 1; 
    }


    bool isAnEmptyLine(List<int> line) {
        bool isEmpty = true;
        foreach (int element in line) {
            if (element != 9) isEmpty = false;
        }
        return isEmpty;
    }

    List<int> getCluesByIndex(int index, bool isRow) {
     
        if (isRow) {
            return X[index];
        }
        else return Y[index];
    
    }
    

    void markCell(int xIndex, int yIndex, int oneOrZero) { //0 marks discarded, 1 marks confirmed
        this.baseNonogram[xIndex][yIndex] = oneOrZero;
    }

    void setColumnAccessList(int collumnIndex) { //Sets the values of the column access list with the given collumn index
        this.columnEasyAccessList.Clear();

        foreach (List<int> row in this.baseNonogram) {
            for (int cellIndex = 0; cellIndex < this.length; cellIndex++) {
                if (cellIndex == collumnIndex) {
                    this.columnEasyAccessList.Add(row[cellIndex]);
                }
            }
        }

        
    }

    List<int> commonConfirmedCellsBetweenLines(List<int> lineA, List<int> lineB){ //Lines the same size

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

    bool isEvenSize(List<int> list){
        if (list.Count % 2 == 0){
            return true;
        } else return false;

    }

    void printMatrix(List<List<int>> matrix) {
        foreach (List<int> row in matrix) {
            string test = "[";
            foreach (int cell in row) {
                test += cell + ",";
            }
            test += "]";
            Debug.Log(test);
        }
    }
    
    void printList(List<int> list) {
        string test = "[";
        foreach (int cell in list) {
            test += cell + ",";
        }
        test += "]";
        Debug.Log(test);
    }

}
