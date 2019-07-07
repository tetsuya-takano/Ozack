using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozack
{
	public interface IOzackParseSystem<TCommand> :IDisposable
	{
		IReadOnlyList<TCommand> Parse(string contents);
	}
	public class OzackParseSystem<TCommand> : IOzackParseSystem<TCommand>
	{
		//======================================
		// enum
		//======================================
		private enum ParseState
		{
			None,//未処理
			Parsing,//処理中
		}

		//======================================
		// 変数
		//======================================
		private IFormatter m_formatter = null;
		private IOzackCommandBuilder<TCommand> m_builder = null;

		//======================================
		// 関数
		//======================================

		public OzackParseSystem( IFormatter formatter, IOzackCommandBuilder<TCommand> builder)
		{
			m_formatter = formatter;
			m_builder = builder;
		}

		public void Dispose()
		{
			m_formatter?.Dispose();
			m_builder?.Dispose();
		}
		public IReadOnlyList<TCommand> Parse( string contents )
		{
			// コマンドと値の一覧
			var lines = m_formatter.Split( contents );
			// 作業
			var currentCmd = string.Empty;
			var valueList = new List<string>();
			var cmdNum = 0;
			var state = ParseState.None;
			// コマンドリスト
			var cmdList = new List<TCommand>();

			// 一覧を漁ってコマンドとして解釈する
			foreach (var arg in lines)
			{
				// コマンド名だったら
				if( m_builder.IsCmd( arg ))
				{
					//解析開始
					if (state == ParseState.None)
					{
						currentCmd = arg; // コマンド名
						// 解析開始
						state = ParseState.Parsing;
						valueList.Clear();
						continue;
					}
					// 解析完了
					// コマンドセット読み込み完了したら1コマンド作って変換
					var args = new OzackComandArgs( currentCmd, cmdNum, valueList.ToArray());
					var cmd = m_builder.Build(args);
					// 記録
					cmdList.Add(cmd);

					// 次
					currentCmd = arg;
					valueList.Clear();
					cmdNum++;
					continue;
				}
				// ただのパラメータ
				valueList.Add(arg);
			}

			return cmdList;
		}
	}
}