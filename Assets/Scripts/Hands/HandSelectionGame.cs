using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSelectionGame : Hand
{

	SelectableObject objectSelected;

	//gli inputs vanno overridati perhe' non e' piu' un drag 
	protected override void CheckInputs ()
	{

		//non bella soluzione ma serve a noi per differenziare se siamo nella magic room o no
		if (inMagicRoom)
		{
			//se sto cliccando >>> Inizia il drag
			if (currentSkeleton.isRightHandClosed (0.1f))
				SelectObject ();
		}
		//se non sono nella magic room e quindi il controllo deve essere effettuato col mouse e basta
		else
		{
			if (Input.GetMouseButtonDown (0))
				SelectObject ();
		}
	}

	//da implementare ancora
	protected void SelectObject ()
	{
		if (objectSelected != null)
			Debug.Log ("Ho selezionato" + objectSelected.name);
		else
			Debug.Log ("nessun oggetto selezionato");

	}

	protected override void OnTriggerEnter2D (Collider2D collision)
	{
		if (!dragging)
		{
			if (collision.gameObject.GetComponent<SelectableObject> () != null)
				objectSelected = collision.gameObject.GetComponent<SelectableObject> ();
		}
	}

	protected override void OnTriggerExit2D (Collider2D collision)
	{
		if (!dragging)
		{
			if (collision.gameObject.GetComponent<SelectableObject> () != null)
				objectSelected = null;
		}
	}

}
