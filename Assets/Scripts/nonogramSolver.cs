using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nonogramSolver : MonoBehaviour {
    private int height;
    private int length;
    private List<List<int>> baseNonogram = new List<List<int>>();
    private List<List<int>> RowClues = new List<List<int>>();
    private List<List<int>> ColumnClues = new List<List<int>>();
    private bool skip = false;
    public GameObject cell;

    public Matrix matrix;

    // Start is called before the first frame update
    void Start() {

        createCluesMatrix();
        createLogicalMatrixRepresentation();
        
        Board board = Board.MakeFooObject(height, length);
        
        board.draw(cell);
        
        matrix = new Matrix(baseNonogram, RowClues, ColumnClues, height, length);
    }

    private void createLogicalMatrixRepresentation() {

        for (int i = 0; i < height; i++) {
            List<int> row = new List<int>();
            for (int j = 0; j < length; j++) {
                row.Add(9);
            }

            baseNonogram.Add(row);
        }

    }

    private void createCluesMatrix() {
        string[] lines = System.IO.File.ReadAllLines(@"Assets\Scripts\input2.txt");
        bool rows = true;
        foreach (string line in lines) {
            if (!skip) {
                skip = true;
                string[] words = line.Split(',');
                length = int.Parse(words[0].Trim());
                height = int.Parse(words[1].Trim());
            }
            else {
                if (!line.Contains("FILAS")) {
                    if (line.Contains("COLUMNAS")) {
                        rows = false;
                    }
                    else {  
                        string[] words = line.Split(',');

                        List<int> listToAdd = new List<int>();
                        
                        foreach (string number in words)
                        {
                            listToAdd.Add(int.Parse(number.Trim()));
                        }
                        
                        if (rows) {
                            RowClues.Add(listToAdd);
                        }
                        else {
                            ColumnClues.Add(listToAdd);
                        }
                    }
                }
            }
        }
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
