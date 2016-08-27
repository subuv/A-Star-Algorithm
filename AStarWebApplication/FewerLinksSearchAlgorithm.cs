using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AStarWebApplication {
    public class FewerLinksSearchAlgorithm {
        public LocationInfo _startNode, _endNode;       // class level variable for the start and end nodes initialized.

        public List<LocationInfo> _locationsInfo;       // variable for the location information list
        public List<ConnectionInfo> _connectionsInfo;   // variable for the connections information list

        public List<LocationInfo> _openNodes, _visitedNodes;    // variables for the open and closed nodes

        string _minLinkPath;

        // constructor
        public FewerLinksSearchAlgorithm(LocationInfo startNode, LocationInfo endNode, List<LocationInfo> locationsInfo, List<ConnectionInfo> connectionsInfo)
        {
            _startNode = startNode;
            _endNode = endNode;
            _connectionsInfo = connectionsInfo;
            _locationsInfo = locationsInfo;

            _openNodes = new List<LocationInfo>();
            _visitedNodes = new List<LocationInfo>();
            _minLinkPath = "";
        }

        /// <summary>
        /// Method: searchByLinks
        /// Input: Nil
        /// Returns: string of locations
        /// </summary>
        public string searchByLinks()
        {
            // Instantiate a priority queue
            var queueOfNodes = new Queue<LocationInfo>();

            _startNode._parentNode = null;
            queueOfNodes.Enqueue(_startNode); //Adopt a BFS approach and add the start node to the queue

            // variables for usage in the algorithm
            LocationInfo visitingNode;
            List<ConnectionInfo> neighborsList;
            List<String> neighboringNodes;
            String[] sNeighboringNodes;
            LocationInfo openedNode;

            while (queueOfNodes.Count > 0)
            {
                visitingNode = queueOfNodes.Dequeue();  //Dequeue the first element

                if (visitingNode._node._node.Equals(_endNode._node._node))
                {
                    var finalTraversal = new List<LocationInfo> { visitingNode };
                    while (visitingNode._parentNode != null)
                    {
                        finalTraversal.Insert(0, visitingNode._parentNode);
                        visitingNode = visitingNode._parentNode;
                    }

                    foreach (LocationInfo minLinkNode in finalTraversal)
                        _minLinkPath += minLinkNode._node._node + ',';
                    _minLinkPath.TrimEnd(',');
                    return _minLinkPath;
                }

                // use the linq feature in c# to find if the parent node and visiting node to get its neighbors list
                var neighborsForVisitingNode = _connectionsInfo.Where(query => query._parentNode._node == visitingNode._node._node);
                neighborsList = neighborsForVisitingNode.ToList();  // cast it to a list
                neighboringNodes = new List<String>();

                foreach (ConnectionInfo availableConnections in neighborsList)
                    neighboringNodes.Add(availableConnections._connectingNode._node); // add the node names to a string list

                sNeighboringNodes = neighboringNodes.ToArray(); // cast it to a string array to be used later

                // find the corresponding neighbor from the string array using linq feature and make a list of neighbors to visit
                var neighborsVisitList = _locationsInfo.Where(query => sNeighboringNodes.Contains(query._node._node));
                List<LocationInfo> neighbors = neighborsVisitList.ToList();

                // iterate over each neighbor found
                foreach (LocationInfo neighbor in neighbors) {
                    if (_visitedNodes.Contains(neighbor))
                        continue;

                    // use the linq feature in c# to find out the recently opened node from the open list
                    openedNode = _openNodes.FirstOrDefault(query => query._node._node == neighbor._node._node);

                    if (openedNode == null && !queueOfNodes.Contains(neighbor)) {
                        neighbor._parentNode = visitingNode;
                        queueOfNodes.Enqueue(neighbor);
                    }
                }
                _visitedNodes.Add(visitingNode);
            }
            return null;
        }
    }
}