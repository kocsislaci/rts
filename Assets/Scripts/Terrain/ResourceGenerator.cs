using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Terrain
{
    using UnityEngine;
    
    [RequireComponent(typeof(Terrain))]
    [RequireComponent(typeof(TerrainCollider))]    
    public class ResourceGenerator : MonoBehaviour
    {
        public GameObject treePrefab;
        public GameObject stonePrefab;
        public GameObject goldPrefab;
        
        public UnityAction ResourceGenerationAction;
        public float scale;
        
        public float ratioOfTrees;
        public float chanceOfTree;
        public float ratioOfStone;
        public float chanceOfStone;
        public float ratioOfGold;
        public float chanceOfGold;
        
        private List<GameObject> _generatedTrees;
        private List<GameObject> _generatedStones;
        private List<GameObject> _generatedGold;

        private Terrain _terrain;

        private void Awake()
        {
            _generatedTrees = new List<GameObject>();
            _generatedStones = new List<GameObject>();
            _generatedGold = new List<GameObject>();

            _terrain = GetComponent<Terrain>();
            // ResourceGenerationAction += ResourceGeneration;
        }

        private void ResourceGeneration()
        {
            _generatedTrees.ForEach(Destroy);
            _generatedStones.ForEach(Destroy);
            _generatedGold.ForEach(Destroy);
            
            var terrainData = _terrain.terrainData;
            float width = terrainData.size.x;
            float height = terrainData.size.y;
            float length = terrainData.size.z;
            bool[,] isItOccupied = new bool[(int)width, (int)length];
            float offsetX = Random.Range(0, 1000);
            float offsetZ = Random.Range(0, 1000);
            
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    float xCoord = x / width * scale * 5 + offsetX;
                    float zCoord = z / length * scale * 5 + offsetZ;
                    var noiseFloat = Mathf.PerlinNoise(xCoord, zCoord);
                    isItOccupied[x, z] = false;
                    if (noiseFloat < ratioOfGold && Random.Range(0.0f, 1.0f) <= chanceOfGold && isItOccupied[x, z] == false)
                    {
                        _generatedGold.Add(Instantiate<GameObject>(
                            goldPrefab, 
                            new Vector3(
                                x, 
                                _terrain.SampleHeight(new Vector3(
                                    x,
                                    0,
                                    z)), 
                                z), 
                            new Quaternion()
                        ));
                        isItOccupied[x, z] = true;
                    }
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    float xCoord = x / width * scale * 5 + offsetX;
                    float zCoord = z / length * scale * 5 + offsetZ;
                    var noiseFloat = Mathf.PerlinNoise(xCoord, zCoord);
                    if (noiseFloat > (1.0f - ratioOfStone) && Random.Range(0.0f, 1.0f) <= chanceOfStone && isItOccupied[x, z] == false)
                    {
                        _generatedStones.Add(Instantiate<GameObject>(
                            stonePrefab, 
                            new Vector3(
                                x, 
                                _terrain.SampleHeight(new Vector3(
                                    x,
                                    0,
                                    z)), 
                                z), 
                            new Quaternion()
                        ));
                        isItOccupied[x, z] = true;
                    }
                    else
                    {
                        isItOccupied[x, z] = false;
                    }
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    float xCoord = x / width * scale + offsetX;
                    float zCoord = z / length * scale + offsetZ;
                    var noiseFloat = Mathf.PerlinNoise(xCoord, zCoord);

                    if (noiseFloat < ratioOfTrees && Random.Range(0.0f, 1.0f) <= chanceOfTree && isItOccupied[x, z] == false)
                    {
                        _generatedTrees.Add(Instantiate<GameObject>(
                            treePrefab,
                            new Vector3(
                                x,
                                _terrain.SampleHeight(new Vector3(
                                    x,
                                    0,
                                    z)),
                                z),
                            new Quaternion()
                            ));
                        isItOccupied[x, z] = true;
                    }
                    else
                    {
                        isItOccupied[x, z] = false;
                    }
                }
            }
        }
    }
}