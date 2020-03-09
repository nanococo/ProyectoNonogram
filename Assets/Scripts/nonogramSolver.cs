using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nonogramSolver : MonoBehaviour {
    private int height;
    private int lenght;
    private List<List<int>> baseNonogram = new List<List<int>>();
    private List<List<int>> X = new List<List<int>>();
    private List<List<int>> Y = new List<List<int>>();
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
    }

    // Update is called once per frame
    void Update() {

    }

    void createLogicalMatrix() {
        
        for (int i = 0; i < this.height; i++) {
            List<int> row = new List<int>();
            for (int j = 0; j < this.lenght; j++) {
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
                this.lenght = int.Parse(words[0].Trim());
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
                            //Debug.Log(int.Parse(number.Trim()));
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

   
    void simpleBoxes() {

    }
    void simpleSpaces() {

    }


}
