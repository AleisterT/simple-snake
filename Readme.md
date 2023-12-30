This is my implementation of a simple snake game.
My implementation assumes that the snake moves in fixed steps, where each step length is equal to the element length.

With this project, I've tried to show my preferred coding style and architecture approach, without making too much unnecessary abstraction and boilerplate (given the small scope of a project).

The entry point for gameplay scripts is the GameplayContext class (on MainScene). It creates, initializes, and tracks the lifetime of all other class instances.

Classes are divided into
- Configs - Globally accessible read-only scriptable objects. This assumes a single instance of all configs but I think it is a reasonable assumption.
- - Configs are stored in "Assets/Configs/Game" and allow modification of initial game setup (speed, snake length), edibles properties, random weights.
- Views - MonoBehaviour classes attached and managing scene objects.
- - Prefabs where Views are attached are stored in the "Assets/Prefabs" directory.
- Controllers - MonoBehaviour classes responsible for managing views.
- - Controllers are attached to Scene objects, under "GameplayContext".
- Services - Plain C# classes responsible for managing data, controllers, game state, etc.
- There are also some "Utility" classes.
- - In my opinion, the most interesting one is "GameSpaceVector". It wraps 2D vector and allows for easy changing the game axis in the future e.g. to XY instead of XZ.

Notes:

In this project, the division of classes is only information to a programmer. I did not implement the "View", "Controller", or "Service" classes. I did not find it necessary given the scope. 
In bigger projects, base classes would be useful for unified scope and dependency management.

I also do not consider any DI. I was thinking about using the Service Locator pattern. I decided against it, given the small amount of services.
I also decided against using Assembly Definitions.

In bigger projects, I prefer separating pure data (like score) from services into separate model classes. However, given the minimal amount of data in this project I decided against it.

One consideration I'd also like to mention is splitting data and execution of Rewards. Moving the "GrantReward" method into a separate script "RewardHandler" and matching data-execution classes by type in a separate service. This would allow for nicer data separation.
