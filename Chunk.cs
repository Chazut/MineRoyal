using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{

    public Material cubeMaterial;
    public Material fluidMaterial;
    public Block[,,] chunkData;
    public GameObject chunk;
    public GameObject fluid;
    public enum ChunkStatus { DRAW, DONE, KEEP};
    public ChunkStatus status;
    public float touchedTime;
    public ChunkMB mb;
    bool treesCreated = false;

    void BuildChunk()
    {
        touchedTime = Time.time;
        chunkData = new Block[World.chunkSize, World.chunkSize, World.chunkSize];

        for (int z = 0; z < World.chunkSize; z++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int x = 0; x < World.chunkSize; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    int worldX = (int)(x + chunk.transform.position.x);
                    int worldY = (int)(y + chunk.transform.position.y);
                    int worldZ = (int)(z + chunk.transform.position.z);

                    int surfaceHeight = Utils.GenerateHeight(worldX, worldZ);

                    if (worldY == 0)
                    {
                        chunkData[x, y, z] = new Block(Block.BlockType.BEDROCK, pos, chunk.gameObject, this);
                    }
                    else if (worldY <= Utils.GenerateStoneHeight(worldX, worldZ))
                    {
                        //<x change probability of diamond
                        if (Utils.fBM3D(worldX, worldY, worldZ, 0.1f, 2) < 0.39f && worldY < 17)
                            chunkData[x, y, z] = new Block(Block.BlockType.DIAMOND, pos, chunk.gameObject, this);
                        else if (Utils.fBM3D(worldX, worldY, worldZ, 0.0993f, 3) < 0.41f && worldY < 17)
                            chunkData[x, y, z] = new Block(Block.BlockType.REDSTONE, pos, chunk.gameObject, this);
                        else
                            chunkData[x, y, z] = new Block(Block.BlockType.STONE, pos, chunk.gameObject, this);
                    }
                    else if (worldY == Utils.GenerateHeight(worldX, worldZ))
                        chunkData[x, y, z] = new Block(Block.BlockType.GRASS, pos, chunk.gameObject, this);
                    else if (worldY < Utils.GenerateHeight(worldX, worldZ))
                        chunkData[x, y, z] = new Block(Block.BlockType.DIRT, pos, chunk.gameObject, this);
                    else if(worldY < 75)
                        chunkData[x, y, z] = new Block(Block.BlockType.WATER, pos, fluid.gameObject, this);
                    else
                        chunkData[x, y, z] = new Block(Block.BlockType.AIR, pos, chunk.gameObject, this);

                    if (worldY == surfaceHeight && chunkData[x, y, z].bType == Block.BlockType.GRASS)
                    {
                        if (Utils.fBM3D(worldX, worldY, worldZ, 0.4f, 2) < 0.4f)
                            chunkData[x, y, z] = new Block(Block.BlockType.WOODBASE, pos, chunk.gameObject, this);
                        else
                            chunkData[x, y, z] = new Block(Block.BlockType.GRASS, pos, chunk.gameObject, this);
                    }

                    if (chunkData[x, y, z].bType != Block.BlockType.BEDROCK && chunkData[x,y,z].bType != Block.BlockType.WATER && chunkData[x, y, z].bType != Block.BlockType.AIR && Utils.fBM3D(worldX, worldY, worldZ, 0.1f, 3) < 0.42f)
                    {
                        if(!chunkData[x, y, z].HasWaterNeighbour())
                        {
                            chunkData[x, y, z] = new Block(Block.BlockType.AIR, pos, chunk.gameObject, this);
                        }
                    }
                    status = ChunkStatus.DRAW;
                }
    }

    public void Redraw()
    {
        GameObject.DestroyImmediate(chunk.GetComponent<MeshFilter>());
        GameObject.DestroyImmediate(chunk.GetComponent<MeshRenderer>());
        GameObject.DestroyImmediate(chunk.GetComponent<Collider>());
        GameObject.DestroyImmediate(fluid.GetComponent<MeshFilter>());
        GameObject.DestroyImmediate(fluid.GetComponent<MeshRenderer>());
        GameObject.DestroyImmediate(fluid.GetComponent<Collider>());
        DrawChunk();
    }

    public void UpdateChunk()
    {
        for (int z = 0; z < World.chunkSize; z++)
        {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    //if (chunkData[x, y, z].bType == Block.BlockType.SAND)
                    //{
                    //    mb.StartCoroutine(mb.Drop(chunkData[x, y, z], Block.BlockType.SAND, 20));
                    //}
                    //if (chunkData[x, y, z].bType == Block.BlockType.WATER)
                    //{
                    //    mb.StartCoroutine(mb.Flow(chunkData[x, y, z], Block.BlockType.WATER, 10, 50));
                    //}
                    //if (chunkData[x, y, z].GetBlock(x, y - 1, z) != null)
                    //{
                    //    if (chunkData[x, y, z].bType == Block.BlockType.WATER && chunkData[x, y, z].GetBlock(x, y - 1, z).bType != Block.BlockType.WATER)
                    //    {
                    //        chunkData[x, y, z].GetBlock(x, y - 1, z).SetType(Block.BlockType.SAND);
                    //        if (chunkData[x, y, z].GetBlock(x, y - 2, z) != null)
                    //            if (chunkData[x, y, z].GetBlock(x, y - 2, z).bType == Block.BlockType.DIRT || chunkData[x, y, z].GetBlock(x, y - 2, z).bType == Block.BlockType.GRASS)
                    //                chunkData[x, y, z].GetBlock(x, y - 2, z).SetType(Block.BlockType.SAND);
                    //        if (chunkData[x, y, z].GetBlock(x, y - 3, z) != null)
                    //            if (chunkData[x, y, z].GetBlock(x, y - 3, z).bType == Block.BlockType.DIRT || chunkData[x, y, z].GetBlock(x, y - 3, z).bType == Block.BlockType.GRASS)
                    //                chunkData[x, y, z].GetBlock(x, y - 3, z).SetType(Block.BlockType.SAND);
                    //    }
                    //}
                    for (int i = -3; i < 4; i++)
                    {
                        if (i != 0)
                        {
                            if (chunkData[x, y, z].bType == Block.BlockType.WATER && chunkData[x, y, z].GetBlock(x, y - 1, z).bType != Block.BlockType.WATER)
                            {
                                if (i < 0)
                                {
                                    if (chunkData[x, y, z].GetBlock(x, y + i, z) != null)
                                    {
                                        if (chunkData[x, y, z].GetBlock(x, y + i, z).bType != Block.BlockType.STONE
                                            && chunkData[x, y, z].GetBlock(x, y + i, z).bType != Block.BlockType.WATER)
                                        {
                                            chunkData[x, y, z].GetBlock(x, y + i, z).SetType(Block.BlockType.SAND);
                                        }
                                    }
                                }
                                if (chunkData[x, y, z].GetBlock(x + i, y, z) != null)
                                {
                                    if (chunkData[x, y, z].GetBlock(x + i, y, z).bType != Block.BlockType.STONE 
                                        && chunkData[x, y, z].GetBlock(x + i, y, z).bType != Block.BlockType.WATER)
                                    {
                                        chunkData[x, y, z].GetBlock(x + i, y, z).SetType(Block.BlockType.SAND);
                                    }
                                }
                                if (chunkData[x, y, z].GetBlock(x, y, z + i) != null)
                                {
                                    if (chunkData[x, y, z].GetBlock(x, y, z + i).bType != Block.BlockType.STONE 
                                        && chunkData[x, y, z].GetBlock(x, y, z + i).bType != Block.BlockType.WATER)
                                    {
                                        chunkData[x, y, z].GetBlock(x, y, z + i).SetType(Block.BlockType.SAND);
                                    }
                                }
                            }
                            
                        }
                    }
                }
            }
        }
    }

    public void DrawChunk()
    {
        UpdateChunk();
        if (!treesCreated)
        {
            for (int z = 0; z < World.chunkSize; z++)
                for (int y = 0; y < World.chunkSize; y++)
                    for (int x = 0; x < World.chunkSize; x++)
                    {
                        BuildTrees(chunkData[x, y, z], x, y, z);
                    }
            treesCreated = true;
        }
        for (int z = 0; z < World.chunkSize; z++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int x = 0; x < World.chunkSize; x++)
                {
                    chunkData[x, y, z].Draw();
                }
        CombineQuads(chunk.gameObject, cubeMaterial);
        MeshCollider collider = chunk.gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        collider.sharedMesh = chunk.transform.GetComponent<MeshFilter>().mesh;
        CombineQuads(fluid.gameObject, fluidMaterial);
        status = ChunkStatus.DONE;
    }

    void BuildTrees(Block trunk, int x, int y, int z)
    {
        if (trunk.bType != Block.BlockType.WOODBASE) return;

        Block t = trunk.GetBlock(x, y + 1, z);
        if (t != null)
        {
            if (t.bType == Block.BlockType.WATER)
            {
                trunk.SetType(Block.BlockType.GRASS);
                return;
            }
            t.SetType(Block.BlockType.WOOD);
            Block t1 = trunk.GetBlock(x, y + 2, z);
            if (t1 != null)
            {
                t1.SetType(Block.BlockType.WOOD);

                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        for (int k = 3; k <= 4; k++)
                        {
                            Block t2 = trunk.GetBlock(x + i, y + k, z + j);

                            if (t2 != null)
                            {
                                t2.SetType(Block.BlockType.LEAVES);
                            }
                            else return;
                        }
                Block t3 = trunk.GetBlock(x, y + 5, z);
                if (t3 != null)
                {
                    t3.SetType(Block.BlockType.LEAVES);
                }
            }
        }
    }

    public Chunk() { }

    // Use this for initialization
    public Chunk(Vector3 position, Material c, Material t)
    {
        chunk = new GameObject(World.BuildChunkName(position));
        chunk.transform.position = position;
        mb = chunk.AddComponent<ChunkMB>();
        fluid = new GameObject(World.BuildChunkName(position) + "_F");
        fluid.transform.position = position;
        cubeMaterial = c;
        fluidMaterial = t;
        BuildChunk();
    }

    public void CombineQuads(GameObject o, Material m)
    {
        //1. Combine all children meshes
        MeshFilter[] meshFilters = o.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        //2. Create a new mesh on the parent object
        MeshFilter mf = (MeshFilter)o.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();

        //3. Add combined meshes on children as the parent's mesh
        mf.mesh.CombineMeshes(combine);

        //4. Create a renderer for the parent
        MeshRenderer renderer = o.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = m;

        //5. Delete all uncombined children
        foreach (Transform quad in o.transform)
        {
            GameObject.Destroy(quad.gameObject);
        }
    }

}
