﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainCollections
{
    public class TerrainDetector_MaterialData : MonoBehaviour
    {
        public List<TerrainLayer> terrainLayers = new List<TerrainLayer>(14);
        public List<PhysicMaterial> physicMaterials = new List<PhysicMaterial>(14);
    }
}