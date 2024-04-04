using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualPack.LevelGeneration
{
    [CreateAssetMenu(fileName = "Level Generator", menuName = "HyperCasualPack/Level Generator", order = 0)]
    public class LevelGenerator : ScriptableObject
    {
        [SerializeField] LevelGeneratorData[] _datas;
        [SerializeField] Portal _townPortal;
        GameObject _levelEntrance;

        List<GameObject> _instantiatedLevel;

        void OnEnable()
        {
            _instantiatedLevel = new List<GameObject>();
        }
        
        // Each battle arena have tiles, and every tile have an enemy based on difficulty
        public void Generate(int level, int difficulty, int tileLength, Vector3 tileStartingPoint, Vector3 backPortalPoint, GameObject levelEntrance)
        {
            _levelEntrance = levelEntrance;
            levelEntrance.SetActive(false);
            SpawnBackPortal(tileStartingPoint, backPortalPoint);
            SpawnLevel(level, difficulty, tileLength, tileStartingPoint);
        }

        public void DestroyLevel()
        {
            _levelEntrance.SetActive(true);
            foreach (GameObject instantiatedLandPiece in _instantiatedLevel)
            {
                Destroy(instantiatedLandPiece);
            }

            _instantiatedLevel.Clear();
        }

        void SpawnBackPortal(Vector3 tileStartPoint, Vector3 portalSpawnPointRelative)
        {
            Portal p = Instantiate(_townPortal, tileStartPoint + portalSpawnPointRelative, Quaternion.identity);
            p.Initialize(this, false);
            _instantiatedLevel.Add(p.gameObject);
        }

        void SpawnLevel(int level, int difficulty, int tileLength, Vector3 startingPos)
        {
            LevelGeneratorData currentLevelData = _datas[level];
            Tile land = currentLevelData.GetTile();
            Tile previousLand = InstantiateLand(land, startingPos);
            for (int i = 0; i < tileLength; i++)
            {
                previousLand = InstantiateLand(land, previousLand.GetEndPoint().position);
                if ((tileLength - i) * _datas[level].MaxLevel <= difficulty)
                {
                    // give highest difficulty battle group
                    previousLand.SpawnGroup(currentLevelData.GetGroup(_datas[level].MaxLevel));
                    difficulty -= _datas[level].MaxLevel;
                }
                else
                {
                    // give random battle group
                    int rnd = Random.Range(0, _datas[level].MaxLevel);
                    previousLand.SpawnGroup(currentLevelData.GetGroup(rnd));
                    difficulty -= rnd;
                }
            }

            previousLand = InstantiateLand(land, previousLand.GetEndPoint().position);
            previousLand.SpawnPortal(_townPortal, this);
        }

        Tile InstantiateLand(Tile land, Vector3 position)
        {
            Tile landPiece = Instantiate(land, position, Quaternion.identity);
            _instantiatedLevel.Add(landPiece.gameObject);
            return landPiece;
        }
    }
}