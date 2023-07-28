import os
import sys
import cv2
import socket
import threading

import numpy as np
import mediapipe as mp
import tensorflow as tf 

# Define all actions to detect
ALL_ACTIONS = np.array([
                        "HOPELESS",   "SWEETHEART",   "ALL", 
                        "ME",         "LIFE",         "COUPLE", 
                        "SURROUND",   "TIME",         "I", 
                        "GUESS",      "MEANS",        "SOMETHING", 
                        "WHY",        "FEEL",         "LONELY", 
                        "WISH",       "FIND",         "LOVER", 
                        "HUG",        "NOW",          "CRY", 
                        "ROOM",       "SKEPTICAL",    "LOVE", 
                        "BUT",        "STILL",        "MORE", 
                        "GIVE",       "NEW",          "CHANCE", 
                        "CUPID",      "STUPID",       "HE",
                        "MAKE",       "THAT",         "NOT", 
                        "REAL",       "DUMB",         "NO_DETECTIONS"
                      ])

# Create a UDP client to connect with the UDP server
unity_websocket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
server_address = ("127.0.0.1", 50505)

# Initial data transfer to the UDP server from the client
unity_websocket.sendto(str.encode("Connecting to server..."), server_address)
server_response = ""

# Define additional functions to be used during runtime
def receive_data():
    global server_response

    while True:
        try:
            server_response = unity_websocket.recvfrom(1024)[0].decode()

        except socket.error:
            break

def mediapipe_detection(frame, holistic_model):
    frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    frame.flags.writeable = False

    mediapipe_results = holistic.process(frame)

    frame.flags.writeable = True
    frame = cv2.cvtColor(frame, cv2.COLOR_RGB2BGR)

    return frame, mediapipe_results

def extract_keypoints(results):
    face = np.array([[res.x, res.y, res.z] for res in results.face_landmarks.landmark]).flatten() if results.face_landmarks else np.zeros(468*3)
    pose = np.array([[res.x, res.y, res.z, res.visibility] for res in results.pose_landmarks.landmark]).flatten() if results.pose_landmarks else np.zeros(33*4)
    left_hand = np.array([[res.x, res.y, res.z] for res in results.left_hand_landmarks.landmark]).flatten() if results.left_hand_landmarks else np.zeros(21*3)
    right_hand = np.array([[res.x, res.y, res.z] for res in results.right_hand_landmarks.landmark]).flatten() if results.right_hand_landmarks else np.zeros(21*3)
    
    return np.concatenate([face, pose, left_hand, right_hand])

# Setup the ASL Recognition and MediaPipe models
asl_model = tf.keras.models.load_model(os.path.join(sys.path[0], "ASL Recognition Model.h5"))
with mp.solutions.holistic.Holistic(min_detection_confidence=0.5, min_tracking_confidence=0.5) as holistic:
    # Start a thread to receive data while the rest of the script runs
    receive_thread = threading.Thread(target=receive_data)
    receive_thread.start()
    
    while True:
        # Run specific actions based on the response from the server
        if server_response == "RUN PREDICTIONS":
            print("Running predictions...")

            sequence = []
            sequence_length = 25

            video_capture = cv2.VideoCapture(0)

            while video_capture.isOpened():
                # Read video feed
                success, frame = video_capture.read()

                # Make MediaPipe detections
                frame, mediapipe_results = mediapipe_detection(frame, holistic)

                # Make ASL predictions using keypoints
                keypoints = extract_keypoints(mediapipe_results)

                sequence.append(keypoints)
                sequence = sequence[-sequence_length:]
                
                if len(sequence) == sequence_length:
                    asl_prediction = asl_model.predict(np.expand_dims(sequence, axis=0), verbose=0)[0]
                    unity_websocket.sendto(str.encode(str(ALL_ACTIONS[np.argmax(asl_prediction)])), server_address)

                # Stop running predictions
                if server_response == "STOP PREDICTIONS":
                    print("Stopping predictions...")
                    break

            video_capture.release()
            cv2.destroyAllWindows()

        if server_response == "TERMINATE":
            print("Exiting program...")
            break

    unity_websocket.close()