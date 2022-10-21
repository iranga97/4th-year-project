using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TouchController : MonoBehaviour
{
    public enum SelMode : int
    {
        OnlyParent = 0,
        AndChildren = 1
    }
    public SelMode SelectionMode = SelMode.AndChildren;

    public Material selectionMat;

    private Vector2 startPos;
    private Vector2 endPos;

    //to identify long press
    private float timePressed = 0.0f;
    private float timeLastPress = 0.0f;
    public float timeDelayThreshold = 1.0f;

    private bool multisilectEnable = false;

    public GameObject root;
    public GameObject rotationPoint;

    private MultiSelectStore multiSelectStore;

    private MaterialController materialController;

    
    private List<GameObject> allGameObjects = new List<GameObject>();

    private float initialDist; //For zooming
    private Vector3 initialScale;
    private Vector3 initialPosition; //initial camera position
    private Camera cam;

    private float dist; //For move in x y plane
    private bool dragging;
    private Vector3 offset;

    void OnEnale()
    {

        Inital();
    }

    void Inital()
    {
        cam = GetComponent<Camera>();
        multiSelectStore = MultiSelectStore.Instance;
        allGameObjects.AddRange(GameObject.FindGameObjectsWithTag("Object"));
        materialController = new MaterialController(allGameObjects);
    }

    private void Awake()
    {
        Inital();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startPos = touch.position;
                        timePressed = Time.time;
                        break;
                    case TouchPhase.Moved:
                        root.transform.RotateAround(rotationPoint.transform.position,new Vector3(touch.deltaPosition.y,-touch.deltaPosition.x,0f),2);
                        break;
                    case TouchPhase.Ended:
                        endPos = touch.position;
                        timeLastPress = Time.time;

                        if (endPos == startPos)
                        {

                            if ((timeLastPress - timePressed) > timeDelayThreshold)
                            {
                                multisilectEnable = true;
                            }
                            SelectObject(touch);
                        }
                        break;
                }
            }
            /*if(Input.touchCount == 2){
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);
                //Touch touch3 = Input.GetTouch(2);
                Vector3 v3;

                Ray ray1 = cam.ScreenPointToRay(touch1.position);
                RaycastHit hit1;
                Ray ray2 = cam.ScreenPointToRay(touch2.position);
                RaycastHit hit2;
                //Ray ray3 = cam.ScreenPointToRay(touch3.position);
                //RaycastHit hit3;
                
                if(Physics.Raycast(ray1, out hit1)&& Physics.Raycast(ray2, out hit2)){
                    Transform hitObject = root.transform;
                    //Transform hitObject = hit1.transform;
                                      
                    if(touch1.phase ==TouchPhase.Began && touch2.phase == TouchPhase.Began){
                        dist = hitObject.position.z - cam.transform.position.z;
                        v3 = new Vector3(touch1.position.x, touch1.position.y, dist);
                        v3 = cam.ScreenToWorldPoint(v3);
                        offset = hitObject.position - v3;
                        dragging = true;
                    }
                     if(dragging && touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved ){
                        v3 = new Vector3(touch1.position.x,touch1.position.y,dist);
                        v3 = cam.ScreenToWorldPoint(v3);
                        hitObject.position = v3+offset;
                    }
                    if(touch1.phase == TouchPhase.Ended && touch2.phase == TouchPhase.Ended ){
                        dragging = false;
                    }
                }
               

            }*/

            if(Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if(touch1.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled || touch2.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Canceled)
                {
                    return;
                }

                if(touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    initialDist = Vector2.Distance(touch1.position,touch2.position);
                    initialScale = root.transform.localScale;
                    initialPosition = cam.transform.position;
                }
                if(touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved){
                    var currentDist = Vector2.Distance(touch1.position,touch2.position);

                    if(Mathf.Approximately(initialDist,0)){
                        return;
                    }

                    var factor = (currentDist-initialDist)*0.05f;

                    cam.transform.position = new Vector3(initialPosition.x,initialPosition.y,initialPosition.z + factor);
                }
            }

        }
    }


    
    public void SelectObject(Touch touch)
    {
        Ray ray = cam.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            Debug.Log(hitObject.name);
            if (multiSelectStore.findObject(hitObject))
            {
                multiSelectStore.removeObject(hitObject);
                materialController.removeMaterial(hitObject);
                if (SelectionMode == SelMode.AndChildren)
                {
                    List<GameObject> childrenRenderers = new List<GameObject>();
                    foreach (Transform child in hit.transform){
                        childrenRenderers.Add(child.gameObject);
                    }
                    childrenRenderers.ForEach(r =>
                    {
                        multiSelectStore.removeObject(r);
                        materialController.removeMaterial(r);
                    });
                }
            }
            else
            {
                if (multisilectEnable)
                {
                    multiSelectStore.addObject(hitObject);
                    materialController.addMaterial(hitObject,selectionMat);
                    if (SelectionMode == SelMode.AndChildren)
                    {
                        List<GameObject> childrenRenderers = new List<GameObject>();
                        foreach (Transform child in hit.transform){
                            childrenRenderers.Add(child.gameObject);
                        }
                        childrenRenderers.ForEach(r =>
                        {
                            multiSelectStore.addObject(r);
                            materialController.addMaterial(r,selectionMat);
                        });
                    }
                }
                else
                {
                    multiSelectStore.removeAllObject();
                    materialController.removeMaterialOfAllObjects();
                    multiSelectStore.addObject(hitObject);
                    materialController.addMaterial(hitObject,selectionMat);
                    if (SelectionMode == SelMode.AndChildren)
                    {
                        List<GameObject> childrenRenderers = new List<GameObject>();
                        foreach (Transform child in hit.transform){
                            childrenRenderers.Add(child.gameObject);
                        }
                        childrenRenderers.ForEach(r =>
                        {
                            multiSelectStore.addObject(r);
                            materialController.addMaterial(r,selectionMat);
                        });
                    }
                }
            }
            //Debug.Log(multiSelectStore.getSelectedObjects().Count);

        }
        else
        {
            if (multisilectEnable)
            {
                multiSelectStore.removeAllObject();
                materialController.removeMaterialOfAllObjects();
                multisilectEnable = false;
            }
            else
            {
                multiSelectStore.removeAllObject();
                materialController.removeMaterialOfAllObjects();
            }
        }
    }

}

