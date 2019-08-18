using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process_Downloads
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = "server=localhost;user=root;database=vfmobo6d_aria2_remote;port=3306;password=;SslMode=none";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                // Perform database operations
                string sql = "SELECT * FROM `tasks` ORDER BY `id`";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("URL : " + rdr[1]);

                    try
                    {
                        var uri = new Uri(rdr[1].ToString().Contains("http")?rdr[1].ToString():"http://"+rdr[1].ToString());
                        string filename = Path.GetFileName(uri.AbsolutePath);
                        Console.WriteLine("File : " + filename);

                        Process myProcess = new Process();
                        try
                        {
                            myProcess.StartInfo.UseShellExecute = false;
                            myProcess.StartInfo.FileName = "C:\\Program Files\\Everything\\Everything.exe";
                            myProcess.StartInfo.Arguments = "-s " + filename;
                            myProcess.Start();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Process Exception : " + e.ToString());
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Filename Exception : " + ex.ToString());
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("MySQL Exception : " + ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}
