using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile16x16 : Tile
{
    /// <summary>
    /// An array with all the waterTiles that we have in our game
    /// </summary>
    [SerializeField]
    private Sprite[] tileSprites;

    //A preview of the tile
    [SerializeField]
    private Sprite preview;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        return base.StartUp(position, tilemap, go);
    }

    /// <summary>
    /// Refreshes this tile when something changes
    /// </summary>
    /// <param name="position">The tiles position in the grid</param>
    /// <param name="tilemap">A reference to the tilemap that this tile belongs to.</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int y = -1; y <= 1; y++) //Runs through all the tile's neighbours 
        {
            for (int x = -1; x <= 1; x++)
            {
                //We store the position of the neighbour 
                Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);

                if ( HasTile(tilemap, nPos)) //If the neighbour has water on it
                {
                    tilemap.RefreshTile(nPos); //Them we make sure to refresh the neighbour aswell
                }
            }
        }
    }

    /// <summary>
    /// Changes the tiles sprite to the correct sprites based on the situation
    /// </summary>
    /// <param name="location">The location of this sprite</param>
    /// <param name="tilemap">A reference to the tilemap, that this tile belongs to</param>
    /// <param name="tileData">A reference to the actual object, that this tile belongs to</param>
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = preview;

        /*
        *    0   1  2    3   4    5    6    7    8     9   10  11   12    13
        *    /= -v- =\  //=  =\\  |=   +   =|    \\=  =//  \=  -^-   =/   +(special)
        */

        string composition = string.Empty;//Makes an empty string as compostion, we need this so that we change the sprite

        for (int j = 1; j >= -1; j--)
        {
            for (int i = -1; i <= 1; i++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                composition += HasTile(tilemap, new Vector3Int(location.x + i, location.y + j, location.z)) ? "1" : "0";
            }
        }

        Regex[] waterTileRegex = new Regex[] {new Regex(@"00.01.11"), new Regex(@".0.11111"), new Regex(@".001011."),
                                              new Regex(@"01111111"), new Regex(@"11011111"),
                                              new Regex(@".1101.11"), new Regex(@"11111111"), new Regex(@"11.1011."),
                                              new Regex(@"11111011"), new Regex(@"11111110"),
                                              new Regex(@".110100."), new Regex(@"11111.0."), new Regex(@"11.10.00"),
                                              };
        
        // /= -v- =\
        if (waterTileRegex[0].IsMatch(composition))
        {
            tileData.sprite = tileSprites[0];
        }
        else if (waterTileRegex[1].IsMatch(composition))
        {
            tileData.sprite = tileSprites[1];
        }
        else if (waterTileRegex[2].IsMatch(composition))
        {
            tileData.sprite = tileSprites[2];
        }

        // //=  =\\
        else if (waterTileRegex[3].IsMatch(composition))
        {
            tileData.sprite = tileSprites[3];
        }
        else if (waterTileRegex[4].IsMatch(composition))
        {
            tileData.sprite = tileSprites[4];
        }

        // |=   +   =| 
        else if (waterTileRegex[5].IsMatch(composition))
        {
            tileData.sprite = tileSprites[5];
        }
        else if (waterTileRegex[6].IsMatch(composition))
        {
            int rand = UnityEngine.Random.Range(0, 100);
            if (rand < 10)
            {
                tileData.sprite = tileSprites[13];
            }
            else
            {
                tileData.sprite = tileSprites[6];
            }
        }
        else if (waterTileRegex[7].IsMatch(composition))
        {
            tileData.sprite = tileSprites[7];
        }

        // \\=  =//
        else if (waterTileRegex[8].IsMatch(composition))
        {
            tileData.sprite = tileSprites[8];
        }
        else if (waterTileRegex[9].IsMatch(composition))
        {
            tileData.sprite = tileSprites[9];
        }

        // \=  -^-  =/
        else if (waterTileRegex[10].IsMatch(composition))
        {
            tileData.sprite = tileSprites[10];
        }
        else if (waterTileRegex[11].IsMatch(composition))
        {
            tileData.sprite = tileSprites[11];
        }
        else if (waterTileRegex[12].IsMatch(composition))
        {
            tileData.sprite = tileSprites[12];
        }
    }

    private bool HasTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/My16x16Tiles")]
    public static void CreateWaterTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save16x16Tile", "New 16x16Tile", "asset", "Save 16x16Tile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Tile16x16>(), path);
    }

#endif
}
