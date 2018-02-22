using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;

namespace Custom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        Brick brick;
        enum State { Foward, Back, Stopped, Unconnected };
        State state = State.Unconnected;

        static int turnSpeed = 40;
        static int normalSpeed = 80;

        int rightSpeed = normalSpeed;
        int leftSpeed = normalSpeed;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void KeyPress_Handler(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                Output.Text = "ENTER";
                brick = new Brick(new UsbCommunication());
                await brick.ConnectAsync();
                await brick.DirectCommand.PlayToneAsync(40, 2, 500);
                state = State.Stopped;
            }
            if (Keyboard.IsKeyDown(Key.W) && state == State.Stopped)
            {
                Output.Text = "UP";
                brick.BatchCommand.TurnMotorAtSpeed(OutputPort.A, leftSpeed);
                brick.BatchCommand.TurnMotorAtSpeed(OutputPort.D, rightSpeed);
                brick.BatchCommand.StartMotor(OutputPort.A);
                brick.BatchCommand.StartMotor(OutputPort.D);
                await brick.BatchCommand.SendCommandAsync();
                state = State.Foward;
            }
            if (Keyboard.IsKeyDown(Key.S) && state == State.Stopped)
            {
                Output.Text = "UP";
                brick.BatchCommand.TurnMotorAtSpeed(OutputPort.A, -leftSpeed);
                brick.BatchCommand.TurnMotorAtSpeed(OutputPort.D, -rightSpeed);
                brick.BatchCommand.StartMotor(OutputPort.A);
                brick.BatchCommand.StartMotor(OutputPort.D);
                await brick.BatchCommand.SendCommandAsync();
                state = State.Back;
            }
            if (Keyboard.IsKeyDown(Key.Space) && (state == State.Foward || state == State.Back))
            {
                Output.Text = "SPACE";
                brick.BatchCommand.StopMotor(OutputPort.A, true);
                brick.BatchCommand.StopMotor(OutputPort.D, true);
                await brick.BatchCommand.SendCommandAsync();
                state = State.Stopped;
            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                Output.Text = "LEFT";
                leftSpeed = turnSpeed;
            }
            if (Keyboard.IsKeyDown(Key.D))
            {
                Output.Text = "RIGHT";
                rightSpeed = turnSpeed;
            }
            if (Keyboard.IsKeyDown(Key.Q))
            {
                Output.Text = "Q";
                rightSpeed = normalSpeed;
                leftSpeed = -normalSpeed;
            }
            if (Keyboard.IsKeyDown(Key.E))
            {
                Output.Text = "E";
                leftSpeed = normalSpeed;
                rightSpeed = -normalSpeed;
            }
        }

        private async void KeyUp_Handler(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.W || e.Key == Key.S) && (state == State.Foward || state == State.Back))
            {
                Output.Text = "KEY UP";
                brick.BatchCommand.StopMotor(OutputPort.A, false);
                brick.BatchCommand.StopMotor(OutputPort.D, false);
                await brick.BatchCommand.SendCommandAsync();
                state = State.Stopped;
            }
            if (e.Key == Key.A)
            {
                Output.Text = "LEFT UP";
                leftSpeed = normalSpeed;
            }
            if (e.Key == Key.D)
            {
                Output.Text = "RIGHT UP";
                rightSpeed = normalSpeed;
            }
            if (e.Key == Key.Q)
            {
                Output.Text = "Q UP";
                rightSpeed = normalSpeed;
                leftSpeed = normalSpeed;
            }
            if (e.Key == Key.E)
            {
                Output.Text = "E UP";
                rightSpeed = normalSpeed;
                leftSpeed = normalSpeed;
            }
        }
    }
}
