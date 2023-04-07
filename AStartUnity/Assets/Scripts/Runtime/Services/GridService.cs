﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Runtime.Grid.Data;
using Runtime.Grid.Mappers;
using Runtime.Grid.Services;
using Runtime.Terrains;
using UnityEngine;

namespace Runtime.Services
{
    public interface IGridLayoutRepository
    {
        string[] ListSaves();

        UniTask<IGridCell[]> LoadAsync(string filename, ITerrainVariant[] terrainVariants,
            CancellationToken token = default);

        UniTask SaveAsync(IEnumerable<IGridCell> cells, CancellationToken token = default);
    }

    public sealed class GridLayoutRepository : IGridLayoutRepository
    {
        private static string GetFilepath()
        {
            var path = Path.Combine(Application.persistentDataPath, "Saves");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }

        private static string CreateFilename() => DateTime.Now.ToString("yyyy_dd_MM_hh_mm_ss");

        public  string[] ListSaves()
        {
            var filepath = GetFilepath();
            return Directory.GetFiles(filepath, "*.json");
        }

        public  async UniTask<IGridCell[]> LoadAsync(string filename, ITerrainVariant[] terrainVariants,
            CancellationToken token = default)
        {
            var fullPath = Path.Combine(GetFilepath(), filename);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"File not found at path: {fullPath}");

            var json = await File.ReadAllTextAsync(fullPath, token);
            var data = JsonConvert.DeserializeObject<GridCellSave[]>(json);

            return data.Select(x => GridCellMapper.GridCellFromSave(x, terrainVariants)).ToArray();
        }


        public  async UniTask SaveAsync(IEnumerable<IGridCell> cells, CancellationToken token = default)
        {
            var saves = cells.Select(GridCellMapper.ToGridCellSave).ToArray();
            var fullPath = Path.Combine(GetFilepath(), $"{CreateFilename()}.json");
            var json = JsonConvert.SerializeObject(saves);
            await File.WriteAllTextAsync(fullPath, json, token);
        }

    }
}