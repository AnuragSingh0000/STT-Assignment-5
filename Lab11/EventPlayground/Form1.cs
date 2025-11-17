// using System;
// using System.Drawing;
// using System.Windows.Forms;

// namespace EventPlayground
// {
//     public partial class Form1 : Form
//     {
//         public delegate void ColorChangedEventHandler(Color newColor);
//         public delegate void TextChangedEventHandler(string newText);

//         public event ColorChangedEventHandler ColorChangedEvent;
//         public event TextChangedEventHandler TextChangedEvent;

//         public Form1()
//         {
//             InitializeComponent();
//             InitializeCustomComponents();

//             ColorChangedEvent += OnColorChanged;
//             TextChangedEvent += OnTextChanged;
//         }

//         private void InitializeCustomComponents()
//         {
//             this.Text = "Event Playground";
//             this.Size = new Size(600, 350);
//             this.StartPosition = FormStartPosition.CenterScreen;
//             this.BackColor = Color.WhiteSmoke;

//             Label lblDisplay = new Label
//             {
//                 Name = "lblDisplay",
//                 Text = "Welcome to Events Lab",
//                 Font = new Font("Segoe UI", 16, FontStyle.Bold),
//                 ForeColor = Color.Black,
//                 AutoSize = false,
//                 TextAlign = ContentAlignment.MiddleCenter,
//                 Location = new Point(50, 40),
//                 Size = new Size(480, 50) 
//             };
//             this.Controls.Add(lblDisplay);

//             ComboBox cmbColors = new ComboBox
//             {
//                 Name = "cmbColors",
//                 Location = new Point(50, 120),
//                 DropDownStyle = ComboBoxStyle.DropDownList,
//                 Width = 200,
//                 Font = new Font("Segoe UI", 10, FontStyle.Regular)
//             };
//             cmbColors.Items.AddRange(new string[] { "Red", "Green", "Blue" });
//             cmbColors.SelectedIndex = 0;
//             this.Controls.Add(cmbColors);

//             Button btnChangeColor = new Button
//             {
//                 Name = "btnChangeColor",
//                 Text = "Change Color",
//                 Font = new Font("Segoe UI", 10, FontStyle.Regular),
//                 Location = new Point(50, 180),
//                 Size = new Size(130, 40)
//             };
//             btnChangeColor.Click += (s, e) =>
//             {
//                 string selectedColor = cmbColors.SelectedItem.ToString();
//                 Color color = Color.Black;

//                 switch (selectedColor)
//                 {
//                     case "Red": color = Color.Red; break;
//                     case "Green": color = Color.Green; break;
//                     case "Blue": color = Color.Blue; break;
//                 }
//                 ColorChangedEvent?.Invoke(color);
//             };
//             this.Controls.Add(btnChangeColor);
//             Button btnChangeText = new Button
//             {
//                 Name = "btnChangeText",
//                 Text = "Change Text",
//                 Font = new Font("Segoe UI", 10, FontStyle.Regular),
//                 Location = new Point(200, 180),
//                 Size = new Size(130, 40)
//             };
//             btnChangeText.Click += (s, e) =>
//             {
//                 string newText = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
//                 TextChangedEvent?.Invoke(newText);
//             };
//             this.Controls.Add(btnChangeText);
//         }


//         private void OnColorChanged(Color newColor)
//         {
//             Label lbl = this.Controls["lblDisplay"] as Label;
//             if (lbl != null)
//                 lbl.ForeColor = newColor;
//         }

//         private void OnTextChanged(string newText)
//         {
//             Label lbl = this.Controls["lblDisplay"] as Label;
//             if (lbl != null)
//                 lbl.Text = newText;
//         }
//     }
// }




using System;
using System.Drawing;
using System.Windows.Forms;

namespace EventPlayground
{
    public partial class Form1 : Form
    {
        // Step 1: Define a custom EventArgs class
        public class ColorEventArgs : EventArgs
        {
            public string ColorName { get; }
            public Color SelectedColor { get; }

            public ColorEventArgs(string colorName, Color selectedColor)
            {
                ColorName = colorName;
                SelectedColor = selectedColor;
            }
        }

        // Step 2: Define custom delegates
        public delegate void ColorChangedEventHandler(object sender, ColorEventArgs e);
        public delegate void TextChangedEventHandler(object sender, string newText);

        // Step 3: Define events
        public event ColorChangedEventHandler ColorChangedEvent;
        public event TextChangedEventHandler TextChangedEvent;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();

            // Step 4: Subscribe multiple handlers to the same event (multicast)
            ColorChangedEvent += UpdateLabelColor;
            ColorChangedEvent += ShowNotification;

            TextChangedEvent += UpdateLabelText;
        }

        // Step 5: UI setup
        private void InitializeCustomComponents()
        {
            // === Form settings ===
            this.Text = "Event Playground";
            this.Size = new Size(600, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // === Label ===
            Label lblDisplay = new Label
            {
                Name = "lblDisplay",
                Text = "Welcome to Events Lab",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 40),
                Size = new Size(480, 50)
            };
            this.Controls.Add(lblDisplay);

            // === ComboBox ===
            ComboBox cmbColors = new ComboBox
            {
                Name = "cmbColors",
                Location = new Point(50, 120),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            cmbColors.Items.AddRange(new string[] { "Red", "Green", "Blue" });
            cmbColors.SelectedIndex = 0;
            this.Controls.Add(cmbColors);

            // === Button: Change Color ===
            Button btnChangeColor = new Button
            {
                Name = "btnChangeColor",
                Text = "Change Color",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(50, 180),
                Size = new Size(130, 40)
            };
            btnChangeColor.Click += (s, e) =>
            {
                // Step 6: Raise ColorChangedEvent using ColorEventArgs
                string selectedColorName = cmbColors.SelectedItem.ToString();
                Color selectedColor = Color.Black;

                switch (selectedColorName)
                {
                    case "Red": selectedColor = Color.Red; break;
                    case "Green": selectedColor = Color.Green; break;
                    case "Blue": selectedColor = Color.Blue; break;
                }

                ColorEventArgs args = new ColorEventArgs(selectedColorName, selectedColor);
                ColorChangedEvent?.Invoke(this, args);
            };
            this.Controls.Add(btnChangeColor);

            // === Button: Change Text ===
            Button btnChangeText = new Button
            {
                Name = "btnChangeText",
                Text = "Change Text",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(200, 180),
                Size = new Size(130, 40)
            };
            btnChangeText.Click += (s, e) =>
            {
                string newText = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
                TextChangedEvent?.Invoke(this, newText);
            };
            this.Controls.Add(btnChangeText);
        }

        // Step 7: Event Handlers for ColorChangedEvent (Multicast)
        private void UpdateLabelColor(object sender, ColorEventArgs e)
        {
            Label lbl = this.Controls["lblDisplay"] as Label;
            if (lbl != null)
            {
                lbl.ForeColor = e.SelectedColor;
            }
        }

        private void ShowNotification(object sender, ColorEventArgs e)
        {
            MessageBox.Show($"Label color changed to: {e.ColorName}",
                            "Color Changed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        // Step 8: Handler for TextChangedEvent
        private void UpdateLabelText(object sender, string newText)
        {
            Label lbl = this.Controls["lblDisplay"] as Label;
            if (lbl != null)
            {
                lbl.Text = newText;
            }
        }
    }
}
