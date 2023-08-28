# Just Sign - An Interactive ASL Game

Welcome to the GitHub repository for **"Just Sign"**, an interactive game that immerses players into the world of American Sign Language (ASL). Ready to jump into the fun world of ASL? Follow the instructions below to get started!

## How to Download and Play

1. **Navigate to the Releases Tab:** At the top of this GitHub repository, you'll find a tab labeled "Releases." Click on it.
2. **Find the Right Release:** Look for the release titled "Just Sign v1.0."
3. **Download the Game:** Within the release details, locate the `Just Sign v1.0` file and click on it to begin the download.
4. **Run the Game:** Once the download is complete, navigate to your download folder or wherever you saved the file. Double-click on the "Just Sign v1.0" application to launch the game.
5. **Enjoy:** Dive into the interactive world of ASL!

### Credits

Menu Music: "Tokyo Bounce" by MACROSS 82-99

# Detailed Overview

**"Just Sign"** is an interactive game that educates and challenges players on American Sign Language (ASL) recognition. By leveraging advanced machine learning techniques, the game offers a seamless experience in detecting and understanding a multitude of ASL gestures.

https://github.com/christalind03/Just-Sign/assets/97999032/5403378c-a67f-4cef-a25e-b7e6bae824e0

## Machine Learning Model Architecture

The core of the game's ASL recognition capability lies in its meticulously designed machine learning model. The model employs a sequence of layers, primarily based on Long Short-Term Memory (LSTM) units, which are highly effective in handling sequences like time-series data or, in this case, video frames.

### Model Breakdown:

- **1st Layer (LSTM):**
  - Nodes: 128
  - Input: Tensor of shape (25, 1662), where 25 represents the number of frames, and 1662 is the count of keypoints within each frame.
  - Output: Fed into the 2nd layer.
  
- **2nd Layer (LSTM):**
  - Nodes: 264
  - Input: Takes the output from the 1st layer.
  - Output: Directed into the 3rd layer.
  
- **3rd Layer (LSTM):**
  - Nodes: 128
  
- **4th Layer (Dense):**
  - Nodes: 128
  
- **5th Layer (Dense):**
  - Nodes: 64
  
- **6th Layer (Dense):**
  - Nodes: 41, corresponding to the total number of ASL signs the model was trained on.

### Methodologies

- **Data Collection:** A foundational step was collecting a diverse and comprehensive dataset. This not only enhanced the model's accuracy but also made sure it recognized the intricacies associated with ASL gestures.
  
- **Data Preprocessing:** Using OpenCV and MediaPipe, user movements were captured, which were then transformed into a format suitable for model training.
  
- **Sequential Modeling:** LSTM layers were chosen for their capability to remember sequences, making them ideal for frame-by-frame sign recognition.
  
- **Scoring Approach Refinement:** The game transitioned from a time-centric evaluation to a meticulous frame-by-frame scoring approach, addressing the challenges faced during sign transitions.
  
- **Hyperparameter Tuning:** The model's excellence was achieved by iterative experiments with various hyperparameters, optimizing performance.
  
- **Augmentation & Stratification:** Data augmentation techniques were employed to artificially increase the dataset's size and diversity. Stratification ensured a balanced representation of signs during model training.


## Game Development in Unity

Unity, a leading game development platform, served as the foundation for crafting the **"Just Sign"** experience.

- **Scene Design:** The game's visual elements, from backgrounds to interactive assets, were designed and assembled in Unity's scene editor, ensuring an intuitive and engaging player experience.
  
- **Scripting and Logic:** C# scripts were employed to incorporate game logic, user interactions, and integrate the machine learning model outputs to provide real-time feedback and scores to players.
  
- **Animation and Feedback:** Unity's animation system was used to give dynamic feedback to players as they attempt signs, enhancing engagement and understanding.
  
- **Integration with Machine Learning:** Unity's ability to interface with external systems allowed for seamless integration of the ML model. User's signs captured in real-time were sent to the model, with the interpreted sign relayed back as in-game feedback.

## Conclusion

Marrying the immersive capabilities of Unity with the precision of modern machine-learning techniques, **"Just Sign"** stands as a testament to the potential of interdisciplinary innovation. It offers an unmatched tool for those enthusiastic about imbibing ASL in a lively, interactive setting.

Thanks for your interest in "Just Sign"! Happy gaming and happy signing! 🤟🎮
