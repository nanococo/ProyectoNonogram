using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nonogramSolver : MonoBehaviour {
    private int height;
    private int length;
    private List<List<int>> baseNonogram = new List<List<int>>();
    private List<List<int>> X = new List<List<int>>();
    private List<List<int>> Y = new List<List<int>>();
    private List<int> collumnAccessList = new List<int>();
    private bool skip = false;
    public GameObject cell;


    public Matrix matrix;

    // Start is called before the first frame update
    void Start() {

        createCluesMatrix();
        createLogicalMatrix();
        
        matrix = new Matrix();

        Board board = Board.MakeFooObject(this.height, this.length);

        Debug.Log(this.length);
        board.draw(cell);
        solvePuzzle();
       
        // Debug.Log("MATRIX");
        // printMatrix(baseNonogram);
        // Debug.Log("MATRIX");
        
        

        
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

        bool hasToStartWithRows = startWithRows();

        
            Debug.Log("Rows");
            goThroughRows();
            Debug.Log("Collumns");
            goThroughColumns();
        
        
        
    }

    private bool startWithRows(){ //NO
        bool hasToStartWithRows = !(getGreaterSumOfRowsInAnIntArraw(Y) > getGreaterSumOfRowsInAnIntArraw(X));
        return hasToStartWithRows;
    }

    private int getGreaterSumOfRowsInAnIntArraw(List<List<int>> array){ //NO
        int greaterSum = 0;
        int sum;
        foreach (List<int> row in array){
            sum = addListElements(row); 
            if (sum > greaterSum){
                greaterSum = sum;
            }
        }
        return greaterSum;
    }


    private void goThroughRows(){ //NO
        for(int index = 0; index < height; index++){
            analyzeLine(true, index);
        }
    }


    private void goThroughColumns(){ //NO

        for(int index = 0; index < length; index++){
            analyzeLine(false, index);
        }
    
    }

    private void analyzeLine(bool isRow, int index) {
        List<int> lineBeingAnalyzed;
        List<int> clues;
        if (isRow) {
            lineBeingAnalyzed = this.baseNonogram[index]; //NO
            clues = getCluesByIndex(index, true); //NO

        } else {
            setColumnAccessList(index);
            lineBeingAnalyzed = collumnAccessList; //NO
            clues = getCluesByIndex(index, false); //NO

        }

        if (isUpToSimpleBoxes(lineBeingAnalyzed, clues)) {
            lineBeingAnalyzed = simpleBoxes(lineBeingAnalyzed, clues);
            printList(lineBeingAnalyzed);
            copyResultIntoNonogram(isRow, index, lineBeingAnalyzed);
        }
        
    }


    

    private void copyResultIntoNonogram(bool isRow, int index, List<int> lineProcessed){

        if(isRow){

            baseNonogram[index] = lineProcessed;

        } else{

            copyResultIntoColumn(index, lineProcessed);

        }

    }

    private void copyResultIntoColumn(int index, List<int> lineProcessed){
        
        for(int nonogramIndex = 0; nonogramIndex < baseNonogram.Count; nonogramIndex++){
            baseNonogram[nonogramIndex][index] = lineProcessed[nonogramIndex];
        }

    }


    private List<int> simpleBoxes(List<int> lineBeingAnalyzed, List<int> clues) {

        int lineSize = lineBeingAnalyzed.Count;

        List<int> minCase = getMinCase(clues, lineSize);
        List<int> maxCase = getMaxCase(clues, lineSize);

        List<int> simpleBoxesResult = commonConfirmedCellsBetweenLines(minCase, maxCase);

        return simpleBoxesResult;
    
    }
    
    


    private List<int> getMinCase(List<int> clues, int lineSize) {

        List<int> minCase = getMinClueDistribution(clues, lineSize);

        while(minCase.Count < lineSize){
            minCase.Add(9);
        }
        return minCase;
    
    }

    private List<int> getMaxCase(List<int> clues, int lineSize) {

        List<int> maxCase = getMinClueDistribution(clues, lineSize);


        while(maxCase.Count < lineSize){
            maxCase.Insert(0,9);
        }

        return maxCase;
    }




    private List<int> getMinClueDistribution(List<int> clues, int lineSize){
        
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

    private bool isUpToSimpleBoxes(List<int> lineBeingAnalyzed, List<int> clues) {
        
        int sumOfClues = addListElements(clues);
        if (sumOfClues > (lineBeingAnalyzed.Count / 2) && isAnEmptyLine(lineBeingAnalyzed)) { 
            return true;
        }
        else return false;
    
    }
    
    
    private bool isUpToSimpleSpaces(List<int> line, List<int> clues)
    {
        
        return true;
        
    }

    private void simpleSpaces(List<int> lineBeingAnalyzed, List<int> clues)
    {
        
    }

    private int getNextConfirmedIndex(List<int> line, int departureIndex)
    {
        int index = departureIndex;
        while (line[index] != 1)
        {
            index++;
        }
        return index;
    }

    private int getMaxSpread(int clue, int clueCompleteness)
    {

        return clue - clueCompleteness;

    }

    private int getNumberOfConfirmedInARow(int beginIndex, List<int> line)
    {
        int confirmedInARow = 0;
        while (line[beginIndex] == 1 && beginIndex < line.Count)
        {
            confirmedInARow++;
            beginIndex++;
        }

        return confirmedInARow;
    }


    private int distanceToBorder(bool isRightBorder, int lineSize, int index)
    {
        int distance;
        if (isRightBorder)
        {
            distance = lineSize - (index + 1);
        }
        else
        {
            distance = index;
        }
        
        return distance;
    }

    static int addListElements(List<int> list) {
        int sum = 0;        
        foreach (int element in list){
            sum += element;
        }
        return sum;
    }


    static int getObligatorySpacesBetweenClues(List<int> clues) {
        return clues.Count - 1; 
    }


    static bool isAnEmptyLine(List<int> line) {
        bool isEmpty = true;
        foreach (int element in line) {
            if (element != 9) isEmpty = false;
        }
        return isEmpty;
    }

    private List<int> getCluesByIndex(int index, bool isRow) {
     
        if (isRow) {
            return X[index];
        }
        else return Y[index];
    
    }

    private void setColumnAccessList(int collumnIndex) { //Sets the values of the column access list with the given collumn index

        this.collumnAccessList.Clear();

        foreach (List<int> row in this.baseNonogram) {
            for (int cellIndex = 0; cellIndex < this.length; cellIndex++) {
                if (cellIndex == collumnIndex) {
                    this.collumnAccessList.Add(row[cellIndex]);
                }
            }
        }
        
    }

    static List<int> commonConfirmedCellsBetweenLines(List<int> lineA, List<int> lineB){ //Lines the same size

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

    private bool isEvenSize(List<int> list){
        if (list.Count % 2 == 0){
            return true;
        } else return false;
    }
    
    private void discardCell(int xIndex, int yIndex) {
        this.baseNonogram[xIndex][yIndex] = 0;
    }

    private void printMatrix(List<List<int>> matrix) {
        foreach (List<int> row in matrix) {
            string test = "[";
            foreach (int cell in row) {
                test += cell + ",";
            }
            test += "]";
            Debug.Log(test);
        }
    }

    private static void printList(List<int> list) {
        string test = "[";
        foreach (int cell in list) {
            test += cell + ",";
        }
        test += "]";
        Debug.Log(test);
    }
}
