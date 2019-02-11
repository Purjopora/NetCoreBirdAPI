using MySql.Data.MySqlClient;
using NetCoreBirdAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using NetCoreBirdAPI.Algorithms;
using NetCoreBirdAPI.Common;

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
        private static int FRESH_BIRD_TIMESPAN = 1; //24 hours for now

        public static bool AddTowerToDB(Tower tower)
        {
            var connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);
            var query = "INSERT INTO towers (id, municipal, towername, longitudecoord, latitudecoord) VALUES('@id', '@municipal', '@towername', '@longitudecoord', '@latitudecoord')";

            query = query.Replace("@id", tower.id)
                .Replace("@municipal", tower.municipal)
                .Replace("@towername", tower.towername)
                .Replace("@longitudecoord", tower.longitudecoord)
                .Replace("@latitudecoord", tower.latitudecoord);

            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
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


        //---------------------------------------------------------------
        //ACCOUNT MANAGEMENT STUFF BELOW

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

        //---------------------------------------------
        //ADD NEW SIGHTINGS TO DB

        public static bool UpdateSightingsToDB(BirdSighting sighting)
        {
            var connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);
            var query = "INSERT INTO sightings (username, specie, longitudecoord, latitudecoord, comment, timestamp)" +
                " VALUES('@username', '@specie', @longitudecoord, @latitudecoord,'@comment',@timestamp)";
            query = query.Replace("@username", sighting.username)

                //ADD empty comment if none provided


                .Replace("@specie", sighting.specie)
                //.Replace("@longitudecoord", sighting.longitudecoord)
                //.Replace("@latitudecoord", sighting.latitudecoord)
                .Replace("@comment", sighting.comment);
            //.Replace("@timestamp", sighting.timestamp.ToString());

            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@timestamp", DateTime.Now);
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

        public static DataTable GetSightingsForBird(string bird)
        {
            DataTable dt = new DataTable();
            bird = bird.Replace("-", "");
            string query = "SELECT * FROM testdb.bird" + bird + ";";
            return executeQuery(query);
        }
        public static DataTable getTables()
        {
            DataTable dt = new DataTable();
            string query = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'testdb';";

            return executeQuery(query);
        }

        public static bool updateSightings()
        {
            DateTime now = DateTime.Now;
            DataTable resultdt = DbConnector.GetSpeciesFromDB();
            if (resultdt == null)
            {
                return false;
            }
            foreach (DataRow row in resultdt.Rows)
            {
                String birdTableName = "testdb.bird" + row["speciename"].ToString();
                birdTableName = birdTableName.Replace("-", "");
                String clearQuery = "DELETE from " + birdTableName + ";";
                executeNonQuery(clearQuery);


                string sightingsQuery = "SELECT * FROM testdb.sightings WHERE specie = '@name';";
                sightingsQuery = sightingsQuery.Replace("@name", row["speciename"].ToString());
                DataTable birds = executeQuery(sightingsQuery);
                if (birds == null)
                {
                    return false;
                }
                var freshBirds = new List<BirdSighting>();
                foreach (DataRow birbRow in birds.Rows)
                {
                    DateTime time = Convert.ToDateTime(birbRow["timestamp"]);
                    TimeSpan span = now.Subtract(time);
                    double lon = Convert.ToDouble(birbRow["longitudecoord"]);
                    double lat = Convert.ToDouble(birbRow["latitudecoord"]);
                    if (span.CompareTo(TimeSpan.FromDays(FRESH_BIRD_TIMESPAN)) < 0)
                    {
                        var sighting = new BirdSighting
                        {
                            username = "",
                            specie = row["speciename"].ToString(),
                            longitudecoord = lon,
                            latitudecoord = lat,
                            comment = "",
                            timestamp = now
                        };
                        freshBirds.Add(sighting);
                    }
                }
                double[][] clusters = KekMeansLocationProviderAdapter.ClusterPartitions(freshBirds.ToList<LocationProvider>(), KekMeansLocationProviderAdapter.DEFAULT_CLUSTER_AMOUNT);
                string insertQuery = "INSERT INTO " + birdTableName + " VALUES(@latitude, @longtitude);";
                for (int i = 0; i < clusters.Length; i++)
                {
                    string insertCopy = String.Copy(insertQuery);
                    insertCopy = insertCopy.Replace("@latitude", clusters[i][0].ToString());
                    insertCopy = insertCopy.Replace("@longtitude", clusters[i][1].ToString());
                    executeNonQuery(insertCopy);
                }
            }
            return true;
        }
        private static void executeNonQuery(string query)
        {
            var connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        private static DataTable executeQuery(string query)
        {
            var connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                try
                {
                    var dt = new DataTable();

                    connection.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    da.Fill(dt);
                    return dt;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
        }
    }
}
