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
    [Tree(DiploConstants.SectionAlias, "diploDictEditor", "Edit Dictionary Items")]
    [PluginController(DiploConstants.SectionAlias)]
    public class EditorTreeController : TreeController
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
                CreateTreeNode("diploDictEditor", id, qs, "Edit Dictionary", "icon-flag", false),
                CreateTreeNode("diploDictExport", id, qs, "Export Dictionary", "icon-page-down", false, $"{DiploConstants.SectionAlias}/diploDictEditor/export/csv"),
                CreateTreeNode("diploDictImport", id, qs, "Import Dictionary", "icon-page-up", false, $"{DiploConstants.SectionAlias}/diploDictEditor/import/csv")
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
