using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AStarWebApplication
{
    public class ShortestDistanceSearchAlgorithm
    {
        public LocationInfo _startNode, _endNode;       // class level variable for the start and end nodes initialized.

        public List<LocationInfo> _locationsInfo;       // variable for the location information list
        public List<ConnectionInfo> _connectionsInfo;   // variable for the connections information list

        public List<LocationInfo> _openNodes, _visitedNodes;    // variables for the open and closed nodes

        // constructor
        public ShortestDistanceSearchAlgorithm(LocationInfo startNode, LocationInfo endNode, List<LocationInfo> locationsInfo, List<ConnectionInfo> connectionsInfo)
        {
            _startNode = startNode;
            _endNode = endNode;
            _connectionsInfo = connectionsInfo;
            _locationsInfo = locationsInfo;

            _openNodes = new List<LocationInfo>();
            _visitedNodes = new List<LocationInfo>();
        }

        /// <summary>
        /// Method: AStarTraversal
        /// Input: Nil
        /// Returns: list of locations
        /// </summary>
        public List<LocationInfo> AStarTraversal()
        {
            // Instantiate a priority queue
            Queue queueOfNodes = new Queue();

            // initial cost from start is made 0 and the heuristic cost to goal computed. parent node is also set to null
            _startNode._costOfTraversal._g = 0.0;
            _startNode._costOfTraversal._h = getEdgeDistance(_startNode, _endNode);
            _startNode._parentNode = null;

            // add the start node information to the open list and the priority queue
            _openNodes.Add(_startNode);
            queueOfNodes.enQueue(_startNode);

            // variables for usage in the algorithm
            LocationInfo visitingNode;
            List<ConnectionInfo> neighborsList;
            List<String> neighboringNodes;
            String[] sNeighboringNodes;
            LocationInfo openedNode;

            while (queueOfNodes.queueLength > 0)
            {   // when the priority queue is having some node
                visitingNode = queueOfNodes.deQueue();  // remove the first item from the queue
                _openNodes.Remove(visitingNode);        // remove the corresponding node from the open list

                if (visitingNode._node._node.Equals(_endNode._node._node))
                { // visiting node the same as the end node implies we are home 
                    // track the parent nodes and insert them to the list of traversed locations to be printed
                    var finalTraversal = new List<LocationInfo> { 
                        visitingNode 
                    };
                    while (visitingNode._parentNode != null)
                    {
                        finalTraversal.Insert(0, visitingNode._parentNode);
                        visitingNode = visitingNode._parentNode;
                    }
                    return finalTraversal;
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
                foreach (LocationInfo neighbor in neighbors)
                {
                    if (_visitedNodes.Contains(neighbor))
                        continue;

                    // Update the traversal cost with the cost of the neighbor - g(n) from parent to neighbor
                    // and h(n) heuristic from neighbor to goal
                    double g_cost = visitingNode._costOfTraversal._g + getEdgeDistance(visitingNode, neighbor);
                    double h_cost = getEdgeDistance(neighbor, _endNode);

                    double newCostOfTraversal = g_cost + h_cost;    // total cost f(n) = g(n) + h(n)

                    // use the linq feature in c# to find out the recently opened node from the open list
                    openedNode = _openNodes.FirstOrDefault(query => query._node._node == neighbor._node._node);

                    // make sure that the cost incurred due to traversing that neighbor is minimal
                    if (openedNode != null && openedNode._costOfTraversal.getCostOfTraversal() <= newCostOfTraversal)
                        continue;

                    // set the updated cost parameters of g(n) and h(n) in cost of traversal bean.
                    neighbor._costOfTraversal._g = g_cost;
                    neighbor._costOfTraversal._h = h_cost;
                    neighbor._parentNode = visitingNode;    // make the visiting node the parent now and continue tracing the path.

                    if (openedNode != null)
                    {
                        _openNodes.Remove(neighbor);        // remove the neighbor from the open list and  queue
                        queueOfNodes.remove(openedNode);
                    }
                    _openNodes.Add(neighbor);           // add the neighbor to the open list and queue
                    queueOfNodes.enQueue(neighbor);
                }
                _visitedNodes.Add(visitingNode);    // add the current node the closed list
            }
            return null;
        }

        /// <summary>
        /// Method: getEdgeTraversalCost
        /// Input: location from and to
        /// Returns: g(n) + h(n) cost function in double
        /// </summary>
        public double getEdgeTraversalCost(LocationInfo locationReference, LocationInfo locationTo) {
            double xComponent = Math.Pow((locationReference.xCoOrdinate - locationTo.xCoOrdinate), 2);
            double yComponent = Math.Pow((locationReference.yCoOrdinate - locationTo.yCoOrdinate), 2);

            return Math.Sqrt(xComponent + yComponent);
        }

        /// <summary>
        /// Method: getEdgeDistance
        /// Input: location from and to
        /// Returns: euclidean distance function in double
        /// </summary>
        public double getEdgeDistance(LocationInfo locationReference, LocationInfo locationTo) {
            double xComponent = Math.Pow((locationReference.xCoOrdinate - locationTo.xCoOrdinate), 2);
            double yComponent = Math.Pow((locationReference.yCoOrdinate - locationTo.yCoOrdinate), 2);

            return Math.Sqrt(xComponent + yComponent);
        }
    }
}