using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMainScript : MonoBehaviour
{
    [SerializeField] private Transform emptySpace = null;
    private Camera mainCamera;
    [SerializeField] private TilesScript[] tiles;
    private static int emptySpaceIndex = 8;
    private bool _isFinished;
    [SerializeField] private GameObject endPanel;
    [SerializeField] TextMeshProUGUI endPanelTimeText;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        _isFinished = false;
        endPanel.SetActive(false);
        Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                //Debug.Log(hit.transform.name);
                if (Vector2.Distance(emptySpace.position,hit.transform.position) < 2)
                {
                    Vector2 lastEmptySpacePosition = emptySpace.position;
                    TilesScript thisTile = hit.transform.GetComponent<TilesScript>();
                    emptySpace.position = thisTile.targetPosition;
                    thisTile.targetPosition = lastEmptySpacePosition;
                    int tileIndex = findIndex(thisTile);
                    tiles[emptySpaceIndex] = tiles[tileIndex];
                    tiles[tileIndex] = null;
                    emptySpaceIndex = tileIndex;


                    //emptySpace.position = hit.transform.position;
                    //hit.transform.position = lastEmptySpacePosition;
                }
            }
        }

        if (!_isFinished)
        {
        int correctTiles = 0;
        foreach (var a in tiles)
        {
            if (a != null)
            {
                if (a.inRightPlace)
                {
                    correctTiles++;
                }
            }
        }

        if (correctTiles == tiles.Length -1)
        {
                _isFinished = true;
                //Debug.Log("You Won");
                endPanel.SetActive(true);
                //GetComponent<TimerScript>().StopTimer();
                var a = GetComponent<TimerScript>();
                a.StopTimer();
                endPanelTimeText.text = (a.minutes < 10 ? "0" : "")+ a.minutes + ":" + (a.seconds < 10 ? "0" : "") + a.seconds;
        }

        
        }



    }


    public void playAgain()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Shuffle();
        SceneManager.LoadScene(0);
    }


    public void Shuffle()
    {

        if (emptySpaceIndex != 8)
        {
            var tileOn8LastPos = tiles[8].targetPosition;
            tiles[8].targetPosition = emptySpace.position;
            emptySpace.position = tileOn8LastPos;
            tiles[emptySpaceIndex] = tiles[8];
            tiles[8] = null;
            emptySpaceIndex = 8;
        }
        int inversion;
        do
        {
            for (int i = 0; i < 7; i++)
            {
                if (tiles[i] != null)
                {
                    var lastPos = tiles[i].targetPosition;
                    int randomIndex = Random.Range(0, 7);
                    tiles[i].targetPosition = tiles[randomIndex].targetPosition;
                    tiles[randomIndex].targetPosition = lastPos;
                    var tile = tiles[i];
                    tiles[i] = tiles[randomIndex];
                    tiles[randomIndex] = tile;
                }
            }
            inversion = GetInversions();
            Debug.Log("Puzzle Shuffled");
        } while (inversion %2 != 0);


        
    }

    public int findIndex(TilesScript ts)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null)
            {
                if (tiles[i] == ts)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    int GetInversions()
    {
        int inversionsSum = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            int thisTileInversion = 0;
            for (int j = i; j < tiles.Length; j++)
            {
                if (tiles[j] != null)
                {
                    if (tiles[i].number > tiles[j].number)
                    {
                        thisTileInversion++;
                    }
                }
            }
            inversionsSum += thisTileInversion;
        }
        return inversionsSum;
    }

}
