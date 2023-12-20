import pandas as pd
import numpy as np

# data = pd.read_csv('gameState.txt', sep=' ', header=None)
# data_npy = data.values
#output.txt
data = np.loadtxt("output2k4.txt", usecols=list(range(0,91)))
np.save('output2k4.npy', data)

# array = np.load('output.npy')
# print(array)

# Set the number of columns and rows for each array
columns = 10
rows = 91

# Read the text file
with open('gameState2k4.txt', 'r') as file:
    content = file.read()

# Split the content into blocks using an empty line as a delimiter
blocks = content.strip().split('\n\n')

# Initialize a list to store the arrays
data3 = np.empty((0, 1, 91, 10))

# Iterate through each block and convert it to a NumPy array
for block in blocks:
    # Split each line in the block into individual numbers
    lines = block.split('\n')
    data = [list(map(int, line.split())) for line in lines]
    
    # Convert the list of lists to a NumPy array
    array = np.array(data)
    data1 = np.reshape(array, (1, 1, 91, 10), order='C')

    data3 = np.concatenate((data3, data1), axis=0)
    # Append the array to the list
    # arrays.append(array)

np.save('gameState2k4.npy', data3)
# array = np.load('gameState.npy')
# print(array)

    