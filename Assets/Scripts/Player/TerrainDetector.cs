using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainDetector_MaterialData", menuName = "ScriptableObjects/TerrainDetector_MaterialData", order = 1)]
public class TerrainDetector_MaterialData : ScriptableObject
{
    public List<TerrainLayer> terrainLayers = new List<TerrainLayer>(14);
    public List<PhysicMaterial> physicMaterials = new List<PhysicMaterial>(14);

}
public class TerrainDetector
{
    private TerrainData terrainData;
    private Terrain terrain; 
    private int alphamapWidth;
    private int alphamapHeight;
    private float[,,] splatmapData;
    private int numTextures;
    private TerrainDetector_MaterialData materialData;

    public TerrainDetector(Terrain t)
    {
        terrainData = t.terrainData;
        alphamapWidth = terrainData.alphamapWidth;
        alphamapHeight = terrainData.alphamapHeight;

        splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);
        materialData = Resources.Load<TerrainDetector_MaterialData>("PhysicMaterials\\TerrainData\\TerrainDetector_MaterialData");
        terrain = t;
    }
    
    private Vector3 ConvertToSplatMapCoordinate(Vector3 worldPosition)
    {
        Vector3 splatPosition = new Vector3();        
        Vector3 terPosition = terrain.transform.position;
        splatPosition.x = (worldPosition.x - terPosition.x) / terrain.terrainData.size.x * terrain.terrainData.alphamapWidth;
        splatPosition.z = (worldPosition.z - terPosition.z) / terrain.terrainData.size.z * terrain.terrainData.alphamapHeight;
        return splatPosition;
    }

    public int GetActiveTerrainTextureIdx(Vector3 position)
    {
        Vector3 terrainCord = ConvertToSplatMapCoordinate(position);
        int activeTerrainIndex = 0;
        float largestOpacity = 0f;

        for (int i = 0; i < numTextures; i++)
        {
            if (largestOpacity < splatmapData[(int)terrainCord.z, (int)terrainCord.x, i])
            {
                activeTerrainIndex = i;
                largestOpacity = splatmapData[(int)terrainCord.z, (int)terrainCord.x, i];
            }
        }

        return activeTerrainIndex;
    }

    internal int GetIndexFromMaterial(PhysicMaterial sharedMaterial)
    {
        for (int i = 0; i < materialData.physicMaterials.Count; i++)
        {
            if (sharedMaterial == materialData.physicMaterials[i])
                return i;
        }
        return 0;
    }
}