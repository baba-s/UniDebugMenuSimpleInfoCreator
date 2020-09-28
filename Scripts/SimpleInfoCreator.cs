using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kogane.DebugMenu
{
	/// <summary>
	/// テキストのリストにシンプルな文字列を表示するクラス
	/// </summary>
	public class SimpleInfoCreator : ListCreatorBase<ActionData>
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		private readonly IReadOnlyList<ActionData> m_sourceList;
		private readonly string                    m_text;

		//==============================================================================
		// 変数
		//==============================================================================
		private ActionData[] m_list;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public override int Count => m_list.Length;

		public override ActionData[] OptionActionList =>
			new[]
			{
				new ActionData( "コピー", () => GUIUtility.systemCopyBuffer = m_text ),
			};

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 作成して返します
		/// </summary>
		public SimpleInfoCreator( string text, int count = 80 )
		{
			m_text = text;

			if ( string.IsNullOrWhiteSpace( text ) )
			{
				m_sourceList = new ActionData[0];
				return;
			}

			m_sourceList = text
					.Split( '\n' )
					.SelectMany( x => SubstringAtCount( x, count ) )
					.Select( x => new ActionData( x ) )
					.ToArray()
				;
		}

		/// <summary>
		/// リストの表示に使用するデータを作成します
		/// </summary>
		protected override void DoCreate( ListCreateData data )
		{
			m_list = m_sourceList
					.Where( c => data.IsMatch( c.Text ) )
					.ToArray()
				;

			if ( data.IsReverse )
			{
				Array.Reverse( m_list );
			}
		}

		/// <summary>
		/// 指定されたインデックスの要素の表示に使用するデータを返します
		/// </summary>
		protected override ActionData DoGetElemData( int index )
		{
			return m_list.ElementAtOrDefault( index );
		}

		/// <summary>
		/// 指定された文字列を指定された文字数で分割して返します
		/// </summary>
		private static string[] SubstringAtCount( string self, int count )
		{
			var result = new List<string>();
			var length = ( int ) Math.Ceiling( ( double ) self.Length / count );

			for ( int i = 0; i < length; i++ )
			{
				int start = count * i;

				if ( self.Length <= start )
				{
					break;
				}

				if ( self.Length < start + count )
				{
					result.Add( self.Substring( start ) );
				}
				else
				{
					result.Add( self.Substring( start, count ) );
				}
			}

			return result.ToArray();
		}
	}
}