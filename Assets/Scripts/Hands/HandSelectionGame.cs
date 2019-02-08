using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandSelectionGame : Hand
{
	StartRaycast canvasRaycast;

	public delegate void OnSelectableObjectClicked(SelectableObject selectableObject);
	public static event OnSelectableObjectClicked SelectableObjectClicked;

	public delegate void OnSelectableObjectEnter(SelectableObject selectableObject);
	public static event OnSelectableObjectEnter SelectableObjectEnter;

	public delegate void OnSelectableObjectExit(SelectableObject selectableObject);
	public static event OnSelectableObjectExit SelectableObjectExit;

	[SerializeField]
	private SelectableObject objectSelected;

	protected override void Start()
	{
		base.Start();
		canvasRaycast = FindObjectOfType<StartRaycast>();

	}
	//gli inputs vanno overridati perhe' non e' piu' un drag 
	protected override void CheckInputs()
	{
		//non bella soluzione ma serve a noi per differenziare se siamo nella magic room o no
		if (inMagicRoom)
		{
			Debug.Log(currentSkeleton);
			//se sto cliccando >>> Inizia il drag
			if (currentSkeleton.isRightHandClosed(0.075f))
			{
				if (handState != "closed")
					canvasRaycast.DoRaycast(Camera.main.WorldToScreenPoint(transform.position));
				
				ChangeHandSprite("closed");
				Debug.Log("Hand is close");
				SelectObject();
			}
			else
			{
				ChangeHandSprite("open");
			}
		}
		//se non sono nella magic room e quindi il controllo deve essere effettuato col mouse e basta
		else
		{
			if (Input.GetMouseButtonDown(0))
			{
				ChangeHandSprite("closed");
				SelectObject();
			}
			else
				ChangeHandSprite("open");
		}
	}

	//da implementare ancora
	protected void SelectObject()
	{
		//se sono posizionato con la mano sopra un'oggetto selzionabile e ho cliccato allora invia il messaggio che ho selezionato 
		if (objectSelected != null)
		{
			SelectableObjectClicked(objectSelected);
		}
	}

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if (!dragging)
		{
			if (collision.gameObject.GetComponent<SelectableObject>() != null)
			{
				objectSelected = collision.gameObject.GetComponent<SelectableObject>();
				SelectableObjectEnter(objectSelected);
			}
		}
	}

	protected override void OnTriggerExit2D(Collider2D collision)
	{
		if (!dragging)
		{
			if (collision.gameObject.GetComponent<SelectableObject>() != null)
			{
				SelectableObjectExit(objectSelected);
				objectSelected = null;
			}
		}
	}

}
