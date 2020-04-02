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
        createLogicalMatrix();
        
        matrix = new Matrix();

        Board board = Board.MakeFooObject(height, length);
        
        board.draw(cell);

        // Debug.Log("MATRIX");
        // printMatrix(baseNonogram);
        // Debug.Log("MATRIX");
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
                        //listToAdd.Add(0); //Initial Zero for completenes of row.
                        foreach (string number in words) {
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
