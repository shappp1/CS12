using System;
using System.Drawing;
using System.Windows.Forms;

namespace PeakCraft {
    public partial class PeakCraft : Form {
        private BufferedGraphicsContext gContext;
        private BufferedGraphics gBuff;
        private Block[,,] world = new Block[8, 8, 8];
        private Player player = new Player(0, 0, -5);

        private Color background_color = Color.White;
        private Brush filled_brush = new SolidBrush(Color.BlanchedAlmond);
        private Pen wireframe_pen = new Pen(Color.Black);

        public PeakCraft() {
            InitializeComponent();
            gContext = BufferedGraphicsManager.Current;
            gBuff = gContext.Allocate(CreateGraphics(), DisplayRectangle);

            // setup world
            for (int x = 0; x < world.GetLength(0); x++) {
                for (int y = 0; y < world.GetLength(1); y++) {
                    for (int z = 0; z < world.GetLength(2); z++) {
                        world[x, y, z] = new Block(true);
                    }
                }
            }

            // configure settings
            // default settings are okay for now
            Settings.FOV = 60;

            timer1.Enabled = true;
        }

        private void Draw() {
            // DrawWorldWireframe();
            DrawWorldFilled();
        }

        private void DrawUI() {

        }

        private void DrawWorldFilled() {
            gBuff.Graphics.Clear(background_color);

            Vec3[] projected;
            PointF[] to_draw = new PointF[8];
            bool[] should_draw = new bool[8];

            for (int x = 0; x < world.GetLength(0); x++) {
                for (int y = 0; y < world.GetLength(1); y++) {
                    for (int z = 0; z < world.GetLength(2); z++) {
                        if (!world[x, y, z].Visible) continue;

                        //points = Block.GetVertices(x, y, z);
                        projected = ProjectPoints(Block.GetVertices(x, y, z), .01, 100);

                        // convert projected points to screen coordinates
                        for (int i = 0; i < 8; i++) {
                            // random arbitrary bounds for X and Y
                            if (projected[i].Z < -1 || projected[i].Z > 1) {
                                should_draw[i] = false;
                            } else {
                                to_draw[i].X = (float)(Width * (1 + projected[i].X) / 2);
                                to_draw[i].Y = (float)(Height * (1 - projected[i].Y) / 2);
                                should_draw[i] = true;
                            }
                        }

                        foreach (int[] indices in Block.FaceIndices) {
                            // don't draw face
                            if (!(should_draw[indices[0]] && should_draw[indices[1]] && should_draw[indices[2]] && should_draw[indices[3]]))
                                continue;

                            // cull faces not facing player
                            if (Vec3.CrossProduct(projected[indices[1]] - projected[indices[0]], projected[indices[2]] - projected[indices[1]]).Z >= 0)
                                continue;
                            
                            

                            PointF[] points = new PointF[] { to_draw[indices[0]], to_draw[indices[1]], to_draw[indices[2]], to_draw[indices[3]] };
                            gBuff.Graphics.FillPolygon(filled_brush, points);
                            gBuff.Graphics.DrawPolygon(wireframe_pen, points);
                        }
                    }
                }
            }
            gBuff.Render();
        }

        private void DrawWorldWireframe() {
            gBuff.Graphics.Clear(background_color);

            Vec3[] projected;
            PointF[] to_draw = new PointF[8];

            for (int x = 0; x < world.GetLength(0); x++) {
                for (int y = 0; y < world.GetLength(1); y++) {
                    for (int z = 0; z < world.GetLength(2); z++) {
                        if (!world[x, y, z].Visible) continue;

                        //points = Block.GetVertices(x, y, z);
                        projected = ProjectPoints(Block.GetVertices(x, y, z), .01, 100);

                        PointF invalid = new PointF(int.MinValue, int.MinValue);
                        // convert projected points to screen coordinates
                        for (int i = 0; i < 8; i++) {
                            if (projected[i].X < -100 || projected[i].X > 100 || projected[i].Y < -100 || projected[i].Y > 100 || projected[i].Z < -1 || projected[i].Z > 1) {
                                to_draw[i] = invalid;
                            } else {
                                to_draw[i].X = (float)(Width * (1 + projected[i].X) / 2);
                                to_draw[i].Y = (float)(Height * (1 - projected[i].Y) / 2);
                            }
                        }

                        foreach (int[] indices in Block.FaceIndices) {
                            if (to_draw[indices[0]].Equals(invalid) || to_draw[indices[1]].Equals(invalid) || to_draw[indices[2]].Equals(invalid) || to_draw[indices[3]].Equals(invalid))
                                continue;
                            gBuff.Graphics.DrawPolygon(wireframe_pen, new PointF[] { to_draw[indices[0]], to_draw[indices[1]], to_draw[indices[2]], to_draw[indices[3]] });
                        }
                    }
                }
            }
            gBuff.Render();
        }

        private Vec3[] ProjectPoints(Vec3[] points, double near, double far) {
            Vec3[] rotY = new Vec3[points.Length];
            Vec3[] rotX = new Vec3[points.Length];
            Vec3[] proj = new Vec3[points.Length];

            double ycos = Math.Cos(player.Yaw);
            double ysin = Math.Sin(player.Yaw);
            double rcos = Math.Cos(player.Pitch);
            double rsin = Math.Sin(player.Pitch);
            for (int i = 0; i < points.Length; i++) {
                points[i] -= player.Position;

                rotY[i].X = ycos * points[i].X - ysin * points[i].Z;
                rotY[i].Y = points[i].Y;
                rotY[i].Z = ycos * points[i].Z + ysin * points[i].X;

                rotX[i].X = rotY[i].X;
                rotX[i].Y = rcos * rotY[i].Y - rsin * rotY[i].Z;
                rotX[i].Z = rcos * rotY[i].Z + rsin * rotY[i].Y;

                double top = near * Math.Tan(Settings.FOV / 2 * (Math.PI / 180));
                double right = top * Width / Height;
                proj[i].X = near * rotX[i].X / (right * rotX[i].Z);
                proj[i].Y = near * rotX[i].Y / (top * rotX[i].Z);
                proj[i].Z = (-(far+near)*rotX[i].Z + 2*far*near) / ((far - near) * rotX[i].Z); // fix later
            }
            return proj;
        }

