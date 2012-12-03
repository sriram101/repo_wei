using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telavance.AdvantageSuite.BusinessEntity.Common;

namespace Telavance.AdvantageSuite.Wei.WeiDashboard.UserControls
{
    public partial class ucMessageViewer : System.Web.UI.UserControl
    {
        // Delegate declaration
        public delegate void OnButtonClick(string strValue);

        // Event declaration
        public event OnButtonClick BtnHandler;

        /// <summary>
        /// Violation Status List
        /// </summary>
        public List<Messages> Messages;


        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        /// <value>
        /// The sort direction.
        /// </value>
        public SortDirection SortDirection
        {
            get
            {
                if (ViewState["SortDirection"] == null)
                {
                    return SortDirection.Descending;
                }
                return (SortDirection) ViewState["SortDirection"];
            }
            set { ViewState["SortDirection"] = value; }
        }

        /// <summary>
        /// Gets or sets the sort exp.
        /// </summary>
        /// <value>
        /// The sort exp.
        /// </value>
        public string SortExp
        {
            get
            {
                if (ViewState["SortExp"] == null)
                {
                    return "Code";
                }
                return (string) ViewState["SortExp"];
            }
            set { ViewState["SortExp"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}