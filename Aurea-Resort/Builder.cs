using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public static Builder Instance { get; private set; }

    [SerializeField] int id;
    [SerializeField] Camera cam;
    [SerializeField] LayerMask layerMaskPlaceable;
    [SerializeField] GameObject square;
    [SerializeField] GameObject dust, redCube;

    private bool isSelected, isPlacing, isDestroying;
    private Vector3 mousePosition;
    private GameObject newBuilding;
    private Plane plane = new Plane(Vector3.up, 0);
    private float distance;
    private Vector3 pos;
    private GameObject place, previousPlace;
    private int n=0;
    private Transform[] roomSquares;
    private Building currentBuilding;
    public Transform doorPos;

    public List<GameObject> planeList = new List<GameObject>();
    public List<Transform> redCubeList = new List<Transform>();
    public List<Transform> corridorList = new List<Transform>();
    public List<GameObject> buildings = new List<GameObject>();

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        createSquares(2, -2, 0, 0);
        FindObjectOfType<BuildingsUI>().initialCheck();
    }

    private void Update() {
        pos = BuildingMatrix.Instance.returnWorldPosition();
        place = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(pos.x), convertToMatrix(pos.z));
        if(isSelected && !isDestroying){
            if(Input.GetMouseButtonDown(1)){
                cancelBuilding();
            }else if(Input.GetMouseButtonDown(0) && !isPlacing){
                placeBuilding();
            }
            else{
                followMouse();
            }

            if(newBuilding != null && newBuilding.tag == "isRoom" && Input.GetButtonDown("Rotate") && !isPlacing){
                rotate();
            }
        }else if(isDestroying){
            if(Input.GetMouseButtonDown(0)){
                destroyBuilding();
            }

            if(place == null || place != null && place.tag == "plane"){
                redCubeSpawner(false);
                previousPlace = place;
            }else if(place != previousPlace){
                registerRoomSquares(place);
                redCubeSpawner(false);
                previousPlace = place;
                redCubeSpawner(true);
            }
        }
    }

    public void selectBuilding(Building building){
        if(!isSelected){
            //FindObjectOfType<AudioManager>().Play("select");
            isDestroying = false;
            currentBuilding = building;
            Cursor.visible = false;
            isSelected = true;
            mousePosition = Input.mousePosition;
            mousePosition = cam.ScreenToWorldPoint(mousePosition);
            newBuilding = Instantiate(building.prefab, new Vector3(mousePosition.x, 0f, mousePosition.z), Quaternion.identity);
            roomSquares = new Transform[5];
            if(newBuilding.tag == "isRoom"){
                registerRoomSquares(newBuilding);
            }
        }
    }

    public void selectDestroy(){
        isDestroying = !isDestroying;
    }

    private void destroyBuilding(){
        if(place!=null && place.tag != "plane"){
            if(place.tag == "isRoom"){
                int n=0;
                Transform[] sq = new Transform[5];

                BuildingDetails details = place.GetComponent<BuildingDetails>();
                if(details != null){
                    buildings.Remove(place);
                }
                
                foreach(Transform child in place.transform){
                    if(child.tag == "roomSquare"){
                        sq[n] = child;
                        n++;
                    }
                }
                foreach(Transform g in sq)
                    if(g!=null){
                        BuildingMatrix.Instance.storeInMatrix(null, convertToMatrix(g.position.x), convertToMatrix(g.position.z));
                        dustParticle(g.position);
                    }

                killGuests();
                Destroy(place); 
                checkArea();
            }else if(corridorList.Count>1 && !place.GetComponent<CorridorDetails>().isDoor){
                BuildingMatrix.Instance.storeInMatrix(null, convertToMatrix(pos.x), convertToMatrix(pos.z));
                corridorList.Remove(place.transform);
                dustParticle(pos);
                GameObject checkedPos;
                checkedPos = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(pos.x+4), convertToMatrix(pos.z));
                if(checkedPos != null && checkedPos.tag == "plane"){
                    Destroy(checkedPos);
                }
                checkedPos = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(pos.x-4), convertToMatrix(pos.z));
                if(checkedPos != null && checkedPos.tag == "plane"){
                    Destroy(checkedPos);
                }
                checkedPos = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(pos.x), convertToMatrix(pos.z+4));
                if(checkedPos != null && checkedPos.tag == "plane"){
                    Destroy(checkedPos);
                }
                checkedPos = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(pos.x), convertToMatrix(pos.z-4));
                if(checkedPos != null && checkedPos.tag == "plane"){
                    Destroy(checkedPos);
                }
                killGuests();
                Destroy(place); 
                checkArea();
            }
        }
    }

    private void RedoCorridorSquares(GameObject corridor){
        GameObject posi = null;
        posi = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(corridor.transform.position.x+4), convertToMatrix(corridor.transform.position.z));
        if(posi == null || posi.tag == "plane"){
            createSquares(corridor.transform.position.x, corridor.transform.position.z, 4, 0, corridor);
        }
        posi = null;
        posi = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(corridor.transform.position.x-4), convertToMatrix(corridor.transform.position.z));
        if(posi == null || posi.tag == "plane"){
            createSquares(corridor.transform.position.x, corridor.transform.position.z, -4, 0, corridor);
        }
        posi = null;
        posi = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(corridor.transform.position.x), convertToMatrix(corridor.transform.position.z+4));
        if(posi == null || posi.tag == "plane"){
            createSquares(corridor.transform.position.x, corridor.transform.position.z, 0, 4, corridor);
        }
        posi = null;
        posi = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(corridor.transform.position.x), convertToMatrix(corridor.transform.position.z-4));
        if(posi == null || posi.tag == "plane"){
            createSquares(corridor.transform.position.x, corridor.transform.position.z, 0, -4, corridor);
        }
    }

    private void checkArea(){
        for(int i = -10; i<=10; i+=4)
            for(int j = -10; j<=10; j+=4){
                GameObject currentCorridor = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(pos.x+i), convertToMatrix(pos.z+j));
                if(currentCorridor != null && currentCorridor.tag == "Corridor")
                    if(!(i==0 && j == 0))
                        RedoCorridorSquares(currentCorridor);
            }
    }

    private void followMouse(){
        if(place != null && place.tag == "plane"){
            newBuilding.transform.position =  place.transform.position;
        }else{
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(plane.Raycast(ray, out distance)){
                newBuilding.transform.position =  ray.GetPoint(distance);
            }
        }
    }

    private void cancelBuilding(){
        isSelected = false;
        Destroy(newBuilding);
        Cursor.visible = true;
    }

    private void placeBuilding(){
        if(pos.x > 250f || pos.y > 250f || pos.x < -250f || pos.y < -250f){
            Debug.Log("Out of bounds");
        }else{
            if(place != null && place.tag == "plane" && newBuilding.tag != "isRoom" || place != null && place.tag == "plane" && newBuilding.tag == "isRoom" && checkRoomSquare() && FindObjectOfType<ResourcesManager>().newBuilding(currentBuilding) && checkDoor()){
                newBuilding.GetComponent<BuildingCosts>().enabled = true;
                isPlacing = true;
                Destroy(place);
                place = null;
                isSelected = false;
                Cursor.visible = true;
                n=0;
                newBuilding.transform.position = pos;
                newBuilding.GetComponent<Rigidbody>().isKinematic = true;
                if(newBuilding.tag != "isRoom"){
                    BuildingMatrix.Instance.storeInMatrix(newBuilding, convertToMatrix(pos.x), convertToMatrix(pos.z));
                    corridorList.Add(newBuilding.transform);
                    createSquares(pos.x, pos.z, 4, 0, newBuilding);
                    createSquares(pos.x, pos.z, -4, 0, newBuilding);
                    createSquares(pos.x, pos.z, 0 ,4, newBuilding);
                    createSquares(pos.x, pos.z, 0, -4, newBuilding);
                    selectBuilding(currentBuilding);
                }else{
                    storeRoom();
                    PersonTracker tracker = newBuilding.GetComponent<PersonTracker>();
                    if(tracker!=null)
                        tracker.spawn(true);
                    BuildingDetails details = newBuilding.GetComponent<BuildingDetails>();
                    if(details != null){
                        buildings.Add(newBuilding);
                    }
                    newBuilding = null;
                }
            }else{
                Debug.Log("Occupied");
            }
        }
        isPlacing = false;
    }

    private int convertToMatrix(float x){
        return (int)((x/4)+250);
    }

    private void createSquares(float x, float y, float changeX, float changeY, GameObject assignedCor = null){
        GameObject location, newSquare;
        location = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(x+changeX), convertToMatrix(y+changeY));
        if(location == null || location.tag == "plane"){
            newSquare = Instantiate(square, new Vector3(x+changeX, 0f, y+changeY), Quaternion.identity);
            planeList.Add(newSquare);
            if(assignedCor!=null){
                newSquare.transform.parent = assignedCor.transform;
            }

            BuildingMatrix.Instance.storeInMatrix(newSquare, convertToMatrix(x+changeX), convertToMatrix(y+changeY));

            if(changeX<0){
                newSquare.transform.localRotation = Quaternion.Euler(0, 180f, 0);
            }else if(changeY<0){
                newSquare.transform.localRotation = Quaternion.Euler(0, 90f, 0);
            }else if(changeY>0){
                newSquare.transform.localRotation = Quaternion.Euler(0, -90f, 0);
            }else if(changeY == 0 && changeX == 0){
                foreach (Transform child in newSquare.transform) {
                    GameObject.Destroy(child.gameObject);
                }
            }

            if(location!=null){
                location.transform.parent = newSquare.transform;
            }
        }
    }

    public void rotate(){
        n++;
        switch(n){
            case 0: newBuilding.transform.localRotation = Quaternion.Euler(0, 0, 0); break;
            case 1: newBuilding.transform.localRotation = Quaternion.Euler(0, 90, 0); break;
            case 2: newBuilding.transform.localRotation = Quaternion.Euler(0, 180, 0); break;
            case 3: newBuilding.transform.localRotation = Quaternion.Euler(0, 270, 0); break;
            case 4: newBuilding.transform.localRotation = Quaternion.Euler(0, 0, 0); n = 0; break;
        }
    }

    private void registerRoomSquares(GameObject local){
        int num=0;
        foreach(Transform child in local.transform){
            if(child.tag == "roomSquare"){
                roomSquares[num] = child;
                num++;    
            }else if(child.tag == "door"){
                doorPos = child;
            }
        }
    }

    private bool checkRoomSquare(){
        GameObject roomSquarePos;
        for(int i_ = 0; i_ <= roomSquares.Length; i_++){
            if(roomSquares[i_]!=null){
                roomSquarePos = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(roomSquares[i_].position.x), convertToMatrix(roomSquares[i_].position.z));
                if(roomSquarePos != null && roomSquarePos.tag != "plane")
                    return false;
            }else{
                break;
            }
        }
        return true;
    }

    private void storeRoom(){
        GameObject gPlane;
        foreach(Transform t in roomSquares)
            if(t!=null){
                gPlane = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(t.position.x), convertToMatrix(t.position.z));
                if(gPlane != null)
                    Destroy(gPlane);
                BuildingMatrix.Instance.storeInMatrix(newBuilding, convertToMatrix(t.position.x), convertToMatrix(t.position.z));
            }
    }

    private bool checkDoor(){
        GameObject corr = BuildingMatrix.Instance.checkInMatrix(convertToMatrix(doorPos.position.x), convertToMatrix(doorPos.position.z));
        if(corr != null && corr.transform.tag == "Corridor"){
            corr.GetComponent<CorridorDetails>().isDoor = true; 
            newBuilding.GetComponent<BuildingDetails>().corridor = corr;
            return true;
        }else{
            return false;
        }
    }

    private void dustParticle(Vector3 position){
        GameObject dustP = Instantiate(dust, new Vector3(position.x, position.y+2f, position.z), Quaternion.identity);
        float timer = 3f;
        if(timer<=0f){
            Destroy(dustP);
        }else{
            timer -= Time.deltaTime;
        }
    }

    private void redCubeSpawner(bool spawn){
        if(spawn){
            if(place.tag == "Corridor"){
                Transform newCube = Instantiate(redCube, new Vector3(place.transform.position.x, place.transform.position.y+2f, place.transform.position.z), Quaternion.identity).transform;
                redCubeList.Add(newCube);
            }else{
                foreach(Transform r in roomSquares){
                    if(r!=null){
                        Transform newCube = Instantiate(redCube, new Vector3(r.position.x, r.position.y+2f, r.position.z), Quaternion.identity).transform;
                        redCubeList.Add(newCube);
                    }
                }
            }
        }else{
            if(redCubeList != null){
                foreach(Transform c in redCubeList){
                    Destroy(c.gameObject);
                }
                redCubeList = new List<Transform>();
            }
        }
    }

    private void killGuests(){
        foreach(Transform c in redCubeList){
            c.GetComponent<destroyerCube>().destroy();
        }
    }
}
