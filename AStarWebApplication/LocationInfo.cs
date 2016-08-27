using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AStarWebApplication {
    public class LocationInfo
    {
        public Node _node;                          // Node in consideration
        public LocationInfo _parentNode;            // Information about the parent node
        public int xCoOrdinate;                     // X Co-ordinate of a location
        public int yCoOrdinate;                     // Y Co-ordinate of a location
        public CostOfTraversal _costOfTraversal;    // Cost of traversal from one location to the other.

        public LocationInfo()
        {
            //Blank constructor for instantiation
        }

        // Constructor with all the default input parameters.
        public LocationInfo(Node node, int x, int y, CostOfTraversal costOfTraversal, LocationInfo parentNode)
        {
            _node = node;
            xCoOrdinate = x;
            yCoOrdinate = y;
            _costOfTraversal = costOfTraversal;
            _parentNode = parentNode;
        }

        /// <summary>
        /// Method: getLocationsInfo
        /// Input: fileDescriptor
        /// Returns: list of locations
        /// </summary>
        public List<LocationInfo> getLocationsInfo(String fileDescriptor)
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
    }
}