using System.Collections.Generic;
using System.Linq;
using TerrainCollections;
using UnityEngine;

public class StepSoundData : Singleton<StepSoundData>
{
    public enum TypeOfMovement { None, Walk, Run, JumpLand, JumpStart }
    public enum Layers
    {
        Rock, Sand, Leaves, LeavesOld, Swamp,
        Grass, Moss, MossRock, DirtyGround, VeryDirtyGround, Tile, VeryLeaves, VeryTile, VeryGroundTile
    }
    public enum OnlyColliderLayer { Wood }
    private Dictionary<(TypeOfMovement type, int matIndex), List<AudioClip>> stepSounds;
    private readonly Dictionary<PhysicMaterial, int> indexFromMats = new Dictionary<PhysicMaterial, int>();
    private TerrainDetector terrainDetector;
    private Transform terrainTr;
    private List<AudioClip> LoadAsset(string l, TypeOfMovement type) =>
               Resources.LoadAll<AudioClip>($"Footsteps\\{l}\\{type}\\").ToList();
    private void Awake()
    {
        terrainDetector = new TerrainDetector(Terrain.activeTerrain);
        terrainTr = Terrain.activeTerrain.transform;
        stepSounds = new Dictionary<(TypeOfMovement type, int matIndex), List<AudioClip>>();
        int lastMatIndex = 0;
        for (int k = 0; k < System.Enum.GetNames(typeof(Layers)).Length; k++)
        {
            lastMatIndex = terrainDetector.GetIndexFromMaterial(Resources.Load<PhysicMaterial>($"PhysicMaterials\\{(Layers)k}"));
            for (int i = 1; i < 5; i++)
            {
                TypeOfMovement type = (TypeOfMovement)i;
                stepSounds.Add((type, lastMatIndex), LoadAsset(((Layers)k).ToString(), type));
            }
        }
        for (int k = 0; k < System.Enum.GetNames(typeof(OnlyColliderLayer)).Length; k++)
        {
            lastMatIndex++;
            indexFromMats.Add(Resources.Load<PhysicMaterial>($"PhysicMaterials\\{(OnlyColliderLayer)k}"), lastMatIndex);
            for (int i = 1; i < 5; i++)
            {
                TypeOfMovement type = (TypeOfMovement)i;
                stepSounds.Add((type, lastMatIndex), LoadAsset(((OnlyColliderLayer)k).ToString(), type));
            }
        }
    }
    internal bool ContainsKey((TypeOfMovement movementType, int physicMaterialIndex) key)
    {
        return stepSounds.ContainsKey(key);
    }
    public AudioClip GetClipFromIndex((TypeOfMovement movementType, int physicMaterialIndex) key)
    {
        var s = stepSounds[key];
        int index = Random.Range(0, s.Count);

        return s[index];
    }
    /// <summary>
    /// вызов если у террейна нет такого материала(дерево к примеру)
    /// </summary>
    /// <param name="physMat"></param>
    /// <returns></returns>
    private int GetIndexFromMaterial(PhysicMaterial physMat)
    {        
        if (physMat == null)
            return 0;
        return indexFromMats[physMat];
    }
    public TerrainDetector GetTerrainDetector() => terrainDetector;
    public int GetIndexFromRayCast(RaycastHit hit, Vector3 gmPosition)
    {        
        if (hit.transform != terrainTr)
        {
            var physMat = hit.collider.sharedMaterial;            
            var index = terrainDetector.GetIndexFromMaterial(physMat);


            if (index == -1)
                index = GetIndexFromMaterial(physMat);

            return index;
        }
        else
            return terrainDetector.GetActiveTerrainTextureIdx(gmPosition);
    }
}
