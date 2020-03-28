using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    // Process target from path
    Vector3 targetPos = Vector3.zero;

    public int minBodyCount = 5;
    public int maxBodyCount = 5;
    private int bodyCount = 5;

    public GameObject headGameObject;
    public GameObject bodyGameObject;
    public GameObject tailGameObject;

    public float moveSpeed = 1f;

    private float bodyOffset = -0.6f;
    private BodyPiece[] bodyPieces;

    private Vector3[] positions;
    private float cacheDist = 0f;

    // Path finding logic
    private ShipRuntime targetShip;
    private NavCell[] curPath;

    private int nextIndex = 1;

    private float wiggleTimer = 0;

    private bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    void Init()
    {
        if (!initialized)
        {
            initialized = true;
            bodyCount = Random.Range(minBodyCount, maxBodyCount + 1);

            bodyPieces = new BodyPiece[bodyCount + 2];
            positions = new Vector3[bodyCount + 3];

            int sortingRenderID = bodyCount + 2;

            BodyPiece head = Instantiate(headGameObject, transform).GetComponent<BodyPiece>();
            head.spriteRenderer.sortingOrder = sortingRenderID;
            head.transform.localPosition = Vector3.zero;

            bodyPieces[bodyCount + 1] = head;
            positions[positions.Length - 1] = head.transform.position;

            float offsetFrac = 1.0f / bodyCount;


            for (int i = 0; i < bodyCount; ++i)
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
    }


    public void NavToRandom(ShipRuntime targetShip)
    {
        bool success = false;
        Vector2Int pos = targetShip.GetRandomNavigateableCellPos(out success);

        if (success)
            Navigate(
                targetShip,
                new Vector2Int(Mathf.RoundToInt(transform.localPosition.x / 3.2f), Mathf.RoundToInt(transform.localPosition.y / 3.2f)),
                pos);
        else
            StartCoroutine("DelaySearch");
    }

    public void Navigate(ShipRuntime targetShip, Vector2Int globalStartPos, Vector2Int globalEndPos)
    {
        Init();
        this.targetShip = targetShip;

        if (globalStartPos == globalEndPos)
        {
            StartCoroutine("DelaySearch");
            return;
        }

        nextIndex = 1;

        CellTemplate startCell = targetShip.GetCellByGlobalPos(globalStartPos);
        CellTemplate endCell = targetShip.GetCellByGlobalPos(globalEndPos);

        // Invalid path supplied
        if (startCell == null || endCell == null || startCell.CellState == 0 || endCell.CellState == 0)
            return;

        ShipPiece startPiece = targetShip.GetPieceByGlobalCellPos(globalStartPos);
        ShipPiece endPiece = targetShip.GetPieceByGlobalCellPos(globalEndPos);


        NavGrid startGrid = new NavGrid(startPiece);
        startGrid.Generate();

        NavGrid goalGrid = startGrid;

        if (startPiece != endPiece)
        {
            goalGrid = new NavGrid(endPiece);
            goalGrid.Generate();
        }

        AStarAlgorithm aStarAlgorithm = new AStarAlgorithm(startGrid, goalGrid, globalStartPos, globalEndPos);

        curPath = aStarAlgorithm.AStarSearch();

        wiggleTimer = Random.Range(0, 6.28318f);
    }

    // Update is called once per frame
    void Update()
    {
        // Procedural animation towards target
        Vector3 deltaMove = Vector3.up * moveSpeed * Time.deltaTime;

        transform.Translate(deltaMove);

        transform.up = targetShip.transform.TransformDirection((targetPos - transform.localPosition).normalized);

        // Cache new position
        float deltaMoveMag = deltaMove.magnitude;
        cacheDist += deltaMoveMag;

        if (cacheDist > .5f)
        {
            for(int i = 0; i < bodyPieces.Length; ++i)
            {
                positions[i] = positions[i + 1];
            }

            positions[positions.Length - 1] = transform.localPosition;
            cacheDist = 0;
        }

        int count = bodyPieces.Length;

        for (int i = 0; i < count; ++i)
        {
            int inverseCount = ((count - 1) - i);

            if (i < count)
            {
                // Sin offset
                bodyPieces[i].transform.position = targetShip.transform.TransformPoint(Vector3.Lerp(positions[i], positions[i + 1], cacheDist / .5f));
            }

            // orient look at the piece in front, head to look to front pos
            if (inverseCount < count - 1)
            {
                Vector3 dir = (transform.TransformPoint(bodyPieces[inverseCount + 1].transform.localPosition) - transform.TransformPoint(bodyPieces[inverseCount].transform.localPosition));

                bodyPieces[inverseCount].transform.up = dir;
            }
            else
            {
                Vector3 dir = targetShip.transform.TransformPoint(targetPos) - transform.TransformPoint(bodyPieces[inverseCount].transform.localPosition);

                bodyPieces[inverseCount].transform.up = dir;
            }
        }


        // Target select
        if (curPath != null)
        {
            if (curPath.Length > 1)
            {
                wiggleTimer += Time.deltaTime;

                if (wiggleTimer >= 6.28318f)
                    wiggleTimer -= 6.28318f;

                // within range
                if (new Vector2Int(Mathf.RoundToInt(transform.localPosition.x / 3.2f), Mathf.RoundToInt(transform.localPosition.y / 3.2f)) == curPath[nextIndex].position)
                {
                    nextIndex++;
                }

                if (nextIndex < curPath.Length)
                {
                    targetPos = curPath[nextIndex].PositionShipSpace() + (targetShip.transform.InverseTransformDirection(transform.right) * Mathf.Sin(wiggleTimer * 5f) * 1.6f);
                }
                else
                {
                    targetPos = curPath[curPath.Length - 1].PositionShipSpace();
                    StartCoroutine("DelaySearch");
                }
            }
            else
            {
                targetPos = curPath[0].PositionShipSpace();
                StartCoroutine("DelaySearch");
            }
        }
    }

    private IEnumerator DelaySearch()
    {
        curPath = null;

        yield return new WaitForSeconds(1f);

        if (targetShip)
        {
            NavToRandom(targetShip);
        }
    }
}
