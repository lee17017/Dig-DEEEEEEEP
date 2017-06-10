using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour {
    
    public int width, height;//=pixeldichte
    public Texture2D tex;
    //ppu = 10+ Scale = 10+; width = 10- => faktor = 10
    public int pWidth, pHeight;
    public GameObject player;
    Color32[] digged, empty;
    bool once = true;
    public int numb;
    Sprite sprite;
    public Texture2D[] sprites;

    public void removeShit()
    {
        Debug.Log(numb);
        Debug.Log("A: " + height / 3 * 2);
        Debug.Log(height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                sprite.texture.SetPixels32(x * tex.width, y * tex.height, tex.width, tex.height, tex.GetPixels32());
            }
        }
        sprite.texture.Apply(true);
    }
    public void init()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                sprite.texture.SetPixels32(x * tex.width, y * tex.height, tex.width, tex.height, tex.GetPixels32());
            }
        }

        Debug.Log(sprites);
        if (sprites.Length > 0)
        {
            int anz = UnityEngine.Random.Range(1, 5);
            int[] xpos = new int[anz];
            int[] ypos = new int[anz];
            Debug.Log("anz" + anz);

            for (int i = 0; i < anz; i++)
            {
                bool ok = false;
                int x, y;
                do
                {
                    x = (int)UnityEngine.Random.Range(0, width * tex.width - sprites[0].width);
                    y = (int)UnityEngine.Random.Range(0, height * tex.height - sprites[0].height);
                    for (int j = 0; j < i; j++)
                    {
                        if (x > xpos[j] - sprites[0].width && x < xpos[j] + sprites[0].width && y > ypos[j] - sprites[0].height && y < ypos[j] + sprites[0].height)
                        {
                            ok = true;
                            break;
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                } while (ok);
                Debug.Log(i + ": x" + x + "- y" + y);
                xpos[i] = x; ypos[i] = y;
                sprite.texture.SetPixels32(x, y, sprites[0].width, sprites[0].height, sprites[0].GetPixels32());


            }
        }
        sprite.texture.Apply(true);
    }
    void Awake()
    {


        if (tex == null)
            Debug.LogError("could not find texture");

        sprite = GetComponent<SpriteRenderer>().sprite;
        sprite.texture.Resize(width * tex.width, height * tex.height);
        Debug.Log(width * tex.width);
        empty = new Color32[pWidth * pHeight];
        for (int i = 0; i < pWidth * pHeight; i++)
        {
            empty[i] = new Color32(0, 0, 0, 0);
        }

        init();
        
    }
    float mod(float a, int b)
    {
        return (a % b + b) % b;
    }
    void Update()
    {



        if (numb % 10 == 1)
        { 
            Vector3 pos;
            if (numb / 10 == 1)
                pos = player.transform.position;
            else
                pos = player.transform.position + Vector3.left * 32;

            pos.y = mod(pos.y, 32);

            int posX= (int)(20 * pos.x) - pWidth / 2, posY= (int)(20 * pos.y) - pHeight / 2;
            if ((int)(20 * pos.x) - pWidth / 2 < 0)
                posX = 0;
            else if ((int)(20 * pos.x) + pWidth / 2 > tex.width*width)
                posX = tex.width*width-pWidth;

            if ((int)(20 * pos.y) - pHeight / 2 < 0)
                posY = 0;
            else if ((int)(20 * pos.y) + pHeight / 2 > tex.height * height)
                posY = tex.height * height - pHeight;

            sprite.texture.SetPixels32(posX, posY, pWidth, pHeight, empty);
            sprite.texture.Apply();
        }
    }
   /* void OnApplicationQuit()
    {
        sprite.texture.Resize(tex.width, tex.height);
        sprite.texture.SetPixels32(tex.width, tex.height, tex.width, tex.height, tex.GetPixels32());
    }*/
    /*SpriteRenderer rend = GetComponent<SpriteRenderer>();
    Texture2D dest = rend.sprite.texture;





    var cols = dest.GetPixels32(0);
    for (var i = 0; i < cols.Length; ++i)
    {
        cols[i] = new Color32(cols[i].r, cols[i].b, cols[i].g, 0);
        Debug.Log(cols[i]);
    }
    dest.SetPixels32(0,0,20,20,cols, 0);
    */

    /*
    Color32[] pix = input.GetPixels32();
    var colors = new Color32[3];
    colors[0] = Color.red;
    colors[1] = Color.green;
    colors[2] = Color.blue;
    var mipCount = Mathf.Min(3, dest.mipmapCount);

    // tint each mip level
    for (var mip = 0; mip < mipCount; ++mip)
    {
        var cols = dest.GetPixels32(mip);
        for (var i = 0; i < cols.Length; ++i)
        {
            cols[i] = Color32.Lerp(cols[i], colors[mip], 0.33f);
            Debug.Log(cols[i]);
        }
        dest.SetPixels32(cols, mip);
    }*/

}
