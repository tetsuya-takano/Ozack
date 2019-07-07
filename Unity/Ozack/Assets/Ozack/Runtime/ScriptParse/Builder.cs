using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ozack
{
	public struct OzackComandArgs
	{
		public string Cmd { get; }
		public int CmdNumber { get; }
		public IReadOnlyList<string> Params { get; }

		public OzackComandArgs(string cmd, int number, IReadOnlyList<string> param)
		{
			Cmd = cmd;
			CmdNumber = number;
			Params = param;
		}
	}

	public interface IOzackCommandBuilder<TCommand> : IDisposable
	{
		bool IsCmd( string arg );
		TCommand Build( OzackComandArgs arg );
	}
	/// <summary>
	/// コマンドビルダー
	/// </summary>
	public class OzackCommandBuilder<TCommand> : IOzackCommandBuilder<TCommand>
	{
		public delegate TCommand ConstructCommand( OzackComandArgs arg );
		//==============================
		// 変数
		//==============================
		private HashSet<string>  m_table = new HashSet<string>();
		private ConstructCommand m_construct = null;
		//==============================
		// 関数
		//==============================

		public OzackCommandBuilder( HashSet<string> table, ConstructCommand onConstract )
		{
			m_table = table;
			m_construct = onConstract;
		}

		public void Dispose()
		{
			m_table.Clear();
			m_construct = null;
		}

		public bool IsCmd(string arg)
		{
			return m_table.Contains(arg);
		}

		public TCommand Build( OzackComandArgs args )
		{
			if (m_construct == null)
			{
				return default;
			}
			return m_construct.Invoke(args);
		}
	}
}