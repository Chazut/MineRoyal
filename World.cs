using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Realtime.Messaging.Internal;

public class World : MonoBehaviour
{

    public GameObject player;
    public Material textureAtlas;
    public Material fluidTexture;
    public static int columnHeight = 16;
    public static int chunkSize = 16;
    public int worldSize = 3;
    public static Dictionary<string, Chunk> chunks;
    //public static int radius = 5;
    //public static ConcurrentDictionary<string, Chunk> dyn_chunks;
    public static bool firstbuild = true;
    //public static List<string> toRemove = new List<string>();

    public Slider loadingAmount;
    public Button playButton;

    public static CoroutineQueue queue;
    public static uint maxCoroutines = 10000;
    public Vector3 lastbuildPos;

    public static string BuildChunkName(Vector3 v)
    {
        return (int)v.x + "_" +
                     (int)v.y + "_" +
                     (int)v.z;
    }

    IEnumerator BuildChunkColumn()
    {
        for (int i = 0; i < columnHeight; i++)
        {
            Vector3 chunkPosition = new Vector3(this.transform.position.x,
                                                i * chunkSize,
                                                this.transform.position.z);
            Chunk c = new Chunk(chunkPosition, textureAtlas, fluidTexture);
            c.chunk.transform.parent = this.transform;
            chunks.Add(c.chunk.name, c);
        }

        foreach (KeyValuePair<string, Chunk> c in chunks)
        {
            c.Value.DrawChunk();
            yield return null;
        }

    }

