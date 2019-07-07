using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozack
{
	public interface IOzackBehaviourBuilder<TCommand> : IDisposable
	{
		IOzackBehaviour<TCommand> Build(TCommand cmd);
	}
	/// <summary>
	/// 処理を作る方
	/// </summary>
	public class OzackBehaviourBuilder<TCommand> 
		: IOzackBehaviourBuilder<TCommand>
	{
        //=================================
        // 変数
        //=================================
        private IOzackContext Context { get; set; }
        private HashSet<IOzackBehaviourFactory<TCommand>> FactoryContainer = new HashSet<IOzackBehaviourFactory<TCommand>>();

        //=================================
        // プロパティ
        //=================================

        //=================================
        // 関数
        //=================================

        public OzackBehaviourBuilder( IOzackContext context, params IOzackBehaviourFactory<TCommand>[] list )
        {
            Context = context;
            foreach (var f in list)
            {
                FactoryContainer.Add(f);
            }
        }

		public void Dispose()
		{
            Context = null;
            foreach (var f in FactoryContainer)
            {
                f?.Dispose();
            }
            FactoryContainer.Clear();
        }

		public IOzackBehaviour<TCommand> Build( TCommand cmd )
		{
            var factory = GetFactory( cmd );
            var behaviour = factory.Create( Context, cmd );
            return behaviour;
		}

        private IOzackBehaviourFactory<TCommand> GetFactory( TCommand cmd )
        {
            foreach (var f in FactoryContainer)
            {
                if (f.CanCreate(cmd))
                {
                    return f;
                }
            }
            return null;
        }
	}
}