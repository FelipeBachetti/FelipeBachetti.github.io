using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMatrix : MonoBehaviour
{
    public static BuildingMatrix Instance { get; private set;}

    private GameObject[,] matrix = new GameObject[500, 500];
    Plane plane = new Plane(Vector3.up, 0);
    private Camera cam;

    private void Awake() {
        Instance = this;
        cam = Camera.main;
        initialize();
    }

    public Vector3 returnWorldPosition(){
        Vector3 worldPosition = new Vector3(0f,0f,0f);
        float distance;
        Plane plane = new Plane(Vector3.up, 0);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }
        worldPosition = new Vector3(findCoordinates(worldPosition.x), 0f, findCoordinates(worldPosition.z));
        return worldPosition;
    }

    public void storeInMatrix(GameObject g, int x, int y){
        matrix[x, y] = g;
    }

    public GameObject checkInMatrix(int x, int y){
        if(x < 500 && x > 0 && y < 500 && y > 0)
            return matrix[x, y];
        else
            return null;
    }

    private float findCoordinates(float x){
        x+=250f;
        if(x%4>2){
            return (x + (4-(x%4))) - 250f;
        }else{
            return (x - (x%4))-250f;
        }
    }

    private void initialize(){
        for(int i = 0; i < 500; i++)
            for(int j = 0; j < 500; j++)
                matrix[i,j] = null;
    }
}
