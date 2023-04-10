using System;
using UnityEngine;

namespace Runtime.Grid.Presenters
{
    public sealed class TerrainVariantRenderer : ITerrainVariantRenderer
    {
        public static readonly int IsHovered = Shader.PropertyToID("_Is_Hovered");
        public static readonly int IsSelected = Shader.PropertyToID("_Is_Selected");
        public static readonly int MainTex = Shader.PropertyToID("_MainTex");

        private readonly MaterialPropertyBlock _materialPropertyBlock;
        private readonly Renderer _renderer;

        public TerrainVariantRenderer(Renderer renderer, MaterialPropertyBlock materialPropertyBlock = null)
        {
            _renderer = renderer ? renderer : throw new ArgumentNullException(nameof(renderer));
            _materialPropertyBlock = materialPropertyBlock ?? new MaterialPropertyBlock();
        }

        public void SetIsHighlighted(bool value)
        {
            _materialPropertyBlock.SetFloat(IsHovered, value ? 1 : 0);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }

        public void SetIsSelected(bool value)
        {
            _materialPropertyBlock.SetFloat(IsSelected, value ? 1 : 0);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }

        public void SetMainTexture(Texture texture)
        {
            _materialPropertyBlock.SetTexture(MainTex, texture);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}