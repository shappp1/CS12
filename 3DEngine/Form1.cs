using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3DEngine {
    public partial class Form1 : Form {
        private Graphics g;
        private Player player;
        // defaults to all VACUUM   x  y  z
        private Block[,,] world = new Block[8, 1, 8];

        public Form1() {
            InitializeComponent();
            Width = 1024;
            Height = 1024;
            g = CreateGraphics();

            // fill bottom layer with SLEEPY_STONE
            for (int i = 0; i < world.GetLength(0); i++) {
                for (int j = 0; j < world.GetLength(2); j++) {
                    world[i, 0, j] = new Block(BlockType.SLEEPY_STONE, new Point3D(i, 0, j));
                }
            }

            player = new Player(new Point3D(0, 1, 0));
            Settings.FOV = 60;
            Settings.PLAYER_SPEED = 01;

            GameTimer.Enabled = true;
        }

        private void Tick(object sender, EventArgs e) {
            g.Clear(Color.SkyBlue);

            foreach(Block block in world) {
                DrawWireframe(Color.Black, block);
            }

            GameTimer.Enabled = false;
        }

        private void DrawWireframe(Color color, Block block) {
            Point3D[][] faces = { block.GetFace(BlockFace.FRONT), block.GetFace(BlockFace.BACK), block.GetFace(BlockFace.TOP), block.GetFace(BlockFace.BOTTOM), block.GetFace(BlockFace.LEFT), block.GetFace(BlockFace.RIGHT) };
            double FL = 1.0 / Math.Tan(Settings.FOV * (Math.PI / 180) / 2.0);

            foreach (Point3D[] points in faces) {
                Point[] projected = new Point[points.Length];

                for (int i = 0; i < points.Length; i++) {
                    points[i] = player.GetRelativePoint(points[i]);
                    projected[i].X = (int)(Width * (1 + points[i].x * FL / (FL + points[i].z)) / 2);
                    projected[i].Y = (int)(Height * (1 - points[i].y * FL / (FL + points[i].z)) / 2);
                }
                
                g.DrawPolygon(new Pen(color), projected);
            }
        }

        private void KeyPressed(object sender, KeyPressEventArgs e) {
            switch (e.KeyChar) {
                case 'w':
                    player.pos.z += Settings.PLAYER_SPEED;
                    break;
                case 's':
                    player.pos.z -= Settings.PLAYER_SPEED;
                    break;
                case 'a':
                    player.pos.x -= Settings.PLAYER_SPEED;
                    break;
                case 'd':
                    player.pos.x += Settings.PLAYER_SPEED;
                    break;
                case 'q':
                    player.pos.y -= Settings.PLAYER_SPEED;
                    break;
                case 'e':
                    player.pos.y += Settings.PLAYER_SPEED;
                    break;
                default:
                    break;
            }

            System.Diagnostics.Debug.WriteLine($"{player.pos.x} {player.pos.y} {player.pos.z}");

            Tick(sender, e);
        }
    }

    public static class Settings {
        public static double FOV; // in degrees
        public static double PLAYER_SPEED;
    }

    public class Player {
        public Point3D pos;
        public double pitch, yaw;

        public Player(Point3D pos = null, double pitch = 0, double yaw = 0) {
            if (pos == null) {
                this.pos = new Point3D(0, 0, 0);
            } else {
                this.pos = new Point3D(pos.x, pos.y, pos.z);
            }

            this.pitch = pitch;
            this.yaw = yaw;
        }

        public Point3D GetRelativePoint(Point3D world) {
            return new Point3D(world.x - pos.x, world.y - pos.y, world.z - pos.z);
        }
    }

    public class Point3D {
        public double x, y, z;

        public Point3D(double x, double y, double z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class Block {
        public BlockType type;

        // redundant, can just use index into world to find pos, but using for now
        public Point3D pos;

        public Block(BlockType type, Point3D pos) {
            this.type = type;
            this.pos = new Point3D(pos.x, pos.y, pos.z);
        }

        public Point3D[] GetFace(BlockFace face) {
            switch (face) {
                case BlockFace.FRONT:
                    return new Point3D[] { new Point3D(pos.x, pos.y + 1, pos.z), new Point3D(pos.x + 1, pos.y + 1, pos.z), new Point3D(pos.x + 1, pos.y, pos.z), new Point3D(pos.x, pos.y, pos.z) };
                case BlockFace.BACK:
                    return new Point3D[] { new Point3D(pos.x + 1, pos.y + 1, pos.z + 1), new Point3D(pos.x, pos.y + 1, pos.z + 1), new Point3D(pos.x, pos.y, pos.z + 1), new Point3D(pos.x + 1, pos.y, pos.z + 1) };
                case BlockFace.TOP:
                    return new Point3D[] { new Point3D(pos.x, pos.y + 1, pos.z + 1), new Point3D(pos.x + 1, pos.y + 1, pos.z + 1), new Point3D(pos.x + 1, pos.y + 1, pos.z), new Point3D(pos.x, pos.y + 1, pos.z) };
                case BlockFace.BOTTOM:
                    return new Point3D[] { new Point3D(pos.x, pos.y, pos.z), new Point3D(pos.x + 1, pos.y, pos.z), new Point3D(pos.x + 1, pos.y + 1, pos.z), new Point3D(pos.x, pos.y + 1, pos.z) };
                case BlockFace.LEFT:
                    return new Point3D[] { new Point3D(pos.x, pos.y + 1, pos.z + 1), new Point3D(pos.x, pos.y + 1, pos.z), new Point3D(pos.x, pos.y, pos.z), new Point3D(pos.x, pos.y, pos.z + 1) };
                case BlockFace.RIGHT:
                    return new Point3D[] { new Point3D(pos.x + 1, pos.y + 1, pos.z), new Point3D(pos.x + 1, pos.y + 1, pos.z + 1), new Point3D(pos.x + 1, pos.y, pos.z + 1), new Point3D(pos.x + 1, pos.y, pos.z) };
                default:
                    return null;
            }
        }
    }

    public enum BlockType {
        VACUUM, AWAKE_STONE, COBBLED_STONE, SOAPY_STONE, GRASSLESS_DIRT, NON_GRASSLESS_DIRT, TABLE_OF_CRAFTNESS, SLEEPY_STONE
    }
    public enum BlockFace {
        FRONT, BACK, TOP, BOTTOM, LEFT, RIGHT
    }
}
