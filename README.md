SID: 2258528

C# 2D Graphics and Mathematics Coursework:
This repository contains a comprehensive suite of C# applications developed over a seven-week computer graphics course. The projects transition from fundamental GDI+ screen rendering to complex linear algebra engines, demonstrating a deep practical understanding of how modern 2D and 3D rendering pipelines operate under the hood.

Technical Highlights: 
Engineered a custom matrix algebra engine from scratch to process complex 2D spatial transformations (rotation, translation) without relying on pre-built matrix libraries. 
Optimized rendering architecture by isolating vertex coordinate data (Points tables) from edge connectivity instructions (Lines tables). 
Eliminated graphical screen tearing and application freezing by migrating blocking loops to asynchronous Timers and implementing custom in-memory double-buffering.Applied recursive mathematical formulas to dynamically generate self-replicating fractal geometries down to the sub-pixel level.

Getting Started: 
Clone this repository to your local machine using Git.
Open the master ComputerGraphics_Coursework.sln solution file in Visual Studio 2026.
Right-click the specific weekly project you want to test in the Solution Explorer and select Set as Startup Project.
Press F5 to compile and run the application.
