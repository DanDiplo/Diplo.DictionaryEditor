using System;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;

namespace Diplo.Dictionary.Sections
{
    public class RegisterSection : ApplicationEventHandler
    {
        /// <summary>
        /// Registers new section during application start if it doesn't already exist
        /// </summary>
        /// <param name="umbraco">Umbraco base</param>
        /// <param name="context">Umbraco Application context</param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbraco, ApplicationContext context)
        {
            // Gets a reference to the section (if already added)
            Section section = context.Services.SectionService.GetByAlias(DiploConstants.SectionAlias);

            if (section != null)
                return;

            // Add a new Dictionary Editor section
            context.Services.SectionService.MakeNew("Diplo Dictionary Editor", DiploConstants.SectionAlias, "icon-umb-translation");

            // Adds access to members of the admin group
            var adminGroup = context.Services.UserService.GetUserGroupByAlias("admin");

            if (adminGroup != null)
            {
                if (!adminGroup.AllowedSections.Contains(DiploConstants.SectionAlias))
                {
                    try
                    {
                        adminGroup.AddAllowedSection(DiploConstants.SectionAlias);
                        context.Services.UserService.Save(adminGroup);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error<RegisterSection>($"Could not grant access to {DiploConstants.SectionAlias} to the admin User group...", ex);
                    }
                }
            }

            // Attempts to add a new custom dashboard too!
            DashboardConfigurator.UpdateDashboard();
        }
    }
}
