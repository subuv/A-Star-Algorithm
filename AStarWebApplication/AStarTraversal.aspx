<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AStarTraversal.aspx.cs" Inherits="AStarWebApplication.AStarTraversal" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" style="height:100%">
    <head runat="server">
        <title>A Star Heuristic Search</title>
        <link rel="stylesheet" href="[file path]/kendo.common.min.css" />
        <link rel="stylesheet" href="[file path]/kendo.default.min.css" />
        <script src="http://visjs.org/dist/vis.js"></script>
    </head>
    <body style="height:100%; margin:0px"">
        <form id="AStarWebApplicationForm" runat="server" style="height:100%">
            <style type="text/css">
                .customCSS
                {
                    color: white;
                    font-family:Arial;
                    font-size: 12px;
                }
            </style>
            <telerik:RadScriptManager runat="server" ID="AStarRADScriptManager" />
                <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" Skin="Black" />
                <telerik:RadSplitter runat="server" ID="AStarSplitter" Orientation="Vertical" width="100%" height="100%" ResizeWithBrowserWindow="true" Skin="Black">
                <telerik:RadSplitBar runat="server" ID="AStarSplitBar" ResizeStep="10"/>
                <telerik:RadPane runat="server" id="GraphPane" Width="65%" Height="100%" BackColor="#999999">
                    <div id="viz"></div>
                    <script>
                        function populateNodesAndEdges(searchPath, locationList, connectionList) {
                            var nodes = []; //nodes to be passed to vis
                            var nodesPath = []; //to determine the search path
                            var edges = []; //edges to be passed to vis

                            if (searchPath)
                                nodesPath = searchPath.split(",");

                            //Create a list of connections that show the edges
                            var tempEdges = connectionList.split(",");
                            for (var conLoop = 0; conLoop < tempEdges.length; conLoop++) {
                                var isConnSet = 1;
                                var tempConns = tempEdges[conLoop].split("-");
                                for (var j = 1; j < nodesPath.length; j++) {
                                    if (tempConns[0] == nodesPath[j - 1] && tempConns[1] == nodesPath[j]) {
                                        isConnSet = 0;
                                        edges[conLoop] = { from: tempConns[0], to: tempConns[1], arrows: { to: { enabled: true } }, color: 'yellow' };
                                    }
                                }
                                if (isConnSet) {
                                    edges[conLoop] = { from: tempConns[0], to: tempConns[1], arrows: { to: { enabled: true } }, color: 'white' };
                                }
                            }

                            //Create a list of the locations that can be shown as nodes
                            var temporaryLocs = locationList.split(",");
                            for (var locLoop = 0; locLoop < temporaryLocs.length; locLoop++) {
                                var isLocSet = 1;
                                var tempLocs = temporaryLocs[locLoop].split("-");
                                for (var j = 0; j < nodesPath.length ; j++) {
                                    if (tempLocs[0] == nodesPath[j]) {
                                        isLocSet = 0;
                                        nodes[locLoop] = { label: tempLocs[0], id: tempLocs[0], x: tempLocs[1], y: tempLocs[2], color: { background: 'yellow' } };
                                    }
                                }
                                if (isLocSet) {
                                    nodes[locLoop] = { label: tempLocs[0], id: tempLocs[0], x: tempLocs[1], y: tempLocs[2], color: { background: 'white' } };
                                }
                            }

                            var container = document.getElementById('viz');

                            //Pass the populated nodes and edges to show the data in the graph in vis.
                            var data = { nodes: nodes, edges: edges };
                            var options = { width: '700px', height: '700px' };
                            var network = new vis.Network(container, data, options);
                        }
                    </script>
                </telerik:RadPane>
                <telerik:RadPane runat="server" id="UserInputsPane" Width="35%" Height="100%" BackColor="#666666">
                    <telerik:RadSplitter runat="server" id="AStarSplitter2" Orientation="Horizontal"  ResizeWithBrowserWindow="true">
                    <telerik:RadPane runat="server" id="TopPane" Width="100%" Height="100%">
                    <asp:Table ID="AStarInputTable" runat="server"> 
                        <asp:TableRow></asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="locationsLabel" runat="server" Text="Locations" CssClass="customCSS"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:FileUpload ID="locationsFileUpload" runat="server"/>
                            </asp:TableCell>
                            <asp:TableCell>
                                <telerik:RadButton ID="locationsFileUploadButton" Text="Upload" runat="server" OnClick="locationsFileUploaded">
                                </telerik:RadButton>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="connectionsLabel" runat="server" CssClass="customCSS" Text="Connections"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:FileUpload ID="connectionsFileUpload" runat="server"/>
                            </asp:TableCell>
                            <asp:TableCell>
                                <telerik:RadButton ID="connectionsFileUploadButton" Text="Upload" runat="server" OnClick="connectionsFileUploaded"></telerik:RadButton>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="excludeNodesLabel" runat="server" Font-Family="Verdana" Font-Size="15px" CssClass="customCSS" Text="Exclusion Nodes"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <telerik:RadComboBox ID="excludeLocationsNodes" runat="server" CheckBoxes="true" Width="250" AutoPostBack="false" OnSelectedIndexChanged ="updateLocsAndConns"  Skin="BlackMetroTouch"></telerik:RadComboBox>
                                <telerik:RadButton ID="excludeLocationsNodesButton" runat="server" Text="Exclude" AutoPostBack="true" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Literal ID="literals" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="startNodeLabel" runat="server" CssClass="customCSS" Text="Start"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <telerik:RadComboBox ID="startLocationsList" runat="server"></telerik:RadComboBox>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="endNodeLabel" runat="server" CssClass="customCSS" Text="End"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <telerik:RadComboBox ID="endLocationsList" runat="server"></telerik:RadComboBox>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="selectHeuristicLabel" runat="server" CssClass="customCSS" Text="Heuristic Search"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TableCell>
                                    <telerik:RadComboBox ID="selectHeuristicCombo" runat="server">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="ShortestDistance" Text="Shortest Distance" />
                                            <telerik:RadComboBoxItem Value="FewestLinks" Text="Fewest Links" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </asp:TableCell>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow></asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell>
                                    <telerik:RadButton ID="selectHeuristicButton" OnClick="SearchBasedHeuristic" runat="server" Text="A Star Search"></telerik:RadButton>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        </telerik:RadPane>
                        <telerik:RadPane ID="bottomRightPane" runat="server" Height="50%" Width="100%">
                            <asp:Label ID="ResultsPane" runat="server" Text="Results" CssClass="customCSS"></asp:Label>
                            <asp:Label ID="path" runat="server" Text="Path" CssClass="customCSS"></asp:Label>
                            <asp:Label ID="pathInfo" runat="server" CssClass="customCSS"></asp:Label>
                        </telerik:RadPane>
                    </telerik:RadSplitter>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </form>
    </body>
</html>