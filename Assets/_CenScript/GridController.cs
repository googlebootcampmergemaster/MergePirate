using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] CharacterType characterType;

    Ray ray;
    RaycastHit characterHit;
    RaycastHit gridHit;

    GameObject selectedCharacter;
    GameObject previousCharacter;
    GameObject selectedGrid;


    void Update()
    {
        if (Input.touchCount > 0)
        {
            CharacterMove();
        }
        else
        {
            GridSelector();
        }
    }

    void CharacterMove()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out characterHit, Mathf.Infinity, characterType.characterLayerMask)) //karakterlerin hareketleri için atýlan ýþýn
        {
            selectedCharacter = characterHit.transform.gameObject; //selected character equal hit of ray
            if (selectedCharacter == previousCharacter || previousCharacter == null)
            {
                previousCharacter = characterHit.transform.gameObject; //last character
                previousCharacter.GetComponent<CharacterController>().canHit = true;
                previousCharacter.GetComponent<Collider>().isTrigger = true;
                previousCharacter.GetComponent<Rigidbody>().isKinematic = true;
                //move game object
                characterHit.transform.position = Vector3.
                    MoveTowards(characterHit.transform.position,
                    new Vector3(characterHit.point.x, characterHit.transform.position.y, characterHit.point.z), 1f);
                characterHit.transform.position = new Vector3(Mathf.Clamp(characterHit.transform.position.x, -5.82f, 6.7f), 1, Mathf.Clamp(characterHit.transform.position.z, -51f, -41f));
            }
        }
    }
    public void GridSelector()
    {
        if (selectedCharacter != null)
        {
            //the second ray does not match on the character's
            if (Physics.Raycast(selectedCharacter.transform.position, Vector3.down, out gridHit, Mathf.Infinity, characterType.gridLayerMask) && !Physics.Raycast(selectedCharacter.transform.position, Vector3.down, out characterHit, Mathf.Infinity, characterType.characterLayerMask)) //seçtiðimiz karakterlerin karelere yerleþmesi için atýlan ýþýn
            {
                selectedGrid = gridHit.transform.gameObject; //the ray which match the grid object
                previousCharacter.GetComponent<Collider>().isTrigger = false;
                previousCharacter.GetComponent<Rigidbody>().isKinematic = false;
                // gameobject move to selected grid.
                selectedCharacter.transform.position = new Vector3(selectedGrid.transform.position.x,
                    selectedCharacter.transform.position.y, selectedGrid.transform.position.z);

                selectedCharacter.GetComponent<CharacterController>().lastPosition = selectedGrid.transform.position;
            }
            selectedCharacter = null;
            previousCharacter = null;
        }
    }
}
