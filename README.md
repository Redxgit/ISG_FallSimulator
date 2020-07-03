# ISG_FallSimulator
 Fall Simulator to generate accelerometer data for machine learning

Inside the Build folder the FallSimulation.exe can be executed to automatically generate either forward or syncope falls, the toggle button switchs between them.

The generated .csv will be wrote inside the \build\FallSimulation_Data\StreamingAssets\outcsvs folder, indicating date and time as well as the type of fall.

Inside the csv the current and previous position are found as well as the direction of the gravity force in the x, y, z axes. Additionally there is a movementType tag which indicates the state of the humanoid, Transition, Walking, Fall and AfterFall.

Finally, the source files used to generate the application are found on the project folder.

Unity Version used: 2019.2.20f1
