README
------
   
INTRODUCTION:
In this project we implement A* algorithm using two heuristics, namely shortest distance and the fewest number of links traversed from the start node to end node. 
The user provides the inputs. The user can browse and select the location and connection files and can enter the start and end nodes, can select the type of heuristic and can see the path visually.

REQUIREMENTS:
.NET framework - Visual studio 2013, C#, ASP.NET, VIS, Telerik

EXECUTION STEPS:
1. Goto "AStarWebApplication\"
2. Execute "AStarWebApplication.sln" using Microsoft Visual Studio and run the AStarTraversal.aspx.cs
3. Enter the requisite user inputs. The file paths for location and connection files, start city and end city, cities to be excluded and heuristic to be used. (city names are case sensitive.)
4. The algorithm will then generate the graph based upon the information provided. This graph on searching, will highlight the solution path. This path will also be dispalyed in the right hand side bottom corner in textual format with the list of cities visited.


FILES:
AstarTraversal.aspx.cs: 			This is the main class where the required input from the user is taken. 
AstarTraversal.aspx:    			It contains web design page where the graph is presented to the user.
LocationInfo.cs:        			This class contains the information about nodes,i.e node name and it's x,y co-ordinates.
ConnectionInfo.cs:      			This class contains the  information about the connectivity of the given nodes.It contains information about the edges.
Queue.cs:               			Implementation of Priority Queue used in A* algorithm
ShortestDistanceSearchAlgorithm.cs: This class contains the logic for implementation of A* algorithm using shortest distance as the heuristic
Node.cs: 							Contains all the node names. This class is basically used to represent the nodes visually.
FewerLinksSearchAlgorithm.cs: 		This contains the logic for implementation of A* algorithm using fewest number of links as the heuristic
CostOfTraversal.cs:					This class contains the information about various heuristic costs, the g and h respectively.


PROJECT DONE BY:
Subramanian Viswanathan
Mehul Suresh Kumar

HELP FROM EXTERNAL LIBRARIES AND LINKS USED FOR THIS PROJECT:
vis.js [http://visjs.org/] :  										Used to represent the graph.
Telerik UI for ASP.NET MVC [http://www.telerik.com/aspnet-mvc] : 	To improve the look and feel of the UI.
MSDN Microsoft Developers Network:									For C# help