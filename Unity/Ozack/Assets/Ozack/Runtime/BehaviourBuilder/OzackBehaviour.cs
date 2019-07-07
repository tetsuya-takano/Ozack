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
		void Init( );
		void Begin();
		void Update(float dt);

		void End();
	}
    /// <summary>
    /// 生成側
    /// </summary>
    public interface IOzackBehaviourGenerater<TCommand> : IOzackBehaviour<TCommand>
    {
        void Bind( IOzackContext context, TCommand cmd );
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
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            Context = null;
            DoDispose();
            IsDisposed = true;
        }


        public void Init() => DoInit();
		public void Begin() => DoBegin();
        public void Update(float dt)
        {
            DoPreUpdate(dt);
            DoUpdate(dt);
            DoPostUpdate(dt);
        }
        public void End() => DoEnd();


        protected virtual void DoInit( ) { }
        protected virtual void DoBegin() { }
        protected virtual void DoEnd() { }
        protected virtual void DoUpdate(float dt) { }
        protected virtual void DoPreUpdate(float dt) { }
        protected virtual void DoPostUpdate(float dt) { }
        protected virtual void DoDispose() { }


        public void Bind(IOzackContext context, TCommand cmd)
        {
            Context = context;
            DoBuild(cmd);
        }
        protected abstract void DoBuild(TCommand cmd);
	}
}