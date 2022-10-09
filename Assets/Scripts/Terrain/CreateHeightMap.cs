using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

namespace Terrain
{
    using Terrain = UnityEngine.Terrain;

    [RequireComponent(typeof(Terrain))]
    [RequireComponent(typeof(TerrainCollider))]
    public class CreateHeightMap : MonoBehaviour
    {
        /// <summary>
        /// Debug flags
        /// </summary>
        [Header("Debug flags")]
        [SerializeField] [Space(10)]
        private bool areMountainsIncluded; 

        /// <summary>
        /// Parameters to modify the Perlin noise
        /// </summary>
        [Header("Terrain Base Generation")] 
        [Range(0, 4)] [Tooltip("")] [SerializeField]
        private int numberOfOctaves;
        [Range(0, 4)] [Tooltip("")] [SerializeField]
        private float persistence;
        [Range(0, 4)] [Tooltip("")] [SerializeField]
        private float lacunarity;
        [Range(0, 64)] [Tooltip("")] [SerializeField]
        private float offsetY;
        [Range(0, 10)] [Tooltip("")] [SerializeField] [Space(10)]
        private float scale;

        /// <summary>
        /// Size of the map, is set in the InitSizes() function
        /// </summary>
        private int _width; // x
        private int _height; // y
        private int _length; // z
        private Vector3 _size;
        
        /// <summary>
        /// To modify mountain params
        /// </summary>
        [Range(0, 10)]
        public float mountainScale;
        [Range(0, 10)] [Space(10)]

        public float mountainFreq;

        /// <summary>
        /// Feed for random function
        /// </summary>
        private float _MaximumValueForRandomRange = 1000.0f;
        
        /// <summary>
        /// Randomly generated offsets to be able to generate "always" different terrains
        /// </summary>
        private float _baseOffsetX = 0;
        private float _baseOffsetZ = 0;
        private float _mountainOffsetX = 0;
        private float _mountainOffsetZ = 0;
        
        /// <summary>
        /// Saved height range of the actual generated map
        /// </summary>
        private float _minY;
        private float _maxY;

        /// <summary>
        /// There is two predefined spawnPoint on the map, where the two players will start
        /// </summary>
        private List<Transform> _spawnPoints;

        /// <summary>
        /// Saved references to the terrain components
        /// </summary>
        private Terrain _terrain;
        private TerrainCollider _terrainCollider;
        
        /// <summary>
        /// Action which can be invoked from the outside to trigger callbacks
        /// </summary>
        public UnityAction onMapGeneration;
        
        private void Awake()
        {
            InitSizes();
            Init();
        }

       

        private void GenerateHeightMap()
        {
            GenerateRandomParams();
            
            CalculateBaseHeightMap();
        }

        private void InitSizes()
        {
            _width = 512; // x
            _height = 128; // y
            _length = _width; // z // always square shaped base
            // save these numbers to a Vector3
            _size.x = _width;
            _size.y = _height;
            _size.z = _length;        
        }
        private void Init()
        {
            // Save references to the found spawnPoints on terrain
            _spawnPoints.AddRange(gameObject.GetComponentsInChildren<Transform>());
            // Save references to the components
            _terrain = GetComponent<Terrain>();
            _terrainCollider = GetComponent<TerrainCollider>();
            // Add callback to the action
            onMapGeneration += GenerateHeightMap;
        }
        
        private void GenerateRandomParams()
        {
            // Base
            _baseOffsetX = Random.Range(0, _MaximumValueForRandomRange);
            _baseOffsetZ = Random.Range(0, _MaximumValueForRandomRange);
            
            // Mountains
            _mountainOffsetX = Random.Range(0, _MaximumValueForRandomRange);
            _mountainOffsetZ = Random.Range(0, _MaximumValueForRandomRange);
        }

        private void CalculateBaseHeightMap()
        {
            // Every generation starts with an init
            _maxY = 0;
            _minY = offsetY;
            //

            TerrainData localTerrainData = InitializeTerrainData(_terrain.terrainData);

            // Heightmap to store the local calculated values
            float[,] heights = new float[_width, _length];
            
            // Calculate the base heightMap of the terrain
            heights = CalculateBaseTerrain(heights);
            
            
            // Calculate
            foreach (var spawnPoint in _spawnPoints)
            {
                var pointPosition = spawnPoint.transform.localPosition;
                float averageHeight = CalculateAverageHeight(pointPosition, heights);
                heights = SetHeightAroundSpawnPoint(pointPosition, heights, averageHeight);
            }
            
            // Load calculated data to localTerrainData object
            localTerrainData.SetHeights(0, 0, heights);
            
            // Load calculated data to terrain component references
            _terrain.terrainData = localTerrainData;
            _terrainCollider.terrainData = localTerrainData;
            
            // Apply
            _terrain.Flush();
        }
        
