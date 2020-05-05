using System.Collections.Generic;
using UnityEngine;

public class NonogramSolver : MonoBehaviour {
    private int _height;
    private int _length;
    private readonly List<List<int>> _baseNonogram = new List<List<int>>();
    private readonly List<List<int>> _rowClues = new List<List<int>>();
    private readonly List<List<int>> _columnClues = new List<List<int>>();
    private bool _skip;
    public GameObject cell;

    public Matrix matrix;

    // Start is called before the first frame update
    void Start()
    {

        //TESTS tests = new TESTS();
        
        CreateCluesMatrix();
        CreateLogicalMatrixRepresentation();
        
        // foreach (var rowClue in _rowClues) {
        //     foreach (var i in rowClue) {
        //         Debug.Log(i);
        //     }
        //     Debug.Log("-------");
        // }
        
        Board board = Board.MakeFooObject(_height, _length);
        
        board.Draw(cell);
        
        matrix = new Matrix(_rowClues, _columnClues, _height, _length);
    }

    private void CreateLogicalMatrixRepresentation() {

        for (int i = 0; i < _height; i++) {
            List<int> row = new List<int>();
            for (int j = 0; j < _length; j++) {
                row.Add(9);
            }

            _baseNonogram.Add(row);
        }

    }

    private void CreateCluesMatrix() {
        string[] lines = System.IO.File.ReadAllLines(@"Assets\Scripts\input4.txt");
        bool rows = true;
        foreach (string line in lines) {
            if (!_skip) {
                _skip = true;
                string[] words = line.Split(',');
                _length = int.Parse(words[0].Trim());
                _height = int.Parse(words[1].Trim());
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
                            _rowClues.Add(listToAdd);
                        }
                        else {
                            _columnClues.Add(listToAdd);
                        }
                    }
                }
            }
        }
    }
    
    private void PrintMatrix(List<List<int>> matrix) {
        foreach (List<int> row in matrix) {
            string test = "[";
            foreach (int cell in row) {
                test += cell + ",";
            }
            test += "]";
            Debug.Log(test);
        }
    }

    private static void PrintList(List<int> list) {
        string test = "[";
        foreach (int cell in list) {
            test += cell + ",";
        }
        test += "]";
        Debug.Log(test);
    }
}
