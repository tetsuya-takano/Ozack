using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozack
{
    public interface IOzackBehaviourFactory<TCommand> : IDisposable
    {
        IOzackBehaviour<TCommand> Create(IOzackContext context, TCommand cmd );
        bool CanCreate(TCommand cmd);
    }
    public abstract class OzackBehaviourFactory<TCommand> : IOzackBehaviourFactory<TCommand>
    {

        public void Dispose()
        {
            DoDispose();
        }

        protected virtual void DoDispose() { }

        /// <summary>
        /// 生成判定
        /// </summary>
        public abstract bool CanCreate(TCommand cmd);

        /// <summary>
        /// 作成
        /// </summary>
        public IOzackBehaviour<TCommand> Create(IOzackContext context, TCommand cmd )
        {
            var behaviour = DoCreate();
            behaviour.Bind( context, cmd );
            return behaviour;
        }


        protected abstract IOzackBehaviourGenerater<TCommand> DoCreate();
    }
}
