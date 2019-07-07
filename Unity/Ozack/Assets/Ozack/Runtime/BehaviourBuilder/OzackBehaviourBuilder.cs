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
        private List<IOzackBehaviourFactory<TCommand>> FactoryList = new List<IOzackBehaviourFactory<TCommand>>();

        //=================================
        // プロパティ
        //=================================

        //=================================
        // 関数
        //=================================

        public OzackBehaviourBuilder(IOzackContext context)
        {
            Context = context;
        }

		public void Dispose()
		{
            Context = null;
            FactoryList.ForEach(c => c?.Dispose());
            FactoryList.Clear();
        }

		public IOzackBehaviour<TCommand> Build( TCommand cmd )
		{
            var factory = GetFactory( cmd );
            var behaviour = factory.Create( Context );
            return behaviour;
		}

        private IOzackBehaviourFactory<TCommand> GetFactory( TCommand cmd )
        {
            foreach (var f in FactoryList)
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