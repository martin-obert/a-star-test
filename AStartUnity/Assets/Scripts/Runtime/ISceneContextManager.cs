namespace Runtime
{
    public interface ISceneContextManager
    {
        void SetContext(SceneContext value);
        SceneContext GetContext();
    }
}