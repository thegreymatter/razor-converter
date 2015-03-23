using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Telerik.RazorConverter.Razor.CodeConverters
{

	public static class CodeCOnverters
	{
		public static ICodeConverter[] converters =
		{
			new CssResouceCOnverter(), new FeatureControlConverter(),
			new JSResouceCOnverter(), new SubviewRenderConverter(),
			new ViewComponenetConverter(),new RoutesContextConverter(), 
			new MasterPageContextConverter(), new RenderScriptsAndCSSConverter(),
			new RoutesRenderConverter(), new DomainsConverter(), 
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
			var result =
			 Regex.Replace(codeBlock, @"SubviewRenderer\.Render", "SubviewRenderer.Render");
			return Regex.Replace(result, @"[^.\w]Render\(", "@Html.Render(");
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
			return codeBlock.Replace("ViewBase.ResourceRenderer.StylesheetTags(Require)", "Html.RenderStyleSheet()").Replace("ViewBase.ResourceRenderer.ScriptTags(Require)", "Html.RenderScripts()");
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



}
