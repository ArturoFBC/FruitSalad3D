using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieceDropper : MonoBehaviour
{
    [SerializeField] private GameObject piecePrefab;

    private GameObject currentPiece;
    private PieceBahaviour currentPieceBahaviour;

    [SerializeField] private Vector3 initialPosition;

    [SerializeField] private float pieceSpeed = 0.2f;

    [SerializeField] private int minPieceSize = 1;
    [SerializeField] private int maxPieceSize = 4;

    [SerializeField] private float dropDelayTime = 0.4f;
    private float dropDelayCounter = 0.0f;

    [SerializeField] private Transform northBound, southBound, westBound, eastBound;

    private void Awake()
    {
        GameManager.GameStart += InstantiateNewPiece;
    }

    private void OnDestroy()
    {
        GameManager.GameStart -= InstantiateNewPiece;
    }

    private void Update()
    {
        ManageDropPieceTiming();

        Vector2 input = Vector2.zero;
        input.y = Input.GetAxis("Vertical");
        input.x = Input.GetAxis("Horizontal");
        if (input != Vector2.zero)
            MovePiece(input, Time.deltaTime, Camera.main.transform.position);
    }

    private void ManageDropPieceTiming()
    {
        if (dropDelayCounter > 0)
        {
            dropDelayCounter -= Time.deltaTime;
        }
        else
        {
            if (currentPiece == null)
            {
                InstantiateNewPiece();
            }
            else if (Input.GetAxis("Fire1") > 0)
            {
                DropPiece();
                dropDelayCounter = dropDelayTime;
            }
        }
    }

    private void MovePiece(Vector2 input, float deltaTime, Vector3 cameraPosition)
    {
        Vector3 relativeDirection = new Vector3(input.x, 0, input.y);

        Vector3 cameraDirection = transform.position - cameraPosition;
        cameraDirection.y = 0;
        float angle = Vector3.Angle(cameraDirection, Vector3.forward);
        print(angle + " " + relativeDirection);

        
        relativeDirection = Quaternion.AngleAxis(angle, Vector3.up) * relativeDirection;
        
        Vector3 intendedPosition = currentPiece.transform.position + relativeDirection * (pieceSpeed * deltaTime);

        float pieceRadius = currentPiece.transform.lossyScale.x / 2f;
        intendedPosition.x = Mathf.Clamp(intendedPosition.x, westBound.position.x + pieceRadius, eastBound.position.x - pieceRadius);
        intendedPosition.z = Mathf.Clamp(intendedPosition.z, southBound.position.z+ pieceRadius, northBound.position.z - pieceRadius);
     //   print(intendedPosition);
        currentPiece.transform.position = intendedPosition;
    }

    private void DropPiece()
    {
        currentPiece.GetComponent<Collider>().enabled = true;
        currentPiece.GetComponent<Rigidbody>().useGravity = true;
        currentPiece = null;
    }

    private void InstantiateNewPiece()
    {
        currentPiece = Instantiate(piecePrefab, initialPosition, Quaternion.identity);
        currentPiece.GetComponent<Collider>().enabled = false;
        currentPiece.GetComponent<Rigidbody>().useGravity = false;
        currentPiece.GetComponent<PieceBahaviour>().SetSize(UnityEngine.Random.Range(minPieceSize, maxPieceSize));
    }
}
