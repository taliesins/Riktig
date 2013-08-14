namespace Riktig.CoordinationService
{
    using System;
    using Autofac;
    using Automatonymous;
    using Automatonymous.Binders;
    using Coordination;


    /// <summary>
    /// This allows an activity for a state machine to be resolved from the container
    /// however it does not currently observe (or has not been verified to observe)
    /// the current lifetime scope of a message handler
    /// </summary>
    public class AutofacStateMachineActivityFactory<TInstance> :
        IStateMachineActivityFactory<TInstance>
    {
        readonly IComponentContext _context;

        public AutofacStateMachineActivityFactory(IComponentContext context)
        {
            _context = context;
        }

        public Activity<TInstance, TData> GetActivity<TActivity, TData>()
            where TActivity : Activity<TInstance, TData>
        {
            return _context.Resolve<TActivity>();
        }

        public Activity<TInstance> GetActivity<TActivity>()
            where TActivity : Activity<TInstance>
        {
            return _context.Resolve<TActivity>();
        }
    }
}