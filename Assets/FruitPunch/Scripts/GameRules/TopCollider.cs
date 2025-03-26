using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TopCollider : MonoBehaviour
{
    private class PieceOutOfBounds
    {
        public Rigidbody piece;
        public float outOfBoundsTimeCounter = 0;
    }
    
    [SerializeField] private float outOfBoundsTimeLimit = 2f;
    [SerializeField] private List<PieceOutOfBounds> piecesOutOfBounds = new List<PieceOutOfBounds>();
    

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigidBody = other.GetComponent<Rigidbody>();
        if (rigidBody != null)
        {
            for (var index = 0; index < piecesOutOfBounds.Count; index++)
            {
                var piece = piecesOutOfBounds[index];
                if (piece.piece == rigidBody)
                {
                    piecesOutOfBounds.Remove(piece);
                    break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rigidBody = other.GetComponent<Rigidbody>();
        if (rigidBody != null)
        {
            bool alreadyInList = false;
            
            for (var index = 0; index < piecesOutOfBounds.Count; index++)
            {
                var piece = piecesOutOfBounds[index];
                if (piece.piece == rigidBody)
                {
                    alreadyInList = true;
                    break;
                }
            }

            if (alreadyInList == false)
            {
                piecesOutOfBounds.Add(new PieceOutOfBounds()
                {
                    piece = rigidBody,
                    outOfBoundsTimeCounter = 0f
                });
            }
        }
    }

    private void FixedUpdate()
    {
        List<Rigidbody> piecesToRemove = new List<Rigidbody>();

        for (var index = piecesOutOfBounds.Count - 1; index >= 0; index--)
        {
            if (piecesOutOfBounds[index] == null)
                piecesOutOfBounds.RemoveAt(index);
        }

        for (var index = 0; index < piecesOutOfBounds.Count; index++)
        {
            var piece = piecesOutOfBounds[index];

            if (piece.outOfBoundsTimeCounter > outOfBoundsTimeLimit)
            {
                GameManager.instance.EndGame();
                gameObject.SetActive(false);
                break;
            }

            print(piece.piece.name + " " + piece.outOfBoundsTimeCounter);
            
            piece.outOfBoundsTimeCounter += Time.fixedDeltaTime;


        }
    }
}
