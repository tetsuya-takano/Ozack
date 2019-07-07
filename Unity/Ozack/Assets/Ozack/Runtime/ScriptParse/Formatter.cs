using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ozack
{
	public interface IFormatter : IDisposable
	{
		IEnumerable<string> Split( string contents );
	}

	/// <summary>
	/// スクリプトファイルをコマンド/パラメータの羅列にする
	/// </summary>
	public class Formatter : IFormatter
	{
		//==============================
		// 変数
		//==============================
		private string[] m_separeter = new string[ 0 ];
		private StringSplitOptions m_option = StringSplitOptions.None;
		//==============================
		// 関数
		//==============================

		public Formatter( 
			string[] separator, 
			StringSplitOptions option = StringSplitOptions.None 
		)
		{
			m_separeter = separator;
			m_option = option;
		}

		public void Dispose()
		{
			m_separeter = null;
		}
		public IEnumerable<string> Split( string contents )
		{
			var list = new List<string>();
			using (var reader = new StringReader(contents))
			{
				while( reader.Peek() > -1 )
				{
					// 1行
					var line = reader.ReadLine();
					// ダブルクオーテーション数が足りなければ次も連結
					while (Count(line, "\"") % 2 == 1 )
					{
						if( reader.Peek() <= 0)
						{
							break;
						}
						var next = reader.ReadLine();
						// 改行を外さない
						var prev = line
									.Replace("\n", Environment.NewLine )
									.Replace("\r", Environment.NewLine )
									.Replace("\r\n", Environment.NewLine);
						line = string.Concat(prev, Environment.NewLine, next);
					}
					// ダブルクオーテーション削る
					line = line.Replace("\"", string.Empty);
					// 出来上がった1行を分解して返す
					list.AddRange(line.Split(m_separeter, m_option));
				}
			}
			// 分割
			return list;
		}

		private int Count(string line, string target)
		{
			return line.Length - (line.Replace(target, string.Empty).Length);
		}
	}
}