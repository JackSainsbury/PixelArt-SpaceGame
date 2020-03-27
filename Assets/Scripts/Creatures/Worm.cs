using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    public Transform moveTowards;
    public int bodyCount = 5;

    public GameObject headGameObject;
    public GameObject bodyGameObject;
    public GameObject tailGameObject;

    public float moveSpeed = 1f;

    private float bodyOffset = -0.6f;
    private BodyPiece[] bodyPieces;

    public Vector3[] positions;
    private float cacheDist = 0f;

    // Start is called before the first frame update
    void Start()
    {
        bodyPieces = new BodyPiece[bodyCount + 2];
        positions = new Vector3[bodyCount + 3];

        int sortingRenderID = bodyCount + 2;

        BodyPiece head = Instantiate(headGameObject, transform).GetComponent<BodyPiece>();
        head.spriteRenderer.sortingOrder = sortingRenderID;
        head.transform.localPosition = Vector3.zero;

        bodyPieces[bodyCount + 1] = head;
        positions[positions.Length - 1] = head.transform.position;

        float offsetFrac = 1.0f / bodyCount;


        for(int i = 0; i < bodyCount; ++i)
        {
            sortingRenderID--;

            BodyPiece body = Instantiate(bodyGameObject, transform).GetComponent<BodyPiece>();
            body.transform.localPosition = new Vector3(0, bodyOffset * (i + 1), 0);
            body.spriteRenderer.sortingOrder = sortingRenderID;
            body.animator.SetFloat("Offset", i * offsetFrac);
            
            bodyPieces[bodyCount + 1 - (i + 1)] = body;
            positions[(positions.Length - 1) - (i + 1)] = body.transform.position;
        }

        BodyPiece tail = Instantiate(tailGameObject, transform).GetComponent<BodyPiece>();
        tail.transform.localPosition = new Vector3(0, bodyOffset * (bodyCount + 1), 0);
        tail.spriteRenderer.sortingOrder = sortingRenderID;
        tail.animator.SetFloat("Offset", 1);

        bodyPieces[0] = tail;
        positions[0] = tail.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 deltaMove = Vector3.up * moveSpeed * Time.deltaTime;

        transform.Translate(deltaMove);

        transform.up = moveTowards.position - transform.position;

        // Cache new position
        cacheDist += deltaMove.magnitude;
        if (cacheDist > .5f)
        {
            for(int i = 0; i < bodyPieces.Length; ++i)
            {
                positions[i] = positions[i + 1];
            }
            positions[positions.Length - 1] = transform.position;
            cacheDist = 0;
        }

        int count = bodyPieces.Length;

        for (int i = 0; i < count; ++i)
        {
            int inverseCount = ((count - 1) - i);

            if (i < count)
            {
                // Sin offset
                bodyPieces[i].transform.localPosition = transform.InverseTransformPoint(Vector3.Lerp(positions[i], positions[i + 1], cacheDist / .5f));
            }

            // orient look at the piece in front, head to look to front pos
            if (inverseCount < count - 1)
                bodyPieces[inverseCount].transform.up = bodyPieces[inverseCount + 1].transform.position - bodyPieces[inverseCount].transform.position;
            else
                bodyPieces[inverseCount].transform.up = moveTowards.position - bodyPieces[inverseCount].transform.position;
        }
    }
}
