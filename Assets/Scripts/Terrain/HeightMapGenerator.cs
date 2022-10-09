using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Terrain
{
    using Terrain = UnityEngine.Terrain;

    [RequireComponent(typeof(Terrain))]
    [RequireComponent(typeof(TerrainCollider))]
    public class HeightMapGenerator : MonoBehaviour
    {
        /// <summary>
        /// Debug flags
        /// </summary>
        [Header("Debug flags")]
        [SerializeField] private bool isRandom;
        [SerializeField] private bool isBaseMapCalculated;
        [SerializeField] private bool areMountainsIncluded;
        [SerializeField] private bool areLoneMountainsIncluded;
        [SerializeField] private bool isSpawnAreaSet;
        [SerializeField] private bool isBlurApplied;
        [SerializeField] private bool isFineNoiseIncluded;

        /// <summary>
        /// Parameters to modify the Perlin noise of the base heightMap
        /// </summary>
        [Header("Terrain Base Generation")] 
        [Range(0, 10)] [Tooltip("")] [SerializeField] private int baseNumberOfOctaves;
        [Range(0, 4)] [Tooltip("")] [SerializeField] private float basePersistence;
        [Range(0, 4)] [Tooltip("")] [SerializeField] private float baseLacunarity;
        [Range(0, 64)] [Tooltip("")] [SerializeField] private float baseOffsetY;
        [Range(0, 10)] [Tooltip("")] [SerializeField] private float baseScale;

        /// <summary>
        /// To modify mountain params
        /// </summary>
        [Header("Terrain Wall-like Mountains Generation")]
        [SerializeField] [Range(0, 1)] private float mountainWidth;
        [SerializeField] [Range(0, 1)] private float mountainLevel;
        [SerializeField] [Range(0, 10)] private float mountainNoiseScale;
        [SerializeField] [Range(0, 10)] private float mountainNoiseFreq;
        [SerializeField] [Range(0, 50)] private float mountainMaxHeight;
        [SerializeField] private bool isRedistributedMountain;
        [SerializeField] [Range(0.01f, 10)] private float redistributionExponentMountain;

        /// <summary>
        /// To modify mountain params
        /// </summary>
        [Header("Terrain Lone Mountains Generation")]
        [SerializeField] [Range(0, 1)] private float loneMountainLevel;
        [SerializeField] [Range(0, 10)] private float loneMountainNoiseScale;
        [SerializeField] [Range(0, 10)] private float loneMountainNoiseFreq;
        [SerializeField] [Range(0, 50)] private float loneMountainMaxHeight;
        [SerializeField] private bool isRedistributedLoneMountain;
        [SerializeField] [Range(0.01f, 10)] private float redistributionExponentLoneMountain;
        

        /// <summary>
        /// There is two predefined spawnPoint on the map, where the two players will start
        /// </summary>
        [Header("Spawn point parameters")]
        [SerializeField] [Range(0, 80)] private float radiusOfSpawnArea;
        [SerializeField] private List<Transform> spawnPoints;
        
        [Header("Fine noise parameters")]
        [Range(5, 12)] [Tooltip("")] [SerializeField] private int fineNoiseNumberOfOctaves;
        [Range(0, 1)] [Tooltip("")] [SerializeField] private float fineNoisePersistence;
        [Range(0, 1)] [Tooltip("")] [SerializeField] private float fineNoiseLacunarity;
        [Range(1, 100)] [Tooltip("")] [SerializeField] private float fineNoiseScale;
        [Range(0, 1)] [Tooltip("")] [SerializeField] private float fineNoiseHeight;

        [Header("Gaussian Blur to smoothen heightMap")]
        [Range(1, 30)] [Tooltip("")] [SerializeField] private int blurRadius;


        /// <summary>
        /// Saved references to the terrain components
        /// </summary>
        [Header("Added self contained components")]
        [SerializeField] private Terrain terrain;
        [SerializeField] private TerrainCollider terrainCollider;

        /// <summary>
        /// Size of the map, is set in the InitSizes() function
        /// </summary>
        private const int Width = 512; // x
        private const int Height = 128; // y
        private const int Length = 512; // z
        
        /// <summary>
        /// Feed for random function
        /// </summary>
        private const float MaximumValueForRandomRange = 1000.0f;

        /// <summary>
        /// Randomly generated offsets to be able to generate "always" different terrains
        /// </summary>
        private float _baseRandomOffsetX;
        private float _baseRandomOffsetZ;
        private float _mountainRandomOffsetX;
        private float _mountainRandomOffsetZ;
        
        /// <summary>
        /// Action which can be invoked from the outside to trigger callbacks
        /// </summary>
        public UnityAction onMapGeneration;
        private void SetCallbackToEvent()
        {
            onMapGeneration += GenerateHeightMap;
        }
        private void Awake()
        {
            SetCallbackToEvent();
        }
        
        /// <summary>
        /// Generates every aspect of the heightMap depending on the debug flags
        /// </summary>
        [ContextMenu("GenerateHeightMap")]
        private void GenerateHeightMap()
        {
            // Initialize the terrainData
            TerrainData localTerrainData = InitializeTerrainData(terrain.terrainData);
            
            // Heightmap to store the local calculated values
            float[,] heights = new float[Width, Length];

            // Here starts the generation pipeline
            // 
            // Generate random seed
            if (isRandom)
            {
                // Base
                _baseRandomOffsetX = Random.Range(0, MaximumValueForRandomRange);
                _baseRandomOffsetZ = Random.Range(0, MaximumValueForRandomRange);

                // Mountains
                _mountainRandomOffsetX = Random.Range(0, MaximumValueForRandomRange);
                _mountainRandomOffsetZ = Random.Range(0, MaximumValueForRandomRange);
            }
            else
            {
                // Base
                _baseRandomOffsetX = 0;
                _baseRandomOffsetZ = 0;
                
                // Mountains
                _mountainRandomOffsetX = 0;
                _mountainRandomOffsetZ = 0;
            }
            
            // Calculate the base heightMap of the terrain
            if (isBaseMapCalculated)
            {
                heights = CalculateBaseTerrain(heights);
            }
            
            // Calculate the additive mountain heightMap of the terrain
            if (areMountainsIncluded)
            {
                heights = CalculateMountains(heights);
            }

            // Calculate the additive lone mountain heightMap of the terrain
            if (areLoneMountainsIncluded)
            {
                heights = CalculateLoneMountains(heights);
            }
            
            // Calculate every spawn area height
            if (isSpawnAreaSet)
            {
                foreach (var spawnPoint in spawnPoints)
                {
                    var pointPosition = spawnPoint.transform.localPosition;
                    float averageHeight = CalculateAverageHeight(
                        heights,
                        pointPosition
                    );
                    heights = SetHeightAroundSpawnPoint(
                        heights,
                        pointPosition,
                        averageHeight
                    );
                }
            }
            
            // Calculate smoothed heightMaps
            if (isBlurApplied)
            {
                heights = ApplyBlur(heights);
            }
            
            // Calculate fine noise over everything
            if (isFineNoiseIncluded)
            {
                heights = CalculateFineNoiseOverTerrain(heights);
            }
            
            //
            // End of generation pipeline
            
            // Load calculated data to localTerrainData object
            localTerrainData.SetHeights(0, 0, heights);
            
            // Load calculated data to terrain component references
            terrain.terrainData = localTerrainData;
            terrainCollider.terrainData = localTerrainData;
            
            // Apply
            terrain.Flush();
        }
        
        // Generation pipeline functions
        //
        private TerrainData InitializeTerrainData(TerrainData terrainData)
        {
            terrainData.size = new Vector3(Width, Height, Length);
            terrainData.SetDetailResolution(Width, 32);
            terrainData.baseMapResolution = Width;
            terrainData.heightmapResolution = Width + 1;
            return terrainData;
        }
        private float[,] CalculateBaseTerrain(float[,] heights)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Length; z++)
                {
                    float y = 0;
                    float amp = 1.0f;
                    float freq = 1.0f;
                    for (int i = 0; i < baseNumberOfOctaves; i++)
                    {
                        float xCoord = (float)x / Width * baseScale * freq;
                        float zCoord = (float)z / Length * baseScale * freq;
                        if (isRandom)
                        {
                            xCoord += _baseRandomOffsetX;
                            zCoord += _baseRandomOffsetZ;
                        }
                        var noiseFloat = Mathf.PerlinNoise(xCoord, zCoord) - 0.5f; // - 0.5f so the values are between [0.5f, -0.5f]
                        y += noiseFloat * amp;
                        amp *= basePersistence;
                        freq *= baseLacunarity;
                    }
                    heights[x, z] = y + baseOffsetY / Height;
                }
            }
            return heights;
        }
        private float[,] CalculateMountains(float[,] heights)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Length; z++)
                {
                    // float y = 0;
                
                    float xCoord = (float)x / Width * mountainNoiseScale * mountainNoiseFreq;
                    float zCoord = (float)z / Length * mountainNoiseScale * mountainNoiseFreq;
                    if (isRandom)
                    {
                        xCoord += _mountainRandomOffsetX;
                        zCoord += _mountainRandomOffsetZ;
                    }
                    float calculatedNoise = Mathf.PerlinNoise(xCoord, zCoord);
                    if (calculatedNoise > (mountainLevel - mountainWidth / 2) &&
                        calculatedNoise < (mountainLevel + mountainWidth / 2)
                       )
                    {
                        float finalValue;
                        if (isRedistributedMountain)
                        {
                            finalValue = Mathf.Pow(calculatedNoise, redistributionExponentMountain) * mountainMaxHeight / Height;
                        }
                        else
                        {
                            finalValue = mountainMaxHeight / Height;
                        }
                        heights[x, z] += /*calculatedNoise **/ finalValue;
                    }
                }
            }
            return heights;
        }
        private float[,] CalculateLoneMountains(float[,] heights)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Length; z++)
                {
                    // float y = 0;
                
                    float xCoord = (float)x / Width * loneMountainNoiseScale * loneMountainNoiseFreq;
                    float zCoord = (float)z / Length * loneMountainNoiseScale * loneMountainNoiseFreq;
                    if (isRandom)
                    {
                        xCoord += _mountainRandomOffsetX;
                        zCoord += _mountainRandomOffsetZ;
                    }
                    float calculatedNoise = Mathf.PerlinNoise(xCoord, zCoord);

                    var finalValue = 0f;
                    if (isRedistributedLoneMountain)
                    {
                        finalValue = Mathf.Pow(calculatedNoise, redistributionExponentLoneMountain) * loneMountainMaxHeight / Height;
                    }
                    else
                    {
                        if (calculatedNoise > loneMountainLevel)
                        {
                            finalValue = calculatedNoise * loneMountainMaxHeight / Height;
                        }
                    }
                    heights[x, z] += finalValue;
                    
                }
            }
            return heights;
        }
        private float CalculateAverageHeight(
            float[,] heightMap,
            Vector3 spawnPoint
        )
        {
            // Return value we pass at the add
            var average = 0f;
            int measurementPoints = 0;
            
            // Setting the centre coordinates on the global system
            var xCen = (int)spawnPoint.x;
            var zCen = (int)spawnPoint.z;
            Vector2 centerVec2 = new Vector2(xCen, zCen);

            // Iterating over the examined area
            for (float xLoc = xCen - radiusOfSpawnArea; xLoc < xCen + radiusOfSpawnArea; xLoc++)
            {
                for (float zLoc = zCen - radiusOfSpawnArea; zLoc < zCen + radiusOfSpawnArea; zLoc++)
                {
                    // Local vector where we actually are at a given iteration respect to the global, as the center vector
                    var movingVec2 = new Vector2(xLoc, zLoc);
                    
                    // Distance calculation to check if we are inside
                    float distanceFromCenter = (centerVec2 - movingVec2).magnitude;
                    if (distanceFromCenter < radiusOfSpawnArea)
                    {
                        // Actual measurement
                        average += heightMap[(int)xLoc, (int)zLoc];
                        measurementPoints++;
                    }
                }
            }
            return (average / measurementPoints);
        }
        private float[,] SetHeightAroundSpawnPoint(
            float[,] heightMap,
            Vector3 spawnPoint,
            float heightToSetTo
        )
        {
            // Set center coordinates on the global system
            var xCen = (int)spawnPoint.x;
            var zCen = (int)spawnPoint.z;
            Vector2 centerVec2 = new Vector2(xCen, zCen);

            // Iterating over the examined area
            for (float xLoc = xCen - radiusOfSpawnArea; xLoc < xCen + radiusOfSpawnArea; xLoc++)
            {
                for (float zLoc = zCen - radiusOfSpawnArea; zLoc < zCen + radiusOfSpawnArea; zLoc++)
                {
                    // Local Vec2 to simplify distance calculation
                    var movingVec2 = new Vector2(xLoc, zLoc);
                    
                    // calculated distance between the two vectors
                    float distanceFromCenter = (centerVec2 - movingVec2).magnitude;
                    if (distanceFromCenter < radiusOfSpawnArea)
                    {
                        // Overwriting the value if its in the radius to the average height of the 
                        heightMap[(int)xLoc, (int)zLoc] = heightToSetTo;
                    }
                }
            }
            return heightMap;
        }
        private float[,] ApplyBlur(
            float[,] heights
            )
        {
            for (var x = 0; x < Width; x++)
            {
                for (var z = 0; z < Length; z++)
                {
                    var value = 0D;
                    var weightSum = 0D;
                    for (var xz = x - blurRadius; xz < x + blurRadius + 1; xz++)
                    {
                        for (var xx = z - blurRadius; xx < z + blurRadius + 1; xx++)
                        {
                            if (xz is < 0 or >= Width ||
                                xx is < 0 or >= Length)
                            {
                                continue;
                            }
                            var dsq = (xx-z)*(xx-z)+(xz-x)*(xz-x);
                            var weight = Math.Exp( -dsq / (2*blurRadius*blurRadius) ) / (Math.PI*2*blurRadius*blurRadius);
                            value += heights[xz, xx] * weight;
                            weightSum += weight;
                        }
                    }
                    heights[x, z] = (float)/*Math.Round*/(value / weightSum);            
                }
            }
            return heights;
        }
        private float[,] CalculateFineNoiseOverTerrain(float[,] heights)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Length; z++)
                {
                    float amp = 1f;
                    float freq = 10.0f;
                    for (int i = 4; i < fineNoiseNumberOfOctaves; i++)
                    {
                        float xCoord = (float)x / Width * fineNoiseScale * freq;
                        float zCoord = (float)z / Length * fineNoiseScale * freq;
                        if (isRandom)
                        {
                            xCoord += _baseRandomOffsetX;
                            zCoord += _baseRandomOffsetZ;
                        }
                        var noiseFloat = Mathf.PerlinNoise(xCoord, zCoord); // - 0.5f so the values are between [0.5f, -0.5f]
                        heights[x, z] += noiseFloat * fineNoiseHeight * amp / Height;
                        amp *= fineNoisePersistence;
                        freq *= fineNoiseLacunarity;
                    }
                }
            }
            return heights;
        }
        private void PaintTextureMap()
        {
            // terrain.
        }
        //
        // End of generation pipeline functions
    }
}