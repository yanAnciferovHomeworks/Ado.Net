using System;
using System.Configuration;
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
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace AuthorClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ObservableCollection<Author> Authors;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public MainWindow()
        {
            InitializeComponent();


            Authors = new ObservableCollection<Author>();
            LV.ItemsSource = Authors;
            Update();
        }


        private void Update()
        {
            string sqlExpression = "SELECT * FROM Authors";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Authors.Clear();

                    while (reader.Read())
                    {
                        Authors.Add(new Author() { Id = reader.GetInt32(0), FirstName = reader.GetString(1), LastName = reader.GetString(2) });
                    }
                }
            }
        }

        private void InsertToDB(Author author)
        {
            string sqlExpression = $"INSERT INTO Authors(FirstName, LastName) VALUES('{author.FirstName}', '{author.LastName}')";



            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Добавлено объектов: {number}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddForm addForm = new AddForm();
            if (addForm.ShowDialog() == true)
            {
                var author = new Author() { FirstName = addForm.FirstName, LastName = addForm.LastName };
                InsertToDB(author);
                Update();
            }
        }

        private void DeleteAuthor(int Id)
        {
            string sqlExpression = $"delete from Authors where Authors.Id = {Id}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    int number = command.ExecuteNonQuery();
                    Update();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    
                }
               
            }
            
        }



        private void ClickHandler(object sender, MouseButtonEventArgs e)
        {
            Delete.IsEnabled = LV.SelectedIndex != -1;  
            Edit.IsEnabled = LV.SelectedIndex != -1;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(LV.SelectedIndex != -1)
            {
                var ut = (Author)LV.SelectedItem;
                DeleteAuthor(ut.Id);
                ClickHandler(null, null);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if(LV.SelectedIndex != -1)
            {
                var ut = (Author)LV.SelectedItem;
                
                AddForm addForm = new AddForm();
                addForm.FirstName = ut.FirstName;
                addForm.LastName = ut.LastName;
                if (addForm.ShowDialog() == true)
                {
                    UpdateAuthor(ut.Id, addForm.FirstName, addForm.LastName);
                    ClickHandler(null, null);
                }
            }

          
        }

        private void UpdateAuthor(int Id, String firstName, String lastName)
        {
            string sqlExpression = $"update Authors Set FirstName = '{firstName}', LastName = '{lastName}' where Authors.Id = {Id}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    int number = command.ExecuteNonQuery();
                    Update();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }

            }
        }

    }

    public class Author
        {
            public int Id { get; set; }

            public String FirstName { get; set; }
            public String LastName { get; set; }

            public String FullName { get { return $"{FirstName} {LastName}"; } }
        }
}
