using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gm : MonoBehaviour
{

    [SerializeField] Transform piece;
    [SerializeField] Transform border;
    [SerializeField] Texture2D[] pic;
    [SerializeField] Transform gameHolder;
    [Range(2, 6)]
    [SerializeField] int startDifficulty;
   


    public static int difficulty = 0;
    public static int picCounter = 0;

    private float scale;
    private Vector2Int ratio;
    private float scaleFactor;
    private Vector2 pieceSize;
    private float offsetX;
    private float offsetY;

    private List<PuzzlePiece> pieces = new List<PuzzlePiece>();
    private int correctCounter = 0;

    void Start()
    {
        if (startDifficulty + 3 <= difficulty) 
        {
           
            return;
        }
        if (difficulty == 0)
        {
            difficulty = startDifficulty;
        }
        scale = Camera.main.orthographicSize;
        scaleFactor = scale * 1.5f;  // присваиваем уже после того, как scale получил значение
        ratio = GetDimentions(pic[picCounter]);
        UpdateGameHolder();
        CreatePuzzles(pic[picCounter], ratio.x, ratio.y);
        UpdateBorder(pieceSize.x, pieceSize.y);
        Scattering();
    }
    private Vector2Int GetDimentions(Texture2D picture)
    {
        Vector2Int dimensions = Vector2Int.zero;
        if (picture.width < picture.height)
        {
            dimensions.x = difficulty;
            dimensions.y = (difficulty * picture.height) / picture.width;
        }
        else
        {
            dimensions.y = difficulty;
            dimensions.x = (difficulty * picture.width) / picture.height;
        }
        return dimensions;
    }
    private void UpdateGameHolder()
    {
        float height = scale * 2f;
        float width = height * Camera.main.aspect;
        gameHolder.localScale = new Vector3(width, height, 1f);
    }
    private void UpdateBorder(float x, float y)
    {
        border.localScale = new Vector3(ratio.x * x, ratio.y * y, 1f);
        border.localPosition = new Vector3((ratio.x - 1) * x * 0.5f - offsetX, (ratio.y - 1) * y * 0.5f - offsetY, 1f);
    }
    private void CreatePuzzles(Texture2D pic, int xW, int yH)
    {
        // размеры текстуры в пикселях
        int texWidth = pic.width;
        int texHeight = pic.height;

        // размеры одного куска в пикселях
        int pieceWidthPx = texWidth / xW;
        int pieceHeightPx = texHeight / yH;
        int pieceSizePx = Mathf.Min(texWidth / xW, texHeight / yH);

        float height = 1f / ratio.y;
        float aspect = (float)pic.width / pic.height;
        float width = aspect / ratio.x;



        for (int x = 0; x < xW; x++)
        {
            for (int y = 0; y < yH; y++)
            {
                Transform currPiece = Instantiate(piece);

                // создаём кусок из текстуры по пикселям
                Rect rect = new Rect(x * pieceWidthPx, y * pieceHeightPx, pieceWidthPx, pieceHeightPx);
                Sprite sprite = Sprite.Create(pic, rect, new Vector2(0.5f, 0.5f), pieceWidthPx);
                currPiece.GetComponent<SpriteRenderer>().sprite = sprite;
                currPiece.localScale = new Vector3(width * scaleFactor, height * scaleFactor, 1f);

                if (x == 0 && y == 0)
                {

                   
                 
                    pieceSize = new Vector2(currPiece.GetComponent<SpriteRenderer>().bounds.size.x, (currPiece.GetComponent<SpriteRenderer>().bounds.size.y));
                    offsetY = (yH - 1) * pieceSize.y * 0.5f;
                    offsetX = (xW - 1) * pieceSize.x;
                }

                // позиционируем куски в Unity world units
                currPiece.localPosition = new Vector3(
                    x * pieceSize.x - offsetX,
                    (y * pieceSize.y - offsetY),
                    0);
                currPiece.name = $"Piece_{x}_{y}";
                currPiece.parent = gameHolder;
                PuzzlePiece puzzlePiece = currPiece.GetComponent<PuzzlePiece>();
                puzzlePiece.SetCorrectPos(currPiece.localPosition);
                pieces.Add(puzzlePiece);






            }
        }
    }
    private void Scattering()
    {
        // получаем примерный диапазон по размеру контейнера
        float sizeX = gameHolder.GetComponent<SpriteRenderer>().bounds.size.x;
        float sizeY = gameHolder.GetComponent<SpriteRenderer>().bounds.size.y;


        foreach (var p in pieces)
        {
            float scatterRangeX = Random.Range(3f / sizeX, 1f / sizeX);   // от середины до правого края
            float scatterRangeY = Random.Range(-2f / sizeY, 2f / sizeY);       // по вертикали от -Y до +Y

            // задаём случайную позицию
            p.transform.localPosition = new Vector3(scatterRangeX, scatterRangeY, 0);
        }
    }
    private void IncreaseCorrectCounter()
    {
        correctCounter++;
    }

    private void OnEnable()
    {
       
        EventManager.OnPuzzleCorrect += IncreaseCorrectCounter;
        EventManager.OnPuzzleCorrect += CheckCorrect;

    }
    private void OnDisable()
    {
       
        EventManager.OnPuzzleCorrect -= IncreaseCorrectCounter;
        EventManager.OnPuzzleCorrect -= CheckCorrect;
    }
    
    private void CheckCorrect()
    {
        if (difficulty != startDifficulty + 1 && correctCounter == pieces.Count)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                difficulty++;
                picCounter++;
            }

        
        }
       
    }
