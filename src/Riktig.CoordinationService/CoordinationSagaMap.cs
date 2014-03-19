namespace Riktig.CoordinationService
{
    using System;
    using System.Linq.Expressions;
    using Automatonymous;
    using Automatonymous.NHibernateIntegration;
    using Coordination;
    using MassTransit.NHibernateIntegration;
    using NHibernate;
    using NHibernate.Mapping.ByCode;


    public static class AutomatonymousNHibernateExtensions
    {
        public static void StateProperty<T, TMachine>(this IClassMapper<T> mapper,
            Expression<Func<T, State>> stateExpression)
            where T : class
            where TMachine : StateMachine
        {
            mapper.Property(stateExpression, x =>
                {
                    x.Type<AutomatonymousStateUserType<TMachine>>();
                    x.NotNullable(true);
                    x.Length(80);
                });
        }
    }


    public class ImageRetrievalStateMap :
        SagaClassMapping<ImageRetrievalState>
    {
        public ImageRetrievalStateMap()
        {
            // note that this requires the type to be registered separately
            this.StateProperty<ImageRetrievalState, ImageRetrievalStateMachine>(x => x.CurrentState);

            Property(x => x.Created, x => { x.NotNullable(true); });
            Property(x => x.FirstRequested, x => { x.NotNullable(true); });
            Property(x => x.LastRetrieved, x => { x.NotNullable(false); });
            Property(x => x.SourceAddress, x =>
                {
                    x.Type(NHibernateUtil.Uri);
                    x.NotNullable(true);
                });
            Property(x => x.LocalAddress, x =>
                {
                    x.Type(NHibernateUtil.Uri);
                    x.NotNullable(true);
                });
            Property(x => x.ContentLength, x => x.NotNullable(false));
            Property(x => x.ContentType, x =>
                {
                    x.NotNullable(false);
                    x.Length(256);
                });
            Property(x => x.Reason, x =>
                {
                    x.NotNullable(false);
                    x.Length(1000);
                });
        }
    }
}