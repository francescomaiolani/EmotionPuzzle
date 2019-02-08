using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [Header("Vicinanza della mano con il mouse")]
    public float lerpingFactor;

    public SpriteRenderer spriteRenderer;
    public Sprite[] hands;
    protected KinectBodySkeleton currentSkeleton;
    [SerializeField]
    protected bool dragging;
    //[Header("mettere true se siamo in magicRoom")]
    protected bool inMagicRoom;
    //layer per la collisione della mano
    protected LayerMask layerToDetectCollision;

    protected Rigidbody2D rigid;

    public delegate void OnAdviceGiven(string advice);
    public static event OnAdviceGiven adviceGiven;

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        layerToDetectCollision = LayerMask.GetMask("Default");

        inMagicRoom = MagicRoomKinectV2Manager.instance.MagicRoomKinectV2Manager_active;
    }

    // Nella routine di ogni mano devono essere eseguiti gli step di :
    //segui mouse o mano reale
    void Update()
    {
        FollowMouseOrKinectHand();
        CheckInputs();
    }

    //la mano segue il mouse 
    void FollowMouseOrKinectHand()
    {
        Vector2 mousePositionInWorldCoordinates;
        if (inMagicRoom)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.001f, 0.001f), Random.Range(-0.001f, 0.001f), 0);
            currentSkeleton = MagicRoomKinectV2Manager.instance.GetCloserSkeleton();
            if (MagicRoomKinectV2Manager.instance.MagicRoomKinectV2Manager_active)
                mousePositionInWorldCoordinates = (MagicRoomKinectV2Manager.instance.GetCloserSkeleton().HandRight * 11) + randomOffset;
            else
                mousePositionInWorldCoordinates = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        }
        else
            mousePositionInWorldCoordinates = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        Vector2 newPosition;
        newPosition = new Vector2(Mathf.Lerp(transform.position.x, mousePositionInWorldCoordinates.x, lerpingFactor), Mathf.Lerp(transform.position.y, mousePositionInWorldCoordinates.y, lerpingFactor));
        rigid.MovePosition(newPosition);
    }

    //metodo implementato effettivamente nelle classi derivate perche' ogni mano ha un suo modo di gestire gli input
    protected virtual void CheckInputs() { }
    //Check delle collisioni va fatto nelle classi derivate perche' ogni minigioco ha bisogno di input diversi
    protected virtual void OnTriggerEnter2D(Collider2D collision) { }
    protected virtual void OnTriggerExit2D(Collider2D collision) { }

    //disambigua la scelta tra bocca e occhi e da' un consiglio giusto
    protected virtual void ChooseProperAdvice(string type) { }

    //posso chiamare un evento solo da questa classe quindi aggiro chiamando un metodo che chiama l'evento
    protected void GiveAdvice(string advice)
    {
        adviceGiven(advice);
    }

    //Cambia l'immagine della mano da aperta a ciusa e viceversa
    protected void ChangeHandSprite(string state)
    {
        if (state == "closed")
            spriteRenderer.sprite = hands[1];
        else if (state == "open")
            spriteRenderer.sprite = hands[0];
    }
    //droppa nella droppable area il droppable object trascinato
    protected void DropItem(DroppableArea d, DraggableObject draggableComponent)
    {
        draggableComponent.StopDragging(draggableComponent.CheckIfCorrectDropArea(d.GetMainType(), d.GetSubType()), d.gameObject);
        draggableComponent.SetDropAreaDestination(d.transform.position);
        draggableComponent.SetDroppableArea(d.gameObject);
        d.SetContainedPiece(draggableComponent.gameObject);
        d.SetOccupied(true);
    }
}
