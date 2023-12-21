using FruitSalad3D.scripts.audio.sound;
using System;
using System.Collections;
using System.Collections.Generic;
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

    private float pieceDestroyTime = 0.2f;

    private bool initialized = false;

    [SerializeField] private Rigidbody myRigidBody;
    private Transform myTransform;

    [SerializeField] private int mySize = 0;

    [SerializeField] private List<GameObject> graphicPrefabs;
    private GameObject currentGraphic;

    [SerializeField] private List<Color> effectColors;
    [SerializeField] private GameObject destroyFXPrefab;

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

        InstantiateNewGraphic();
    }

    private void InstantiateNewGraphic()
    {
        currentGraphic = Instantiate(graphicPrefabs[mySize], myTransform, false);
        Vector3 finalSize = currentGraphic.transform.localScale;
        currentGraphic.transform.localScale = Vector3.zero;
        currentGraphic.AddComponent<ScaleInXSeconds>().Set(finalSize, pieceDestroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision, true);
    }

    private void CheckCollision(Collision collision, bool withSound)
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

        if (withSound)
            SoundManager._Ref.PlaySound(SoundType.DROP, collision.relativeVelocity.magnitude / 20f, 1.5f / mySize);
    }

    private void OnCollisionStay(Collision collision)
    {
        CheckCollision(collision, false);
    }

    private void CombineWith(PieceBahaviour otherPiece)
    {
        Vector3 combinePosition = (otherPiece.transform.position + myTransform.position) / 2f;

        if (currentGraphic != null)
            DestroyFX(combinePosition);

        SoundManager._Ref.PlaySound(SoundType.COMBINE);

        mySize += 1;
        UpdateSize();

        myTransform.position = combinePosition;

        myRigidBody.velocity += otherPiece.BeAbsorbed(myTransform.position);

        PieceCombinationEvent?.Invoke(mySize);
    }

    private Vector3 BeAbsorbed(Vector3 absorbPosition)
    {
        Vector3 returnVelocity = myRigidBody.velocity;
        GameObject myGameObject = gameObject;
        myGameObject.SetActive(false);

        if (currentGraphic!= null)
            DestroyFX(absorbPosition);

        Destroy(myGameObject);
        return returnVelocity;
    }

    private void DestroyFX(Vector3 absorbPosition)
    {
        currentGraphic.transform.SetParent(null);
        currentGraphic.AddComponent<DisplaceTowardsInXSeconds>().Set(absorbPosition, pieceDestroyTime);
        currentGraphic.GetComponent<ScaleInXSeconds>().Set(Vector3.zero, pieceDestroyTime);
        Destroy(currentGraphic, pieceDestroyTime);
        ParticleSystem particleFX = Instantiate(destroyFXPrefab).GetComponent<ParticleSystem>();
        particleFX.startColor = effectColors[mySize];
        particleFX.transform.position = absorbPosition;
        Destroy(particleFX.gameObject, pieceDestroyTime * 3f);
    }

    public int GetSize()
    {
        return mySize;
    }

    internal void SetSize(int newSize)
    {
        if (currentGraphic != null)
            DestroyFX(transform.position);

        mySize = newSize;
        UpdateSize();
    }
}
