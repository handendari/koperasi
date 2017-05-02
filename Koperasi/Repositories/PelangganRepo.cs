using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using koperasi.Models;
using System.Data;
using koperasi.GeneralClasses;

namespace koperasi.Repositories
{
    public class PelangganRepo
    {
        private readonly static log4net.ILog Log = log4net.LogManager.GetLogger("PelangganRepo");

        public List<pelangganModels> GetListHutang(string kdPelanggan)
        {
            List<pelangganModels> dList = new List<pelangganModels>();
            string SqlString = @"Select a.id,a.kode as no_trans,a.tgl_registrasi,a.nama,a.alamat,a.kode as kd_pelanggan,
                                        a.batas_kredit,a.bunga as persen_bunga, a.waktu as jangka_waktu ,
                                        a.tbunga as bunga_per_bulan, a.bunga2 as pokok_per_bulan,
                                        a.pinjaman as sisa_pinjaman, a.angsuran as angsuran_per_bulan ,
                                        b.simwajib,b.simpanan as simpanan_tmk
                                from anggota as b LEFT join pelanggan as a on a.kota = b.noang
                                where a.pinjaman > 10 and b.noang = ? ";

            string ConnStr = ManageString.GetConnStr();
            using (OleDbConnection conn = new OleDbConnection(ConnStr))
            {
                conn.Open();

                using (OleDbCommand cmd = new OleDbCommand(SqlString, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("noang", kdPelanggan);

                    using (OleDbDataReader aa = cmd.ExecuteReader())
                    {
                        if (aa.HasRows)
                        {
                            //Log.Debug(DateTime.Now + " GetPelangganREPO ====>>>>>> Jumlah Data : " + aa.Cast<object>().Count());
                            //Log.Debug(DateTime.Now + " aa READ >>>>>> " + aa.Read().ToString());

                            while (aa.Read())
                            {
                                //Log.Debug(DateTime.Now + " NOTRANS ====>>>>>> " + aa["no_trans"].ToString());

                                //string f_0 = aa.GetFieldType(0).ToString();
                                //string f_1 = aa.GetFieldType(1).ToString();
                                //string f_2 = aa.GetFieldType(2).ToString();
                                //string f_3 = aa.GetFieldType(3).ToString();
                                //string f_4 = aa.GetFieldType(4).ToString();
                                //string f_5 = aa.GetFieldType(5).ToString();
                                //string f_6 = aa.GetFieldType(6).ToString();
                                //string f_7 = aa.GetFieldType(7).ToString();
                                //string f_8 = aa.GetFieldType(8).ToString();
                                //string f_9 = aa.GetFieldType(9).ToString();
                                //string f_10 = aa.GetFieldType(10).ToString();
                                //string f_11 = aa.GetFieldType(11).ToString();
                                //string f_12 = aa.GetFieldType(12).ToString();

                                //Log.Debug(DateTime.Now + " F_0 : " + f_0 + "\n" + " F_1 : " + f_1 + "\n" + " F_2 : " + f_2 + "\n" + 
                                //                         " F_3 : " + f_3 + "\n" + " F_4 : " + f_4 + "\n" + " F_5 : " + f_5 + "\n" +
                                //                         " F_6 : " + f_6 + "\n" + " F_7 : " + f_7 + "\n" + " F_8 : " + f_8 + "\n" +
                                //                         " F_9 : " + f_9 + "\n" + " F_10 : " + f_10 + "\n" + " F_11 : " + f_11 + "\n" +
                                //                         " F_12 : " + f_12);
                     
                                pelangganModels item = new pelangganModels();

                                item.id = aa.GetInt32(0);
                                item.no_trans = aa.GetString(1);
                                item.tgl_registrasi = aa.GetDateTime(2);
                                item.nama = aa.GetString(3);
                                item.alamat = aa.GetString(4);
                                item.kd_pelanggan = aa.GetString(5);
                                item.batas_kredit = aa.GetDecimal(6);
                                item.persen_bunga = Math.Round(Convert.ToDecimal(aa.GetFloat(7)), 2);
                                item.jangka_waktu = aa.GetInt16(8);
                                item.bunga_per_bulan = Math.Round(Convert.ToDecimal(aa.GetDouble(9)),2);
                                item.pokok_per_bulan = Math.Round(Convert.ToDecimal(aa.GetDouble(10)),2);
                                item.sisa_pinjaman = Math.Round(Convert.ToDecimal(aa.GetDouble(11)),2);
                                item.angsuran_per_bulan = Math.Round(Convert.ToDecimal(aa.GetDouble(12)),2);
                                item.simpanan_wajib = Math.Round(Convert.ToDecimal(aa.GetDouble(13)),2);
                                item.simpanan_tmk = Math.Round(Convert.ToDecimal(aa.GetDouble(14)),2);
                                dList.Add(item);
                            }
                            //Log.Debug(DateTime.Now + " GetPelangganREPO ====>>>>>> Jumlah LIST : " + dList.Count());
                        }
                    }
                }
            }
            return dList;
        }

        public List<pelangganModels> GetListHutangByCompany(string kdCompany)
        {
            List<pelangganModels> dList = new List<pelangganModels>();
            string strSQL = @"Select b.noang,b.nama,b.alamat,b.simwajib,b.simpanan as simpanan_tmk,
                                       a.tgl_registrasi,
                                       IIF(ISNULL(a.batas_kredit),0,a.batas_kredit) as batas_kredit,
                                       IIF(ISNULL(a.bunga),0,a.bunga) as persen_bunga, 
                                       IIF(ISNULL(a.waktu),0,a.waktu) as jangka_waktu ,
                                       IIF(ISNULL(a.tbunga),0,a.tbunga) as bunga_per_bulan, 
                                       IIF(ISNULL(a.bunga2),0,a.bunga2) as pokok_per_bulan,
                                       IIF(ISNULL(a.pinjaman),0,a.pinjaman) as sisa_pinjaman, 
                                       IIF(ISNULL(a.angsuran),0,a.angsuran) as angsuran_per_bulan 
                                FROM anggota as b 
                                LEFT JOIN (SELECT id,kota,kode,tgl_registrasi,nama,alamat,kode,batas_kredit,bunga,waktu,tbunga,bunga2,pinjaman,angsuran 
                                                    FROM pelanggan WHERE pinjaman > 10) as a on a.kota = b.noang
                                WHERE b.sektor = ? ORDER BY b.nama";

            string ConnStr = ManageString.GetConnStr();
            using (OleDbConnection conn = new OleDbConnection(ConnStr))
            {
                conn.Open();

                using (OleDbCommand cmd = new OleDbCommand(strSQL, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("sektor", kdCompany);

                    using (OleDbDataReader aa = cmd.ExecuteReader())
                    {
                        if (aa.HasRows)
                        {

                            while (aa.Read())
                            {
                                //string f_0 = aa.GetFieldType(0).ToString();
                                //string f_1 = aa.GetFieldType(1).ToString();
                                //string f_2 = aa.GetFieldType(2).ToString();
                                //string f_3 = aa.GetFieldType(3).ToString();
                                //string f_4 = aa.GetFieldType(4).ToString();
                                //string f_5 = aa.GetFieldType(5).ToString();
                                //string f_6 = aa.GetFieldType(6).ToString();
                                //string f_7 = aa.GetFieldType(7).ToString();
                                //string f_8 = aa.GetFieldType(8).ToString();
                                //string f_9 = aa.GetFieldType(9).ToString();
                                //string f_10 = aa.GetFieldType(10).ToString();
                                //string f_11 = aa.GetFieldType(11).ToString();
                                //string f_12 = aa.GetFieldType(12).ToString();

                                //Log.Debug(DateTime.Now + " F_0 : " + f_0 + "\n" + " F_1 : " + f_1 + "\n" + " F_2 : " + f_2 + "\n" +
                                //                         " F_3 : " + f_3 + "\n" + " F_4 : " + f_4 + "\n" + " F_5 : " + f_5 + "\n" +
                                //                         " F_6 : " + f_6 + "\n" + " F_7 : " + f_7 + "\n" + " F_8 : " + f_8 + "\n" +
                                //                         " F_9 : " + f_9 + "\n" + " F_10 : " + f_10 + "\n" + " F_11 : " + f_11 + "\n" +
                                //                         " F_12 : " + f_12);

                                pelangganModels item = new pelangganModels();

                                item.kd_pelanggan = aa.GetString(0);
                                item.nama = aa.GetString(1);
                                item.alamat = aa.GetString(2);
                                item.simpanan_wajib = Math.Round(Convert.ToDecimal(aa.GetDouble(3)), 2);
                                item.simpanan_tmk = Math.Round(Convert.ToDecimal(aa.GetDouble(4)), 2);
                                //item.tgl_registrasi = aa.GetDateTime(5);

                                DateTime? dt = aa[5] as DateTime?;
                                item.tgl_registrasi = dt;

                                item.batas_kredit = aa.GetDecimal(6);
                                item.persen_bunga = Math.Round(Convert.ToDecimal(aa.GetDouble(7)), 2);
                                item.jangka_waktu = aa.GetInt32(8);
                                item.bunga_per_bulan = Math.Round(Convert.ToDecimal(aa.GetDouble(9)), 2);
                                item.pokok_per_bulan = Math.Round(Convert.ToDecimal(aa.GetDouble(10)), 2);
                                item.sisa_pinjaman = Math.Round(Convert.ToDecimal(aa.GetDouble(11)), 2);
                                item.angsuran_per_bulan = Math.Round(Convert.ToDecimal(aa.GetDouble(12)), 2);
                                dList.Add(item);
                            }
                            Log.Debug(DateTime.Now + " GetPelangganREPO ====>>>>>> Jumlah LIST : " + dList.Count());
                        }
                    }
                }
            }
            return dList;
        }

        public anggotaModels GetInfoAnggota(string kdPelanggan)
        {
            var item = new anggotaModels();

            string SqlString = @"Select b.id,b.NoAng, b.Nama,b.alamat, b.kota, b.sektor as kantor, b.tgl_registrasi,
                                        b.simwajib,b.simpanan as simpanan_tmk
                                from anggota as b where b.noang = ? ";

            string ConnStr = ManageString.GetConnStr();
            try
            {
                using (OleDbConnection conn = new OleDbConnection(ConnStr))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(SqlString, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("noang", kdPelanggan);

                        using (OleDbDataReader aa = cmd.ExecuteReader())
                        {
                            if (aa.HasRows)
                            {
                                //Log.Debug(DateTime.Now + " GetPelangganREPO ====>>>>>> Jumlah Data : " + aa.Cast<object>().Count());
                                while (aa.Read())
                                {
                                   // Log.Debug(DateTime.Now + "GetInfoAnggota Repo, Nama : " + aa["nama"].ToString());

                                    item.id = aa.GetInt32(0);
                                    item.no_anggota = aa.GetString(1);
                                    item.nama_anggota = aa.GetString(2);
                                    item.alamat = aa.GetString(3);
                                    item.kota = aa.GetString(4);
                                    item.kantor = aa.GetString(5);
                                    item.tgl_registrasi = aa.GetDateTime(6);
                                    item.simpanan_wajib = Math.Round(Convert.ToDecimal(aa.GetDouble(7)), 2);
                                    item.simpanan_tmk = Math.Round(Convert.ToDecimal(aa.GetDouble(8)), 2);
                                }
                                //Log.Debug(DateTime.Now + " GetPelangganREPO ====>>>>>> Jumlah LIST : " + dList.Count());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug(DateTime.Now + " GetInfoAnggota ERROR ====>>>> No Anggota : " + kdPelanggan,ex);
            }

            return item;
        }

    }
}