// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AccelerationMeasurement
{
    public partial class AccelerationBar : Control
    {
        public AccelerationBar()
        {
            InitializeComponent();
            BackColor = Color.White;
        }


        // length og the ticks in pixels
        private const int TickLength = 5;

        // total number of ticks on each side of 'zero'
        private const int Ticks = 5;

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Graphics g = pe.Graphics;

            // draw gauge
            Rectangle gaugeBox = new Rectangle(
                ClientRectangle.Left, ClientRectangle.Top + TickLength,
                ClientRectangle.Width - 2, ClientRectangle.Height - TickLength * 2);
            g.DrawRectangle(Pens.Black, gaugeBox);

            // draw ticks
            g.DrawLine(Pens.Black, ClientRectangle.Width / 2, 0, ClientRectangle.Width / 2, ClientRectangle.Height);

            int totalTicks = (Ticks * 2) + 1;
            float tickSpacing = ClientRectangle.Width / ((float)totalTicks - 1);
            for (int n = 1; n < totalTicks; n++)
            {

                g.DrawLine(Pens.Black,
                    tickSpacing * n, gaugeBox.Bottom,
                    tickSpacing * n, ClientRectangle.Bottom);
            }

            // draw indicator
            float pixelsPerUnit = gaugeBox.Width / (float)totalTicks;
            float gaugeCenter = gaugeBox.Width / 2f + gaugeBox.Left;
            float gaugeMiddle = gaugeBox.Height / 2f + gaugeBox.Top;
            float indicatedPosition = gaugeCenter + (pixelsPerUnit * _acceleration);
            float indicatorOffset = Math.Max(Math.Min(indicatedPosition, gaugeBox.Width), gaugeBox.Left);
            PointF[] indicator = new PointF[]
            {
                new PointF ( indicatorOffset, gaugeBox.Top ),
                new PointF ( indicatorOffset + TickLength, gaugeMiddle ),
                new PointF ( indicatorOffset, gaugeBox.Bottom ),
                new PointF ( indicatorOffset - TickLength, gaugeMiddle ),
                new PointF ( indicatorOffset, gaugeBox.Top )
            };

            Brush fill = (indicatorOffset == indicatedPosition) ? new SolidBrush(Color.Red) : new SolidBrush(Color.Gray);
            g.FillPolygon(fill, indicator, FillMode.Winding);
        }

        public float Acceleration
        {
            set
            {
                _acceleration = value;
                Invalidate();
            }
            get => _acceleration;
        }
        private float _acceleration = 0;
    }
}