        private void FormResized(object sender, EventArgs e) {
            if (DisplayRectangle.Height == 0) return;
            gBuff = gContext.Allocate(CreateGraphics(), DisplayRectangle);
            //Draw();
        }

        // this is just used for now to draw the world after form initialization
        private void Tick(object sender, EventArgs e) {
            if (KBState.KeyDown(Keys.W))
                player.Position += Vec3.RotateY(new Vec3(0, 0, .25), player.Yaw);
            if (KBState.KeyDown(Keys.S))
                player.Position += Vec3.RotateY(new Vec3(0, 0, -.25), player.Yaw);
            if (KBState.KeyDown(Keys.A))
                player.Position += Vec3.RotateY(new Vec3(-.25, 0, 0), player.Yaw);
            if (KBState.KeyDown(Keys.D))
                player.Position += Vec3.RotateY(new Vec3(.25, 0, 0), player.Yaw);
            if (KBState.KeyDown(Keys.Space))
                player.Position += new Vec3(0, .25, 0);
            if (KBState.KeyDown(Keys.ShiftKey))
                player.Position += new Vec3(0, -.25, 0);
            if (KBState.KeyDown(Keys.Up))
                player.Pitch += 0.05;
            if (KBState.KeyDown(Keys.Down))
                player.Pitch -= 0.05;
            if (KBState.KeyDown(Keys.Left))
                player.Yaw -= 0.05;
            if (KBState.KeyDown(Keys.Right))
                player.Yaw += 0.05;
            Draw();
        }

        private void KDown(object sender, KeyEventArgs e) {
            KBState.SetKeyDown(e.KeyCode);
            if (e.KeyCode == Keys.OemSemicolon) {

            }
        }

        private void KUp(object sender, KeyEventArgs e) {
            KBState.SetKeyUp(e.KeyCode);
        }
    }

    class Player {
        public Vec3 Position { get; set; }
        public double Pitch { get; set; } // ignore for now
        public double Yaw { get; set; }

        public Player(double x, double y, double z, double pitch = 0, double yaw = 0) {
            Position = new Vec3(x, y, z);
            Pitch = pitch;
            Yaw = yaw;
        }
    }

    static class Settings {
        public static double FOV { get; set; } = 60;
        public static double SPEED { get; set; } = .1;
        public static double SENSITIVITY { get; set; } = .01;
    }

    class Block {
        public bool Tangible { get; set; } // determines whether a block has collision
        public bool Visible { get; set; } // determines whether a block is drawn

        // can index into this with BlockFace
        public static int[][] FaceIndices { get; private set; } = {
            new int[] { 0, 1, 2, 3 }, // front
            new int[] { 4, 5, 6, 7 }, // back
            new int[] { 5, 0, 3, 6 }, // left
            new int[] { 1, 4, 7, 2 }, // right
            new int[] { 5, 4, 1, 0 }, // top
            new int[] { 3, 2, 7, 6 }, // bottom
        };

        public Block() {
            Tangible = false;
            Visible = false;
        }
        public Block(bool visible = false, bool tangible = false) {
            Visible = visible;
            Tangible = tangible;
            
        }

        //public static T[][] MapSides<T>(ref T[] arr, Face face) {
        //    T[][] ret = new T[6][];
        //    return new T[] { arr[Indices[(int)face][0]], arr[Indices[(int)face][1]], arr[Indices[(int)face][2]], arr[Indices[(int)face][3]] };
        //}

        public static Vec3[] GetVertices(int x, int y, int z) => new Vec3[] { new Vec3(x, y + 1, z), new Vec3(x + 1, y + 1, z), new Vec3(x + 1, y, z), new Vec3(x, y, z), new Vec3(x + 1, y + 1, z + 1), new Vec3(x, y + 1, z + 1), new Vec3(x, y, z + 1), new Vec3(x + 1, y, z + 1) };
    }

    enum Face {
        FRONT = 0, BACK = 1, LEFT = 2, RIGHT = 3, TOP = 4, BOTTOM = 5
    }

    struct Vec3 {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vec3(double x, double y, double z) {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vec3 RotateY(Vec3 vec, double theta) {
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);

            return new Vec3(cos * vec.X + sin * vec.Z, vec.Y, cos * vec.Z - sin * vec.X);
        }

        public static Vec3 operator +(Vec3 a, Vec3 b) {
            return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3 operator -(Vec3 a) {
            return new Vec3(-a.X, -a.Y, -a.Z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b) {
            return a + (-b);
        }
        
        public static Vec3 CrossProduct(Vec3 a, Vec3 b) {
            return new Vec3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public override string ToString() {
            return $"{{X={X},Y={Y},Z={Z}}}";
        }
    }

    static class KBState {
        // index into this with Keys, true if key is currently down
        static bool[] state = new bool[Enum.GetNames(typeof(Keys)).Length];

        public static bool KeyDown(Keys key) {
            return state[(int)key];
        }

        public static void SetKeyDown(Keys key) {
            state[(int)key] = true;
        }

        public static void SetKeyUp(Keys key) {
            state[(int)key] = false;
        }
    }
}
