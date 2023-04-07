using System;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Runtime.Grid.Data;
using Runtime.Grid.Mappers;
using Runtime.Terrains;
using UnityEngine;

namespace Runtime.Grid.Services
{
    public sealed class GridLayoutRepository : IGridLayoutRepository
    {
        private readonly ITerrainVariantRepository _terrainVariantRepository;

        public GridLayoutRepository(ITerrainVariantRepository terrainVariantRepository)
        {
            _terrainVariantRepository = terrainVariantRepository;
        }
        
        private static string GetFilepath()
        {
            var path = Path.Combine(Application.persistentDataPath, "Saves");
            Debug.Log(path);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }

        private static string CreateFilename() => DateTime.Now.ToString("yyyy_dd_MM_hh_mm_ss");

        public string[] ListSaves()
        {
            var filepath = GetFilepath();
            return Directory.GetFiles(filepath, "*.json");
        }

        public async UniTask<IGridCell[]> LoadAsync(string filename, CancellationToken token = default)
        {
            var fullPath = Path.Combine(GetFilepath(), filename);
            
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"File not found at path: {fullPath}");

            var json = await File.ReadAllTextAsync(fullPath, token);
            var data = JsonConvert.DeserializeObject<GridCellSave[]>(json);

            await _terrainVariantRepository.InitAsync();
            
            return data.Select(x => GridCellMapper.GridCellFromSave(x, _terrainVariantRepository.GetTerrainVariant(x.TerrainType))).ToArray();
        }


        public async UniTask SaveAsync(IGridCell[] cells, CancellationToken token = default)
        {
            var saves = cells.Select(GridCellMapper.ToGridCellSave).ToArray();
            var fullPath = Path.Combine(GetFilepath(), $"{CreateFilename()}.json");
            var json = JsonConvert.SerializeObject(saves);
            await File.WriteAllTextAsync(fullPath, json, token);
        }
    }
}