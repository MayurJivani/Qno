using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour 
{
    public string playerName {  get; set; }
    public GameObject handObject { get; set; }
    public Animator handObjectAnimatorController { get; set; }
    public Camera frontCamera { get; set; }
    public Camera backCamera { get; set; }
    public HandManager handManager { get; set; }

    public GameObject cardPrefab { get; set; }

    public Player(string playerName, GameObject handObject, Animator handObjectAnimatorController, Camera frontCamera, Camera backCamera, GameObject cardPrefab)
    {
        this.playerName = playerName;
        this.handObject = handObject;
        this.handObjectAnimatorController = handObjectAnimatorController;
        this.frontCamera = frontCamera;
        this.backCamera = backCamera;
        this.cardPrefab = cardPrefab;
        handManager = new HandManager(handObject, cardPrefab);
    }






}
