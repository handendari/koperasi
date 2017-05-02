using koperasi.GeneralClasses;
using koperasi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace koperasi.Repositories
{
    public class UserRepo
    {
        private readonly static log4net.ILog Log = log4net.LogManager.GetLogger("UserRepo");

        public void InsertPassword(passwordModels pModel)
        {

            Log.Debug(DateTime.Now + " REPO INSERT Pass 111 ====>>>>>> No Anggota : " + pModel.no_anggota + " Pass : " + pModel.password);

            string SqlString = @"INSERT INTO mpassword (no_anggota,[password],keterangan)
                                            VALUES (?,?,?)";

            string ConnStr = ManageString.GetConnStr();
            using (OleDbConnection conn = new OleDbConnection(ConnStr))
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(SqlString, conn))
                {

                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("no_anggota", pModel.no_anggota);
                    cmd.Parameters.AddWithValue("pass", pModel.password);
                    cmd.Parameters.AddWithValue("keterangan", " ");


                    try
                    {
                        var status = cmd.ExecuteNonQuery();
                        Log.Debug(DateTime.Now + " REPO INSERT Pass ====>>>>>> No Anggota : " + pModel.no_anggota + " Pass : " + pModel.password);

                    }
                    catch (Exception ex)
                    {
                        Log.Debug(DateTime.Now + " REPO INSERT Pass ERR ====>>>>>> No Anggota : " + 
                            pModel.no_anggota + " Pass : " + pModel.password + " err : " + ex.Message);
                    }
                }
            }
        }
       
        public List<passwordModels> getAllAnggotaList()
        {
            var vListUser = new List<passwordModels>();
            string sqlUser = @"SELECT NoAng,Nama 
                                FROM Anggota as an LEFT JOIN mpassword as mp on an.noang = mp.no_anggota
                                WHERE mp.no_anggota is null";

            string ConnStr = ManageString.GetConnStr();
            using (OleDbConnection conn = new OleDbConnection(ConnStr))
            {
                using (OleDbCommand cmd = new OleDbCommand(sqlUser, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();

                    using (OleDbDataReader aa = cmd.ExecuteReader())
                    {
                        if (aa.HasRows)
                        {
                            while (aa.Read())
                            {
                                //Log.Debug(DateTime.Now + " NO Anggota ====>>>>>> " + aa["NoAng"].ToString());
                                var m = new passwordModels
                                {
                                    no_anggota = aa.GetString(0),
                                    nama_anggota = aa.GetString(1)
                                };
                                vListUser.Add(m);
                            }
                            //Log.Debug(DateTime.Now + " GetPasswordREPO ====>>>>>> Jumlah Data : " + aa.Count());
                        }
                    }
                }
            }
            return vListUser;
        }

        public void UpdatePassword(passwordModels pModel)
        {
            string SqlString = @"UPDATE mpassword SET [password] = ?,keterangan = ?
                                 WHERE no_anggota = ?";

            Log.Debug(DateTime.Now + " ======>>>> UPDATE PASS, SQLstr = " + SqlString);

            string ConnStr = ManageString.GetConnStr();

            using (OleDbConnection conn = new OleDbConnection(ConnStr))
            {
                using (OleDbCommand cmd = new OleDbCommand(SqlString, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("password", pModel.password);
                    cmd.Parameters.AddWithValue("keterangan", pModel.keterangan);
                    cmd.Parameters.AddWithValue("no_anggota", pModel.no_anggota);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public passwordModels GetPassword(string pNoAnggota)
        {
            var dPass = new passwordModels();
            string SqlString = @"Select mp.no_anggota,an.nama as nama_anggota, mp.[password], mp.keterangan,
                                        IIF (mp.no_anggota = 'SBY-HLD0025' , 'SSD IT' , an.sektor ) as sektor,
                                        mp.[as_admin],mp.id
                                FROM mPassword as mp INNER JOIN anggota AS an ON mp.no_anggota = an.NoAng
                                where mp.no_anggota = ?";

            string ConnStr = ManageString.GetConnStr();

            try
            {
                using (OleDbConnection conn = new OleDbConnection(ConnStr))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(SqlString, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("noanggota", pNoAnggota);

                        using (OleDbDataReader aa = cmd.ExecuteReader())
                        {
                            if (aa.HasRows)
                            {
                                //Log.Debug(DateTime.Now + " MASUK HAS ROWS, ID Type : " + aa.GetFieldType(6).ToString());
                                while (aa.Read())
                                {
                                    dPass.no_anggota = aa.GetString(0);
                                    dPass.nama_anggota = aa.GetString(1);
                                    dPass.password = aa.GetString(2);
                                    dPass.keterangan = aa.GetString(3);
                                    dPass.sektor = aa.GetString(4);
                                    dPass.as_admin = aa.GetInt16(5);
                                    dPass.id = aa.GetInt32(6);

                                    //Log.Debug(DateTime.Now + " NO Anggota : " + aa["no_anggota"].ToString() + " ID : " + aa.GetInt32(6));
                                }
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now + " GetPasswordREPO ====>>>>>> Kode : " + pNoAnggota,ex);
            }
            return dPass;
        }

    }
}