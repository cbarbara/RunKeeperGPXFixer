using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RunKeeperGPXFixer
{
	class Program
	{
		static void Main( string[] args )
		{
			if ( args.Length != 1 )
			{
				return;
			}
			string fileName = args[ 0 ];
			StringBuilder fileContent = new StringBuilder( System.IO.File.ReadAllText( fileName ) );
			
			XmlDocument xdoc = new XmlDocument();
			xdoc.LoadXml( fileContent.ToString() );

			System.Xml.XmlNamespaceManager xmlnsManager = new System.Xml.XmlNamespaceManager( xdoc.NameTable );
			xmlnsManager.AddNamespace( "gpx", "http://www.topografix.com/GPX/1/0" );

			XmlNodeList timeNodes = xdoc.SelectNodes( "//gpx:time", xmlnsManager );
			foreach ( XmlNode timeNode in timeNodes )
			{
				string[] split1 = timeNode.InnerText.Split( 'T' );
				string[] splitDays = split1[ 0 ].Split( '-' );
				string[] splitHours = split1[ 1 ].Replace( "Z", "" ).Split( ':' );

				fileContent.Replace( timeNode.InnerText, new DateTime( int.Parse( splitDays[ 0 ] ), int.Parse( splitDays[ 1 ] ), int.Parse( splitDays[ 2 ] ),
					int.Parse( splitHours[ 0 ] ), int.Parse( splitHours[ 1 ] ), int.Parse( splitHours[ 2 ] ) ).ToUniversalTime().ToString( "s" ) + "Z" );
			}

			System.IO.File.WriteAllText( fileName.Replace( ".gpx", "-fix.gpx" ), fileContent.ToString() );
		}
	}
}
