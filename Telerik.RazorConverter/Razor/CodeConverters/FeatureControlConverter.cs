using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Telerik.RazorConverter.Razor.CodeConverters
{

	public static class ExpressionCodeConverters
	{
		public static ICodeConverter[] Converters =
		{
			new CssResouceCOnverter(), new FeatureControlConverter(),
			new JSResouceCOnverter(),new ViewComponenetConverter(),
			new RoutesContextConverter(), 
			new MasterPageContextConverter(), new RenderScriptsAndCSSConverter(),
			new RoutesRenderConverter(), new DomainsConverter(), new RenderConverter(), 
		};
	}

	public static class CodeBlockCodeConvertors
	{
		public static ICodeConverter[] Converters =
		{
			new CssResouceCOnverter(), new FeatureControlConverter(),
			new JSResouceCOnverter(), new SubviewRenderConverter(),
			new ViewComponenetConverter(),new RoutesContextConverter(), 
			new MasterPageContextConverter(), new RenderScriptsAndCSSConverter(),
			new RoutesRenderConverter(), new DomainsConverter(),
			new RenderConverter(true), 
		};
	}

	public class MasterPageContextConverter : ICodeConverter
	{
		public string ConvertCodeBlock(string codeBlock)
		{
			return codeBlock.Replace("MasterPageContext.", "Html.MasterPageContext().");
		}
	}

	public class RoutesContextConverter : ICodeConverter
	{
		public string ConvertCodeBlock(string codeBlock)
		{
			return codeBlock.Replace("new Routes().", "Html.Routes().");
		}
	}

	public class DomainsConverter : ICodeConverter
	{
		public string ConvertCodeBlock(string codeBlock)
		{
			return codeBlock.Replace("Domains.", "Html.Domains().");
		}
	}

	public class FeatureControlConverter : ICodeConverter
	{
		public string ConvertCodeBlock(string codeBlock)
		{
			return codeBlock.Replace("this.IsFeatureEnabled", "Html.IsFeatureEnabled");
		}
	}

	public interface ICodeConverter
	{
		string ConvertCodeBlock(string codeBlock);
	}

	public class SubviewRenderConverter : ICodeConverter
	{
		public string ConvertCodeBlock(string codeBlock)
		{
			var searchRegex = new Regex(@"[^.\w]SubviewRenderer.Render\((?<page>.*?)\);?", RegexOptions.Singleline | RegexOptions.Multiline);
			var result =  searchRegex.Replace(codeBlock, m => string.Format("Html.Render({0});", m.Groups["page"].Value.Trim()));

			return result;
		}
	}

	public class RenderConverter : ICodeConverter
	{
		private readonly bool _isCodeBlock;

		public RenderConverter(bool isCodeBlock = false)
		{
			_isCodeBlock = isCodeBlock;
		}

		public string ConvertCodeBlock(string codeBlock)
		{
			var searchRegex = new Regex(@"[^.\w]Render\((?<page>.*?)\);?", RegexOptions.Singleline | RegexOptions.Multiline);
			var result = searchRegex.Replace(codeBlock, m => string.Format("Html.Render({0})".AddAtPrefix(_isCodeBlock), m.Groups["page"].Value.Trim()));
			return result;
		}
	}

	public class RoutesRenderConverter : ICodeConverter
	{
		public string ConvertCodeBlock(string codeBlock)
		{

			return Regex.Replace(codeBlock, @"[^.\w]Routes.", "Html.Routes().");
		}
	}


	public class RenderScriptsAndCSSConverter : ICodeConverter
	{
		public string ConvertCodeBlock(string codeBlock)
		{
			return codeBlock.Replace("ViewBase.ResourceRenderer.StylesheetTags(Require)", "Html.RenderStyleSheet()")
				.Replace("ViewBase.ResourceRenderer.ScriptTags(Require)", "Html.RenderScripts()");
		}
	}

	public class ViewComponenetConverter : ICodeConverter
	{
		public string ConvertCodeBlock(string codeBlock)
		{
			return codeBlock.Replace("ViewComponent<", "Html.ViewComponent<");
		}
	}

	public class JSResouceCOnverter : ICodeConverter
	{
		public string ConvertCodeBlock(string codeBlock)
		{
			return Regex.Replace(codeBlock, "Require\\.Js\\(\"(\\S*)\"\\);", "Html.Resource(x => x.Js(\"$1\"));");
		}
	}

	public class CssResouceCOnverter : ICodeConverter
	{
		public string ConvertCodeBlock(string codeBlock)
		{
			return Regex.Replace(codeBlock, "Require\\.Css\\(\"(\\S*)\"\\);", "Html.Resource(x => x.Css(\"$1\"));");
		}
	}

	public static class StringExt
{
		public static string AddAtPrefix(this string str, bool condition)
		{
			if (condition)
				return "@" + str;

			return str;
		}
}

}
