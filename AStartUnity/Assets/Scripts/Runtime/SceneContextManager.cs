using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Grid.Data;

namespace Runtime
{
    public sealed class SceneContext
    {
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public IGridCell[] Cells { get; set; }
    }
    public interface ISceneContextManager
    {
        void SetContext(SceneContext value);
        SceneContext GetContext();
    }
    public sealed class SceneContextManager : ISceneContextManager
    {
        private SceneContext _context;

        public void SetContext(SceneContext value)
        {
            _context = value;
        }

        public SceneContext GetContext()
        {
           return _context;
        }
    }
}