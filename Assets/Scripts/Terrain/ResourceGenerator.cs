using System.Collections.Generic;
using GameManagers;
using Unit.ResourceObject;
using Unit.ResourceObject.ImplementedResources;
using UnityEngine.Events;

namespace Terrain
{
    using UnityEngine;
    
    [RequireComponent(typeof(Terrain))]
    [RequireComponent(typeof(TerrainCollider))]    
    public class ResourceGenerator : MonoBehaviour
    {
        [SerializeField] private Terrain terrain;

        [Header("Noise parameter")]
        [SerializeField] [Range(1, 100)] private float scale;
        [SerializeField] private bool isRandom;
        [SerializeField] private bool isSpawnAreaTakenIntoAccount;
        [SerializeField] [Range(0, 80)] private float radiusOfSpawnArea;
        [SerializeField] private List<GameObject> spawnCenters;

        [Header("Resources parameters")] 
        [SerializeField] private bool isWoodGenerated;
        [SerializeField] [Range(0, 1)] private float ratioOfTrees;
        [SerializeField] [Range(0, 1)] private float chanceOfTree;
        [SerializeField] private bool isStoneGenerated;
        [SerializeField] [Range(0, 1)] private float ratioOfStone;
        [SerializeField] [Range(0, 1)] private float chanceOfStone;
        [SerializeField] private bool isGoldGenerated;
        [SerializeField] [Range(0, 1)] private float ratioOfGold;
        [SerializeField] [Range(0, 1)] private float chanceOfGold;

        private float width;
        private float height;
        private float length;

        private float offsetX;
        private float offsetZ;

        /// <summary>
        /// Action which can be invoked from the outside to trigger callbacks
        /// </summary>
        public UnityAction onResourceGeneration;
        private void SetCallbackToEvent()
        {
            onResourceGeneration += ResourceGenerationCallback;
        }
        
        private void Awake()
        {
            SetCallbackToEvent();
            InitializeFields();
        }

        private void InitializeFields()
        {
            var terrainData = terrain.terrainData;
            width = terrainData.size.x;
            height = terrainData.size.y;
            length = terrainData.size.z;
        }

        [ContextMenu("Generate resources")]
        private void ResourceGenerationCallback()
        {
            DeleteResources();

            bool[,] isItOccupied = new bool[(int)width, (int)length];
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    isItOccupied[x, z] = false;
                }
            }
            
            if (isRandom)
            {
                offsetX = Random.Range(0, 1000);
                offsetZ = Random.Range(0, 1000);
            }
            else
            {
                offsetX = 0f;
                offsetZ = 0f;
            }
            
            isItOccupied = SpawnAreaTakenIntoAccount(isItOccupied);

            if (isGoldGenerated)
            {
                isItOccupied = GoldGeneration(isItOccupied);
            }
            
            if (isStoneGenerated)
            {
                isItOccupied = StoneGeneration(isItOccupied);
            }

            if (isWoodGenerated)
            {
                isItOccupied = WoodGeneration(isItOccupied);
            }
        }
        
        [ContextMenu("Delete resources")]
        private void DeleteResources()
        {
            GameManager.WOOD_RESOURCES.Clear();
            GameManager.STONE_RESOURCES.Clear();
            GameManager.GOLD_RESOURCES.Clear();
        }

        private bool[,] SpawnAreaTakenIntoAccount(bool[,] isItOccupied)
        {
            foreach (var spawnCenter in spawnCenters)
            {
                var centerPos3 = spawnCenter.transform.position;
                var centerPos2 = new Vector2(centerPos3.x, centerPos3.z);

                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < length; z++)
                    {
                        var actualPos2 = new Vector2(x, z);
                        var distance = (actualPos2 - centerPos2).magnitude;
                        if (distance < radiusOfSpawnArea && isSpawnAreaTakenIntoAccount)
                            isItOccupied[x, z] = true;
                    }
                }
            }
            return isItOccupied;
        }

        private bool[,] GoldGeneration(bool[,] isItOccupied)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    if (isItOccupied[x, z]) continue;

                    float xCoord = x / width * scale * 5 + offsetX;
                    float zCoord = z / length * scale * 5 + offsetZ;
                    var noiseFloat = Mathf.PerlinNoise(xCoord, zCoord);
                    if (noiseFloat < ratioOfGold && Random.Range(0.0f, 1.0f) <= chanceOfGold)
                    {
                        isItOccupied[x, z] = true;
                        var gold = new GoldResource(
                            new Vector3(
                                x,
                                terrain.SampleHeight(new Vector3(
                                    x,
                                    0,
                                    z)),
                                z)
                        );
                        GameManager.GOLD_RESOURCES.Add(gold);
                    }
                }
            }
            return isItOccupied;
        }

        private bool[,] StoneGeneration(bool[,] isItOccupied)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    if (isItOccupied[x, z]) continue;
                    
                    float xCoord = x / width * scale * 5 + offsetX;
                    float zCoord = z / length * scale * 5 + offsetZ;
                    var noiseFloat = Mathf.PerlinNoise(xCoord, zCoord);
                    if (noiseFloat > (1.0f - ratioOfStone) && Random.Range(0.0f, 1.0f) <= chanceOfStone)
                    {
                        isItOccupied[x, z] = true;
                        var stone = new StoneResource(
                            new Vector3(
                                x,
                                terrain.SampleHeight(new Vector3(
                                    x,
                                    0,
                                    z)),
                                z)
                        );
                        GameManager.STONE_RESOURCES.Add(stone);
                    }
                }
            }
            return isItOccupied;
        }

        private bool[,] WoodGeneration(bool[,] isItOccupied)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    if (isItOccupied[x, z]) continue;
                    
                    float xCoord = x / width * scale + offsetX;
                    float zCoord = z / length * scale + offsetZ;
                    var noiseFloat = Mathf.PerlinNoise(xCoord, zCoord);
                    if (noiseFloat < ratioOfTrees && Random.Range(0.0f, 1.0f) <= chanceOfTree)
                    {
                        isItOccupied[x, z] = true;
                        var wood = new WoodResource(
                            new Vector3(
                                x,
                                terrain.SampleHeight(new Vector3(
                                    x,
                                    0,
                                    z)),
                                z)
                        );
                        GameManager.WOOD_RESOURCES.Add(wood);
                    }
                }
            }
            return isItOccupied;
        }
    }
}
