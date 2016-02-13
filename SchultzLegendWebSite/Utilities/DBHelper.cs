using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace SchultzLegendWebSite.Utilities
{
    public class DBHelper
    {
        #region Tracker
        public static List<Page> DBGetTrackerPages()
        {
            List<Page> pages = new List<Page>();
            MySqlConnection cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sldbConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand();

            string sSql = "CALL `schultzlegenddb`.`TRACKER_GET_PAGES`();";

            cmd.Connection = cn;
            cmd.CommandText = sSql;
            cmd.Connection.Open();
            MySqlDataReader reader =  cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            while(reader.Read())
            {
                Page page = new Page();
                page.Name = Convert.ToString(reader["page_name"]);
                page.Link = Convert.ToString(reader["page_link"]);
                pages.Add(page);
            }
            reader.Close();
            cmd.Connection.Close();
            return pages;
        }

        #region Tasks
        public static List<Task> DBGetTasks(string userid)
        {
            List<Task> tasks = new List<Task>();
            MySqlConnection cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sldbConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand();

            string sSql = String.Format("CALL `schultzlegenddb`.`TRACKER_TASKS_GET_BY_USERID`('{0}');", userid);

            cmd.Connection = cn;
            cmd.CommandText = sSql;
            cmd.Connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            while (reader.Read())
            {
                Task task = new Task();
                task.Name = Convert.ToString(reader["name"]);
                task.UserId = Convert.ToString(reader["user"]);
                task.TaskId = (int)reader["task_id"];
                task.Parent = (int)reader["parent"];
                task.Complete = (sbyte)reader["complete"];
                task.Priority = (int)reader["priority"];
                tasks.Add(task);
            }
            reader.Close();
            cmd.Connection.Close();
            return tasks;
        }

        public static Task DBGetTaskByName(string name)
        {
            Task task = new Task();
            MySqlConnection cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sldbConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand();

            string sSql = String.Format("CALL `schultzlegenddb`.`TRACKER_TASK_GET_BY_NAME`('{0}');", name);

            cmd.Connection = cn;
            cmd.CommandText = sSql;
            cmd.Connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            if (reader.Read())
            {
                task.Name = Convert.ToString(reader["name"]);
                task.UserId = Convert.ToString(reader["user"]);
                task.TaskId = (int)reader["task_id"];
                task.Parent = (int)reader["parent"];
                task.Complete = (sbyte)reader["complete"];
                task.Priority = (int)reader["priority"];
            }
            reader.Close();
            cmd.Connection.Close();
            return task;
        }

        public static void DBAddTask(string userid, string name, int parent = 0)
        {
            MySqlConnection cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sldbConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand();

            string sSql = String.Format("CALL `schultzlegenddb`.`TRACKER_TASKS_ADD`('{0}', '{1}', '{2}');", userid, name, parent);

            cmd.Connection = cn;
            cmd.CommandText = sSql;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }

        public static void DBChangeTaskName(string userid, int taskid, string newname)
        {
            MySqlConnection cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sldbConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand();

            string sSql = String.Format("CALL `schultzlegenddb`.`TRACKER_TASK_CHANGE_NAME`('{0}', '{1}', '{2}');", userid, taskid, newname);

            cmd.Connection = cn;
            cmd.CommandText = sSql;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }

        public static void DBChangeTaskStatus(string userid, int taskid, sbyte complete )
        {
            MySqlConnection cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sldbConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand();

            string sSql = String.Format("CALL `schultzlegenddb`.`TRACKER_TASK_CHANGE_STATUS`('{0}', '{1}', '{2}');", userid, taskid, complete);

            cmd.Connection = cn;
            cmd.CommandText = sSql;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }

        public static void DBDeleteTask(string userid, int taskid)
        {
            MySqlConnection cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sldbConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand();

            string sSql = String.Format("CALL `schultzlegenddb`.`TRACKER_TASK_DELETE`('{0}', '{1}');", taskid, userid);

            cmd.Connection = cn;
            cmd.CommandText = sSql;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }

        #endregion


        #endregion

        #region User Login
        public static void DBCreateUserLogin(string email, string passwordHash)
        {
            MySqlConnection cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sldbConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand();

            string sSql = String.Format("CALL `schultzlegenddb`.`USER_NEW`('{0}', '{1}');", email, passwordHash);

            cmd.Connection = cn;
            cmd.CommandText = sSql;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            
        }

        public static void DBConfirmUserEmail(string email)
        {
            MySqlConnection cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sldbConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand();

            string sSql = String.Format("CALL `schultzlegenddb`.`USER_CONFIRM_EMAIL`('{0}');", email);

            cmd.Connection = cn;
            cmd.CommandText = sSql;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }

        public static UserLogin DBGetUserLogin(string email)
        {
            UserLogin user = new UserLogin();

            MySqlConnection cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sldbConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand();

            string sSql = String.Format("CALL `schultzlegenddb`.`USER_GET_LOGIN`('{0}');", email);

            cmd.Connection = cn;
            cmd.CommandText = sSql;
            cmd.Connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            if (reader.Read())
            {
                user.Id = Convert.ToString(reader["id"]);
                user.Email = Convert.ToString(reader["email"]);
                user.EmailConfirmed = Convert.ToBoolean(reader["email_conf"]);
                user.PasswordHash = Convert.ToString(reader["phash"]);
            }
            else
            {
                reader.Close();
                cmd.Connection.Close();
                return null;
            }
            reader.Close();
            cmd.Connection.Close();
            return user;
        }

        public static UserLogin DBGetUserLoginFromToken(string token)
        {
            UserLogin user = new UserLogin();

            MySqlConnection cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sldbConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand();

            string sSql = String.Format("CALL `schultzlegenddb`.`USER_GET_LOGIN_FROM_TOKEN`('{0}');", token);

            cmd.Connection = cn;
            cmd.CommandText = sSql;
            cmd.Connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            if (reader.Read())
            {
                user.Id = Convert.ToString(reader["id"]);
                user.Email = Convert.ToString(reader["email"]);
                user.EmailConfirmed = Convert.ToBoolean(reader["email_conf"]);
                user.PasswordHash = Convert.ToString(reader["phash"]);
            }
            else
            {
                reader.Close();
                cmd.Connection.Close();
                return null;
            }
            reader.Close();
            cmd.Connection.Close();
            return user;
        }

        public static void DBCChangeUserPassword(string email, string phash)
        {
            MySqlConnection cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sldbConnectionString"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand();

            string sSql = String.Format("CALL `schultzlegenddb`.`USER_CHANGE_PASSWORD`('{0}', '{1}');", email, phash);

            cmd.Connection = cn;
            cmd.CommandText = sSql;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        #endregion
    }
}