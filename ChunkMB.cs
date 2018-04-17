using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMB : MonoBehaviour
{
    Chunk owner;
    public ChunkMB() { }
    public void SetOwner(Chunk o)
    {
        owner = o;
        InvokeRepeating("SaveProgress", 10, 1000);

    }

    //public IEnumerator Drop(Block b, Block.BlockType bt, int maxdrop)
    //{
    //    Block thisBlock = b;
    //    int x = (int)b.position.x;
    //    int y = (int)b.position.y;
    //    int z = (int)b.position.z;
    //    Block prevBlock = null;
    //    for (int i = 0; i < maxdrop; i++)
    //    {
    //        Block.BlockType previousType = thisBlock.bType;
    //        thisBlock.SetType(bt);
    //        if (prevBlock != null)
    //            prevBlock.SetType(previousType);

    //        prevBlock = thisBlock;
    //        b.owner.Redraw();

    //        yield return new WaitForSeconds(0.2f);
    //        Vector3 pos = thisBlock.position;

    //        thisBlock = thisBlock.GetBlock((int)pos.x, (int)pos.y - 1, (int)pos.z);
    //        if (thisBlock.isSolid)
    //        {
    //            thisBlock.owner.Redraw();
    //            yield break;
    //        }
    //    }
    //}

    //public IEnumerator Flow(Block b, Block.BlockType bt, int strength, int maxsize)
    //{
    //    //reduce the strenth of the fluid block
    //    //with each new block created
    //    if (maxsize <= 0) yield break;
    //    if (b == null) yield break;
    //    if (strength <= 0) yield break;
    //    if (b.bType != Block.BlockType.AIR) yield break;

    //    int x = (int)b.position.x;
    //    int y = (int)b.position.y;
    //    int z = (int)b.position.z;

    //    if (b.GetBlock(x, y - 1, z).bType == Block.BlockType.WATER && b.GetBlock(x, y + 1, z).bType != Block.BlockType.WATER)
    //    {
    //        yield break;
    //    }
    //    else
    //    {
    //        b.SetType(bt);
    //        b.owner.Redraw();
    //        yield return new WaitForSeconds(0.25f);
    //    }
        
    //    //flow down if air block beneath
    //    Block below = b.GetBlock(x, y - 1, z);
    //    if (below != null && below.bType == Block.BlockType.AIR)
    //    {
    //        StartCoroutine(Flow(b.GetBlock(x, y - 1, z), bt, strength, --maxsize));
    //        yield break;
    //    }
    //    else //flow outward
    //    {
    //        --strength;
    //        --maxsize;
    //        //flow left
    //        StartCoroutine(Flow(b.GetBlock(x - 1, y, z), bt, strength, maxsize));
    //        yield return new WaitForSeconds(0.25f);

    //        //flow right
    //        StartCoroutine(Flow(b.GetBlock(x + 1, y, z), bt, strength, maxsize));
    //        yield return new WaitForSeconds(0.25f);

    //        //flow forward
    //        StartCoroutine(Flow(b.GetBlock(x, y, z + 1), bt, strength, maxsize));
    //        yield return new WaitForSeconds(0.25f);

    //        //flow back
    //        StartCoroutine(Flow(b.GetBlock(x, y, z - 1), bt, strength, maxsize));
    //        yield return new WaitForSeconds(0.25f);
    //    }



    //}
}
