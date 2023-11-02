Create and run levels that teach you to think algorithmically by building sequences of functional blocks on a grid to guide a runner unit from start to end, while maintaining a correct state of its variables.

## Main description
* Levels consist of functional blocks, placed on a grid, and a runner with several numeric variables.
* Each functional block represents one of several well known programming languages' operators such as addition, subtraction, multiplication, condition and other.
* To create a new level user places predefined blocks on the grid, chooses what additional blocks another user can place to pass the level, defines number and initial values of runner variables, and enters expected output to "Finish" block data.
* To run a level user places additional blocks allowed by user who created the level, configures placed blocks data (ex. block direction, or condition for "Condition" block), and runs the level.
* While running a level user can see each runner unit step and each variable change in real time.
* There are some visual tips while placing blocks on a level such as blocks tooltips, selected block outline, always visible block direction, "Show/hide data" button, and highlighting of block cycles.

**Some important (or just example) sources:**
* [Blocks/FunctionalBlock.cs](https://github.com/ThatsOrganization/CodingTeachingPlatform/blob/master/Assets/Blocks/FunctionalBlock.cs)
* [GridPlaneController.cs](https://github.com/ThatsOrganization/CodingTeachingPlatform/blob/master/Assets/GridPlaneController.cs)
* [Runner/Runner.cs](https://github.com/ThatsOrganization/CodingTeachingPlatform/blob/master/Assets/Runner/Runner.cs)
* [Runner/RunnerField.cs](https://github.com/ThatsOrganization/CodingTeachingPlatform/blob/master/Assets/Runner/RunnerField.cs)
* [UI/BlockPanelController.cs](https://github.com/ThatsOrganization/CodingTeachingPlatform/blob/master/Assets/UI/BlockPanelController.cs)

## Translation to code
Also there is a translation to code feature, accessed through the "Code" button in the level UI. Using this feature user who is running a specific level can see how the sequence of functional blocks on the grid would look like as a C# or Python code.

**Sources: [CodeTranslation](https://github.com/ThatsOrganization/CodingTeachingPlatform/tree/master/Assets/CodeTranslation)**
