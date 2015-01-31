namespace Telerik.RazorConverter.Razor.Converters
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Telerik.RazorConverter.Razor.DOM;
    using Telerik.RazorConverter.WebForms.DOM;

    public class DirectiveConverter : INodeConverter<IRazorNode>
    {
        private IRazorDirectiveNodeFactory DirectiveNodeFactory
        {
            get;
            set;
        }

        public DirectiveConverter(IRazorDirectiveNodeFactory nodeFactory)
        {
            DirectiveNodeFactory = nodeFactory;
        }

        public IList<IRazorNode> ConvertNode(IWebFormsNode node)
        {
            var result = new List<IRazorNode>();

            var directiveNode = node as IWebFormsDirectiveNode;

            if (directiveNode != null)
            {
                if (directiveNode.Attributes.ContainsKey("inherits"))
                {
                    var inheritsFrom = directiveNode.Attributes["inherits"];
					var viewPageGenericType = new Regex("(?:Ohio.Web.Config.Rendering.SubView|Ohio.Web.UI.Fusion.FusionView|SubView)<(?<type>.*)>");
                    var typeMatch = viewPageGenericType.Match(inheritsFrom);
                    if (typeMatch.Success)
                    {
                        result.Add(DirectiveNodeFactory.CreateDirectiveNode("model", typeMatch.Result("${type}")));
                    }
					else if (inheritsFrom != "Ohio.Web.Config.Rendering.SubView" && inheritsFrom != "Ohio.Web.UI.Fusion.FusionView")
                    {
                        result.Add(DirectiveNodeFactory.CreateDirectiveNode("inherits", directiveNode.Attributes["inherits"]));
                    }
                }
                else if (directiveNode.Attributes.ContainsKey("namespace") &&
                         directiveNode.Directive == DirectiveType.Import)
                {
                    /* Case of of a using directive */
                    var imports = directiveNode.Attributes["namespace"];

                    if (!string.IsNullOrEmpty(imports))
                    {
                        result.Add(DirectiveNodeFactory.CreateDirectiveNode("using", directiveNode.Attributes["namespace"]));
                    }
                }
            }

            return result;
        }

        public bool CanConvertNode(IWebFormsNode node)
        {
            return node as IWebFormsDirectiveNode != null;
        }
    }
}
