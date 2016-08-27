using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace AStarWebApplication {
    public partial class AStarTraversal : System.Web.UI.Page
    {
        public String _absolutePath = @"C:\Users\shriram\Desktop\Study\Fall 2015\Introduction to AI\";

        public LocationInfo _locNodes = new LocationInfo();
        public List<LocationInfo> _locationsInfo = new List<LocationInfo>();

        public ConnectionInfo _connEdges = new ConnectionInfo();
        public List<ConnectionInfo> _connectionsInfo = new List<ConnectionInfo>();

        public List<LocationInfo> _graphicalNodes = new List<LocationInfo>();

        public String shortestDistance = "ShortestDistance";
        public String fewestLinks = "FewestLinks";

        public LocationInfo _startNode = new LocationInfo();
        public LocationInfo _endNode = new LocationInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Do nothign
        }

        protected void locationsFileUploaded(object sender, EventArgs e)
        {
            //upload the locations file
            string locationFileDescriptor = _absolutePath;
            if (locationsFileUpload.HasFile)
            {
                Session.Add("locationFileName", locationsFileUpload.PostedFile.FileName);
                locationFileDescriptor += locationsFileUpload.PostedFile.FileName;
            }

            _locationsInfo = _locNodes.getLocationsInfo(locationFileDescriptor);

            foreach (LocationInfo _locs in _locationsInfo)
            {
                //populate the drop down boxes with the values once locations are uploaded 
                excludeLocationsNodes.Items.Add(new RadComboBoxItem(_locs._node._node));
                startLocationsList.Items.Add(new RadComboBoxItem(_locs._node._node));
                endLocationsList.Items.Add(new RadComboBoxItem(_locs._node._node));
            }
        }

        protected void connectionsFileUploaded(object sender, EventArgs e)
        {
            //populate the connections
            String locFileName = (String)Session["locationFileName"];
            _locationsInfo = _locNodes.getLocationsInfo(_absolutePath + locFileName);

            String connectionFileDescriptor = _absolutePath;
            if (connectionsFileUpload.HasFile)
            {
                Session.Add("connectionFileName", connectionsFileUpload.PostedFile.FileName);
                connectionFileDescriptor += connectionsFileUpload.PostedFile.FileName;
            }

            _connectionsInfo = _connEdges.getConnectionsInfo(connectionFileDescriptor, _locationsInfo);

            //prepare data to send to script
            String locationNodes = "";
            for (int i = 0; i < _locationsInfo.Count; i++)
                locationNodes += _locationsInfo[i]._node._node + "-" + _locationsInfo[i].xCoOrdinate + "-" + _locationsInfo[i].yCoOrdinate + ",";

            locationNodes = locationNodes.TrimEnd(',');
            Session.Add("locationNodes", locationNodes);
            
            String connectionEdges = "";
            for (int i = 0; i < _connectionsInfo.Count; i++)
                connectionEdges += _connectionsInfo[i]._parentNode._node + "-" + _connectionsInfo[i]._connectingNode._node + ",";

            connectionEdges = connectionEdges.TrimEnd(',');
            Session.Add("connectionEdges", connectionEdges);
            
            //Display the connected graph once the connections are uploaded along with the locations
            ScriptManager.RegisterStartupScript(this, typeof(Page), "GraphParameters", String.Format("populateNodesAndEdges('{0}','{1}','{2}');", "", locationNodes, connectionEdges), true);
        }

        public void updateLocsAndConns(object sender, EventArgs e)
        {
            //Update the connections and locations if there is an excluded node
            startLocationsList.Items.Clear();
            endLocationsList.Items.Clear();
            foreach (RadComboBoxItem excludeNode in excludeLocationsNodes.Items)
            {
                if (!(excludeNode.Checked == true))
                {
                    startLocationsList.Items.Add(new RadComboBoxItem(excludeNode.Text));
                    endLocationsList.Items.Add(new RadComboBoxItem(excludeNode.Text));
                }
            }
        }

        public void SearchBasedHeuristic(object sender, EventArgs e)
        {
            String locFileName = (String)Session["locationFileName"];
            _locationsInfo = _locNodes.getLocationsInfo(_absolutePath + locFileName);
            List<LocationInfo> _graphicalNodes = _locationsInfo;
            _graphicalNodes = _locNodes.getLocationsInfo(_absolutePath + locFileName);

            foreach (RadComboBoxItem excludeNodes in excludeLocationsNodes.Items)
            {
                if (!(excludeNodes.Checked == false))
                    _graphicalNodes = _graphicalNodes.Where(query => query._node._node != excludeNodes.Text).ToList();
            }

            double totalCostOfTraversal = 0.0;
            //convert start and end to location obj
            _startNode = _graphicalNodes.FirstOrDefault(query => query._node._node == startLocationsList.SelectedItem.Text);
            _endNode = _graphicalNodes.FirstOrDefault(query => query._node._node == endLocationsList.SelectedItem.Text);

            String connFileName = (String)Session["connectionFileName"];
            _connectionsInfo = _connEdges.getConnectionsInfo(_absolutePath + connFileName, _graphicalNodes);

            string locationsNodes = (string)Session["locationNodes"];
            string connectionsEdges = (string)Session["connectionEdges"];
            
            //Branch to either shortest distance heuristic based on the search heuristic dropdown
            if (selectHeuristicCombo.SelectedItem.Value == shortestDistance) {
                ShortestDistanceSearchAlgorithm aStarSearch = new ShortestDistanceSearchAlgorithm(_startNode, _endNode, _locationsInfo, _connectionsInfo);
                List<LocationInfo> resultantArray = aStarSearch.AStarTraversal();
                String nodesAndCost = "";
                
                foreach (LocationInfo locationInfo in resultantArray)
                {
                    totalCostOfTraversal += locationInfo._costOfTraversal._g;
                    nodesAndCost += ("<br>Node:" + locationInfo._node._node + "| Interim Traversal Cost:" + locationInfo._costOfTraversal._g + "| Cumulative Traversal Cost:" + totalCostOfTraversal);
                }

                String finalOutputNodes = "";
                foreach (LocationInfo locationInfo in resultantArray)
                {
                    finalOutputNodes += locationInfo._node._node + ',';
                    nodesAndCost += ("-" + locationInfo._node._node);
                }
                finalOutputNodes=finalOutputNodes.TrimEnd(',');
                nodesAndCost += ("<br>Total Cost of Travel: " + resultantArray[resultantArray.Count - 1]._costOfTraversal._g);
                nodesAndCost = nodesAndCost.TrimStart('-');
                path.Text = nodesAndCost;

                //Send the output, the locations and connections info so that vis populates the connected graph
                ScriptManager.RegisterStartupScript(this, typeof(Page), "GraphParameters", String.Format("populateNodesAndEdges('{0}','{1}','{2}');", finalOutputNodes, locationsNodes, connectionsEdges), true); 
            }
            //Branch to the fewest links heuristic based on the search heuristic dropdown
            else if (selectHeuristicCombo.SelectedItem.Value == fewestLinks)
            {
                FewerLinksSearchAlgorithm aStarSearch = new FewerLinksSearchAlgorithm(_startNode, _endNode, _locationsInfo, _connectionsInfo);
                String aStarOutput = "";
                String finalOutputNodes = "";
                String resultWithMinLinks = aStarSearch.searchByLinks();
                resultWithMinLinks = resultWithMinLinks.TrimStart(',');
                resultWithMinLinks = resultWithMinLinks.TrimEnd(',');
                finalOutputNodes = resultWithMinLinks.TrimEnd(',');
                int resultWithLinks = resultWithMinLinks.Count(query => query == ',');
                aStarOutput += ("<br>Path Final:" + resultWithMinLinks);
                aStarOutput += ("<br>Links travelled:" + resultWithLinks);
                path.Text = aStarOutput;

                //Send the output, the locations and connections info so that vis populates the connected graph
                ScriptManager.RegisterStartupScript(this, typeof(Page), "GraphParameters", String.Format("populateNodesAndEdges('{0}','{1}','{2}');", finalOutputNodes, locationsNodes, connectionsEdges), true);
            }
        }
    }
}