# Import the necessary packages
import time
import zmq
import numpy as np
import threading

# Node class representing the tiles on the grid
class Node():
    def __init__(self, parent = None, position = None):
        self.parent = parent
        self.position = position
        self.g = 0
        self.h = 0
        self.f = 0

    def __eq__(self, other):
        return self.position == other.position


# A* algorithm function to find the shortest path.
# The function returns a list of tuples as a path that leads from 
# the given start node to the given end node in the grid
def astar(maze, start, end):
    # Create start and end node
    start_node = Node(None, start)
    start_node.g = start_node.h = start_node.f = 0
    end_node = Node(None, end)
    end_node.g = end_node.h = end_node.f = 0

    # Initialize both open and closed list
    open_list = []
    closed_list = []

    # Add the start node
    open_list.append(start_node)

    # Loop until you find the end
    while len(open_list) > 0:

        # Get the current node
        current_node = open_list[0]
        current_index = 0
        for index, item in enumerate(open_list):
            if item.f < current_node.f:
                current_node = item
                current_index = index

        # Pop current off open list, add to closed list
        open_list.pop(current_index)
        closed_list.append(current_node)

        # Found the goal
        if current_node == end_node:
            path = []
            current = current_node
            while current is not None:
                current.position = (current.position[0] + 1, current.position[1])
                path.append(current.position)
                current = current.parent
            return path[::-1]               # Return reversed path

        # Generate children
        children = []
        for new_position in [(0, -1), (0, 1), (-1, 0), (1, 0), (-1, -1), (-1, 1), (1, -1), (1, 1)]: # Adjacent squares

            # Get node position
            node_position = (current_node.position[0] + new_position[0], current_node.position[1] + new_position[1])

            # Make sure within range
            if node_position[0] > (len(maze) - 1) or node_position[0] < 0 or node_position[1] > (len(maze[len(maze)-1]) -1) or node_position[1] < 0:
                continue

            # Non-walkable terrain
            if maze[node_position[0]][node_position[1]] == 1:
                continue

            # Regular tile 
            if maze[node_position[0]][node_position[1]] == 0:
                new_node = Node(current_node, node_position)

            # Grass tile 
            if maze[node_position[0]][node_position[1]] == 2:
                new_node = Node(current_node, node_position)
                new_node.g = 20

            # Water tile 
            if maze[node_position[0]][node_position[1]] == 3:
                new_node = Node(current_node, node_position)
                new_node.g = 200

            # Mud tile 
            if maze[node_position[0]][node_position[1]] == 4:
                new_node = Node(current_node, node_position)
                new_node.g = 40

            # Append
            children.append(new_node)

        # Loop through children
        for child in children:

            var = False

            # Child is on the closed list
            for closed_child in closed_list:
                if child == closed_child:
                    var = True
                    break

            # Create the f, g, and h values
            child.g += current_node.g + 1
            child.h = ((child.position[0] - end_node.position[0]) ** 2) + ((child.position[1] - end_node.position[1]) ** 2)
            child.f = child.g + child.h

            # Child is already in the open list
            for open_node in open_list:
                if child == open_node and child.g > open_node.g:
                    var = True
                    break

            if var == True:
                continue

            # Add the child to the open list
            open_list.append(child)


# Server function to establish TCP communication with Unity
# The socket is bound to 3 different port numbers,
# corresponding to 3 different clients (agent)
def server(socketNumber):
    context = zmq.Context()
    socket = context.socket(zmq.REP)        # Server socket
    socket.bind(socketNumber)               # Port number

    while True:
        # Wait for next request from client
        message = socket.recv()

        size = 32                           # Grid size

        # Decode the received message from bytes to string, 
        # then split the string whenever a space is encountered. split() function returns a list.
        stringSplit = message.decode("utf-8").split(' ')
    
        # startPoint and endPoint are of type tuple
        startPoint = (size - int(stringSplit[0]), int(stringSplit[1]))          #Elements 0 and 1 in the list are the coordinates of the start node
        endPoint = (size - int(stringSplit[2]), int(stringSplit[3]))            #Elements 2 and 3 in the list are the coordinates of the end node
        gridString = stringSplit[4]                                             #Element 4 is an array representing the nodes

        # The grid string array is split into a list of characters
        gridArray = list(gridString)

        # The strings inside the list are converted to integers
        gridInt = [int(i) for i in gridArray]

        # The array of integers is split into chunks, each is equal to the grid width and grid height in Unity
        nodes = np.array([gridInt[i : i + size] for i in range(0, len(gridInt), size)])
        
        # Pass the grid, start node and end node to the A* algorithm function
        algorithmPath = astar(nodes, startPoint, endPoint)

        #Convert the given path from tuples to string, then encode the string to bytes
        pathToUnity = str.encode(" ".join(map(str, algorithmPath)))

        # The connection is set to sleep in each cycle of the loop
        time.sleep(1)

        # Send the path result back to its respective client
        socket.send(pathToUnity)

# Run the server function on multiple threads
# Each thread corresponds to a client, which has a unique port number
if __name__ == "__main__":
    t1 = threading.Thread(target = server, args = ["tcp://*:5555"])
    t2 = threading.Thread(target = server, args = ["tcp://*:5444"])
    t3 = threading.Thread(target = server, args = ["tcp://*:5333"])

    t1.start()
    t2.start()
    t3.start()