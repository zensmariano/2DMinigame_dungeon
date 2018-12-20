using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIImage : MonoBehaviour {

	Image scoreImage;
	public Sprite coinSmall;

	void Start()
	{
		scoreImage = GetComponent<Image> ();
	}

	void Update () {
			scoreImage.sprite = coinSmall;
	}
		
}
