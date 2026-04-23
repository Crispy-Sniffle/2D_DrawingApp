using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2D_DrawingApp
{
    public partial class GrafPack : Form // The class setup
    {
        // This list acts as the dynamic array to store all drawn shapes and it will grow as needed
        private List<Shape> shapesList = new List<Shape>();

        // These will act as the switches and only one of them should be true at once
        private bool selectSquareStatus = false;
        private bool selectTriangleStatus = false;
        private bool selectCircleStatus = false;
        private bool selectRectangleStatus = false;
        private bool selectHexagonStatus = false;
        private bool selectModeStatus = false;
        
        private Point startPoint; //Records the exact X and Y coordinates of where the mouse was first clicked
        private Point currentPoint; // Updates the mouse's current X and Y coordinates continously
        private Point lastMousePosition; // Helps calculate how far the mouse has moved

        private bool isDrawing = false; //Turns true the moment you click on the canvas

        // Tracks if a shape is being dragged
        private bool isMoving = false; 
        private bool isRotating = false;

        // Checks if they are active
        private bool moveModeStatus = false;
        private bool rotateModeStatus = false;
        
        // It wipes the screen clean on every update and redraws everything instantly
        private void GrafPack_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen blackPen = new Pen(Color.Black);

            // Creates a dashed pen for the rubber-band preview
            Pen previewPen = new Pen(Color.Gray);
            previewPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            // Goes through all the saved shapes in shapesList
            foreach (Shape shape in shapesList)
            {
                if (shape.IsSelected)
                {
                    shape.Draw(g, new Pen(Color.Red, 2f)); // This is a highlighter
                }
                else
                {
                    // Use the shape's saved color!
                    shape.Draw(g, new Pen(shape.ShapeColor, 2f));
                }
            }

            // Draws the live rubber-band preview for each shape if the user is dragging the mouse
            if (isDrawing)
            {
                if (selectSquareStatus)
                {
                    Square tempShape = new Square(startPoint, currentPoint);
                    tempShape.Draw(g, previewPen);
                }
                else if (selectCircleStatus)
                {
                    Circle tempShape = new Circle(startPoint, currentPoint);
                    tempShape.Draw(g, previewPen);
                }
                else if (selectTriangleStatus)
                {
                    Triangle tempShape = new Triangle(startPoint, currentPoint);
                    tempShape.Draw(g, previewPen);
                }
                else if (selectRectangleStatus)
                {
                    Rectangle tempShape = new Rectangle(startPoint, currentPoint);
                    tempShape.Draw( g, previewPen);
                }
                else if (selectHexagonStatus)
                {
                    Hexagon tempShape = new Hexagon(startPoint, currentPoint);
                    tempShape.Draw(g, previewPen);
                }
            }
        }

        // This is the constructor. It will run and setup the environment
        public GrafPack() 
        {
            InitializeComponent();
            this.DoubleBuffered = true; // This eliminates the flickering of the screen 
            this.SetStyle(ControlStyles.ResizeRedraw, true); // This redraws the shapes during resizing of the window
            this.WindowState = FormWindowState.Maximized; // Opens the app in full screen
            this.BackColor = Color.White; // Sets the canvas clean and white

            // The following approach uses menu items coupled with mouse clicks
            MainMenu mainMenu = new MainMenu();
            MenuItem createItem = new MenuItem();
            MenuItem selectItem = new MenuItem();
            MenuItem transformItem = new MenuItem();
            MenuItem moveItem = new MenuItem();
            MenuItem rotateItem = new MenuItem();
            MenuItem formatItem = new MenuItem();
            MenuItem colorItem = new MenuItem();
            MenuItem clearAllItem = new MenuItem();
            MenuItem deleteItem = new MenuItem();
            MenuItem exitItem = new MenuItem();
            MenuItem squareItem = new MenuItem();
            MenuItem triangleItem = new MenuItem();
            MenuItem circleItem = new MenuItem();
            MenuItem rectangleItem = new MenuItem();
            MenuItem hexagonItem = new MenuItem();

            createItem.Text = "&Create";
            squareItem.Text = "&Square";
            circleItem.Text = "&Circle";
            triangleItem.Text = "&Triangle";
            rectangleItem.Text = "&Rectangle";
            hexagonItem.Text = "&Hexagon";
            selectItem.Text = "&Select";
            transformItem.Text = "&Transform";
            moveItem.Text = "&Move";
            rotateItem.Text = "&Rotate";
            formatItem.Text = "&Format";
            colorItem.Text = "&Change Color";
            clearAllItem.Text = "&Clear All";
            deleteItem.Text = "&Delete";
            exitItem.Text = "&Exit";

            mainMenu.MenuItems.Add(createItem);
            mainMenu.MenuItems.Add(selectItem);
            mainMenu.MenuItems.Add(transformItem);
            transformItem.MenuItems.Add(moveItem);
            transformItem.MenuItems.Add(rotateItem);
            mainMenu.MenuItems.Add(formatItem);
            formatItem.MenuItems.Add(colorItem);
            formatItem.MenuItems.Add(clearAllItem);
            mainMenu.MenuItems.Add(deleteItem);
            mainMenu.MenuItems.Add(exitItem);
            createItem.MenuItems.Add(squareItem);
            createItem.MenuItems.Add(circleItem);
            createItem.MenuItems.Add(triangleItem);
            createItem.MenuItems.Add(rectangleItem);
            createItem.MenuItems.Add(hexagonItem);

            selectItem.Click += new System.EventHandler(this.selectShape);
            squareItem.Click += new System.EventHandler(this.selectSquare);
            circleItem.Click += new System.EventHandler(this.selectCircle);
            triangleItem.Click += new System.EventHandler(this.selectTriangle);
            rectangleItem.Click += new System.EventHandler(this.selectRectangle);
            hexagonItem.Click += new System.EventHandler(this.selectHexagon);
            moveItem.Click += new System.EventHandler(this.selectMove);
            rotateItem.Click += new System.EventHandler(this.selectRotate);
            colorItem.Click += new System.EventHandler(this.actionChangeColor);
            clearAllItem.Click += new System.EventHandler(this.actionClearAll);
            deleteItem.Click += new System.EventHandler(this.actionDelete);
            exitItem.Click += new System.EventHandler(this.actionExit);

            this.Menu = mainMenu;
            this.MouseDown += new MouseEventHandler(this.GrafPack_MouseDown);
            this.MouseMove += new MouseEventHandler(this.GrafPack_MouseMove);
            this.MouseUp += new MouseEventHandler(this.GrafPack_MouseUp);
            this.Paint += new PaintEventHandler(this.GrafPack_Paint);
        }

        // These small methods gets triggered by the Menu clicks
        private void selectSquare(object sender, EventArgs e)
        {
            selectModeStatus = false;
            selectSquareStatus = true;
            selectCircleStatus = false;
            selectTriangleStatus = false;
            selectRectangleStatus = false;
            selectHexagonStatus = false;

        }
        private void selectCircle(object sender, EventArgs e)
        {
            selectModeStatus = false;
            selectCircleStatus = true;
            selectSquareStatus = false;
            selectTriangleStatus = false;
            selectRectangleStatus = false;
            selectHexagonStatus = false;

        }
        private void selectTriangle(object sender, EventArgs e)
        {
            selectModeStatus = false;
            selectTriangleStatus = true;
            selectSquareStatus = false;
            selectCircleStatus = false;
            selectRectangleStatus = false;
            selectHexagonStatus = false;
        }
        private void selectRectangle(object sender, EventArgs e)
        {
            selectModeStatus = false;
            selectTriangleStatus = false;
            selectSquareStatus = false;
            selectCircleStatus = false;
            selectRectangleStatus = true;
            selectHexagonStatus = false;
        }
        private void selectHexagon(object sender, EventArgs e)
        {
            selectModeStatus = false;
            selectTriangleStatus = false;
            selectSquareStatus = false;
            selectCircleStatus = false;
            selectRectangleStatus = false;
            selectHexagonStatus = true;
        }
        private void selectShape(object sender, EventArgs e)
        {
            selectModeStatus = true;
            selectSquareStatus = false;
            selectCircleStatus = false;
            selectTriangleStatus = false;
            selectRectangleStatus = false;
            selectHexagonStatus = false;
        }
        private void selectMove(object sender, EventArgs e)
        {
            moveModeStatus = true;
            selectModeStatus = false;
            selectSquareStatus = false;
            selectCircleStatus = false;
            selectTriangleStatus = false;
            selectRectangleStatus = false;
            selectHexagonStatus = false;
        }
        private void selectRotate(object sender, EventArgs e)
        {
            rotateModeStatus = true;
            moveModeStatus = false;
            selectModeStatus = false;
            selectSquareStatus = false;
            selectCircleStatus = false;
            selectTriangleStatus = false;
            selectRectangleStatus = false;
            selectHexagonStatus = false;
        }

        // This method triggers the "Delete" button when clicked 
        private void actionDelete(object sender, EventArgs e)
        {
            // Loops backward through the list to safely remove items without breaking the index
            for (int i = shapesList.Count - 1; i >= 0; i--)
            {
                if (shapesList[i].IsSelected)
                {
                    shapesList.RemoveAt(i); // Removes it from memory
                }
            }

            // Force the screen to redraw. Since the shape is no longer in the list, 
            // it will be instantly erased from the screen!
            this.Invalidate();
        }

        // Kills the program on the spot
        private void actionExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Calls the WinForms built-in feature i.e., Windows Colour Picker
        private void actionChangeColor(object sender, EventArgs e)
        {
            // Open the standard Windows Color Picker
            ColorDialog colorPicker = new ColorDialog();
            if (colorPicker.ShowDialog() == DialogResult.OK)
            {
                // Apply the chosen color to any selected shapes
                foreach (Shape s in shapesList)
                {
                    if (s.IsSelected)
                    {
                        s.ShapeColor = colorPicker.Color;
                    }
                }
                this.Invalidate(); // Redraw with new colors
            }
        }
        private void actionClearAll(object sender, EventArgs e)
        {
            shapesList.Clear(); // Empties the entire memory list
            this.Invalidate();  // Wipes the screen clean
        }

        // This method activates when a mouse button is pressed down on the canvas
        private void GrafPack_MouseDown(object sender, MouseEventArgs e)
        {
            // Handle Drawing
            if (e.Button == MouseButtons.Left && (selectSquareStatus || selectCircleStatus || selectTriangleStatus || selectRectangleStatus || selectHexagonStatus))
            {
                isDrawing = true;
                startPoint = new Point(e.X, e.Y);
                currentPoint = startPoint;
            }

            // Handle Selecting
            if (e.Button == MouseButtons.Left && selectModeStatus)
            {
                // This will deselect all shapes
                foreach (Shape s in shapesList)
                {
                    s.IsSelected = false;
                }

                // Checks if any shape's vertex was clicked
                // Loops backward so shapes drawn on top get selected first
                for (int i = shapesList.Count - 1; i >= 0; i--)
                {
                    if (shapesList[i].ContainsPoint(new Point(e.X, e.Y)))
                    {
                        shapesList[i].IsSelected = true;
                        break; // Stop after selecting one shape
                    }
                }
                this.Invalidate(); // Forces a redraw to show highlights
            }
            // Handle Moving
            if (e.Button == MouseButtons.Left && moveModeStatus)
            {
                // First, deselect everything so we start fresh
                foreach (Shape s in shapesList)
                {
                    s.IsSelected = false;
                }

                // Check if the user clicked on a shape (looping backwards to grab the top one)
                for (int i = shapesList.Count - 1; i >= 0; i--)
                {
                    if (shapesList[i].ContainsPoint(new Point(e.X, e.Y)))
                    {
                        shapesList[i].IsSelected = true; // Auto-select it!
                        isMoving = true;
                        lastMousePosition = new Point(e.X, e.Y);
                        break; // Stop checking after found
                    }
                }

                this.Invalidate(); // Redraw immediately to show the red highlight
            }
            // Handle Rotating
            if (e.Button == MouseButtons.Left && rotateModeStatus)
            {
                foreach (Shape s in shapesList) s.IsSelected = false;

                for (int i = shapesList.Count - 1; i >= 0; i--)
                {
                    if (shapesList[i].ContainsPoint(new Point(e.X, e.Y)))
                    {
                        shapesList[i].IsSelected = true;
                        isRotating = true;
                        lastMousePosition = new Point(e.X, e.Y);
                        break;
                    }
                }
                this.Invalidate();
            }
        }

        // This method will remain activated as long as the mouse is moving acrose the canvas
        private void GrafPack_MouseMove(object sender, MouseEventArgs e)
        {
            // Updates the current point and force a redraw
            if (isDrawing)
            {
                currentPoint = new Point(e.X, e.Y);
                this.Invalidate(); // This tells the Paint event to activate immediately to update the preview
            }
            if (isMoving && moveModeStatus)
            {
                // Calculates how far the mouse has moved since the last frame
                int deltaX = e.X - lastMousePosition.X;
                int deltaY = e.Y - lastMousePosition.Y;

                // This will find the selected shape and move it
                foreach (Shape s in shapesList)
                {
                    if (s.IsSelected)
                    {
                        s.Move(deltaX, deltaY);
                    }
                }

                // Update the last mouse position for the next frame
                lastMousePosition = new Point(e.X, e.Y);
                this.Invalidate(); // Redraw the screen immediately
            }
            if (isRotating && rotateModeStatus)
            {
                // Only use horizontal mouse movement (X) to determine the angle
                // Moving right spins clockwise, moving left spins counter-clockwise
                float angle = (e.X - lastMousePosition.X);

                foreach (Shape s in shapesList)
                {
                    if (s.IsSelected)
                    {
                        s.Rotate(angle);
                    }
                }

                lastMousePosition = new Point(e.X, e.Y);
                this.Invalidate();
            }
        }

        // This method activates the moment the mouse button is let go. It finalizes the action
        private void GrafPack_MouseUp(object sender, MouseEventArgs e)
        {
            // When the mouse button is released, this will finalize the shape
            if (isDrawing)
            {
                isDrawing = false;
                currentPoint = new Point(e.X, e.Y);

                Shape newShape = null;

                if (selectSquareStatus)
                {
                    newShape = new Square(startPoint, currentPoint);
                }
                else if (selectCircleStatus)
                {
                    newShape = new Circle(startPoint, currentPoint);
                }
                else if (selectTriangleStatus)
                {
                    newShape = new Triangle(startPoint, currentPoint);
                }
                else if (selectRectangleStatus)
                {
                    newShape = new Rectangle(startPoint, currentPoint);
                }
                else if (selectHexagonStatus)
                {
                    newShape = new Hexagon(startPoint, currentPoint);
                }

                if (newShape != null)
                {
                    shapesList.Add(newShape); // Saves it permanently to shapeList
                }

                this.Invalidate(); // One last redraw to show the final shape
            }
            if (isMoving)
            {
                isMoving = false;
            }
            if (isRotating) isRotating = false;
        }

        abstract class Shape
        {
            // Property to track if this shape is highlighted
            public bool IsSelected { get; set; } = false;
            // Every shape will default to black, but can be changed.
            public Color ShapeColor { get; set; } = Color.Black;
            // Abstract method every shape must implement
            public abstract void Draw(Graphics g, Pen pen);
            // Abstract method to check if a point was clicked
            public abstract bool ContainsPoint(Point p);
            // Abstract method to shift the shape by a certain amount (deltaX, deltaY)
            public abstract void Move(int deltaX, int deltaY);
            // Abstract method that forces all shapes to know how to rotate
            public abstract void Rotate(float angleInDegrees);
            // Helper method to calculate distance from the click to a vertex
            protected bool IsCloseToVertex(Point click, Point vertex)
            {
                // 10 pixels is the threshold so the user doesn't have to be pixel-perfect
                double tolerance = 10.0;
                // Using Euclidean Distance Formula to find the exact distance between two points
                double distance = Math.Sqrt(Math.Pow(click.X - vertex.X, 2) + Math.Pow(click.Y - vertex.Y, 2)); 
                return distance <= tolerance;
            }
            // The trigonometry helper method to rotate a single point around a pivot
            protected Point RotatePoint(Point pt, Point pivot, float angleInDegrees)
            {
                // Since computers calculate trig functions in radians, the program converts angle into radians
                double angleInRadians = angleInDegrees * (Math.PI / 180.0);
                double cosTheta = Math.Cos(angleInRadians);
                double sinTheta = Math.Sin(angleInRadians);
                double x = (cosTheta * (pt.X - pivot.X)) - (sinTheta * (pt.Y - pivot.Y)) + pivot.X;
                double y = (sinTheta * (pt.X - pivot.X)) + (cosTheta * (pt.Y - pivot.Y)) + pivot.Y;

                // Convert back to integer points for WinForms
                return new Point((int)Math.Round(x), (int)Math.Round(y));
            }
        }

        class Square : Shape
        {
            //This class contains the specific details for a square defined in terms of opposite corners
            Point keyPt, oppPt;
            public override void Move(int deltaX, int deltaY)
            {
                keyPt = new Point(keyPt.X + deltaX, keyPt.Y + deltaY);
                oppPt = new Point(oppPt.X + deltaX, oppPt.Y + deltaY);
            }
            public override bool ContainsPoint(Point p)
            {
                // Re-calculate the 4 vertices just like in the Draw method
                double xDiff = oppPt.X - keyPt.X;
                double yDiff = oppPt.Y - keyPt.Y;
                double xMid = (oppPt.X + keyPt.X) / 2;
                double yMid = (oppPt.Y + keyPt.Y) / 2;

                Point v1 = keyPt;
                Point v2 = new Point((int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
                Point v3 = oppPt;
                Point v4 = new Point((int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));

                // Check if the click is near ANY of the four corners
                return IsCloseToVertex(p, v1) || IsCloseToVertex(p, v2) || IsCloseToVertex(p, v3) || IsCloseToVertex(p, v4);
            }

            public override void Rotate(float angleInDegrees)
            {
                // Pivot is the exact center of the square
                Point center = new Point((keyPt.X + oppPt.X) / 2, (keyPt.Y + oppPt.Y) / 2);

                keyPt = RotatePoint(keyPt, center, angleInDegrees);
                oppPt = RotatePoint(oppPt, center, angleInDegrees);
            }
            public Square(Point keyPt, Point oppPt)   // constructor
            {
                this.keyPt = keyPt;
                this.oppPt = oppPt;
            }

            // You will need a different draw method for each kind of shape. Note the square is drawn
            // from first principles. All other shapes should similarly be drawn from first principles. 
            // Ideally no C# standard library class or method should be used to create, draw or transform a shape
            // and instead should utilse user-developed code.
            public override void Draw(Graphics g, Pen blackPen)
            {
                // This method draws the square by calculating the positions of the other 2 corners
                double xDiff, yDiff, xMid, yMid;   // Range and mid points of x & y  

                // Calculates ranges and mid points
                xDiff = oppPt.X - keyPt.X;
                yDiff = oppPt.Y - keyPt.Y;
                xMid = (oppPt.X + keyPt.X) / 2;
                yMid = (oppPt.Y + keyPt.Y) / 2;

                // Draw square
                g.DrawLine(blackPen, (int)keyPt.X, (int)keyPt.Y, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
                g.DrawLine(blackPen, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2), (int)oppPt.X, (int)oppPt.Y);
                g.DrawLine(blackPen, (int)oppPt.X, (int)oppPt.Y, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));
                g.DrawLine(blackPen, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2), (int)keyPt.X, (int)keyPt.Y);
            }
        }

        class Circle : Shape
        {
            Point center;
            Point edgePoint;
            public override void Move(int deltaX, int deltaY)
            {
                center = new Point(center.X + deltaX, center.Y + deltaY);
                edgePoint = new Point(edgePoint.X + deltaX, edgePoint.Y + deltaY);
            }
            public override bool ContainsPoint(Point p)
            {
                // 1. Calculates the radius of the circle
                double radius = Math.Sqrt(Math.Pow(edgePoint.X - center.X, 2) + Math.Pow(edgePoint.Y - center.Y, 2));

                // 2. Calculates the distance from the mouse click (p) to the center
                double distance = Math.Sqrt(Math.Pow(p.X - center.X, 2) + Math.Pow(p.Y - center.Y, 2));

                // 3. If the distance is less than or equal to the radius, then its a circle!
                return distance <= radius;
            }
            public override void Rotate(float angleInDegrees)
            {
                // Center is already defined!
                edgePoint = RotatePoint(edgePoint, center, angleInDegrees);
            }
            public Circle(Point center, Point edgePoint)
            {
                this.center = center;
                this.edgePoint = edgePoint;
            }

            public override void Draw(Graphics g, Pen pen)
            {
                // Calculates the radius using the distance formula
                double radius = Math.Sqrt(Math.Pow(edgePoint.X - center.X, 2) + Math.Pow(edgePoint.Y - center.Y, 2));

                // Approximate the circle using a polygon with many sides
                int segments = 100;
                double angleStep = (2 * Math.PI) / segments;

                // Calculates the very first point
                double startX = center.X + radius * Math.Cos(0);
                double startY = center.Y + radius * Math.Sin(0);
                Point prevPoint = new Point((int)startX, (int)startY);

                // Loops through angles to draw the segments
                for (int i = 1; i <= segments; i++)
                {
                    double theta = i * angleStep;
                    double nextX = center.X + radius * Math.Cos(theta);
                    double nextY = center.Y + radius * Math.Sin(theta);
                    Point nextPoint = new Point((int)nextX, (int)nextY);

                    // Draws a tiny line segment
                    g.DrawLine(pen, prevPoint, nextPoint);

                    prevPoint = nextPoint; // Update for the next iteration
                }
            }
        }
        class Triangle : Shape
        {
            Point v1, v2, v3;

            public Triangle(Point startPt, Point endPt)
            {
                // Calculates the vertices once during creation
                v1 = new Point(startPt.X + (endPt.X - startPt.X) / 2, startPt.Y); // Top Center
                v2 = new Point(startPt.X, endPt.Y); // Bottom Left
                v3 = new Point(endPt.X, endPt.Y); // Bottom Right
            }

            public override void Draw(Graphics g, Pen pen)
            {
                g.DrawLine(pen, v1, v2);
                g.DrawLine(pen, v2, v3);
                g.DrawLine(pen, v3, v1);
            }

            public override void Move(int deltaX, int deltaY)
            {
                v1 = new Point(v1.X + deltaX, v1.Y + deltaY);
                v2 = new Point(v2.X + deltaX, v2.Y + deltaY);
                v3 = new Point(v3.X + deltaX, v3.Y + deltaY);
            }
            public override bool ContainsPoint(Point p)
            {
                return IsCloseToVertex(p, v1) || IsCloseToVertex(p, v2) || IsCloseToVertex(p, v3);
            }
            public override void Rotate(float angleInDegrees)
            {
                // Pivot is the centroid (average of all 3 points)
                Point centroid = new Point((v1.X + v2.X + v3.X) / 3, (v1.Y + v2.Y + v3.Y) / 3);

                v1 = RotatePoint(v1, centroid, angleInDegrees);
                v2 = RotatePoint(v2, centroid, angleInDegrees);
                v3 = RotatePoint(v3, centroid, angleInDegrees);
            }
        }

        class Rectangle : Shape
        {
            Point v1, v2, v3, v4;

            public Rectangle(Point startPt, Point endPt)
            {
                v1 = new Point(startPt.X, startPt.Y);         // Top Left
                v2 = new Point(endPt.X, startPt.Y);           // Top Right
                v3 = new Point(endPt.X, endPt.Y);             // Bottom Right
                v4 = new Point(startPt.X, endPt.Y);           // Bottom Left
            }

            public override void Draw(Graphics g, Pen pen)
            {
                g.DrawLine(pen, v1, v2);
                g.DrawLine(pen, v2, v3);
                g.DrawLine(pen, v3, v4);
                g.DrawLine(pen, v4, v1);
            }

            public override void Move(int deltaX, int deltaY)
            {
                v1 = new Point(v1.X + deltaX, v1.Y + deltaY);
                v2 = new Point(v2.X + deltaX, v2.Y + deltaY);
                v3 = new Point(v3.X + deltaX, v3.Y + deltaY);
                v4 = new Point(v4.X + deltaX, v4.Y + deltaY);
            }

            public override void Rotate(float angleInDegrees)
            {
                Point center = new Point((v1.X + v3.X) / 2, (v1.Y + v3.Y) / 2);
                v1 = RotatePoint(v1, center, angleInDegrees);
                v2 = RotatePoint(v2, center, angleInDegrees);
                v3 = RotatePoint(v3, center, angleInDegrees);
                v4 = RotatePoint(v4, center, angleInDegrees);
            }

            public override bool ContainsPoint(Point p)
            {
                return IsCloseToVertex(p, v1) || IsCloseToVertex(p, v2) || IsCloseToVertex(p, v3) || IsCloseToVertex(p, v4);
            }
        }

        class Hexagon : Shape
        {
            Point[] vertices = new Point[6];

            public Hexagon(Point startPt, Point endPt)
            {
                // Finds the center of the dragged box
                Point center = new Point(startPt.X + (endPt.X - startPt.X) / 2, startPt.Y + (endPt.Y - startPt.Y) / 2);

                // Calculates a radius so it fits in the box
                double radius = Math.Min(Math.Abs(endPt.X - startPt.X), Math.Abs(endPt.Y - startPt.Y)) / 2.0;

                // Calculates the 6 points (60 degrees apart)
                for (int i = 0; i < 6; i++)
                {
                    double angleInDegrees = i * 60;
                    double angleInRadians = angleInDegrees * (Math.PI / 180.0);

                    int x = (int)(center.X + radius * Math.Cos(angleInRadians));
                    int y = (int)(center.Y + radius * Math.Sin(angleInRadians));

                    vertices[i] = new Point(x, y);
                }
            }

            public override void Draw(Graphics g, Pen pen)
            {
                // Loops through the array to draw lines between consecutive points
                for (int i = 0; i < 6; i++)
                {
                    // The modulo (%) ensures the last point connects back to the first point (index 0)
                    g.DrawLine(pen, vertices[i], vertices[(i + 1) % 6]);
                }
            }

            public override void Move(int deltaX, int deltaY)
            {
                for (int i = 0; i < 6; i++)
                {
                    vertices[i] = new Point(vertices[i].X + deltaX, vertices[i].Y + deltaY);
                }
            }

            public override void Rotate(float angleInDegrees)
            {
                // Center is the average of opposite points (e.g., index 0 and 3)
                Point center = new Point((vertices[0].X + vertices[3].X) / 2, (vertices[0].Y + vertices[3].Y) / 2);

                for (int i = 0; i < 6; i++)
                {
                    vertices[i] = RotatePoint(vertices[i], center, angleInDegrees);
                }
            }

            public override bool ContainsPoint(Point p)
            {
                foreach (Point v in vertices)
                {
                    if (IsCloseToVertex(p, v)) return true;
                }
                return false;
            }
        }
    }
}

// The END