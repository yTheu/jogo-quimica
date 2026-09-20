using System.IO;
using UnityEditor;
using UnityEngine;

public static class SpriteExporter
{
    [MenuItem("Tools/Export Selected Sprite")]
    public static void ExportSelectedSprite()
    {
        Sprite sprite = Selection.activeObject as Sprite;

        if (sprite == null)
        {
            EditorUtility.DisplayDialog(
                "Sprite Exporter",
                "Selecione um Sprite no Project.",
                "OK"
            );

            return;
        }

        string texturePath =
            AssetDatabase.GetAssetPath(sprite.texture);

        TextureImporter importer =
            AssetImporter.GetAtPath(texturePath) as TextureImporter;

        if (importer == null)
            return;

        bool originalReadable = importer.isReadable;

        if (!originalReadable)
        {
            importer.isReadable = true;
            importer.SaveAndReimport();
        }

        Rect rect = sprite.rect;

        Texture2D extractedTexture = new Texture2D(
            Mathf.RoundToInt(rect.width),
            Mathf.RoundToInt(rect.height),
            TextureFormat.RGBA32,
            false
        );

        Color[] pixels = sprite.texture.GetPixels(
            Mathf.RoundToInt(rect.x),
            Mathf.RoundToInt(rect.y),
            Mathf.RoundToInt(rect.width),
            Mathf.RoundToInt(rect.height)
        );

        extractedTexture.SetPixels(pixels);
        extractedTexture.Apply();

        string savePath = EditorUtility.SaveFilePanel(
            "Export Sprite",
            Application.dataPath,
            sprite.name + ".png",
            "png"
        );

        if (!string.IsNullOrEmpty(savePath))
        {
            File.WriteAllBytes(
                savePath,
                extractedTexture.EncodeToPNG()
            );

            AssetDatabase.Refresh();
        }

        Object.DestroyImmediate(extractedTexture);

        if (!originalReadable)
        {
            importer.isReadable = false;
            importer.SaveAndReimport();
        }
    }
}