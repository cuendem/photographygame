using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotoTrigger : MonoBehaviour
{
    private HashSet<Collider2D> overlappingObjects = new HashSet<Collider2D>();
    void OnTriggerEnter2D(Collider2D other)
    {
        overlappingObjects.Add(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        overlappingObjects.Remove(other);
    }
}
