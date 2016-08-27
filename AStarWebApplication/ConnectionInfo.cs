using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AStarWebApplication
{
    public class ConnectionInfo
    {
        public Node _parentNode;        // Variable for the parent node.
        public Node _connectingNode;    // Variable for a connecting node.
        public double _distance;        // Variable for the distance between two nodes.

        public ConnectionInfo()
        {
            //Empty constructor for instantiation
        }

        // Constructor with all the default input parameters.
        public ConnectionInfo(Node parentNode, Node connectingNode, double distance)
        {
            _parentNode = parentNode;
            _connectingNode = connectingNode;
            _distance = distance;
        }

        /// <summary>
        /// Method: getConnectionsInfo
        /// Input: file descriptor and the location list
        /// Returns: list of connections
        /// </summary>
        public List<ConnectionInfo> getConnectionsInfo(String fileDescriptor, List<LocationInfo> locations)
        {
            String[] parseLines = System.IO.File.ReadAllLines(fileDescriptor);  //parse the input file line by line
            String endOfFile = "END";

            // variables used for getting the connections
            String[] cons;
            LocationInfo locationVertex, locationLinkedVertex;
            List<ConnectionInfo> connections = new List<ConnectionInfo>();

            // run a for loop for each line and get corresponding details
            foreach (String eachLine in parseLines)
            {
                if (!eachLine.Equals(endOfFile))
                {  //END represents end of file
                    cons = eachLine.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    if (locations.FirstOrDefault(item => item._node._node == cons[0]) != null)
                    {// location specific details of the vertex node
                        var vertex = locations.FirstOrDefault(item => item._node._node == cons[0]);
                        int x1 = vertex.xCoOrdinate;
                        int y1 = vertex.yCoOrdinate;

                        // construct a location detail of the vertex
                        locationVertex = new LocationInfo(vertex._node, x1, y1, new CostOfTraversal(0.0, 0.0), null);

                        int edges = Int32.Parse(cons[1]);   // get the number of neighbors for a particular node

                        // iterate over a loop and get the location details of the neighbors
                        for (int i = 1; i <= edges; i++)
                        {
                            if (locations.FirstOrDefault(item => item._node._node == cons[1 + i]) != null) {
                                // location specific details of the neighbor node
                                var linkedVertex = locations.FirstOrDefault(item => item._node._node == cons[1 + i]);
                                int x2 = linkedVertex.xCoOrdinate;
                                int y2 = linkedVertex.yCoOrdinate;

                                // construct a location detail of the vertex
                                locationLinkedVertex = new LocationInfo(linkedVertex._node, x2, y2, new CostOfTraversal(0.0, 0.0), null);

                                // calculate the distance between both the nodes and store it in the connections bean
                                double euclideanDistance = getEuclideanDistance(locationVertex, locationLinkedVertex);
                                ConnectionInfo connectionInfo = new ConnectionInfo(vertex._node, linkedVertex._node, euclideanDistance);
                                connections.Add(connectionInfo);    // make a list of the various connection beans
                            }
                        }
                    }
                }
            }
            return connections;
        }

        /// <summary>
        /// Method: getEuclideanDistance
        /// Input: reference and to locations
        /// Returns: double format of euclidean distance
        /// </summary>
        public static double getEuclideanDistance(LocationInfo locationReference, LocationInfo locationTo)
        {
            double xComponent = Math.Pow((locationReference.xCoOrdinate - locationTo.xCoOrdinate), 2);
            double yComponent = Math.Pow((locationReference.yCoOrdinate - locationTo.yCoOrdinate), 2);

            return Math.Sqrt(xComponent + yComponent);
        }
    }
}