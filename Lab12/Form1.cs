using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace OrderPipeline;

public partial class Form1 : Form
{
    public class OrderEventArgs : EventArgs
    {
        public string Customer { get; }
        public string Product { get; }
        public int Quantity { get; }
        public OrderEventArgs(string customer, string product, int quantity)
        {
            Customer = customer;
            Product = product;
            Quantity = quantity;
        }
    }

    public class ShipEventArgs : EventArgs
    {
        public string Product { get; }
        public bool Express { get; }
        public ShipEventArgs(string product, bool express)
        {
            Product = product;
            Express = express;
        }
    }

    public event EventHandler<OrderEventArgs> OrderPlaced;
    public event EventHandler OrderRejected;
    public event EventHandler<OrderEventArgs> OrderConfirmed;
    public event EventHandler<ShipEventArgs> OrderShipped;
    private bool isOrderConfirmed = false;


    public Form1()
    {
        InitializeComponent();

        cmbProduct.Items.AddRange(new string[] { "Laptop", "Mouse", "Keyboard" });
        cmbProduct.SelectedIndex = 0;

        OrderPlaced += ValidateOrder;
        OrderPlaced += ShowOrderInfo;
        OrderRejected += ShowRejection;
        OrderConfirmed += ShowConfirmation;

        btnProcessOrder.Click += BtnProcessOrder_Click;
        btnShipOrder.Click += BtnShipOrder_Click;
    }

    private void BtnProcessOrder_Click(object sender, EventArgs e)
    {
        string customer = txtCustomer.Text;
        string product = cmbProduct.SelectedItem.ToString();
        int quantity = (int)numQuantity.Value;

        lblStatus.Text = "Processing order...";
        OrderEventArgs orderArgs = new OrderEventArgs(customer, product, quantity);
        OrderPlaced?.Invoke(this, orderArgs);
    }

    private void ValidateOrder(object sender, OrderEventArgs e)
    {
        if (e.Quantity > 0)
        {
            lblStatus.Text = "Validated order";
            isOrderConfirmed = true;
            OrderConfirmed?.Invoke(this, e);
        }
        else
        {
            isOrderConfirmed = false;
            OrderRejected?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ShowOrderInfo(object sender, OrderEventArgs e)
    {
        MessageBox.Show($"Customer: {e.Customer}\nProduct: {e.Product}\nQuantity: {e.Quantity}", "Order Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ShowRejection(object sender, EventArgs e)
    {
        lblStatus.Text = "Order rejected: Quantity must be greater than zero.";
        MessageBox.Show("Order rejected due to invalid quantity.", "Order Rejected", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void ShowConfirmation(object sender, OrderEventArgs e)
    {
        lblStatus.Text = "Order confirmed.";
        MessageBox.Show("Order has been confirmed!", "Order Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnShipOrder_Click(object sender, EventArgs e)
    {
        if (!isOrderConfirmed)
        {
            MessageBox.Show("Cannot ship order: Order not confirmed.", "Shipping Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        string product = cmbProduct.SelectedItem.ToString();
        bool express = chkExpress.Checked;

        OrderShipped -= ShowDispatch;
        OrderShipped += ShowDispatch;

        if (express)
        {
            OrderShipped += NotifyCourier;
        }
        else
        {
            OrderShipped -= NotifyCourier;
        }

        lblStatus.Text = "Shipping order...";
        ShipEventArgs shipArgs = new ShipEventArgs(product, express);
        OrderShipped?.Invoke(this, shipArgs);

        lblStatus.Text = "Order shipped.";
        MessageBox.Show($"Order for {product} has been shipped. Express: {express}", "Order Shipped", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ShowDispatch(object sender, ShipEventArgs e)
    {
        MessageBox.Show($"Dispatching {e.Product}. Express: {e.Express}", "Dispatch Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    private void NotifyCourier(object sender, ShipEventArgs e)
    {
        MessageBox.Show($"Notifying courier for express delivery of {e.Product}.", "Courier Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
