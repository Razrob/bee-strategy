using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private Vector3 centralPosition;
    [SerializeField] private float height;
    [SerializeField] private float width;

    [SerializeField] private List<GameObject> tilesPrefabs;

    [SerializeField] private List<GameObject> flowerPrefabs;
    
    [SerializeField] private GameObject bushPrefab;
    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private GameObject cloverPrefab;

    [Inject] private BuildingGridConfig _constructionConfig;

    private Vector3 _currentTilePosition;

    public int flowerGenChance;
    public int bushGenChance;
    public int grassGenChance;
    public int cloverGenChance;

    void Start()
    {
        _currentTilePosition = centralPosition;
        _currentTilePosition.x -= width/2;
        _currentTilePosition.z += height/2;
        _currentTilePosition =
            FrameworkCommander.GlobalData.ConstructionsRepository.RoundPositionToGrid(_currentTilePosition);
        _currentTilePosition.z +=  _constructionConfig.HexagonsOffcets.y/2;
        
        GenerateMap();
    }

    private void GenerateMap()
    {
        float yValue = 0;
        bool stopGenerate = false;
        while (!stopGenerate)
        {
            Vector3 spawnPos = FrameworkCommander.GlobalData.ConstructionsRepository.RoundPositionToGrid(_currentTilePosition);
            spawnPos.y = -spawnPos.z/10000;
            int tileNum = (int)Random.Range(0, tilesPrefabs.Count);
            Instantiate(tilesPrefabs[tileNum],spawnPos, Quaternion.Euler(0, 0, 0), this.transform);
            
            int tryToSpawnFlower = (int)Random.Range(0, 100);
            int tryToSpawnBush = (int)Random.Range(0, 100);
            int tryToSpawnGrass = (int)Random.Range(0, 100);
            int tryToSpawnClover = (int)Random.Range(0, 100);

            if (tryToSpawnFlower < flowerGenChance)
            {
                int flowerNum = (int)Random.Range(0, flowerPrefabs.Count);

                Instantiate(flowerPrefabs[flowerNum], spawnPos, Quaternion.Euler(0, 0, 0), this.transform);
            }
            else if (tryToSpawnBush < bushGenChance)
            {
                Instantiate(bushPrefab, spawnPos, Quaternion.Euler(0, 0, 0), this.transform);
            }
            else if (tryToSpawnGrass < grassGenChance)
            {
                Instantiate(grassPrefab, spawnPos, Quaternion.Euler(0, 0, 0), this.transform);
            }
            else if (tryToSpawnClover < cloverGenChance)
            {
                Instantiate(cloverPrefab, spawnPos, Quaternion.Euler(0, 0, 0), this.transform);
            }
            
            _currentTilePosition.x += _constructionConfig.HexagonsOffcets.x/2;

            if (_currentTilePosition.x > centralPosition.x + width/2)
            {
                _currentTilePosition.x = centralPosition.x - width/2;
                _currentTilePosition.z -= _constructionConfig.HexagonsOffcets.y * 2;
                yValue += 0.0001f;
                if (_currentTilePosition.z < centralPosition.z - height/2)
                {
                    stopGenerate = true;
                }
            }  
        }
    }
    
}
