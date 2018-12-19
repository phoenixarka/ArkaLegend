using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour {

    [SerializeField]
    private Texture2D[] mapData;

    [SerializeField]
    private MapElement[] mapElements;

    [SerializeField]
    private Transform map;

    [SerializeField]
    private Sprite defaultTile;

    [SerializeField]
    private SpriteAtlas waterAtlas;

    private Vector3 worldStartpos
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        }
    }

    private Dictionary<string, MapElement> color2MapElement;

	// Use this for initialization
	void Start () {
        color2MapElement = new Dictionary<string, MapElement>();

        foreach (MapElement mapElement in mapElements){
            color2MapElement.Add(ColorUtility.ToHtmlStringRGB(mapElement.ElementColor), mapElement);
        }
        GenerateMap();
	}
	

    private void GenerateMap() {
        for (int i = 0; i < mapData.Length; i++) {
            for (int x = 0; x < mapData[i].width; x++) {
                for (int y = 0; y < mapData[i].height; y++) {
                    string cVal = ColorUtility.ToHtmlStringRGB(mapData[i].GetPixel(x,y));
                    MapElement newElement = null;
                    color2MapElement.TryGetValue(cVal, out newElement);
                    
                    if (newElement != null)
                    {
                        float xPos = worldStartpos.x + (defaultTile.bounds.size.x * x);
                        float yPos = worldStartpos.y + (defaultTile.bounds.size.y * y);

                        GameObject go = Instantiate(newElement.ElementPrefab);
                        go.transform.position = new Vector2(xPos,yPos);

                        if (newElement.ElementTileTag == "tree") {
                            go.GetComponent<SpriteRenderer>().sortingOrder = (mapData[i].height - y) * 2;
                        }

                        if (newElement.ElementTileTag == "water") {
                            go.GetComponent<SpriteRenderer>().sortingOrder = i * 2;
                            string compsition = TileCheck(x, y, mapData[i], cVal);
                            go.name = "water" + x + "_" + y;
                            Debug.Log(go.name + "|" +compsition);
                            //AssignWater1(compsition, go);
                            AssignWater(compsition, go);
                        }
                        go.transform.parent = map;
                    }
                        
                }
            }
        }
    }

    private void AssignWater(string compsition, GameObject currObj)
    {
        /*
         *    0   1  2    3   4    5    6    7    8     9   10  11   12
         *    /= -v- =\  //=  =\\  |=   +   =|    \\=  =//  \=  -^-   =/
         *   39  40  41  42  43    54   55  56    57   58   69   70   71 
         */


        Regex[] waterTileRegex = new Regex[] {new Regex(@"00.01.11"), new Regex(@".0.11111"), new Regex(@".001011."),
                                              new Regex(@"01111111"), new Regex(@"11011111"),
                                              new Regex(@".1101.11"), new Regex(@"11111111"), new Regex(@"11.1011."),
                                              new Regex(@"11111011"), new Regex(@"11111110"),
                                              new Regex(@".110100."), new Regex(@"11111.0."), new Regex(@"11.10.00"),
                                              };

        // /= -v- =\
        if (waterTileRegex[0].IsMatch(compsition))
        {
            currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("base_lands_39");
        }
        else if (waterTileRegex[1].IsMatch(compsition))
        {
            currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("base_lands_40");
        }
        else if (waterTileRegex[2].IsMatch(compsition))
        {
            currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("base_lands_41");
        }

        // //=  =\\
        else if (waterTileRegex[3].IsMatch(compsition))
        {
            currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("base_lands_42");
        }
        else if (waterTileRegex[4].IsMatch(compsition))
        {
            currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("base_lands_43");
        }

        // |=   +   =| 
        else if (waterTileRegex[5].IsMatch(compsition))
        {
            currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("base_lands_54");
        }
        else if (waterTileRegex[6].IsMatch(compsition))
        {
            int rand = UnityEngine.Random.Range(0, 100);
            if (rand < 10)
            {
                currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("modified_water_10");
            }
            else {
                currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("base_lands_55");
            }
        }
        else if (waterTileRegex[7].IsMatch(compsition))
        {
            currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("base_lands_56");
        }

        // \\=  =//
        else if (waterTileRegex[8].IsMatch(compsition))
        {
            currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("base_lands_57");
        }
        else if (waterTileRegex[9].IsMatch(compsition))
        {
            currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("base_lands_58");
        }

        // \=  -^-  =/
        else if (waterTileRegex[10].IsMatch(compsition))
        {
            currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("base_lands_69");
        }
        else if (waterTileRegex[11].IsMatch(compsition))
        {
            currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("base_lands_70");
        }
        else if (waterTileRegex[12].IsMatch(compsition))
        {
            currObj.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("base_lands_71");
        }
    }

    private string TileCheck(int x, int y,Texture2D currMapData,string colorVal) {
        /*  Seq of Composition
         *  0 1 2
         *  3   4 
         *  5 6 7 
         */

        string composition = "";
        for(int j = 1; j >= -1; j--)
        {
            for(int i = -1; i <= 1; i++){
                if (i == 0 && j == 0) {
                    continue;
                }
                composition += (ColorUtility.ToHtmlStringRGB(currMapData.GetPixel(x+i,y+j)) == colorVal) ? "1" : "0";
            }
        }
        return composition;
    }
}

[Serializable]
public class MapElement {
    [SerializeField]
    private string elementTileTag;

    [SerializeField]
    private Color elementColor;

    [SerializeField]
    private GameObject elementPrefab;

    public string ElementTileTag
    {
        get
        {
            return elementTileTag;
        }
    }

    public Color ElementColor
    {
        get
        {
            return elementColor;
        }
    }
    public GameObject ElementPrefab
    {
        get
        {
            return elementPrefab;
        }
    }
}
