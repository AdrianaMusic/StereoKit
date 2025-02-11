﻿using System.Linq;

namespace StereoKitDocumenter
{
	static class StringHelper
	{
		public static string CleanForDescription(string text)
		{
			return text.Replace('\n', ' ')
						.Replace('\r', ' ')
						.Replace(':', '.')
						.Replace("`", "");
		}

		public static string CleanForTable(string text)
		{
			return text.Replace('\n', ' ')
						.Replace('\r', ' ');
		}

		public static string CleanMultiLine(string text)
		{
			return string.Join("\n", text
				.Split('\n')
				.Select(a=>a.Trim())
				.ToArray());
		}

		public static string TypeName(string type)
		{
			switch(type)
			{
				case "Single" : return "float";
				case "Double" : return "double";
				case "Int32"  : return "int";
				case "UInt32" : return "uint";
				case "String" : return "string";
				case "Boolean": return "bool";
				case "Void"   : return "void";
				default: {
					return Program.TryGetClass(type, out DocClass typeDoc)
						? $"[{type}]({typeDoc.UrlName})"
						: type;
				}
			}
		}
	}
}
