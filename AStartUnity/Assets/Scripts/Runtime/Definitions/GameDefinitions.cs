using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Definitions
{
    [Serializable]
    public sealed class GameDefinitions
    {
        [SerializeField] private AssetReference hexGridScene;

        [SerializeField] private AssetLabelReference preloadLabel;

        [SerializeField] private AssetReference hexTile;

        public AssetLabelReference PreloadLabel => preloadLabel;

        public AssetReference HexGridScene => hexGridScene;
        public AssetReference HexTile => hexTile;
    }
}