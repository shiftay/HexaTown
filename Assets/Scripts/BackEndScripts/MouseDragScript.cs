using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragScript : MonoBehaviour {

	// SpriteRenderer currRenderer;
	// Sprite currSprite;
	public CardManager cm;
    public GridController gc;
    public bool draggingItem = false;
    bool description = false;
    bool indArea = false;
    private GameObject draggedObject;
    private Vector2 touchOffset;
	public bool reorderPuz = false;
	public Vector2 droppedPos;
    int valueOfCard = -1;
    float counter = 0f;
    bool deckStats = false;


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

            if(description) {
                cm.CardDescription(false);
                description = false;
            }

            if(indArea) {
                gc.ShowGrid(false);
                indArea = false;
            }

            if(deckStats) {
                GameManager.instance.drawObj.SetActive(false);
                deckStats = false;
            }

            counter = 0;
        }
    }
     
    Vector2 CurrentTouchPosition
    {
        get
        {
            Vector3 inputPos;
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            inputPos.z = -4;
            return inputPos;
        }
    }
 
    private void DragOrPickUp()
    {
        Vector2 inputPosition = CurrentTouchPosition;
     
        if (draggingItem)
        {
            draggedObject.transform.position = inputPosition;
            // Debug.Log("Object: " + draggedObject.transform.position);
            // Debug.Log("Input: " + inputPosition);
            // Debug.Log("Offset: " + touchOffset);

            draggedObject.transform.position = changeZ(draggedObject.transform.position);
            // Vector3 fix = draggedObject.transform.position;
            // fix.z = -5;
            // draggedObject.transform.position = fix;

            counter += Time.deltaTime;
            if(counter > 1f && !description) {
                cm.CardDescription(true, valueOfCard);
                description = true;
            }
        }
        else
        {
            RaycastHit2D[] touches = Physics2D.RaycastAll(inputPosition, inputPosition, 0.5f);
            if (touches.Length > 0 && !GameManager.instance.turnOver && BackEndManager.instance.currentState != (int)STATES.OPTIONS)
            {
                var hit = touches[0];
                if (hit.transform != null) {
                    if(hit.transform.tag == "Card") {
                        draggingItem = true;
                        draggedObject = hit.transform.gameObject;
                        touchOffset = (Vector2)hit.transform.position - inputPosition;
                        // Debug.Log(touchOffset);
                        draggedObject.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
                        Sprite card = draggedObject.GetComponent<SpriteRenderer>().sprite;
                        valueOfCard = cm.ValueOfAll(card);
                        if(cm.isSpell(card)) {
                            // FIND OUT WHAT TYPE OF SPELL.
                            switch(cm.spellType(cm.ValueOfAll(card))) {
                                case SPELLTYPE.ZONE:
                                    // cm.spellArea.color = new Color(1,1,1,1);
                                    cm.spellSpot.SetActive(true);
                                    break;

                                case SPELLTYPE.TARGETED:
                                    draggedObject.GetComponent<SpriteRenderer>().sprite = cm.changeToSpell(cm.ValueOfAll(card));
                                    break;
                            }
                        
                            // cm.spellArea.color = new Color(1,1,1,1);
                        } else {
                            draggedObject.GetComponent<SpriteRenderer>().sprite = cm.changetoTile(card);
                        }
                    }


                    if(hit.transform.tag == "Hex") {
                        TileInfo to = hit.transform.gameObject.GetComponent<TileInfo>();
                        if(to != null) {
                            if(to.type != TILETYPE.EVENT) {
                                cm.CardDescription(true, to.cardNumber);
                                description = true;
                            }
                        }
                    }

                    if(hit.transform.tag == "Book") {
                        indArea = true;
                        gc.ShowGrid(true);
                    }

                    if(hit.transform.tag == "DrawPile") {
                        deckStats = true;
                        GameManager.instance.drawObj.SetActive(true);
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

		if(!cm.hc.reorganizeHand(draggedObject, droppedPos, valueOfCard)) {
            if(!cm.isSpell(draggedObject.GetComponent<SpriteRenderer>().sprite)) {
			    draggedObject.GetComponent<SpriteRenderer>().sprite = cm.changetoCard(draggedObject.GetComponent<SpriteRenderer>().sprite);
            } else {
                draggedObject.GetComponent<SpriteRenderer>().sprite = cm.changetoCard(null, valueOfCard);
            }
		}

        valueOfCard = -1;
    }

    Vector3 changeZ(Vector3 pos) {
        pos.z = -5;
        return pos;
    }

}
