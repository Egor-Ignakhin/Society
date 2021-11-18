using System.Collections.Generic;

using UnityEngine;
namespace Society.Player.TerrainCollections
{
    public class TerrainDetector
    {
        private readonly TerrainData terrainData;
        private readonly Terrain terrain;
        private readonly Transform terrainTr;
        private readonly int alphamapWidth;
        private readonly int alphamapHeight;
        private readonly float[,,] splatmapData;
        private readonly int numTextures;
        private readonly List<PhysicMaterial> physicMaterials;

        public TerrainDetector(Terrain t)
        {
            if (t)
            {
                terrainData = t.terrainData;
                alphamapWidth = terrainData.alphamapWidth;
                alphamapHeight = terrainData.alphamapHeight;

                splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
                numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);
            }
            physicMaterials = Object.Instantiate(Resources.Load<TerrainDetector_MaterialData>("PhysicMaterials\\TerrainData\\TerrainDetector_MaterialData")).physicMaterials;
            if (t)
            {
                terrain = t;
                terrainTr = terrain.transform;
            }
        }

        private Vector3 ConvertToSplatMapCoordinate(Vector3 worldPosition, Transform terrainTr)
        {
            Vector3 splatPosition = new Vector3();
            Vector3 terPosition = terrainTr.position;
            splatPosition.x = (worldPosition.x - terPosition.x) / terrainData.size.x * terrainData.alphamapWidth;
            splatPosition.z = (worldPosition.z - terPosition.z) / terrainData.size.z * terrainData.alphamapHeight;
            return splatPosition;
        }

        public int GetActiveTerrainTextureIdx(Vector3 position)
        {
            Vector3 terrainCord = ConvertToSplatMapCoordinate(position, terrainTr);
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
            for (int i = 0; i < physicMaterials.Count; i++)
            {
                if (sharedMaterial == physicMaterials[i])
                    return i;
            }
            return -1;
        }
    }
}