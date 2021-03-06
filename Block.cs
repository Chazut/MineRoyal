﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{

    enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };
    public enum BlockType { GRASS, DIRT, WATER, STONE, LEAVES, WOOD, WOODBASE, SAND, BEDROCK, REDSTONE, DIAMOND, NOCRACK,
        CRACK1, CRACK2, CRACK3, CRACK4, AIR
    };

    public BlockType bType;
    public BlockType previousbType;
    public bool isSolid;
    public Chunk owner;
    public GameObject parent;
    public Vector3 position;

    public BlockType health;
    int currentHealth;
    int[] blockHealthMax = { 3, 3, 10, 4, 2, 4, 4, 2, -1, 4, 4, 0, 0, 0, 0, 0, 0 };

    Vector2[,] blockUVs = { 
		/*GRASS TOP*/		{new Vector2( 0.125f, 0.375f ), new Vector2( 0.1875f, 0.375f),
                                new Vector2( 0.125f, 0.4375f ),new Vector2( 0.1875f, 0.4375f )},
		/*GRASS SIDE*/		{new Vector2( 0.1875f, 0.9375f ), new Vector2( 0.25f, 0.9375f),
                                new Vector2( 0.1875f, 1.0f ),new Vector2( 0.25f, 1.0f )},
		/*DIRT*/			{new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f),
                                new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )},
        /*WATER*/			{ new Vector2(0.875f,0.125f),  new Vector2(0.9375f,0.125f),
                                 new Vector2(0.875f,0.1875f), new Vector2(0.9375f,0.1875f)},
		/*STONE*/			{new Vector2( 0.0625f, 0.9375f ), new Vector2( 0.125f, 0.9375f),
                                new Vector2( 0.0625f, 1 ),new Vector2( 0.125f, 1 )},
        /*LEAVES*/			{ new Vector2(0.0625f,0.375f),  new Vector2(0.125f,0.375f),
                                 new Vector2(0.0625f,0.4375f), new Vector2(0.125f,0.4375f)},
 		/*WOOD*/			{ new Vector2(0.375f,0.625f),  new Vector2(0.4375f,0.625f),
                                 new Vector2(0.375f,0.6875f), new Vector2(0.4375f,0.6875f)},
 		/*WOODBASE*/		{ new Vector2(0.375f,0.625f),  new Vector2(0.4375f,0.625f),
                                 new Vector2(0.375f,0.6875f), new Vector2(0.4375f,0.6875f)},
        /*SAND*/			{ new Vector2(0.125f,0.875f),  new Vector2(0.1875f,0.875f),
                                 new Vector2(0.125f,0.9375f), new Vector2(0.1875f,0.9375f)},
        /*BEDROCK*/			{new Vector2( 0.0625f, 0.875f ), new Vector2( 0.125f, 0.875f),
                                new Vector2( 0.0625f, 0.9375f ),new Vector2( 0.125f, 0.9375f )},
        /*REDSTONE*/		{new Vector2( 0.1875f, 0.75f ), new Vector2( 0.25f, 0.75f),
                                new Vector2( 0.1875f, 0.8125f ),new Vector2( 0.25f, 0.8125f )},
		/*DIAMOND*/			{new Vector2( 0.125f, 0.75f ), new Vector2( 0.1875f, 0.75f),
                                new Vector2( 0.125f, 0.8125f ),new Vector2( 0.1875f, 0.8125f )},
        /*NOCRACK*/			{new Vector2( 0.6875f, 0f ), new Vector2( 0.75f, 0f),
                                new Vector2( 0.6875f, 0.0625f ),new Vector2( 0.75f, 0.0625f )},
		/*CRACK1*/			{ new Vector2(0f,0f),  new Vector2(0.0625f,0f),
                                 new Vector2(0f,0.0625f), new Vector2(0.0625f,0.0625f)},
 		/*CRACK2*/			{ new Vector2(0.0625f,0f),  new Vector2(0.125f,0f),
                                 new Vector2(0.0625f,0.0625f), new Vector2(0.125f,0.0625f)},
 		/*CRACK3*/			{ new Vector2(0.125f,0f),  new Vector2(0.1875f,0f),
                                 new Vector2(0.125f,0.0625f), new Vector2(0.1875f,0.0625f)},
 		/*CRACK4*/			{ new Vector2(0.1875f,0f),  new Vector2(0.25f,0f),
                                 new Vector2(0.1875f,0.0625f), new Vector2(0.25f,0.0625f)}
                        };

    public Block(BlockType b, Vector3 pos, GameObject p, Chunk o)
    {
        bType = b;
        owner = o;
        parent = p;
        position = pos;
        SetType(bType);
        //if (bType == BlockType.AIR || bType == BlockType.WATER)
        //    isSolid = false;
        //else
        //    isSolid = true;

        //if (bType == BlockType.WATER)
        //{
        //    parent = owner.fluid.gameObject;
        //}
        //else
        //    parent = owner.chunk.gameObject;

        //health = BlockType.NOCRACK;
        //currentHealth = blockHealthMax[(int)bType];
    }

    public Block(BlockType b, Vector3 pos, GameObject p)
    {
        bType = b;
        parent = p;
        position = pos;
        if (bType == BlockType.AIR || bType == BlockType.WATER)
            isSolid = false;
        else
            isSolid = true;

        if (bType == BlockType.WATER)
        {
            parent = owner.fluid.gameObject;
        }
        else
            parent = owner.chunk.gameObject;

        health = BlockType.NOCRACK;
        currentHealth = blockHealthMax[(int)bType];
    }

    public void SetType(BlockType b)
    {
        previousbType = bType;
        bType = b;
        if (bType == BlockType.AIR || bType == BlockType.WATER)
            isSolid = false;
        else
            isSolid = true;

        if (bType == BlockType.WATER)
        {
            parent = owner.fluid.gameObject;
        }
        else
            parent = owner.chunk.gameObject;

        health = BlockType.NOCRACK;
        currentHealth = blockHealthMax[(int)bType];
    }

    public bool BuildBlock(BlockType b)
    {
        //if (b == BlockType.WATER)
        //{
        //    owner.mb.StartCoroutine(owner.mb.Flow(this, BlockType.WATER, blockHealthMax[(int)BlockType.WATER], 50));
        //}
        //else if (b == BlockType.SAND)
        //{
        //    owner.mb.StartCoroutine(owner.mb.Drop(this, BlockType.SAND, 50));
        //}
        //else
        //{
            SetType(b);
            owner.Redraw();
        //}
        return true;
    }

    public bool HitBlock()
    {
        if (currentHealth == -1) return false;
        currentHealth--;
        health++;
        if (currentHealth <= 0)
        {
            bType = BlockType.AIR;
            isSolid = false;
            health = BlockType.NOCRACK;
            owner.Redraw();
            //owner.UpdateChunk();
            return true;
        }
        //this.Draw();
        owner.Redraw();
        return false;
    }

    void CreateQuad(Cubeside side)
    {
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh" + side.ToString();

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];

        List<Vector2> suvs = new List<Vector2>();

        //all possible UVs
        Vector2 uv00;
        Vector2 uv10;
        Vector2 uv01;
        Vector2 uv11;

        if (bType == BlockType.GRASS && side == Cubeside.TOP)
        {
            uv00 = blockUVs[0, 0];
            uv10 = blockUVs[0, 1];
            uv01 = blockUVs[0, 2];
            uv11 = blockUVs[0, 3];
        }
        else if (bType == BlockType.GRASS && side == Cubeside.BOTTOM)
        {
            uv00 = blockUVs[(int)(BlockType.DIRT + 1), 0];
            uv10 = blockUVs[(int)(BlockType.DIRT + 1), 1];
            uv01 = blockUVs[(int)(BlockType.DIRT + 1), 2];
            uv11 = blockUVs[(int)(BlockType.DIRT + 1), 3];
        }
        else
        {
            uv00 = blockUVs[(int)(bType + 1), 0];
            uv10 = blockUVs[(int)(bType + 1), 1];
            uv01 = blockUVs[(int)(bType + 1), 2];
            uv11 = blockUVs[(int)(bType + 1), 3];
        }

        //set cracks
        suvs.Add(blockUVs[(int)(health + 1), 3]);
        suvs.Add(blockUVs[(int)(health + 1), 2]);
        suvs.Add(blockUVs[(int)(health + 1), 0]);
        suvs.Add(blockUVs[(int)(health + 1), 1]);

        //all possible vertices 
        Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

        switch (side)
        {
            case Cubeside.BOTTOM:
                vertices = new Vector3[] { p0, p1, p2, p3 };
                normals = new Vector3[] {Vector3.down, Vector3.down,
                                            Vector3.down, Vector3.down};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.TOP:
                vertices = new Vector3[] { p7, p6, p5, p4 };
                normals = new Vector3[] {Vector3.up, Vector3.up,
                                            Vector3.up, Vector3.up};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.LEFT:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] {Vector3.left, Vector3.left,
                                            Vector3.left, Vector3.left};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.RIGHT:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] {Vector3.right, Vector3.right,
                                            Vector3.right, Vector3.right};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.FRONT:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] {Vector3.forward, Vector3.forward,
                                            Vector3.forward, Vector3.forward};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.BACK:
                vertices = new Vector3[] { p6, p7, p3, p2 };
                normals = new Vector3[] {Vector3.back, Vector3.back,
                                            Vector3.back, Vector3.back};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.SetUVs(1, suvs);
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("Quad");
        quad.transform.position = position;
        quad.transform.parent = this.parent.transform;

        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;

    }

    int ConvertBlockIndexToLocal(int i)
    {
        if (i <= -1)
            i = World.chunkSize + i;
        else if (i >= World.chunkSize)
            i = i - World.chunkSize;
        return i;
    }

    public BlockType GetBlockType(int x, int y, int z)
    {
        Block b = GetBlock(x, y, z);
        if (b == null)
            return BlockType.AIR;
        else
            return b.bType;
    }

    public Block GetBlock(int x, int y, int z)
    {
        Block[,,] chunks;

        if (x < 0 || x >= World.chunkSize ||
           y < 0 || y >= World.chunkSize ||
           z < 0 || z >= World.chunkSize)
        {  //block in a neighbouring chunk

            int newX = 0, newY = 0, newZ = 0;
            if (x < 0 || x >= World.chunkSize)
            {
                int xx = x;
                if (xx < 0)
                    xx = -World.chunkSize + 1 + x;
                newX = (((int)(xx / World.chunkSize)) * World.chunkSize);
            }
            if (y < 0 || y >= World.chunkSize)
            {
                int yy = y;
                if(yy < 0)
                    yy = -World.chunkSize + 1 + y;
                newY = (((int)(yy / World.chunkSize)) * World.chunkSize);
            }
            if (z < 0 || z >= World.chunkSize)
            {
                int zz = z;
                if (zz < 0)
                    zz = -World.chunkSize + z;
                newZ = (((int)(zz / World.chunkSize)) * World.chunkSize);
            }
            
            Vector3 neighbourChunkPos = this.parent.transform.position + new Vector3(newX, newY, newZ);
            string nName = World.BuildChunkName(neighbourChunkPos);

            x = ConvertBlockIndexToLocal(x);
            y = ConvertBlockIndexToLocal(y);
            z = ConvertBlockIndexToLocal(z);

            Chunk nChunk;
            if (World.chunks.TryGetValue(nName, out nChunk))
            {
                chunks = nChunk.chunkData;
            }
            else
                return null;
        }  //block in this chunk
        else
            chunks = owner.chunkData;

        return chunks[x, y, z];
    }

    //public Block GetBlock(int x, int y, int z)
    //{
    //    Block[,,] chunks;

    //    if (x < 0 || x >= World.chunkSize ||
    //       y < 0 || y >= World.chunkSize ||
    //       z < 0 || z >= World.chunkSize)
    //    {  //block in a neighbouring chunk

    //        int newX = x, newY = y, newZ = z;
    //        if (x < 0 || x >= World.chunkSize)
    //            newX = (int) (x / World.chunkSize);
    //        if (y < 0 || y >= World.chunkSize)
    //            newY = (int)(y / World.chunkSize);
    //        if (z < 0 || z >= World.chunkSize)
    //            newZ = (int)(z / World.chunkSize);

    //        Vector3 neighbourChunkPos = this.parent.transform.position + new Vector3(newX, newY, newZ);
    //        string nName = World.BuildChunkName(neighbourChunkPos);

    //        x = x - (int)position.x - 1;
    //        y = y - (int)position.y - 1;
    //        z = z - (int)position.z - 1;

    //        if (newX <= 0 && x < 0)
    //        {
    //            Debug.Log("newX:" + newX + " x:" + x + " y:" + y + " z:" + z);
    //            return null;
    //        }
    //        if (newY <= 0 && y < 0)
    //            return null;
    //        if (newZ <= 0 && z < 0)
    //            return null;

    //        Chunk nChunk;
    //        if (World.chunks.TryGetValue(nName, out nChunk))
    //        {
    //            chunks = nChunk.chunkData;
    //            Debug.Log(nName);
    //            Debug.Log("x:" + x + " y:" + y + " z:" + z);
    //        }
    //        else
    //            return null;
    //    }  //block in this chunk
    //    else
    //        chunks = owner.chunkData;

    //    return chunks[x, y, z];
    //}

    public bool HasSolidNeighbour(int x, int y, int z)
    {
        try
        {
            Block b = GetBlock(x, y, z);
            if(x == 16)
            {
                //Debug.Log(b == null);
            }
            if (b != null)
                return (b.isSolid || b.bType == bType);
        }
        catch (System.IndexOutOfRangeException) { }
        return false;
    }

    public bool CheckIfWaterNeighbour(int x, int y, int z)
    {
        try
        {
            Block b = GetBlock(x, y, z);
            if (b != null)
            {
                if (b.bType == BlockType.WATER)
                    return true;
            }      
        }
        catch (System.IndexOutOfRangeException) { }
        return false;
    }

    public bool HasWaterNeighbour()
    {
        if (CheckIfWaterNeighbour((int)position.x, (int)position.y, (int)position.z + 1)
            || CheckIfWaterNeighbour((int)position.x, (int)position.y, (int)position.z - 1)
            || CheckIfWaterNeighbour((int)position.x, (int)position.y + 1, (int)position.z)
            || CheckIfWaterNeighbour((int)position.x, (int)position.y - 1, (int)position.z)
            || CheckIfWaterNeighbour((int)position.x + 1, (int)position.y, (int)position.z)
            || CheckIfWaterNeighbour((int)position.x - 1, (int)position.y, (int)position.z))
        {
            return true;
        }
        return false;
    }

    public void Draw()
    {
        if (bType == BlockType.AIR) return;

        if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z + 1))
            CreateQuad(Cubeside.FRONT);
        if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z - 1))
            CreateQuad(Cubeside.BACK);
        if (!HasSolidNeighbour((int)position.x, (int)position.y + 1, (int)position.z))
            CreateQuad(Cubeside.TOP);
        if (!HasSolidNeighbour((int)position.x, (int)position.y - 1, (int)position.z))
            CreateQuad(Cubeside.BOTTOM);
        if (!HasSolidNeighbour((int)position.x - 1, (int)position.y, (int)position.z))
            CreateQuad(Cubeside.LEFT);
        if (!HasSolidNeighbour((int)position.x + 1, (int)position.y, (int)position.z))
            CreateQuad(Cubeside.RIGHT);
    }
}
