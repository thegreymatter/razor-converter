using System.Diagnostics;

namespace Telerik.RazorConverter.Razor.Converters
{
    using System.ComponentModel.Composition;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;

    [Export(typeof(IWebFormsConverter<IRazorNode>))]
    public class WebFormsToRazorConverter : IWebFormsConverter<IRazorNode>
    {
        private IRazorNodeConverterProvider NodeConverterProvider
        {
            get;
            set;
        }

        [ImportingConstructor]
        public WebFormsToRazorConverter(IRazorNodeConverterProvider converterProvider)
        {
            NodeConverterProvider = converterProvider;
        }

        public IDocument<IRazorNode> Convert(IDocument<IWebFormsNode> srcDoc)
        {
	   
            var rootNode = new RazorNode();

	        //rootNode.Children.Add(new RazorDirectiveNode("using", "Ohio.Web.Config.X.Infrastructure"));
			//rootNode.Children.Add(new RazorTextNode("\r\n"));
	        //rootNode.Children.Add(new RazorDirectiveNode("using", "Ohio.Web.UI.Views.X"));
			//rootNode.Children.Add(new RazorTextNode("\r\n"));

            foreach (var srcNode in srcDoc.RootNode.Children)
            {
                foreach (var converter in NodeConverterProvider.NodeConverters)
                {
                    if (converter.CanConvertNode(srcNode))
                    {
                        if (srcNode.Type == NodeType.Directive && srcNode.Attributes.ContainsKey("namespace"))
                        {
                            var i = 1;
                        }

                        foreach (var dstNode in converter.ConvertNode(srcNode))
                        {
                            rootNode.Children.Add(dstNode);
                        }
                    }
                }
            }
			
            return new Document<IRazorNode>(rootNode);
        }
    }
}
