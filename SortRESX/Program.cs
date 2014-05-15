using System;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SortRESX
{
	/// <summary>
	/// Assume two inputs, a source .resx file path and a target .resx file path.  
	/// The program reads the source and writes a sorted version of it to the
	/// target .resx file.
	/// </summary>
	public static class Program
	{
		static void Main(string[] args)
		{
			// Check parameters
			if (args.Length != 2 )
			{
				ShowHelp();
				return;
			}
			try
			{
				// Create a linq XML document from the source.
				var doc = XDocument.Load(args[0]);
				// Create a sorted version of the XML
				var sortedDoc = SortDataByName(doc);
				// Save it to the target
				sortedDoc.Save(args[1]);
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine("Error loading resx file {0} or error saving it to {1}: {2}", args[0], args[1], ex.Message);
			}
		}

		/// <summary>
		/// Use Linq to sort the elements.  The comment, schema, resheader, assembly, metadata, data appear in that order, 
		/// with resheader, assembly, metadata and data elements sorted by name attribute.
		/// </summary>
		/// <param name="resx"></param>
		/// <returns></returns>
		private static XDocument SortDataByName(XDocument resx)
		{
			return new XDocument(
				new XElement(resx.Root.Name,
					from comment in resx.Root.Nodes() where comment.NodeType == XmlNodeType.Comment select comment,
					from schema in resx.Root.Elements() where schema.Name.LocalName == "schema" select schema,
					from resheader in resx.Root.Elements("resheader") orderby (string) resheader.Attribute("name") select resheader,
					from assembly in resx.Root.Elements("assembly") orderby (string) assembly.Attribute("name") select assembly,
					from metadata in resx.Root.Elements("metadata") orderby (string)metadata.Attribute("name") select metadata,
					from data in resx.Root.Elements("data") orderby (string)data.Attribute("name") select data
				)
			);
		}

		/// <summary>
		/// Write invocation instructions to stderr.
		/// </summary>
		private static void ShowHelp()
		{
			var sExeName = Process.GetCurrentProcess().ProcessName;
		    Console.Error.WriteLine("Command line format");
            Console.Error.WriteLine("{0} <input resx file> <output resx file>", sExeName);
		}
	}
}
