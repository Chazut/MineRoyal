using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Utils
{

    static int maxHeight = 200;
    static float smooth = 0.01f;
    static int octaves = 4;
    static float persistence = 0.5f;

    public static string alpha_offset = "";
    public static float offset = 0f;
    public static string alpha_xOffset = "";
    public static float xOffset = 0f;
    public static string alpha_yOffset = "";
    public static float yOffset = 0f;
    public static string alpha_zOffset = "";
    public static float zOffset = 0f;

    public static string seed = "";

    private static int nbChar(string chaine, char lettre)
    {
        int nb = 0;
        foreach (char c in chaine)
        {
            if (c == lettre) nb++;
        }
        return nb;
    }

    private static string getStringAt(string chaine, int debut, int fin)
    {
        var builder = new StringBuilder();
        for (int i=debut; i<fin; i++)
        {
            builder.Append(chaine[i]);
        }
        return builder.ToString();
    }

    public static void generateSeed()
    {
        if(seed.Length != 0)
        {
            if (Utils.nbChar(seed, ':') != 3)
            {
                while (seed.Length < 10)
                {
                    seed += seed;
                }
                int nbr = (seed.Length + 1) / 4;
                for (int i = 0; i < seed.Length; i++)
                {
                    if (i < nbr)
                    {
                        alpha_offset += seed[i];
                    }
                    else if (i < nbr * 2)
                    {
                        alpha_xOffset += seed[i];
                    }
                    else if (i < nbr * 3)
                    {
                        alpha_yOffset += seed[i];
                    }
                    else
                    {
                        alpha_zOffset += seed[i];
                    }
                }
            }
            else
            {
                string str = seed;
                int index = str.IndexOf(':');
                alpha_offset = Utils.getStringAt(str, 0, index);

                str = Utils.getStringAt(str, index+1, str.Length);
                index = str.IndexOf(':');
                alpha_xOffset = Utils.getStringAt(str, 0, index);

                str = Utils.getStringAt(str, index+1, str.Length);
                index = str.IndexOf(':');
                alpha_yOffset = Utils.getStringAt(str, 0, index);

                str = Utils.getStringAt(str, index+1, str.Length);
                alpha_zOffset = str;
            }
        }
    }

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
            //xOffset = Random.Range(-100000f, 100000f);
            if(alpha_xOffset.Length == 0)
                alpha_xOffset = RandomString(Random.Range(3,10));
            xOffset = alpha_xOffset.GetHashCode() / 1000;
            while (Mathf.Abs(xOffset) > 250000)
            {
                xOffset = xOffset / 10;
            }
        }
        if (yOffset == 0f)
        {
            //yOffset = Random.Range(-100000f, 100000f);
            if (alpha_yOffset.Length == 0)
                alpha_yOffset = RandomString(Random.Range(3, 10));
            yOffset = alpha_yOffset.GetHashCode() / 1000;
            while(Mathf.Abs(yOffset) > 250000)
            {
                yOffset = yOffset / 10;
            }
        }
        if (zOffset == 0f)
        {
            //zOffset = Random.Range(-100000f, 100000f);
            if (alpha_zOffset.Length == 0)
                alpha_zOffset = RandomString(Random.Range(3, 10));
            zOffset = alpha_zOffset.GetHashCode() / 1000;
            while (Mathf.Abs(zOffset) > 250000)
            {
                zOffset = zOffset / 10;
            }
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
            if (alpha_offset.Length == 0)
                alpha_offset = RandomString(Random.Range(3, 10));
            offset = alpha_offset.GetHashCode() / 1000;
            while (Mathf.Abs(offset) > 250000)
            {
                offset = offset / 10;
            }
            //offset = Random.Range(-100000, 100000f);
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

    private static string RandomString(int length)
    {
        const string pool = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var builder = new StringBuilder();

        for (var i = 0; i < length; i++)
        {
            var c = pool[Random.Range(0, pool.Length)];
            builder.Append(c);
        }

        return builder.ToString();
    }
}
