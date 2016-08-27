using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AStarWebApplication
{
    class Program
    {
        public static List<LocationInfo> locationsInfo;     //Class level variable for list of locations
        public static List<ConnectionInfo> connectionsInfo; //Class level variable for list of connections
        public static String startNode, endNode;            //Class level variable for start and end nodes
        public static String locationsFilePath, connectionsFilePath;  //Class level variable for file paths

        public static void Main(String[] args)
        {
            startNode = null;
            endNode = null;

            // file descriptor for location
            //String locFileDesciptor = @"C:\Users\shriram\Desktop\Study\Fall 2015\Introduction to AI\locsamp.txt";
            // get location information using the locsamp file
            //Console.WriteLine("Enter the path of the locations file");
            //locationsFilePath = Console.ReadLine();

            //locationsInfo = getLocationsInfo(locationsFilePath);

            //// file descriptor for connections
            ////String connFileDescriptor = @"C:\Users\shriram\Desktop\Study\Fall 2015\Introduction to AI\connsamp.txt";
            //// get connection information using the connsamp file
            //Console.WriteLine("Enter the path of the connections file");
            //connectionsFilePath = Console.ReadLine();

            //connectionsInfo = getConnectionsInfo(connectionsFilePath, locationsInfo);

            //collectUserInput();     //collect start and end nodes as input from the user

            //double totalCostOfTraversal = 0.0;  //default the cost of traversal to 0

            // use the linq query feature in c# to find out the start and end nodes 
            // are available from the locations info list
            //LocationInfo start = locationsInfo.FirstOrDefault(item => item._node._node == startNode);
            //LocationInfo end = locationsInfo.FirstOrDefault(item => item._node._node == endNode);

            //// construct the a star search algorithm for the details provided
            //SearchAlgorithm aStarSearch = new SearchAlgorithm(start, end, locationsInfo, connectionsInfo);

            //// get the result of the traversal along with the cost of traversal
            //List<LocationInfo> resultantArray = aStarSearch.AStarTraversal();

            //int sizeOfResultantArray = resultantArray.Count();

            //// print out the report of the result
            //Console.WriteLine("############################################# REPORT ##############################################");
            //foreach (LocationInfo locationInfo in resultantArray) {
            //    totalCostOfTraversal += locationInfo._costOfTraversal._g;
            //    Console.WriteLine("---------------------------------------------------------------------------------");
            //    Console.WriteLine("Node:\t" + locationInfo._node._node + "\t|\tInterim Traversal Cost:\t" + locationInfo._costOfTraversal._g+"\t|\tCumulative Traversal Cost:\t" + totalCostOfTraversal);
            //}
            //Console.WriteLine("---------------------------------------------------------------------------------");
            //Console.WriteLine("A Star Traversal");
            //foreach (LocationInfo locationInfo in resultantArray) {
            //    Console.Write("\t" +locationInfo._node._node);
            //    if(--sizeOfResultantArray > 0)
            //        Console.Write(" --> ");
            //}
            //Console.WriteLine("\n---------------------------------------------------------------------------------");
            //Console.WriteLine("\nTotal Cost of Travel: \t" + totalCostOfTraversal);
        }

        /// <summary>
        /// Method: getLocationsInfo
        /// Input: fileDescriptor
        /// Returns: list of locations
        /// </summary>
        public static List<LocationInfo> getLocationsInfo(String fileDescriptor)
        {
            String[] parseLines = System.IO.File.ReadAllLines(fileDescriptor);  //Read the file line by line
            String endOfFile = "END";

            // variables used for storing the location information in the location bean
            String[] locs;
            int x, y;
            Node node;

            // to create a list of location objects
            List<LocationInfo> locations = new List<LocationInfo>();

            // iterate over a foreach loop for each line in the file and store the details appropriately
            foreach (String eachLine in parseLines)
            {
                if (!eachLine.Equals(endOfFile))
                {  // END symbolises the end of file
                    locs = eachLine.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    node = new Node(locs[0]);
                    x = Int32.Parse(locs[1]);
                    y = Int32.Parse(locs[2]);

                    // after getting the corresponding entries split by space, construct the location object 
                    // and add it to the location list
                    LocationInfo locationInfo = new LocationInfo(node, x, y, new CostOfTraversal(0.0, 0.0), null);
                    locations.Add(locationInfo);
                }
            }
            return locations;
        }

        /// <summary>
        /// Method: getConnectionsInfo
        /// Input: file descriptor and the location list
        /// Returns: list of connections
        /// </summary>
        public static List<ConnectionInfo> getConnectionsInfo(String fileDescriptor, List<LocationInfo> locations)
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

                    // location specific details of the vertex node
                    var vertex = locations.FirstOrDefault(item => item._node._node == cons[0]);
                    int x1 = vertex.xCoOrdinate;
                    int y1 = vertex.yCoOrdinate;

                    // construct a location detail of the vertex
                    locationVertex = new LocationInfo(vertex._node, x1, y1, new CostOfTraversal(0.0, 0.0), null);

                    int edges = Int32.Parse(cons[1]);   // get the number of neighbors for a particular node

                    // iterate over a loop and get the location details of the neighbors
                    for (int i = 1; i <= edges; i++)
                    {
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
            return connections;
        }

        /// <summary>
        /// Method: collectUserInput
        /// Input: nil
        /// Returns: nil
        /// </summary>
        /// Collects user input values for start and end nodes. Validates automatically
        public static void collectUserInput()
        {
            bool success = false;

            while (!success)
            {
                try
                {
                    //String startNode = "A1";
                    Console.WriteLine("Enter the node from which to traverse.");
                    startNode = Console.ReadLine();
                    String present = checkLocation(startNode, locationsInfo);
                    if (present.Equals("Present"))
                        success = true;
                    else
                        Console.WriteLine("This node is not present in the locations list.");
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("Enter a value." + e);
                }
            }

            success = false;

            while (!success)
            {
                try
                {
                    //String endNode = "G4b";
                    Console.WriteLine("Enter the node to which you need to go to.");
                    endNode = Console.ReadLine();
                    String present = checkLocation(endNode, locationsInfo);
                    if (present.Equals("Present"))
                        success = true;
                    else
                        Console.WriteLine("This node is not present in the locations list.");
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("Enter a value." + e);
                }
            }
        }

        /// <summary>
        /// Method: checkLocation
        /// Input: string location and list of locations
        /// Returns: string
        /// </summary>
        public static String checkLocation(String location, List<LocationInfo> locationsList)
        {
            bool present = false;
            foreach (LocationInfo loc in locationsList)
            {
                if (loc._node._node.Equals(location))
                    present = true;
            }
            return present ? "Present" : "Not Present";
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