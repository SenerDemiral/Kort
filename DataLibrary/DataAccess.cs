using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataLibrary.Models;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;

namespace DataLibrary
{

    public class DataAccess : IDataAccess
    {
        string connectionString = "";

        private readonly IConfiguration _config;
        private Dictionary<string, string> myTableFields = new Dictionary<string, string>();
        private List<SexModel> sexList;
        private List<SklModel> rrDrmList;
        private List<SklModel> tgSklList;
        private List<SklModel> uuSklList;
        private List<ZmnModel> zmnList;

        public DataAccess(IConfiguration config)
        {
            _config = config;
            connectionString = config.GetConnectionString("default");
        }
        public DataAccess(string connString)
        {
            connectionString = connString;
            //TableDefs.initTableDefs();

            sexList = new List<SexModel>() {
                new SexModel { Sex = "E", Ad = "Erkek" },
                new SexModel { Sex = "K", Ad = "Kadın" },
                new SexModel { Sex = "X", Ad = "XX" }
            };

            rrDrmList = new List<SklModel>() {
                new SklModel { Skl = "R", Ad = "Rzrv" },
                new SklModel { Skl = "I", Ad = "İptal" },
                new SklModel { Skl = "X", Ad = "Gelmedi" }
            };

            uuSklList = new List<SklModel>() {
                new SklModel { Skl = "U", Ad = "Üye" },
                new SklModel { Skl = "O", Ad = "Öğrenci" },
                new SklModel { Skl = "G", Ad = "Grup" },
                new SklModel { Skl = "M", Ad = "Misafir" },
                new SklModel { Skl = "A", Ad = "Admin" },
                new SklModel { Skl = "H", Ad = "Hoca" },
            };

            tgSklList = new List<SklModel>() {
                new SklModel { Skl = "K", Ad = "Kişi" },
                new SklModel { Skl = "R", Ad = "Rzrvsyn" }
            };

            zmnList = new List<ZmnModel>();
            DateTime Trh = new DateTime(2020, 01, 01, 06, 0, 0);    // 06:00 - 23:30
            for (int i=0; i<36; i++)
						{
                zmnList.Add(new ZmnModel { Dak = Trh.Hour * 60 + Trh.Minute, Ad = Trh.ToString("HH:mm") });
                Trh = Trh.AddMinutes(30);
            }

            //myTableFields = MyTableFields(typeof(KHmodel));
        }

        public List<SexModel> SexList()
        {
            return sexList;
        }

        public List<SklModel> RrDrmList()
        {
            return rrDrmList;
        }
        public List<SklModel> UuSklList()
        {
            return uuSklList;
        }
        public List<SklModel> TgSklList()
        {
            return tgSklList;
        }
        public List<ZmnModel> ZmnList()
        {
            return zmnList;
        }

