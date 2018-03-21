using System;
using System.Linq;
using System.Xml.Linq;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;

namespace Diplo.Dictionary.Sections
{
    internal class DashboardConfigurator
    {
        /// <summary>
        /// Attemps to add a new custom dashboard section - hacky!
        /// </summary>
        /// <remarks>
        /// This is hacky, but can't see a better way ATM as no API for this....
        /// </remarks>
        internal static void UpdateDashboard()
        {
            string dashboardConfigPath = IOHelper.MapPath("~/config/Dashboard.config");

            if (!System.IO.File.Exists(dashboardConfigPath))
            {
                return;
            }

            try
            {
                XDocument dashboard = XDocument.Load(dashboardConfigPath);

                bool exists = dashboard.Root.Elements("section").Any(s => s.HasAttributes && s.Attribute("alias") != null && s.Attribute("alias").Value.Equals(DiploConstants.SectionAlias, StringComparison.OrdinalIgnoreCase));

                if (!exists)
                {
                    string xmlDashboard = @"<section alias=""diploDictionaryEdit"">
                    <areas>
                      <area>diploDictionaryEdit</area>
                    </areas>
                    <tab caption =""Info"">
                       <control>/App_Plugins/DiploDictionaryEdit/intro.html</control>
                     </tab>
                   </section>";

                    XElement elem = XElement.Parse(xmlDashboard);

                    dashboard.Root.Add(elem);

                    dashboard.Root.Save(dashboardConfigPath);
                }
            }
            catch (System.IO.IOException ex)
            {
                LogHelper.Error<DashboardConfigurator>($"IO Exception trying to update Dashboard.config with {DiploConstants.SectionAlias} section", ex);
            }
            catch (System.Xml.XmlException ex)
            {
                LogHelper.Error<DashboardConfigurator>($"XML Exception trying to update Dashboard.config with {DiploConstants.SectionAlias} section", ex);
            }
            catch (Exception ex)
            {
                LogHelper.Error<DashboardConfigurator>($"Exception trying to update Dashboard.config with {DiploConstants.SectionAlias} section", ex);
            }
        }
    }
}
