﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour {
    
    public int width, height;
    public Texture2D tex, tex2;
    public int pWidth, pHeight;
    public GameObject player;
    Color32[] digged, empty;
    bool once = true;
    Sprite sprite;
    void Awake()
    {

        if (tex == null)
            Debug.LogError("could not find texture");

        int targetY = height / 2;
        int targetX = width / 2;

        sprite = GetComponent<SpriteRenderer>().sprite;
        sprite.texture.Resize(targetX * tex.width, targetY * tex.height);
        Debug.Log(targetX * tex.width);
        empty = new Color32[pWidth*pHeight];
        for (int i = 0; i < pWidth * pHeight; i++)
        {
            empty[i] = new Color32(0, 0, 0, 0);
        }
        
        for (int x = 0; x < targetX; x++)
        {
            for (int y = 0; y < targetY; y++)
            {
                sprite.texture.SetPixels32(x * tex.width, y * tex.height, tex.width, tex.height, tex.GetPixels32());
            }
        }
        digged = tex2.GetPixels32();
        sprite.texture.SetPixels32(tex.width, tex.height, tex.width, tex.height, digged);
        sprite.texture.Apply(true);
        
    }

    void Update()
    {

        
            Vector3 pos = player.transform.position;
        try
        {
            sprite.texture.SetPixels32((int)(10 * pos.x) - pWidth / 2, (int)(10 * pos.y) - pHeight / 2, pWidth, pHeight, empty);
            sprite.texture.Apply();
        }
        catch (Exception e)
        {
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
