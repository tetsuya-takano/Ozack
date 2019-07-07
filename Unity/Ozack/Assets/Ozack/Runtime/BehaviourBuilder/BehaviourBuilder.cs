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
		private Func<TCommand, IOzackBehaviour<TCommand>> m_onBuild = null;

		//=================================
		// プロパティ
		//=================================
		public Func<TCommand, IOzackBehaviour<TCommand>> OnBuild { set => m_onBuild = value; }

		//=================================
		// 関数
		//=================================

		public void Dispose()
		{
			m_onBuild = null;
		}

		public IOzackBehaviour<TCommand> Build( TCommand cmd )
		{
			return m_onBuild?.Invoke(cmd);
		}
	}
}