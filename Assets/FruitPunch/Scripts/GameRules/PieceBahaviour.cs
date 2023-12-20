using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PieceBahaviour : MonoBehaviour
{
    enum PieceType
    {
        BLUEBERRY,
        CHERRY,
        GRAPE,
        STRAWBERRY,
        PLUM,
        KIWI,
        APPLE,
        PEACH,
        ORANGE,
        POMEGRATE,
        MELON,
        WATERMELON
    }


    private const float SIZE_RATIO = 0.23f;

    private bool initialized = false;

    [SerializeField] private Rigidbody myRigidBody;
    private Transform myTransform;

    [SerializeField] private int mySize = 0;

    [SerializeField] private List<GameObject> graphicPrefabs;
    private GameObject currentGraphic;

    private bool physicallySet = false;
    private float maxVelocityToConsiderSet = 0.1f;

    public static Action<int> PieceCombinationEvent;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
        UpdateSize();
        physicallySet = false;
    }

    private void FixedUpdate()
    {
        if (physicallySet == true)
            return;

        if (myRigidBody.velocity.sqrMagnitude < maxVelocityToConsiderSet)
            physicallySet = true;
    }

    public bool IsSet()
    {
        return physicallySet;
    }
    
    private void Initialize()
    {
        if (initialized)
            return;

        myRigidBody = GetComponent<Rigidbody>();
        myTransform = GetComponent<Transform>();
        initialized = true;
    }

    private void UpdateSize()
    {
        myTransform.localScale = Vector3.one * (SIZE_RATIO * ((float)mySize + 2.5f));
        myRigidBody.mass = (float)mySize + 2.5f;

        if (currentGraphic != null)
            Destroy(currentGraphic);

        currentGraphic = Instantiate(graphicPrefabs[mySize], myTransform, false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == gameObject.tag)
        {
            PieceBahaviour otherPiece = collision.gameObject.GetComponent<PieceBahaviour>();
            if (otherPiece != null)
            {
                if (otherPiece.GetSize() == mySize)
                    CombineWith(otherPiece);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        OnCollisionEnter(collision);
    }

    private void CombineWith(PieceBahaviour otherPiece)
    {
        mySize += 1;
        UpdateSize();
        myTransform.position = (otherPiece.transform.position + myTransform.position) / 2f;

        myRigidBody.velocity += otherPiece.BeAbsorbed();

        PieceCombinationEvent?.Invoke(mySize);
    }

    private Vector3 BeAbsorbed()
    {
        Vector3 returnVelocity = myRigidBody.velocity;
        GameObject myGameObject = gameObject;
        myGameObject.SetActive(false);
        Destroy(myGameObject);
        return returnVelocity;
    }

    public int GetSize()
    {
        return mySize;
    }

    internal void SetSize(int newSize)
    {
        mySize = newSize;
        UpdateSize();
    }
}
