using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragScript : MonoBehaviour {

	// SpriteRenderer currRenderer;
	// Sprite currSprite;
	public CardManager cm;
    public bool draggingItem = false;
    private GameObject draggedObject;
    private Vector2 touchOffset;
	public bool reorderPuz = false;
	public Vector2 droppedPos;

	void Start()
	{
		// currRenderer = this.GetComponent<SpriteRenderer>();
	}


    void Update ()
    {
        if (HasInput)
        {
            DragOrPickUp();
        }
        else
        {
            if (draggingItem)
                DropItem();
        }
    }
     
    Vector2 CurrentTouchPosition
    {
        get
        {
            Vector2 inputPos;
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return inputPos;
        }
    }
 
    private void DragOrPickUp()
    {
        Vector2 inputPosition = CurrentTouchPosition;
     
        if (draggingItem)
        {
            draggedObject.transform.position = inputPosition + touchOffset;
        }
        else
        {
            RaycastHit2D[] touches = Physics2D.RaycastAll(inputPosition, inputPosition, 0.5f);
            if (touches.Length > 0 && !GameManager.instance.turnOver)
            {
                var hit = touches[0];
                if (hit.transform != null) {
                    if(hit.transform.tag == "Card") {
                        draggingItem = true;
                        draggedObject = hit.transform.gameObject;
                        touchOffset = (Vector2)hit.transform.position - inputPosition;
                        draggedObject.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
                        Sprite card = draggedObject.GetComponent<SpriteRenderer>().sprite;

                        if(cm.isSpell(card)) {
                            cm.spellArea.color = new Color(1,1,1,1);
                        } else {
                            draggedObject.GetComponent<SpriteRenderer>().sprite = cm.changetoTile(card);
                        }
                    }
                }
            }
        }
    }
 
    private bool HasInput
    {
        get
        {
            // returns true if either the mouse button is down or at least one touch is felt on the screen
            return Input.GetMouseButton(0);
        }
    }
 
    void DropItem()
    {
        draggingItem = false;
        draggedObject.transform.localScale = new Vector3(1f,1f,1f);
		// reorderPuz = true;
		droppedPos = CurrentTouchPosition;

		if(!cm.hc.reorganizeHand(draggedObject, droppedPos)) {
            if(!cm.isSpell(draggedObject.GetComponent<SpriteRenderer>().sprite)) {
			    draggedObject.GetComponent<SpriteRenderer>().sprite = cm.changetoCard(draggedObject.GetComponent<SpriteRenderer>().sprite);
            }
		}

        
    }
}
