using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    private int width;
    private int height;

    // Start is called before the first frame update
    void Start() {
        //Instantiate(cell, new Vector2(0,0), Quaternion.identity);
    }

    public static Board MakeFooObject(int height, int width) {
        GameObject go = new GameObject("Board");
        Board ret = go.AddComponent<Board>();
        ret.height = height;
        ret.width = width;
        return ret;
    }

    public void setWidth(int width) {
        this.width = width;
    }

    public void setHeight(int height) {
        this.height = height;
    }

    public void draw(GameObject cell) {
        Debug.Log(this.width);
        int xDisplacement = 0;
        for (int i = 0; i < this.width; i++) {
            Instantiate(cell, new Vector2(xDisplacement, 0), Quaternion.identity);
            xDisplacement += 4;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
