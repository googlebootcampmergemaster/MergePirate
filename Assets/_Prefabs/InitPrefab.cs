using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*

*brief: This script is using to initialize the prefab.
TODO: Buton türlerine göre init değişimi yapılacak.

! All prefabs must have box collider and Rigidbody.
!! eğer objeler istenmeyen şekilde rotate ediliyor ise obje move halinde iken "diğer" tüm objelerin boxCollider'larını devre dışı bırak!


*/

public class InitPrefab : MonoBehaviour
{
    //! Check spehere distance
    public float sphereRadius = 0.5f;

    //button
    public Button MeleeButton;

    public Button RangeButton;

    public GameObject MeleeLvl1;

    public GameObject MeleeLvl2;

    public GameObject MeleeLvl3;

    public GameObject RangeLvl1;

    public GameObject RangeLvl2;

    public GameObject RangeLvl3;

    private Transform objectToPlace;

    public Camera gameCamera;

    private int state = 0;
    private GameObject tempGameObject;

    private Vector3 ObjLastPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MouseBehv());
    }

    // Update is called once per frame
    void Update() { }

    IEnumerator MouseBehv()
    {
        while (true)
        {
            if (state == 0)
            {
                //Debug.Log("state 0");
                if (Input.GetMouseButtonDown(0))
                {
                    //Debug.Log("Mouse down");
                    //create a ray from the camera through the mouse position
                    Ray rayMouse = gameCamera.ScreenPointToRay(Input.mousePosition);
                    //check if the ray hits something tagged "Soldier"
                    if (Physics.Raycast(rayMouse, out RaycastHit hit, 100))
                    {
                        //Debug.Log("state 99");
                        if (
                            hit.transform.tag.Contains("Melee")
                            || hit.transform.tag.Contains("Range")
                        )
                        {
                            //disable the MeshCollider on the soldier
                            hit.transform.GetComponent<BoxCollider>().enabled = false;
                            //Debug.Log("Click on soldier");
                            //save obeject to move
                            objectToPlace = hit.collider.gameObject.transform;
                            state = 1;

                            //save last position of object to move
                            ObjLastPos = objectToPlace.position;

                            //move object 0.5 unit up through the y axis
                        }
                    }
                }
            }
            else if (state == 1)
            {
                MoveObject();
                if (Input.GetMouseButtonDown(0))
                {
                    //enable the BoxCollider on the soldier
                    objectToPlace.GetComponent<BoxCollider>().enabled = true;
                    //checkSphere for soldier
                    if (Physics.CheckSphere(objectToPlace.position, sphereRadius))
                    {
                        //print near object
                        Collider[] hitColliders = Physics.OverlapSphere(
                            objectToPlace.position,
                            0.2f
                        );

                        Debug.Log("Yakınlarda" + hitColliders.Length + " cisim mevcut");

                        if (hitColliders.Length == 2)
                        {
                            // if (hitColliders[0].tag == objectToPlace.tag)
                            // {
                            //     Debug.Log("İkili objeler aynı tagli");
                            // }
                            // else
                            // {
                            //     Debug.Log("İkili objeler farklı tagli");
                            // }

                            //check if its possible to level up
                            if (LevelUp(hitColliders)) { }
                            else
                            {
                                //move back to last position
                                objectToPlace.position = ObjLastPos;
                            }

                            // foreach (Collider hitCollider in hitColliders)
                            // {
                            //     if (hitCollider.tag == "Melee")
                            //     //if (hitCollider.tag == objectToPlace.tag)
                            //     {
                            //         Debug.Log("Yakınlarda" + hitCollider.name + " cisimi mevcut");
                            //         Destroy(hitCollider.gameObject);
                            //     }
                            // }

                            // //create a new soldier from prefab2
                            // tempGameObject = Instantiate(
                            //     MeleeLvl2,
                            //     objectToPlace.position + new Vector3(0, 0.5f, 0),
                            //     Quaternion.identity
                            // );
                        }

                        //destroy everything in the sphere
                    }
                    else
                    {
                        Debug.Log("Yakınlarda cisim yok");
                    }

                    state = 0;
                }
            }

            yield return null;
        }
    }

    void MoveObject()
    {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        //Debug.Log(objectToPlace.position);

        if (Physics.Raycast(ray, out hitInfo))
        {
            objectToPlace.position = hitInfo.point + new Vector3(0, 0.5f, 0);
            objectToPlace.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private bool LevelUp(Collider[] collides)
    {
        //check if every collides array has same tag
        if (collides[0].tag == collides[1].tag)
        {
            if (collides[0].tag == "Range3" || collides[0].tag == "Melee3")
            {
                return false;
            }

            UpgradeSoldier(collides);
        }
        else
        {
            return false;
        }

        return false;
    }

    private void UpgradeSoldier(Collider[] collides)
    {
        switch (collides[0].tag)
        {
            case "Melee":
                //destroy both collider
                Destroy(collides[0].gameObject);
                Destroy(collides[1].gameObject);
                //create a new soldier from prefab2
                tempGameObject = Instantiate(
                    MeleeLvl2,
                    objectToPlace.position + new Vector3(0, 0.5f, 0),
                    Quaternion.identity
                );
                break;
            case "Melee2":
                //destroy both collider
                Destroy(collides[0].gameObject);
                Destroy(collides[1].gameObject);
                //create a new soldier from prefab3
                tempGameObject = Instantiate(
                    MeleeLvl3,
                    objectToPlace.position + new Vector3(0, 0.5f, 0),
                    Quaternion.identity
                );
                break;
            case "Range":
                //destroy both collider
                Destroy(collides[0].gameObject);
                Destroy(collides[1].gameObject);
                //create a new soldier from prefab2
                tempGameObject = Instantiate(
                    RangeLvl2,
                    objectToPlace.position + new Vector3(0, 0.5f, 0),
                    Quaternion.identity
                );
                break;
            case "Range2":
                //destroy both collider
                Destroy(collides[0].gameObject);
                Destroy(collides[1].gameObject);
                //create a new soldier from prefab3
                tempGameObject = Instantiate(
                    RangeLvl3,
                    objectToPlace.position + new Vector3(0, 0.5f, 0),
                    Quaternion.identity
                );
                break;
            default:
                Debug.Log("switch error");
                break;
        }
    }

    private void Awake()
    {
        // adding a delegate with no parameters
        MeleeButton.onClick.AddListener(NoParamaterOnclickMelee);
        RangeButton.onClick.AddListener(NoParamaterOnclickRange);

        // adding a delegate with parameters
        // MeleeButton.onClick.AddListener(
        //     delegate
        //     {
        //         ParameterOnClick("Button was pressed!");
        //     }
        // );
    }

    private void NoParamaterOnclickMelee()
    {
        //Debug.Log("Button clicked with no parameters");
        Instantiate(MeleeLvl1, transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
    }

    private void NoParamaterOnclickRange()
    {
        //Debug.Log("Button clicked with no parameters");
        Instantiate(RangeLvl1, transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
    }

    // private void ParameterOnClick(string test)
    // {
    //     //Debug.Log(test);
    // }
}
