namespace Riktig.Coordination
{
    using Automatonymous;


    public interface IStateMachineActivityFactory<TInstance>
    {
        Activity<TInstance, TData> GetActivity<TActivity, TData>()
            where TActivity : Activity<TInstance, TData>;

        Activity<TInstance> GetActivity<TActivity>()
            where TActivity : Activity<TInstance>;
    }
}