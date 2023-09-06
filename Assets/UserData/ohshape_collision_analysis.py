import json

# Reading the JSON file
with open('OhShape/connor/09-05-2023-07-59-07.json', 'r') as file:
    data = json.load(file)

""" collision event sample
{
    "actutorId":92,
    "timestamp":43.79999923706055,
    "duration":1.0400009155273438,
    "other":"shape1",
    "detail":"Cube.029"
},
"""

import numpy as np

collision_actuator_array = np.zeros((len(data['collusionData'])), np.int16)
duration_array = np.zeros((len(data['collusionData'])), np.float32)
collision_shape_array = np.zeros((len(data['collusionData'])), np.int16)

# Counting the occurrences of actuatorId in collusionData

print("number of collisions = ", len(data['collusionData']))

for i in range(len(data['collusionData'])):
    collision_actuator_array[i] = data['collusionData'][i]['actutorId']
    duration_array[i] = data['collusionData'][i]['duration']
    shape_str = data['collusionData'][i]['other']
    collision_shape_array[i] = int(shape_str.replace('shape', ''))

print(collision_actuator_array[0], duration_array[0], collision_shape_array[0])

unique_actuators, actuator_counts = np.unique(collision_actuator_array, return_counts=True)
unique_shapes, shape_counts = np.unique(collision_shape_array, return_counts=True)

print(unique_actuators)
print(actuator_counts)
print(unique_shapes)
print(shape_counts)

# Total duration of collisions for each actuator
total_duration_per_actuator = {}
for ua in unique_actuators:
    total_duration_per_actuator[ua] = np.sum(duration_array[collision_actuator_array == ua])
print(total_duration_per_actuator)

# Total duration of collisions for each shape
total_duration_per_shape = {}
for us in unique_shapes:
    total_duration_per_shape[us] = np.sum(duration_array[collision_shape_array == us])
print(total_duration_per_shape)
