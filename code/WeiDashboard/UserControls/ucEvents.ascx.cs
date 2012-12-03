using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telavance.AdvantageSuite.BusinessEntity.Common;
using Telavance.AdvantageSuite.Wei.WeiCommon;

namespace Telavance.AdvantageSuite.Wei.WeiDashboard.UserControls
{
    public partial class ucEvents : System.Web.UI.UserControl
    {
         /// <summary>
        /// Events List
        /// </summary>
        public List<Events> EventsList;

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
        /// Gets or sets the current key value.
        /// </summary>
        /// <value>
        /// The current key value.
        /// </value>
        public string CurrentKeyValue
        {
            get { return (string)ViewState["CurrentKeyValue"]; }
            set { ViewState["CurrentKeyValue"] = value; }
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
                    return "EventID";
                }
                return (string) ViewState["SortExp"];
            }
            set { ViewState["SortExp"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //ucPager.PagerChangedHandler += ucPager_PagerChangedHandler;

                lblAction.Text = "";

                if (!IsPostBack)
                {
                   // ucPager.PageSize = 5;
                    //Get Grid data                    
                    GetData();
                    
                    BindGrid();
                }
                
            }
            catch (Exception ex)
            {
                LogUtil.log("Error in Page_Load", ex);
            }

        }

        /// <summary>
        /// Loads the page.
        /// </summary>
        /// <param name="id">The id.</param>
        public void LoadPage(string id)
        {
            try
            {
   
                GetData();
                BindGrid();
            }
            catch (Exception ex)
            {
                LogUtil.log("Error in LoadPage method", ex);
            }

        }

        /// <summary>
        /// Handles the Click event of the btnRefresh control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                GetData();
                BindGrid();
            }
            catch (Exception ex)
            {
                LogUtil.log("Error in btnSearch_Click", ex);
            }
        }

        /// <summary>
        /// Main Method to get data from DB
        /// </summary>
        /// <returns></returns>
        private List<Events> GetData()
        {
            if (string.IsNullOrEmpty(CurrentKeyValue))
            {
                return null;
            }
            try
            {
                EventsList = EventsCommunicator.GetAllEventsByObject(SourceType, CurrentKeyValue);


                if (EventsList != null && EventsList.Count > 0 && !string.IsNullOrEmpty(SortExp))
                {
                    EventsList =
                        Utilities.LinqHelpers<BEEvent>.Sort(EventsList.AsQueryable(), SortExp,
                                                            SortDirection).ToList();
                    Session["Detail"] = EventsList[0].Details;
                }
                return EventsList;
            }

            catch (Exception ex)
            {
                LogUtil.log("Error in GetData", ex);
                return null;
            }
        }
        

        /// <summary>
        /// Bind Gridview to data sourse from serach and reset 
        /// </summary>
        private void BindGrid()
        {
            try
            {
                ucPager.SetPaggerControls(EventsList != null ? EventsList.Count : 0);

                grdView.PagerSettings.Visible = false;
                grdView.PageIndex = ucPager.CurrentPageNumber - 1;
                grdView.PageSize = ucPager.PageSize;
                grdView.DataSource = EventsList;
                grdView.DataBind();
            }
            catch (Exception ex)
            {
                LogUtil.log("Error in BindGrid", ex);
            }
        }

        /// <summary>
        /// Handles the RowCreated event of the grdView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void grdView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (!string.IsNullOrEmpty(SortExp))
                {
                    Utilities.WebUI.AddSortImage(e.Row, grdView, SortExp, SortDirection);
                }
            }
            //highlightGridRow(e.Row);
        }

        /// <summary>
        /// Handles the Sorting event of the grdView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void grdView_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(SortExp))
                {
                    if (string.Compare(e.SortExpression, SortExp, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        SortDirection = (SortDirection == SortDirection.Ascending)
                                            ? SortDirection.Descending
                                            : SortDirection.Ascending;
                    }
                    else
                    {
                        SortExp = e.SortExpression;
                    }

                   // ucPager.CurrentPageNumber = 1;

                    GetData();
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Ucs the pager_ pager changed handler.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ucPager_PagerChangedHandler(EventArgs e)
        {
            GetData();
            BindGrid();

        }

    }
        }
    
