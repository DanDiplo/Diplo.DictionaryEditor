using System;
using System.Net.Http.Formatting;
using umbraco.BusinessLogic.Actions;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using Umbraco.Web.WebApi.Filters;

namespace Diplo.Dictionary.Sections
{
    /// <summary>
    /// Custom tree controller for Dictionary Editor
    /// </summary>
    [UmbracoApplicationAuthorize(DiploConstants.SectionAlias)]
    [Tree(DiploConstants.SectionAlias, "diploDictExport", "Export Dictionary Items")]
    [PluginController(DiploConstants.SectionAlias)]
    public class ExportTreeController : TreeController
    {
        /// <summary>
        /// Gets the nodes that form the tree
        /// </summary>
        /// <param name="id">The tree identifier</param>
        /// <param name="qs">Any posted parameters</param>
        /// <returns>A collection of tree nodes</returns>
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection qs)
        {
            TreeNodeCollection tree = new TreeNodeCollection
            {
                CreateTreeNode("diploDictExport", id, qs, "Export Dictionary", "icon-page-down", false)
            };

            return tree;
        }

        /// <summary>
        /// Gets the menu items that form the right-click menu
        /// </summary>
        /// <param name="id">The tree identifier</param>
        /// <param name="qs">Any posted parameters</param>
        /// <returns>The menu items</returns>
        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection qs)
        {
            var menu = new MenuItemCollection();

            if (id == Constants.System.Root.ToInvariantString())
            {
                menu.Items.Add<RefreshNode, ActionRefresh>("Reload", true);
            }

            return menu;
        }


    }
}
