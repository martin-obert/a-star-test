namespace Runtime
{
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