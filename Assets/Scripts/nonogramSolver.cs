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

    // Start is called before the first frame update
    void Start() {

        createCluesMatrix();
        createLogicalMatrix();

    }

    // Update is called once per frame
    void Update() {

    }

    void createLogicalMatrix() {

        Debug.Log(this.height);
        Debug.Log(this.lenght);

        List<int> row = new List<int>(); ;
        for (int i = 0; i < 1; i++) {
            for (int j = 0; j < 1; j++) {
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

   


}
