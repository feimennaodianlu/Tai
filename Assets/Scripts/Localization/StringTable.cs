using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class StringTable
{
	Dictionary<string, string> m_localization = new Dictionary<string, string>();

	public static StringTable Create(string fileName, Func<string, string> xmlGenerator)
	{
		var stringTable = new StringTable();

		stringTable.LoadXml( fileName, xmlGenerator );

		return stringTable;
	}


	void LoadXml(string fileName, Func<string, string> xmlGenerator)
	{
		using( var stringReader = new StringReader( xmlGenerator( fileName ) ) )
		{
			using( var xml = XmlReader.Create( stringReader ) )
			{
				while( xml.Read() )
				{
					if( !xml.IsStartElement() )
						continue;

					if( "Item" == xml.Name )
					{
						var key = xml.GetAttribute( "id" );
						var value = AdjustNewLineCharacter( xml.GetAttribute( "value" ) );

						if( m_localization.ContainsKey( key ) )
							throw new ArgumentException( string.Format( "{0} already exists in the localization", key ) );

						m_localization.Add( key, value );
					}
					else if( "Include" == xml.Name )
						LoadXml( xml.GetAttribute( "file" ), xmlGenerator );
				}
			}
		}
	}

	string AdjustNewLineCharacter(string str)
	{
		if( str.Contains( @"\n" ) )
			return str.Replace( @"\n", Environment.NewLine );

		return str;
	}

	public string this[string key]
	{
		get
		{
			if( !m_localization.ContainsKey( key ) )
				throw new KeyNotFoundException( key + "does not exist in the dict" );

			return m_localization[key];
		}
	}
}