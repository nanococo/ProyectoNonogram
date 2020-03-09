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

        Board board = Board.MakeFooObject(this.height, this.lenght);
        //board.setHeight(this.height);
        //board.setWidth(this.lenght);
        Debug.Log(this.lenght);
        board.draw(cell);
        markCell(0, 0, 2);
        setColumnAccessList(5);
        Debug.Log("MATRIX");
        printMatrix(baseNonogram);
        Debug.Log("MATRIX");
        Debug.Log(isAnEmptyLine(columnEasyAccessList));
    }

    // Update is called once per frame
    void Update() {

    }

    void createLogicalMatrix() {
        for (int i = 0; i < this.height; i++) {
            List<int> row = new List<int>();
            for (int j = 0; j < this.length; j++) {
                row.Add(0);
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
                        listToAdd.Add(0); //Initial Zero for completenes of row.
                        foreach (string number in words) {
                            listToAdd.Add(int.Parse(number.Trim()));
                           // Debug.Log(int.Parse(number.Trim()));
                        }
                        
                        //string test = "[";
                        //foreach (int num in listToAdd) {
                        //    test += num + ",";
                        //}
                        //test += "]";

                        //Debug.Log(test);


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


    void analyzeRowOrColumn(bool isRow, int index) {
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
            lineBeingAnalyzed = simpleBoxes();
        }
        
    }

   
    List<int> simpleBoxes() {
        return new List<int> { 1, 1 };
    }

    bool isUpToSimpleBoxes(List<int> lineBeingAnalyzed, List<int> clues) {
        int sumOfClues = addListElements(lineBeingAnalyzed);
        
        int conditionSum = sumOfClues + getObligatorySpacesBetweenClues(clues);

        if (conditionSum > (lineBeingAnalyzed.Count / 2) && isAnEmptyLine(lineBeingAnalyzed)) {
            return true;
        }
        else return false;
    }

    int addListElements(List<int> list) {
        return addListElements_aux(list, 0, 0);
    }
    int addListElements_aux(List<int> list, int index, int sum) {
        if (index == list.Count) {
            return sum;
        }
        else return addListElements_aux(list, index++, sum + list[index]);
    }

    int getObligatorySpacesBetweenClues(List<int> clues) {
        return clues.Count - 1;
    }

    bool isAnEmptyLine(List<int> line) {
        bool isEmpty = true;
        foreach (int element in line) {
            if (element != 0) isEmpty = false;
        }
        return isEmpty;
    }

    List<int> getCluesByIndex(int index, bool isRow) {
     
        if (isRow) {
            return X[index];
        }
        else return Y[index];
    
    }

    void markCell(int xIndex, int yIndex, int oneOrTwo) { //2 marks discarded, 1 marks confirmed
        this.baseNonogram[xIndex][yIndex] = oneOrTwo;
    }

    void setColumnAccessList(int collumnIndex) { //Sets the values of the column access list with the given collumn index
        foreach (List<int> row in this.baseNonogram) {
            for (int cellIndex = 0; cellIndex < this.length; cellIndex++) {
                if (cellIndex == collumnIndex) {
                    this.columnEasyAccessList.Add(row[cellIndex]);
                }
            }
        }
        printList(columnEasyAccessList);
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
