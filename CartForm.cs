using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;

namespace TrabajoPractico4
{
    public partial class CartForm : Form
    {

        private Market _market { get; }        
        private User _currentUser { get; }
        private Cart _cart{ get; }
        private List<CartProduct> _products { get; }
        private DataTable dt;

        private Product _productActual;

        private double currentTotal;
        

        public CartForm(Market market, User currentUser)
        {
            InitializeComponent();
             dt = new DataTable();
            _market = market;
            _currentUser = currentUser;
            _cart = _currentUser.cart;
            _products = _cart.CartProducts;

            dt.Columns.Add("ID producto");
            dt.Columns.Add("Nombre del producto");
            dt.Columns.Add("Cantidad");

            cartProductsDataGridView.DataSource = dt;
        }

        private void CartForm_Load(object sender, EventArgs e)
        {

            foreach (CartProduct cartProduct in _products)
            {
                int id = cartProduct.product.productId;
                String name = cartProduct.product.name;
                Double price = cartProduct.product.price;
                int ammount = cartProduct.ammount;

                DataRow row = dt.NewRow();

                row["ID producto"] = id;
                row["Nombre del producto"] = name;
                row["Cantidad"] = ammount;                

                dt.Rows.Add(row);

                currentTotal +=price*cartProduct.ammount;

                if (_currentUser.userType == "client")
                {
                    currentTotal = currentTotal * 1.21;
                }
            }
            changeCurrentTotal();
        }


        private void UpdateProductDataGridView()
        {
            dt.Rows.Clear();
            currentTotal = 0;

            foreach (CartProduct cartProduct in _products)
            {
                int id = cartProduct.product.productId;
                String name = cartProduct.product.name;
                Double price = cartProduct.product.price;
                int ammount = cartProduct.ammount;

                DataRow row = dt.NewRow();

                row["ID producto"] = id;
                row["Nombre del producto"] = name;
                row["Cantidad"] = ammount;

                dt.Rows.Add(row);

                currentTotal += price * cartProduct.ammount;

                if (_currentUser.userType == "client")
                {
                    currentTotal = currentTotal * 1.21;
                }
            }
            changeCurrentTotal();
        }        

        private void changeCurrentTotal()
        {
            totalLabel.Text = currentTotal.ToString();
        }

        private void buyButton_Click(object sender, EventArgs e)
        {

            var validation=_market.validateBeforePurchase(_products);
            
            if(validation== false) 
            { 
                    MessageBox.Show("Esta intentando comprar mas productos de los disponibles", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
            }
            else
            {
                if (_products.Count > 0)
                {
                    _market.AddPurchase(_cart,_currentUser);
                
                    MessageBox.Show("Muchas gracias por su compra," + _currentUser.name + "!!" + "\n Su total es de: " + currentTotal, "Muchas gracias", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No tiene productos en su carrito!", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } 
        }

        private void deleteSelectedProductsButton_Click(object sender, EventArgs e)
        {
                      
            if (cartProductsDataGridView.SelectedCells.Count > 0 ) //hay un item seleccionado o mas
            {
                if (cartProductsDataGridView.SelectedCells.Count < 2)
                {
                    Int64 id = Int64.Parse(cartProductsDataGridView.CurrentRow.Cells[0].Value.ToString());
                    Product product= _market.SearchProduct(id);
                    _market.deleteProductFromCart(_currentUser, product);
                    UpdateProductDataGridView();
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un solo producto!", "Atencion!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un producto", "Atencion!",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void backButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void cartProductsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {                
                Int64 productId = Int64.Parse(cartProductsDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());

                _productActual = _market.SearchProduct(productId);
            }
        }
    }
}
