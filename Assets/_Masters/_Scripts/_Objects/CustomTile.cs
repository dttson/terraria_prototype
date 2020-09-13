#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Tilemaps;
#endif

using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomTile : Tile
{
    public bool isSpring = true;
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.color = Color.black;
        tileData.gameObject.name = "spring";
    }

#if UNITY_EDITOR
    [CreateTileFromPalette]
    public static void getCustomTile()
    {

    }

    [MenuItem("Assets/Create/Custom Tile")]
    public static void CreateAnimatedTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Custom Tile", "New Custom Tile", "asset", "Save Destructable Tile", "Assets");
        if (path == "")
            return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CustomTile>(), path);
    }
#endif
}