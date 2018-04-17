using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{

    static int maxHeight = 200;
    static float smooth = 0.01f;
    static int octaves = 4;
    static float persistence = 0.5f;

    public static float offset = 0f;
    public static float xOffset = 0f;
    public static float yOffset = 0f;
    public static float zOffset = 0f;

    public static int GenerateStoneHeight(float x, float z)
    {
        float height = Map(0, maxHeight - 5, 0, 1, fBM(x * smooth * 2, z * smooth * 2, octaves + 1, persistence));
        return (int)height;
    }

    public static int GenerateHeight(float x, float z)
    {
        float height = Map(0, maxHeight, 0, 1, fBM(x * smooth, z * smooth, octaves, persistence));
        return (int)height;
    }

    public static float fBM3D(float x, float y, float z, float sm, int oct)
    {
        if(xOffset == 0f)
        {
            xOffset = Random.Range(-100000f, 100000f);
        }
        if (yOffset == 0f)
        {
            yOffset = Random.Range(-100000f, 100000f);
        }
        if (zOffset == 0f)
        {
            zOffset = Random.Range(-100000f, 100000f);
        }
        float XY = fBM((x+xOffset) * sm, (y+yOffset) * sm, oct, 0.5f);
        float YZ = fBM((y+yOffset) * sm, (z+zOffset) * sm, oct, 0.5f);
        float XZ = fBM((x+xOffset) * sm, (z+zOffset) * sm, oct, 0.5f);

        float YX = fBM((y+yOffset) * sm, (x+xOffset) * sm, oct, 0.5f);
        float ZY = fBM((z+zOffset) * sm, (y+yOffset) * sm, oct, 0.5f);
        float ZX = fBM((z+zOffset) * sm, (x+xOffset) * sm, oct, 0.5f);

        return (XY + YZ + XZ + YX + ZY + ZX) / 6.0f;
    }

    static float Map(float newmin, float newmax, float origmin, float origmax, float value)
    {
        return Mathf.Lerp(newmin, newmax, Mathf.InverseLerp(origmin, origmax, value));
    }

    static float fBM(float x, float z, int oct, float pers)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        if(offset == 0f)
        {
            offset = Random.Range(-100000, 100000f);
        }
        for (int i = 0; i < oct; i++)
        {
            total += Mathf.PerlinNoise((x+offset) * frequency, (z+offset) * frequency) * amplitude;

            maxValue += amplitude;

            amplitude *= pers;
            frequency *= 2;
        }

        return total / maxValue;
    }
}
