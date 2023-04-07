using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Ui
{
    [Serializable]
    public sealed class GameDefinitions
    {
        [SerializeField] private AssetReference hexGridScene;

        [SerializeField] private AssetLabelReference preloadLabel;

        [SerializeField] private AssetReference hexTile;
        [SerializeField] private AssetLabelReference terrainVariantsLabel;

        public AssetLabelReference PreloadLabel => preloadLabel;

        public AssetReference HexGridScene => hexGridScene;
        public AssetReference HexTile => hexTile;
        public AssetLabelReference TerrainVariantsLabel => terrainVariantsLabel;
    }
}