using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AStarWebApplication {
    public class CostOfTraversal
    {
        public double _g;   // variable that holds the g(n) function - cost from start
        public double _h;   // variable that holds the h(n) function - cost to goal heuristic

        public CostOfTraversal(double g, double h)
        {
            _g = g;
            _h = h;
        }

        /// <summary>
        /// Method: getCostOfTraversal
        /// input: nil
        /// returns: cost from start + cost to goal
        /// </summary>
        public double getCostOfTraversal()
        {
            return (_g + _h);
        }
    }
}