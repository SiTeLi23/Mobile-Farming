using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class WorldManager : MonoBehaviour
{
    private enum ChunkShape 
    {
      None,
      TopRight,
      BottomRight,
      BottomLeft,
      TopLeft,
      Top,
      Right,
      Bottom,
      Left,
      Four
    }

    [Header("Elements")]
    [SerializeField] private Transform world;
    Chunk[,] grid;
    private string dataPath;

    [Header("Settings")]
    [SerializeField] private int gridSize = 20;
    [SerializeField] private int gridScale = 5;

    [Header("Data")]
    private WorldData worldData;
    private bool shouldSave;

    [Header("Chunk Meshes")]
    [SerializeField] private Mesh[] chunkShapes;

    private void Awake()
    {
        Chunk.onUnlocked += ChunkUnlockedCallback;
        Chunk.onPriceChanged += ChunkPriceChangedCallback;
    }

    private void OnDestroy()
    {
        Chunk.onUnlocked -= ChunkUnlockedCallback;
        Chunk.onPriceChanged -= ChunkPriceChangedCallback;
    }
    void Start()
    {
        dataPath = Application.dataPath + "/worldData.txt";
        LoadWorld();
        Initialized();

        InvokeRepeating("TrySaveGame", 1, 1);
    }

    void Update()
    {
        
    }
    private void Initialized()
    {
        for (int i = 0; i < world.childCount; i++)
        {
            //set up initial chun based on whether they have been unlocked
            world.GetChild(i).GetComponent<Chunk>().Initialized(worldData.chunkPrices[i]);
        }

        InitializedGrid();

        UpdateChunkWalls();
        UpdateGridRenerers();

    }

    private void InitializedGrid() 
    {
        grid = new Chunk[gridSize, gridSize];

        for (int i = 0; i < world.childCount; i++)
        {
            //getting all the chunk
            Chunk chunk = world.GetChild(i).GetComponent<Chunk>();
            //sor chunk in the grid using position
            Vector2Int chunkGridPosition = new Vector2Int((int)chunk.transform.position.x / gridScale, (int)chunk.transform.position.z / gridScale);

            chunkGridPosition += new Vector2Int(gridSize / 2, gridSize / 2);

            //store the chunk into the grid
            grid[chunkGridPosition.x, chunkGridPosition.y] = chunk;
        }
    }

    private void UpdateChunkWalls() 
    {
        //loop along the x axis
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            //loop along the z axis
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Chunk chunk = grid[x, y];
                //check if there's valid chunk on this grid position
                if (chunk == null) continue;

                //trying to get chunk from 4 directions
                Chunk frontChunk = IsValidGridPositions(x, y + 1) ? grid[x, y + 1] : null;
                Chunk rightChunk = IsValidGridPositions(x + 1, y) ? grid[x + 1, y] : null;
                Chunk backChunk = IsValidGridPositions(x, y - 1) ? grid[x, y - 1] : null;
                Chunk leftChunk = IsValidGridPositions(x - 1, y) ? grid[x - 1, y] : null;

                int configuration = 0;

                if (frontChunk != null && frontChunk.IsUnlocked())
                    configuration = configuration + 1; //1 = decimal bit pattern 0001

                if (rightChunk != null && rightChunk.IsUnlocked())
                    configuration = configuration + 2; //2 = decimal bit pattern 0010

                if (backChunk != null && backChunk.IsUnlocked())
                    configuration = configuration + 4; //4= decimal bit pattern 0100

                if (leftChunk != null && leftChunk.IsUnlocked())
                    configuration = configuration + 8; //8 = decimal bit pattern 1000

                //we know the configuration of the chunk;
                chunk.UpdateWalls(configuration);

                SetChunkRenderer(chunk, configuration);
            }
        }
    }

    private void SetChunkRenderer(Chunk chunk, int configuration) 
    {
        
        switch (configuration)
        {
            case 0:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Four]);
                break;
            case 1:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Bottom]);
                break;
            case 2:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Left]);
                break;
            case 3:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.BottomLeft]);
                break;
            case 4:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Top]);
                break;
            case 5:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
                break;
            case 6:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.TopLeft]);
                break;
            case 7:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
                break;
            case 8:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Right]);
                break;
            case 9:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.BottomRight]);
                break;
            case 10:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
                break;
            case 11:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.Bottom]);
                break;
            case 12:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.TopRight]);
                break;
            case 13:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
                break;
            case 14:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
                break;
            case 15:
                chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
                break;
        }
    }
    private void UpdateGridRenerers()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            //loop along the z axis
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Chunk chunk = grid[x, y];

                if (chunk == null) continue;
                if (chunk.IsUnlocked()) continue;

                Chunk frontChunk = IsValidGridPositions(x, y + 1) ? grid[x, y + 1] : null;
                Chunk rightChunk = IsValidGridPositions(x + 1, y) ? grid[x + 1, y] : null;
                Chunk backChunk = IsValidGridPositions(x, y - 1) ? grid[x, y - 1] : null;
                Chunk leftChunk = IsValidGridPositions(x - 1, y) ? grid[x - 1, y] : null;

                if (frontChunk != null && frontChunk.IsUnlocked())
                    chunk.DisplayLockedElements();
                else if(rightChunk != null && rightChunk.IsUnlocked())
                    chunk.DisplayLockedElements();
                else if (backChunk != null && backChunk.IsUnlocked())
                    chunk.DisplayLockedElements();
                else if (leftChunk != null && leftChunk.IsUnlocked())
                    chunk.DisplayLockedElements();
            }
        }
    }
    private bool IsValidGridPositions(int x, int y) 
    {
        //make sure there's no negative grid position
        if (x < 0 || x >= gridSize || y < 0 || y >= gridSize)
            return false;

        return true;
    }
    private void TrySaveGame() 
    {
        if (shouldSave) 
        {
            shouldSave = false;
            SaveWorld();
        }
    }


    private void ChunkUnlockedCallback() 
    {
        Debug.Log("Chunck unlocked!");

        UpdateChunkWalls();
        UpdateGridRenerers();

        SaveWorld();
    }

    private void ChunkPriceChangedCallback()
    {
        shouldSave = true;
    }

    private void LoadWorld() 
    {
        string data = "";

        //if there's no save file, create aone
        if (!File.Exists(dataPath))
        {
            FileStream fs = new FileStream(dataPath, FileMode.Create);

            worldData = new WorldData();

            for (int i = 0; i < world.childCount; i++)
            {
                worldData.chunkPrices.Add(world.GetChild(i).GetComponent<Chunk>().GetInitialPrice());
            }

            string worldDataString = JsonUtility.ToJson(worldData, true);//convert the worldData to json type of file

            //convert world data string(json file) to an array of byte since file stream can only use array of byte byte
            byte[] worldDataByte = Encoding.UTF8.GetBytes(worldDataString); 

            fs.Write(worldDataByte); // save data

            fs.Close();
        }
        else
        {
            //if there's existed save file, load the data
            data = File.ReadAllText(dataPath);
            worldData = JsonUtility.FromJson<WorldData>(data);

            if(worldData.chunkPrices.Count < world.childCount) 
            {
                //we need to add new chunks to the list if scene is not compatibal with save file
                UpdateData();
            }
        }
    }

    private void UpdateData() 
    {
        //How many chunks are missing in our data
        int missingData = world.childCount - worldData.chunkPrices.Count;

        for (int i = 0; i < missingData; i++)
        {
            int chunkIndex = world.childCount - missingData + i;
            int chunkPrice = world.GetChild(chunkIndex).GetComponent<Chunk>().GetInitialPrice();
            worldData.chunkPrices.Add(chunkPrice);
        }
    }

    private void SaveWorld() 
    {
        if(worldData.chunkPrices.Count != world.childCount) 
        {
            //if there's update on the scene, reset the save data
            worldData = new WorldData();
        }

        for (int i = 0; i < world.childCount; i++)
        {
            if (worldData.chunkPrices.Count > 1) 
            {
                //we have data, we just need to read it
                worldData.chunkPrices[i] = world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice();
            }
            else 
            {
               //don't have data, we need to add it to the list
                worldData.chunkPrices.Add(world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice());
            }

        }
        string data = JsonUtility.ToJson(worldData, true);
        File.WriteAllText(dataPath, data);
    }
}