        public async Task<T> LoadRec<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = new FbConnection(connectionString))
            {
                var row = await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
                return row;
            }
        }


        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = new FbConnection(connectionString))
            {
                var rows = await connection.QueryAsync<T>(sql, parameters);

                return rows.ToList();
            }
        }

        public Task SaveData<T>(string sql, T parameters)
        {
            using (IDbConnection connection = new FbConnection(connectionString))
            {
                return connection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task UpdateRec<T>(T dataItem, IDictionary<string, object> newValue)
        {
            StringBuilder sql = new System.Text.StringBuilder();
            int dc = newValue.Count;
            bool first = true;
            if (dc > 0)
            {
                Type t = typeof(T);

                var (tblName, keyName) = MyTableNameAndKey(t);


                sql.Append($"update {tblName} set ");
                foreach (var n in newValue)
                {
                    sql.Append($"{(first ? "" : ",")} {n.Key} = @{n.Key} ");
                    first = false;
                    // newValue yu dataItem a da koy, UI de degissin
                    dataItem.GetType().GetProperty(n.Key)?.SetValue(dataItem, n.Value);
                }
                var objVal = dataItem.GetType().GetProperty(keyName)?.GetValue(dataItem);
                sql.Append($" where {keyName} = {objVal}");  // PK
                Console.WriteLine(sql.ToString());
                await SaveData(sql.ToString(), newValue);
            }
        }

        public async Task<T> InsertRec<T>(IDictionary<string, object> newValue) where T : new()
        {
            //string sql = "insert into UST (UstID, Ad) values (@UstID, @Ad)";
            var dataItem = new T();

            int dc = newValue.Count;
            if (dc > 0)
            {
                bool first = true;
                StringBuilder sql = new StringBuilder();
                StringBuilder prm = new StringBuilder();
                var (tblName, keyName) = MyTableNameAndKey(typeof(T));

                newValue.Add(keyName, GetPK()); // Get PK

                sql.Append($"insert into {tblName}(");
                prm.Append("values(");

                foreach (var n in newValue)
                {
                    sql.Append($"{(first ? "" : ",")}{n.Key} ");
                    prm.Append($"{(first ? "" : ",")}@{n.Key} ");
                    first = false;

                    dataItem.GetType().GetProperty(n.Key)?.SetValue(dataItem, n.Value); // Populate new rec
                }
                sql.Append(")");
                prm.Append(")");

                await SaveData($"{sql} {prm}", newValue);
            }

            return dataItem;
        }

        public int GetTablePK(string tblName)
        {
            int pk = 0;
            using (IDbConnection connection = new FbConnection(connectionString))
            {
                pk = connection.ExecuteScalar<int>($"select ID from Get_PK('{tblName}')");
            }
            return pk;
        }
        public int GetPK()
        {
            int pk = 0;
            using (IDbConnection connection = new FbConnection(connectionString))
            {
                pk = connection.ExecuteScalar<int>($"select ID from Get_PK");
            }
            return pk;
        }

        public (bool ok, string usrAd, int aaID, string usrSkl, string aaAd) Login(int usrID, string pwd)
        {
            string ok = "F"; 
            string usrAd = "";
            string usrSkl = "";
            int aaID = 0;
            string aaAd = "";
            string sql = "select * from LOGIN(@UsrID, @Pwd)";
            LoginResultModel res = new LoginResultModel();
            using (IDbConnection connection = new FbConnection(connectionString))
            {
                res = connection.QueryFirst<LoginResultModel>(sql, new { UsrID = usrID, Pwd = pwd });
                //res = connection.QueryFirst<LoginResultModel>(sql, (UsrID: usrID, Pwd: pwd));
            }
            ok = res.OK;
            usrAd = res.UsrAd;
            aaID = res.AaID;
            usrSkl = res.UsrSkl;
            aaAd = res.AaAd;
            //ad = $"{res.UsrAd} #{usrID} @{res.AaAd}";
            return (ok == "T", usrAd, aaID, usrSkl, aaAd);
        }
        public (string tblName, string keyName) MyTableNameAndKey(Type t)
        {
            string tblName = "", keyName = "";
            TableAttribute ta = (TableAttribute)t.GetCustomAttribute(typeof(TableAttribute));
            if (ta != null)
                tblName = ta.Name;

            var properties = t.GetProperties();
            foreach (var property in properties)
            {
                var pAttributes = property.GetCustomAttributes(false);
                KeyAttribute ka = (KeyAttribute)property.GetCustomAttribute(typeof(KeyAttribute));
                if (ka != null)
                {
                    keyName = property.Name;
                    break;
                }
            }
            return (tblName, keyName);
        }

        public Dictionary<string, string> MyTableFields(Type t)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            //foreach (var prop in t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
            foreach (var prop in t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
            {
                //Console.WriteLine($"{prop.Name} {prop.PropertyType.Name} - {prop.CanWrite}");
                d.Add(prop.Name, prop.PropertyType.Name);
            }

            return d;
        }

        public void MyTableFieldsCopy<T, U>(T edtCtx, U oldRow, IDictionary<string, object> newValue)
        {
            // edtCtx::EditFormContext (oldRow disinda baska alanlar da var)
            // oldRow::??model bunun uzerinden git
            foreach (var fld in oldRow.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
            {
                NotMappedAttribute nma = (NotMappedAttribute)fld.GetCustomAttribute(typeof(NotMappedAttribute));

                //if (fld.Name != "SelectedTGs") // SelectedTGs Table da yok: NotMapped olarak isaretlendi
                if (nma == null)
                {
                    if (fld.CanWrite)
                    {
                        var newVal = edtCtx.GetType().GetProperty(fld.Name)?.GetValue(edtCtx);
                        var oldVal = oldRow.GetType().GetProperty(fld.Name)?.GetValue(oldRow);

                        if (!object.Equals(newVal, oldVal))
                        {
                            newValue.Add(fld.Name, newVal);
                            //oldRow.GetType().GetProperty(fld.Name)?.SetValue(oldRow, newVal); // Gerek yok, cagiran yapiyor
                        }
                    }
                }
            }
        }
    }
}
