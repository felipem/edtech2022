using System.Globalization;
//using Tweets;
using UnityEngine;
using UnityEngine.AI;
using World.Resource;

public class NonPlayingCharacter : MonoBehaviour
{
    public Color highlightColour;
    public Sprite avatar;
    public ResourceSingleton resourceSingleton;

    private Color[] colours;
    private Renderer[] renderers;
    
    private NavMeshAgent agent;

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Occupation { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // get singleton instances

        // get renderers of npc model components
        renderers = GetComponentsInChildren<Renderer>();
        colours = new Color[renderers.Length];

        // cache original color
        for (int i = 0; i < renderers.Length; i++)
        {
            colours[i] = renderers[i].material.color;
        }

        // update pathing destination every 10 seconds
        InvokeRepeating("ChangeDestination", 0, 10f);
    }

    private void ChangeDestination()
    {
        // pick a random point on the board
        float x = Random.Range(0, 190f);
        float y = transform.position.y;
        float z = Random.Range(0, 190f);

        // set the destination
        Vector3 destination = new Vector3(x, y, z);
        agent.destination = destination;
    }

    private void OnMouseEnter()
    {
        // highlight npc
        foreach (Renderer rend in renderers)
        {
            rend.material.color = highlightColour;
        }
    }

    private void OnMouseExit()
    {
        // restore original colours
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = colours[i];
        }
    }

    private void OnMouseDown()
    {
    
    }
}