using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AStarWebApplication
{
    public class Queue
    {
        public List<LocationInfo> genericQueue;

        //constructor
        public Queue()
        {
            genericQueue = new List<LocationInfo>();
        }

        // Method that returns the number of elements in the queue
        public int queueLength
        {
            get
            {
                return genericQueue.Count;
            }
        }

        // Method to dequeue and return the first element in the queue
        public LocationInfo deQueue()
        {
            LocationInfo location = genericQueue[0];
            genericQueue.RemoveAt(0);
            return location;
        }

        // Method to add an item to the list
        public void enQueue(LocationInfo location)
        {
            bool present = false;
            for (int i = 0; i < genericQueue.Count; i++)
            {
                LocationInfo placeHolder = genericQueue[i];
                if (placeHolder._costOfTraversal.getCostOfTraversal() >= location._costOfTraversal.getCostOfTraversal())
                {
                    genericQueue.Insert(i, location);
                    present = true;
                    break;
                }
            }
            if (!present)
            {
                genericQueue.Add(location);
            }
        }

        public void enQueueFewerLinks(LocationInfo location)
        {
            bool present = false;
            for (int i = 0; i < genericQueue.Count; i++)
            {
                LocationInfo placeHolder = genericQueue[i];
                if (placeHolder._costOfTraversal.getCostOfTraversal() >= location._costOfTraversal.getCostOfTraversal())
                {
                    genericQueue.Insert(i, location);
                    present = true;
                    break;
                }
            }
            if (!present)
            {
                genericQueue.Add(location);
            }
        }

        // Method to check whether an element is present in the queue or not
        public bool exists(LocationInfo node)
        {
            return genericQueue.Contains(node);
        }

        // Method to remove an item from the priority queue
        public void remove(LocationInfo node)
        {
            genericQueue.Remove(node);
        }
    }
}