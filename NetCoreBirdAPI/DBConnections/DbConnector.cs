using MySql.Data.MySqlClient;
using NetCoreBirdAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBirdAPI.DBConnections
{
    public class DbConnector
    {

        public static string server = "mytestdb.c98qcb3crjgp.us-east-2.rds.amazonaws.com";
        static string database = "testdb";
        static string user = "Juupperi";
        static string password = "12344321";
        static string port = "3306";
        static string sslM = "none";


        //SPECIES QUERIES BELOW----------------------------------------------------------------------------------

        public static DataTable GetSpeciesFromDB()
        {

            var connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);
            var query = "";

            query = "SELECT * FROM testdb.species ORDER BY speciename ASC";



            List<String> species = new List<String>();
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(query, connection);

            try
            {
                var dt = new DataTable();

                connection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(dt);
                command.Dispose();
                connection.Close();
                return dt;

            }
            catch (Exception)
            {
                //throw
                return null;
            }
        }



        //SPECIES QUERIES ABOVE----------------------------------------------------------------------------------
        //USER AUTHORIZATION/CREATION QUERIES BELOW--------------------------------------------------------------

        public static DataTable GetUserFromDB(string username)
        {

            var connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);
            var query = "";

            query = "SELECT * FROM testdb.users WHERE username = '@username'";
            query = query.Replace("@username", username);



            List<String> species = new List<String>();
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(query, connection);

            try
            {
                var dt = new DataTable();

                connection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(dt);
                command.Dispose();
                connection.Close();
                return dt;

            }
            catch (Exception)
            {
                //throw
                return null;
            }
        }

        public static bool AddUserToDB(User usercredentials)
        {
            var connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);
            //var query = "INSERT INTO users (username, passwordhash) VALUES('@username', '@password')";



            //ADD user to users-table
            try
            {
                var query = "INSERT INTO users (username, passwordhash) VALUES('@username', '@password')";
                query = query.Replace("@username", usercredentials.username)
                    .Replace("@password", usercredentials.passwordhash);
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                return true;

            }
            catch (Exception)
            {
                //throw
                return false;
            }
        }

        //USER AUTHORIZATION/CREATION QUERIES ABOVE------------------------------------------------------------
        //TOWER QUERIES BELOW----------------------------------------------------------------------------------

        public static DataTable GetTowersFromDB(string municipal)
        {

            var connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);
            var query = "";
            if (municipal == null)
            {
                query = "SELECT * FROM testdb.towers ORDER BY municipal ASC";
            }
            else
            {
                query = "SELECT * FROM testdb.towers WHERE municipal = '@municipal' ORDER BY  municipal ASC";
                query = query.Replace("@municipal", municipal);
            }

            List<String> towers = new List<String>();
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(query, connection);

            try
            {
                var dt = new DataTable();

                connection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(dt);
                command.Dispose();
                connection.Close();
                return dt;

            }
            catch (Exception)
            {
                //throw
                return null;
            }
        }

        //TOWER QUERIES ABOVE----------------------------------------------------------------------------------
        //SIGHTING QUERIES BELOW-------------------------------------------------------------------------------

        public static bool UpdateSightingsToDB(BirdSighting sighting)
        {
            var connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);
            var query = "INSERT INTO sightings (username, specie, longitudecoord, latitudecoord, comment, timestamp)" +
                " VALUES('@username', '@specie', @longitudecoord, @latitudecoord,'@comment',@timestamp)";
            query = query.Replace("@username", sighting.username)

                //Add string params
                .Replace("@specie", sighting.specie)
                .Replace("@comment", sighting.comment);

            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                //Add non string params
                command.Parameters.AddWithValue("@timestamp", sighting.timestamp);
                command.Parameters.AddWithValue("@longitudecoord", sighting.longitudecoord);
                command.Parameters.AddWithValue("@latitudecoord", sighting.latitudecoord);

                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                return true;
            }
            catch (Exception)
            {
                //throw
                return false;
            }
        }

        public static DataTable GetSightingsFromDB(string username)
        {

            var connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);
            var query = "SELECT * FROM testdb.sightings WHERE username = '@user'";

            query = query.Replace("@user", username);



            List<String> species = new List<String>();
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(query, connection);

            try
            {
                var dt = new DataTable();

                connection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(dt);
                command.Dispose();
                connection.Close();
                return dt;

            }
            catch (Exception)
            {
                //throw
                return null;
            }
        }
    }
}
