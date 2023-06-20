using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace quiztime.classes
{

    internal class data
    {
        private List<ObservableCollection<CQuiz>> quiz = new List<ObservableCollection<CQuiz>>();
        private MySqlConnection conn;
        private MySqlConnection conn2;
        public data()
        {
            string myConnectionString = "server=localhost;uid=quiztime;" +
                "pwd=quiztime;database=quiztime";

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                conn2 = new MySql.Data.MySqlClient.MySqlConnection();
                conn2.ConnectionString = myConnectionString;
                conn2.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        public List<ObservableCollection<CQuiz>> test()
        {
            var quizdata = new List<CQuiz>();

            string sql = @"select * from quizzen where disabled = 1";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            //cmd.Parameters.Add("@name",MySqlDbType.VarChar).Value = name;
            //cmd.Parameters.Add("@picture", MySqlType.VarChar).Value = surname;
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                quizdata.Add(new CQuiz());
                quizdata[quizdata.Count - 1].Id = Convert.ToString(reader["id"]);
                quizdata[quizdata.Count - 1].Name = Convert.ToString(reader["naam"]);
                quizdata[quizdata.Count - 1].Picture = Convert.ToString(reader["afbeelding"]);

                ObservableCollection<CQuiz> tempOC = new ObservableCollection<CQuiz>();
                tempOC.Add(new CQuiz() { Id = Convert.ToString(reader["id"]), Name = Convert.ToString(reader["naam"]), Picture = Convert.ToString(reader["afbeelding"]) });
                quiz.Add(tempOC);
                //quiz.MyList.Add(new CQuiz());
            }
            return quiz;
        }

        public List<string[]> getquestions(int ID)
        {

            List<string[]> questions = new List<string[]>();
            int counter = 1;
            string vraag;
            string sql = @"select * from vragen where quizzen_id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = ID;
            //cmd.Parameters.Add("@picture", MySqlType.VarChar).Value = surname;
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string[] question = new string[12];
                question[0] = Convert.ToString(reader["vraag"]);
                question[5] = Convert.ToString(reader["afbeelding"]);
                question[11] = Convert.ToString(reader["timer"]);

                vraag = Convert.ToString(reader["id"]);

                string sql2 = @"select * from antwoorden where vragen_id = @id";
                MySqlCommand cmd2 = new MySqlCommand(sql2, conn2);
                cmd2.Parameters.Add("@id", MySqlDbType.VarChar).Value = vraag;
                //cmd.Parameters.Add("@picture", MySqlType.VarChar).Value = surname;
                MySqlDataReader reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {
                    question[counter] = Convert.ToString(reader2["antwoord"]);
                    question[counter + 6] = Convert.ToString(reader2["correct"]);
                    counter++;
                }
                reader2.Close();
                sql2 = @"select * from quizzen where id = @id";
                cmd2 = new MySqlCommand(sql2, conn2);
                cmd2.Parameters.Add("@id", MySqlDbType.VarChar).Value = ID;
                //cmd.Parameters.Add("@picture", MySqlType.VarChar).Value = surname;
                reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {
                    question[6] = Convert.ToString(reader2["afbeelding"]);
                }
                reader2.Close();
                questions.Add(question);
                counter = 1;
            }
            return questions;
        }
        public void enterdatabase(List<string[]> quizdata)
        {
            string sql = @"INSERT INTO quizzen (naam,afbeelding) VALUES (@name,@image)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = quizdata[0][0];
            cmd.Parameters.Add("@image", MySqlDbType.VarChar).Value = quizdata[0][1];
            cmd.ExecuteNonQuery();
            long quizid = cmd.LastInsertedId;
            for (int i = 1; i < quizdata.Count; i++)
            {
                string sql2 = @"INSERT INTO vragen (vraag,afbeelding,timer,quizzen_id) VALUES (@question,@image, @timer ,@quizid)";
                MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                cmd2.Parameters.Add("@question", MySqlDbType.VarChar).Value = quizdata[i][0];
                cmd2.Parameters.Add("@image", MySqlDbType.VarChar).Value = quizdata[i][1];
                cmd2.Parameters.Add("@timer", MySqlDbType.VarChar).Value = quizdata[i][2];
                cmd2.Parameters.Add("@quizid", MySqlDbType.VarChar).Value = quizid;
                cmd2.ExecuteNonQuery();
                long vraagid = cmd2.LastInsertedId;
                for (int j = 0; j < 4; j++)
                {
                    string sql3 = @"INSERT INTO antwoorden (antwoord,correct,vragen_id) VALUES (@question,@image,@quizid)";
                    MySqlCommand cmd3 = new MySqlCommand(sql3, conn);
                    cmd3.Parameters.Add("@question", MySqlDbType.VarChar).Value = quizdata[i][j + 3];
                    cmd3.Parameters.Add("@image", MySqlDbType.VarChar).Value = quizdata[i][j + 7];
                    cmd3.Parameters.Add("@quizid", MySqlDbType.VarChar).Value = vraagid;
                    cmd3.ExecuteNonQuery();
                }

            }
        }

        public List<string[]> fetcheditor(int id)
        {
            string vraag;
            string[] question = new string[2];
            List<string[]> quizdata = new List<string[]>();
            string sql2 = @"select * from quizzen where id = @id";
            MySqlCommand cmd2 = new MySqlCommand(sql2, conn2);
            cmd2.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
            //cmd.Parameters.Add("@picture", MySqlType.VarChar).Value = surname;
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                question[0] = Convert.ToString(reader2["naam"]);
                question[1] = Convert.ToString(reader2["afbeelding"]);
            }
            reader2.Close();
            quizdata.Add(question);

            string sql = @"select * from vragen where quizzen_id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
            //cmd.Parameters.Add("@picture", MySqlType.VarChar).Value = surname;
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                question = new string[11];
                question[0] = Convert.ToString(reader["vraag"]);
                question[1] = Convert.ToString(reader["afbeelding"]);
                question[2] = Convert.ToString(reader["timer"]);

                vraag = Convert.ToString(reader["id"]);
                sql2 = @"select * from antwoorden where vragen_id = @id";
                cmd2 = new MySqlCommand(sql2, conn2);
                cmd2.Parameters.Add("@id", MySqlDbType.VarChar).Value = vraag;
                //cmd.Parameters.Add("@picture", MySqlType.VarChar).Value = surname;
                reader2 = cmd2.ExecuteReader();
                int counter = 3;
                while (reader2.Read())
                {
                    question[counter] = Convert.ToString(reader2["antwoord"]);
                    question[counter + 4] = Convert.ToString(reader2["correct"]);
                    counter++;
                }
                reader2.Close();
                quizdata.Add(question);
            }
            reader.Close();
            return quizdata;
        }

        public void updatequiz(int id)
        {

        }
        public void delete(int id)
        {
            string sql = @"UPDATE quizzen SET disabled = 0 WHERE id = @name";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = id;
            cmd.ExecuteNonQuery();
        }
    }
}
