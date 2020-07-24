using UnityEngine;

/// <summary> This class uses dark magic for updating the dirt-background under the drills. </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour
{
    #region Variables
    public int width, height;//=pixeldichte
    public Texture2D tex;
    //ppu = 10+ Scale = 10+; width = 10- => faktor = 10
    public int pWidth, pHeight;
    public GameObject player;
    Color32[] digged, empty;
    public int numb;
    private Sprite sprite;
    public Texture2D[] sprites;
    private int fieldWidth, fieldHeight;
    #endregion

    #region Initialization
    void Awake()
    {
        if (tex == null)
        {
            Debug.LogError("could not find texture");
        }

        fieldWidth = width * tex.width;
        fieldHeight = height * tex.height;
        sprite = GetComponent<SpriteRenderer>().sprite;
        empty = new Color32[pWidth * pHeight];
        for (int i = 0; i < pWidth * pHeight; i++)
        {
            empty[i] = new Color32(0, 0, 0, 0);
        }

        Init();
    }

    public void Init()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                sprite.texture.SetPixels32(x * tex.width, y * tex.height, tex.width, tex.height, tex.GetPixels32());
            }
        }

        if (sprites.Length > 0)
        {
            int anz = Random.Range(1, 5);
            int[] xPos = new int[anz];
            int[] ypos = new int[anz];
            int[] xVal = new int[anz];
            int[] yVal = new int[anz];

            for (int i = 0; i < anz; i++)
            {
                bool ok = false;
                int typ = Random.Range(0, sprites.Length);
                int x, y;
                do
                {
                    x = Random.Range(0, fieldWidth - sprites[typ].width);
                    y = Random.Range(tex.height, fieldHeight - sprites[typ].height);
                    for (int j = 0; j < i; j++)
                    {
                        if (x > xPos[j] - sprites[typ].width && x < xPos[j] + xVal[j] && y > ypos[j] - yVal[j] && y < ypos[j] + sprites[typ].height)
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

                xPos[i] = x; ypos[i] = y;
                xVal[i] = sprites[typ].width; yVal[i] = sprites[typ].height;
                sprite.texture.SetPixels32(x, y, sprites[typ].width, sprites[typ].height, sprites[typ].GetPixels32());
            }
        }
        sprite.texture.Apply(true);
    }
    #endregion

    void Update()
    {
        if (numb % 10 == 1)
        {
            Vector3 pos = player.transform.position + ((numb / 10 == 1) ? Vector3.zero : (Vector3.left * 32));
            pos.y = Mod(pos.y, 32);

            int posX = (int)(20 * pos.x) - pWidth / 2, posY = (int)(20 * pos.y) - pHeight / 2;
            if ((int)(20 * pos.x) - pWidth / 2 < 0)
            {
                posX = 0;
            }
            else if ((int)(20 * pos.x) + pWidth / 2 > tex.width * width)
            {
                posX = tex.width * width - pWidth;
            }

            if ((int)(20 * pos.y) - pHeight / 2 < 0)
            {
                posY = 0;
            }
            else if ((int)(20 * pos.y) + pHeight / 2 > tex.height * height)
            {
                posY = tex.height * height - pHeight;
            }

            sprite.texture.SetPixels32(posX, posY, pWidth, pHeight, empty);
            sprite.texture.Apply();
        }
    }

    /// <summary> Calculates the positive modulo (a%b) </summary>
    /// <returns> The positive modulo </returns>
    private float Mod(float a, int b)
    {
        return (a % b + b) % b;
    }

    /// <summary> Resets the displayed texture. </summary>
    public void RemoveShit()
    {
        for (int x = 0; x < width; x++)
        {
            sprite.texture.SetPixels32(x * tex.width, 0, tex.width, tex.height, tex.GetPixels32());
        }
        sprite.texture.Apply(true);
    }

    #region Unused code
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
    #endregion
}