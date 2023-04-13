using Runtime.Grid.Models;

namespace Runtime.Grid.Services
{
    public sealed class GridSetupManager : IGridSetupManager
    {
        private GridSetup _context;

        public void SetContext(GridSetup value)
        {
            _context = value;
        }

        public GridSetup GetContext()
        {
            return _context;
        }
    }
}