    IEnumerator BuildWorld()
    {
        int posx = (int)Mathf.Floor(player.transform.position.x / chunkSize);
        int posz = (int)Mathf.Floor(player.transform.position.z / chunkSize);

        float totalChunks = worldSize * worldSize * chunkSize * 2;
        int processCount = 0;

        for (int z = 0; z < worldSize; z++)
            for (int x = 0; x < worldSize; x++)
                for (int y = 0; y < columnHeight; y++)
                {
                    Vector3 chunkPosition = new Vector3((x + posx) * chunkSize,
                                                        y * chunkSize,
                                                        (posz + z) * chunkSize);
                    Chunk c = new Chunk(chunkPosition, textureAtlas, fluidTexture);
                    c.chunk.transform.parent = this.transform;
                    c.fluid.transform.parent = this.transform;
                    chunks.Add(c.chunk.name, c);
                    processCount++;
                    loadingAmount.value = processCount / totalChunks * 100;
                    yield return null;
                }

        foreach (KeyValuePair<string, Chunk> c in chunks)
        {
            c.Value.DrawChunk();
            processCount++;
            loadingAmount.value = processCount / totalChunks * 100;
            yield return null;
        }
        player.SetActive(true);
        player.transform.position = new Vector3(worldSize / 2 * chunkSize, 256, worldSize / 2 * chunkSize);
        loadingAmount.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);

    }

    public static Block GetWorldBlock(Vector3 pos)
    {
        int cx, cy, cz;

        if (pos.x < 0)
            cx = (int)((Mathf.Round(pos.x - chunkSize) + 1) / (float)chunkSize) * chunkSize;
        else
            cx = (int)(Mathf.Round(pos.x) / (float)chunkSize) * chunkSize;

        if (pos.y < 0)
            cy = (int)((Mathf.Round(pos.y - chunkSize) + 1) / (float)chunkSize) * chunkSize;
        else
            cy = (int)(Mathf.Round(pos.y) / (float)chunkSize) * chunkSize;

        if (pos.z < 0)
            cz = (int)((Mathf.Round(pos.z - chunkSize) + 1) / (float)chunkSize) * chunkSize;
        else
            cz = (int)(Mathf.Round(pos.z) / (float)chunkSize) * chunkSize;

        int blx = (int)Mathf.Abs((float)Mathf.Round(pos.x) - cx);
        int bly = (int)Mathf.Abs((float)Mathf.Round(pos.y) - cy);
        int blz = (int)Mathf.Abs((float)Mathf.Round(pos.z) - cz);

        string cn = BuildChunkName(new Vector3(cx, cy, cz));
        Chunk c;
        if (chunks.TryGetValue(cn, out c))
        {

            return c.chunkData[blx, bly, blz];
        }
        else
            return null;
    }

    //void BuildChunkAt(int x, int y, int z)
    //{
    //    Vector3 chunkPosition = new Vector3(x * chunkSize,
    //                                        y * chunkSize,
    //                                        z * chunkSize);

    //    string n = BuildChunkName(chunkPosition);
    //    Chunk c;

    //    if (!dyn_chunks.TryGetValue(n, out c))
    //    {
    //        c = new Chunk(chunkPosition, textureAtlas);
    //        c.chunk.transform.parent = this.transform;
    //        dyn_chunks.TryAdd(c.chunk.name, c);
    //    }

    //}

    //IEnumerator BuildRecursiveWorld(int x, int y, int z, int rad)
    //{
    //    rad--;
    //    if (rad <= 0) yield break;

    //    //build chunk front
    //    BuildChunkAt(x, y, z + 1);
    //    queue.Run(BuildRecursiveWorld(x, y, z + 1, rad));
    //    yield return null;

    //    //build chunk back
    //    BuildChunkAt(x, y, z - 1);
    //    queue.Run(BuildRecursiveWorld(x, y, z - 1, rad));
    //    yield return null;

    //    //build chunk left
    //    BuildChunkAt(x - 1, y, z);
    //    queue.Run(BuildRecursiveWorld(x - 1, y, z, rad));
    //    yield return null;

    //    //build chunk right
    //    BuildChunkAt(x + 1, y, z);
    //    queue.Run(BuildRecursiveWorld(x + 1, y, z, rad));
    //    yield return null;

    //    //build chunk up
    //    BuildChunkAt(x, y + 1, z);
    //    queue.Run(BuildRecursiveWorld(x, y + 1, z, rad));
    //    yield return null;

    //    //build chunk down
    //    if (y > 0)
    //    {
    //        BuildChunkAt(x, y - 1, z);
    //        queue.Run(BuildRecursiveWorld(x, y - 1, z, rad));
    //        yield return null;
    //    }
    //}

    //IEnumerator DrawChunks()
    //{
    //    foreach (KeyValuePair<string, Chunk> c in dyn_chunks)
    //    {
    //        if (c.Value.status == Chunk.ChunkStatus.DRAW)
    //        {
    //            c.Value.DrawChunk();
    //        }
    //        if(c.Value.chunk && Vector3.Distance(player.transform.position, c.Value.chunk.transform.position) > radius * chunkSize)
    //        {
    //            toRemove.Add(c.Key);
    //        }
    //        yield return null;
    //    }
    //}

    //IEnumerator RemoveOldChunks()
    //{
    //    for (int i = 0; i < toRemove.Count; i++)
    //    {
    //        string n = toRemove[i];
    //        Chunk c;
    //        if (dyn_chunks.TryGetValue(n, out c))
    //        {
    //            Destroy(c.chunk);
    //            dyn_chunks.TryRemove(n, out c);
    //            yield return null;
    //        }
    //    }
    //}

    //public void BuildNearPlayer()
    //{
    //    StopCoroutine("BuildRecursiveWorld");
    //    queue.Run(BuildRecursiveWorld((int)(player.transform.position.x / chunkSize),
    //                                        (int)(player.transform.position.y / chunkSize),
    //                                        (int)(player.transform.position.z / chunkSize),
    //                                        radius));
    //}

    // Use this for initialization
    // FOR DYNAMIC LOADING
    //void Start()
    //{
    //    Vector3 ppos = player.transform.position;
    //    player.transform.position = new Vector3(ppos.x,
    //                                        Utils.GenerateHeight(ppos.x, ppos.z) + 1,
    //                                        ppos.z);

    //    lastbuildPos = player.transform.position;
    //    player.SetActive(false);

    //    firstbuild = true;
    //    chunks = new ConcurrentDictionary<string, Chunk>();
    //    this.transform.position = Vector3.zero;
    //    this.transform.rotation = Quaternion.identity;
    //    queue = new CoroutineQueue(maxCoroutines, StartCoroutine);

    //    //build starting chunk
    //    BuildChunkAt((int)(player.transform.position.x / chunkSize),
    //                                        (int)(player.transform.position.y / chunkSize),
    //                                        (int)(player.transform.position.z / chunkSize));
    //    //draw it
    //    queue.Run(DrawChunks());

    //    //create a bigger world
    //    queue.Run(BuildRecursiveWorld((int)(player.transform.position.x / chunkSize),
    //                                        (int)(player.transform.position.y / chunkSize),
    //                                        (int)(player.transform.position.z / chunkSize), radius));
    //}

    void Start()
    {
        player.SetActive(false);
        chunks = new Dictionary<string, Chunk>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        // USE FOR DYNAMIC LOADING
        //Vector3 movement = lastbuildPos - player.transform.position;

        //if (movement.magnitude > chunkSize)
        //{
        //    lastbuildPos = player.transform.position;
        //    BuildNearPlayer();
        //}


        //if (!player.activeSelf)
        //{
        //    player.SetActive(true);
        //    firstbuild = false;
        //}
        
        //queue.Run(DrawChunks());
        //queue.Run(RemoveOldChunks());
    }

    public void StartBuild()
    {
        StartCoroutine(BuildWorld());
        long lVal = System.BitConverter.DoubleToInt64Bits(Utils.offset);
        string hex = lVal.ToString("X");
        Debug.Log("seed : " + hex + ":" + Utils.xOffset + ":" + Utils.yOffset + ":" + Utils.zOffset);
    }
}
