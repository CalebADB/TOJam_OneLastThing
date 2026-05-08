using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersonalityInteractionHandler : MonoBehaviour
{
    [SerializeField] public List<GameObject> localPersonalityObjects = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"OnTriggerEnter2D: collision_{collision.name}");

        GameObject collisionGameObject = collision.gameObject;

        Personality personality = collisionGameObject.GetComponent<Personality>();

        if (personality == null)
        {
            return;
        }

        localPersonalityObjects.Add(collisionGameObject);

        Debug.Log($"OnTriggerEnter2D: localPersonalityObjects_{localPersonalityObjects}");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log($"OnTriggerExit2D: collision_{collision.name}");
        GameObject collisionObject = collision.gameObject;

        Personality personality = collisionObject.GetComponent<Personality>();

        if (personality == null)
        {
            return;
        }

        localPersonalityObjects.RemoveAll(obj => collisionObject);
        Debug.Log($"OnTriggerExit2D: localPersonalityObjects_{localPersonalityObjects}");
    }

    public List<GameObject> GetLocalPersonalityObjects()
    {
        return localPersonalityObjects;
    }
}
