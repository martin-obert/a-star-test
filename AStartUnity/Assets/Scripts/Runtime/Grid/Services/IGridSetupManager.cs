using Runtime.Grid.Models;

namespace Runtime.Grid.Services
{
    public interface IGridSetupManager
    {
        void SetContext(GridSetup value);
        GridSetup GetContext();
    }
}