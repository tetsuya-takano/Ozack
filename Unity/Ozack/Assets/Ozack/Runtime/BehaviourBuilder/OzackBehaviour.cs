using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozack
{
	/// <summary>
	/// 挙動
	/// </summary>
	public interface IOzackBehaviour<TCommand>  : IDisposable
	{
		void Init();
		void Begin();
		void Update(float dt);

		void End();
	}
    public interface IOzackBehaviourGenerater<TCommand> : IOzackBehaviour<TCommand>
    {
        void Bind( IOzackContext context );
    }

    public abstract class OzackBehaviour<TCommand> 
        : IOzackBehaviour<TCommand>,
          IOzackBehaviourGenerater<TCommand>
    {
        //==============================
        // プロパティ
        //==============================
        protected IOzackContext Context { get; private set; }
        public bool IsDisposed { get; private set; }

		//==============================
		// 関数
		//==============================
		protected virtual void DoInit() { }

		public void Init() { DoInit(); }

		protected virtual void DoBegin() { }
		public void Begin() { DoBegin(); }

		protected virtual void DoEnd() { }
		public void End() { DoEnd(); }

		public void Update(float dt)
		{
			DoPreUpdate(dt);
			DoUpdate(dt);
			DoPostUpdate(dt);
		}
		protected virtual void DoUpdate(float dt) { }
		protected virtual void DoPreUpdate(float dt) { }
		protected virtual void DoPostUpdate(float dt) { }

		public void Dispose()
		{
			if( IsDisposed )
			{
				return;
			}
            Context = null;
			DoDispose();
			IsDisposed = true;
		}
		protected virtual void DoDispose() { }


        public void Bind( IOzackContext context )
        {
            Context = context;
        }
	}
}