        private TerrainData InitializeTerrainData(TerrainData terrainData)
        {
            terrainData.size = new Vector3(_width, _height, _length);
            terrainData.SetDetailResolution(_width, 32);
            terrainData.baseMapResolution = _width;
            terrainData.heightmapResolution = _width + 1;
            return terrainData;
        }
        
        private float CalculateAverageHeight(Vector3 spawnPoint, float[,] heightMap)
        {
            var x = (int)spawnPoint.x;
            var z = (int)spawnPoint.z;
            var radius = 50;
            var average = 0f;

            for (var xLoc = x - radius; x < x + radius; x++)
            {
                for (var zLoc = z - radius; z < z + radius; z++)
                {
                    average += heightMap[xLoc, zLoc];
                }
            }
            return average / (radius * 2.0f * 2.0f);
        }
        private float[,] SetHeightAroundSpawnPoint(Vector3 spawnPoint, float[,] heightMap, float heightToSetTo)
        {
            var radius = 50;
            var xCenter = (int)spawnPoint.x;
            var zCenter = (int)spawnPoint.z;
            var x = (int)spawnPoint.x - radius;
            var z = (int)spawnPoint.z - radius;
            var xMax = x + 2 * radius;
            var zMax = z + 2 * radius;

            for (; x < xMax; x++)
            {
                for (; z < zMax; z++)
                {
                    var lengthFromCenter = Mathf.Sqrt(Mathf.Pow(x - xCenter, 2) + Mathf.Pow(z - zCenter, 2));
                    if (lengthFromCenter < radius)
                    {
                        heightMap[x, z] = 0f/*heightToSetTo*/;
                    }
                }
            }
            return heightMap;
        }
        
