using UnityEngine;

namespace Runtime.Grid.Presenters
{
    public interface ITerrainVariantRenderer
    {
        void SetIsHighlighted(bool value);
        void SetIsSelected(bool value);
        void SetMainTexture(Texture texture);
    }
}