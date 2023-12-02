using System;
using System.Collections;
using System.Collections.Generic;
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
        for (var index = 0; index < piecesOutOfBounds.Count; index++)
        {
            var piece = piecesOutOfBounds[index];

            print(piece.piece.name + " " + piece.outOfBoundsTimeCounter);
            
            piece.outOfBoundsTimeCounter += Time.fixedDeltaTime;

            if (piece.outOfBoundsTimeCounter > outOfBoundsTimeLimit)
            {
                print("GAME OVER!");
            }
        }
    }
}