        private float[,] CalculateBaseTerrain(float[,] heights)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _length; z++)
                {
                    float y = 0;
                    float amp = 1.0f;
                    float freq = 1.0f;
                    for (int i = 0; i < numberOfOctaves; i++)
                    {
                        float xCoord = (float)x / _width * scale * freq + _baseOffsetX;
                        float zCoord = (float)z / _length * scale * freq + _baseOffsetZ;
                        var noiseFloat = Mathf.PerlinNoise(xCoord, zCoord) - 0.5f;
                        y += noiseFloat * amp;
                        amp *= persistence;
                        freq *= lacunarity;
                    }
                    if (_minY > heights[x, z])
                        _minY = heights[x, z];
                    if (_maxY < heights[x, z])
                        _maxY = heights[x, z];
                    heights[x, z] = y + offsetY / _height /*+ mountains[x, z]*/;
                }
            }
            return heights;
        }
        private float[,] CalculateMountains()
        {
            float randomX = Random.Range(0, 1000);
            float randomY = Random.Range(0, 1000);

            float[,] heights = new float[_width, _length];
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _length; z++)
                {
                    float y = 0;
                
                    float xCoord = (float)x / _width * mountainScale * mountainFreq + _mountainOffsetX;
                    float zCoord = (float)z / _length * mountainScale * mountainFreq + _mountainOffsetZ;
                    heights[x, z] = Mathf.PerlinNoise(xCoord, zCoord) + offsetY / _height;
                }
            }
            return heights;
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        /// <summary>
        /// got this code from Create()
        /// </summary>
        // CreateTexturesAndSetInLayers(new Color[6]
        // {
        //     new Color(0, 0, 1), 
        //     new Color(1, 1, 0),
        //     new Color(0, 1, 0),
        //     new Color(0.28f, 0.1f, 0.05f),
        //     new Color(0.28f, 0.1f, 0.05f),
        //     new Color(1, 1, 1)
        // });
        // ClearSplashMap();
        
        //
        private TerrainLayer _terrainLayer;
        private Texture2D _texture2D;
        
        private Color[] _colors;
        private Gradient _gradient;
        
        private void CreateTexturesAndSetInLayers(Color[] colors)
        {
            TerrainLayer[] terrainLayers = _terrain.terrainData.terrainLayers;
            for (int i = 0; i < terrainLayers.Length; i++)
            {
                // Texture2D texture2D = Texture2D.redTexture;
                // terrainLayers[i].diffuseTexture = texture2D;
                terrainLayers[i].diffuseTexture = CreateTexture(1, colors[i]);
                terrainLayers[i].diffuseTexture.Apply();
            }

            _terrain.terrainData.SetTerrainLayersRegisterUndo(terrainLayers, "undoName");
        }
        private Texture2D CreateTexture(int size, Color color)
        {
            Texture2D texture = new Texture2D(size, size);
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; x < size; x++)
                {
                    texture.SetPixel(x, y, color);
                }
            }

            return texture;
        }
        
        private void ClearSplashMap()
        {
            float[,,] map = new float[_terrain.terrainData.alphamapWidth, _terrain.terrainData.alphamapHeight,
                _terrain.terrainData.alphamapLayers];
            // For each point on the alphamap...
            for (int y = 0; y < _terrain.terrainData.alphamapHeight; y++)
            {
                for (int x = 0; x < _terrain.terrainData.alphamapWidth; x++)
                {
                    float normActualHeight = _terrain.terrainData.GetHeight(y, x) / _height;
                    float[] tresholds = new float[6]
                    {
                        0.05f,
                        0.2f,
                        0.5f,
                        0.75f,
                        0.9f,
                        1.0f
                    };
                    if (normActualHeight <= tresholds[0])
                    {
                        map[x, y, 0] = 1f;
                        map[x, y, 1] = 0f;
                        map[x, y, 2] = 0f;
                        map[x, y, 3] = 0f;
                        map[x, y, 4] = 0f;
                        map[x, y, 5] = 0f;
                    }
                    else if (normActualHeight <= tresholds[1])
                    {
                        map[x, y, 0] = 0f;
                        map[x, y, 1] = Mathf.InverseLerp(tresholds[0], tresholds[1], normActualHeight);
                        map[x, y, 2] = 0f;
                        map[x, y, 3] = 0f;
                        map[x, y, 4] = 0f;
                        map[x, y, 5] = 0f;
                    }
                    else if (normActualHeight <= tresholds[2])
                    {
                        map[x, y, 0] = 0f;
                        map[x, y, 1] = 0f;
                        map[x, y, 2] = Mathf.InverseLerp(tresholds[1], tresholds[2], normActualHeight);
                        map[x, y, 3] = 0f;
                        map[x, y, 4] = 0f;
                        map[x, y, 5] = 0f;
                    }
                    else if (normActualHeight <= tresholds[3])
                    {
                        map[x, y, 0] = 0f;
                        map[x, y, 1] = 0f;
                        map[x, y, 2] = Mathf.InverseLerp(tresholds[2], tresholds[3], normActualHeight);
                        map[x, y, 3] = Mathf.InverseLerp(tresholds[2], tresholds[3], normActualHeight);
                        map[x, y, 4] = 0f;
                        map[x, y, 5] = 0f;
                    }
                    else if (normActualHeight <= tresholds[4])
                    {
                        map[x, y, 0] = 0f;
                        map[x, y, 1] = 0f;
                        map[x, y, 2] = 0f;
                        map[x, y, 3] = Mathf.InverseLerp(tresholds[3], tresholds[4], normActualHeight);
                        map[x, y, 4] = Mathf.Lerp(tresholds[3], tresholds[4], normActualHeight);
                        map[x, y, 5] = 0f;
                    }
                    else if (normActualHeight <= tresholds[5])
                    {
                        map[x, y, 0] = 0f;
                        map[x, y, 1] = 0f;
                        map[x, y, 2] = 0f;
                        map[x, y, 3] = 0f;
                        map[x, y, 4] = Mathf.InverseLerp(tresholds[4], tresholds[5], normActualHeight);
                        map[x, y, 5] = Mathf.Lerp(tresholds[4], tresholds[5], normActualHeight);
                    }
                }
            }

            _terrain.terrainData.SetAlphamaps(0, 0, map);

        }
        private void CreateGradient()
        {
            Gradient gradient = new Gradient();
            GradientColorKey[] gradientColorKeys = new GradientColorKey[6];
            gradientColorKeys[0] = new GradientColorKey(new Color(0, 0, 1), 0.05f);
            gradientColorKeys[1] = new GradientColorKey(new Color(1, 1, 0), 0.2f);
            gradientColorKeys[2] = new GradientColorKey(new Color(0, 1, 0), 0.5f);
            gradientColorKeys[3] = new GradientColorKey(new Color(0.28f, 0.1f, 0.05f), 0.75f);
            gradientColorKeys[4] = new GradientColorKey(new Color(0.28f, 0.1f, 0.05f), 0.90f);
            gradientColorKeys[5] = new GradientColorKey(new Color(1, 1, 1), 1);

            gradient.colorKeys = gradientColorKeys;

            _gradient = gradient;
        }
        //
    }
}