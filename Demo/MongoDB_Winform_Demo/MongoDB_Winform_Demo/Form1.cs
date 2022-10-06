using MongoDB.Driver;
using MongoDB_Winform_Demo.Models;
using System.Configuration;

namespace MongoDB_Winform_Demo
{
    public partial class Form1 : Form
    {
        IMongoCollection<Book> bookCollection;
        public Form1()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            var mongoClient = new MongoClient(connectionString);
            var database = mongoClient.GetDatabase(databaseName);
            bookCollection = database.GetCollection<Book>("Books");
            LoadBook();
        }

        private void LoadBook()
        {
            var filter = Builders<Book>.Filter.Empty;
            var book = bookCollection.Find(filter).ToList();
            dgvBook.DataSource = bookCollection.Find(b => true).ToList();
            this.dgvBook.Columns[0].Visible = false;
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            Book book = new Book
            {
                title = txtTitle.Text,
                author = txtAuthor.Text,
                pages = Convert.ToInt32(nmPage.Value),
                genres = txtGenres.Text.Split(", "),
                rating = Convert.ToInt32(nmRating.Value)
            };
            bookCollection.InsertOne(book);
            LoadBook();
        }

        private void dgvBook_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = dgvBook.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
            txtTitle.Text = dgvBook.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
            txtAuthor.Text = dgvBook.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
            nmPage.Text = dgvBook.Rows[e.RowIndex].Cells[3].FormattedValue.ToString();
            nmRating.Text = dgvBook.Rows[e.RowIndex].Cells[4].FormattedValue.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var filter = Builders<Book>.Filter.Eq(b => b.Id, txtID.Text);
            var update = Builders<Book>.Update
                .Set(b => b.title, txtTitle.Text)
                .Set(b => b.author, txtAuthor.Text)
                .Set(b => b.pages, Convert.ToInt32(nmPage.Value))
                .Set(b => b.genres, txtGenres.Text.Split(", "))
                .Set(b => b.rating, Convert.ToInt32(nmRating.Value));
            bookCollection.UpdateOne(filter, update);
            LoadBook();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var filter = Builders<Book>.Filter.Eq(b => b.Id, txtID.Text);
            bookCollection.DeleteOne(filter);
            LoadBook();
        }
    }
}