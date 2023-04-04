using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Terrains
{
    [CreateAssetMenu(menuName = "Terrain/Terrain Variant Repository", fileName = "TerrainVariantRepository", order = 0)]
    public sealed class TerrainVariantRepository : ScriptableObject, ITerrainVariantRepository
    {
        [SerializeField] private TerrainVariant[] variants;

        public ITerrainVariant GetRandomTerrainVariant(int row, int col)
        {
            return variants[Random.Range(0, variants.Length)];
            
            // I've tried the perlin noise and outcome was nice to look at, but not good for proof of concept
            // var i = Mathf.PerlinNoise(row == 0 ? 0 : 1f / row * 100, col == 0 ? 0 : 1f / col * 100);
            // var index = (int)(variants.Length * i);
            // if (index >= variants.Length)
            //     index = variants.Length - 1;
            // return variants[index];
        }
    }
}