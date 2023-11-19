using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hdfc
{
    internal class Program
    {
        static void Main(string[] args)
        {
           
            string connection = "Server=GOPAL_SASMAL\\SQLEXPRESS;Database=HDFC_BANK;Trusted_Connection=True;MultipleActiveResultSets=True;";
            SqlConnection con = new SqlConnection(connection);
            con.Open();

            string dTransId = "HDFC001D";
            string wTransId = "HDFC002W";

            string CustId;
            double dob;

            string Y = "Yes";
            string N = "No";

            Console.WriteLine("Please Enter Customer Id  : ");
            CustId = Convert.ToString(Console.ReadLine());

            Console.WriteLine("Please Enter Date Of Birth in YYYYMMDD Format : ");
            dob = Convert.ToDouble(Console.ReadLine());


            string SelectString = "SELECT * FROM Customer WHERE CustomerId='" + CustId + "'AND DOB='" + dob + "'";
            SqlCommand data = new SqlCommand(SelectString, con);

            //for Login
            SqlDataAdapter adapt = new SqlDataAdapter(data);
            DataSet ds = new DataSet();
            adapt.Fill(ds);
            int count = ds.Tables[0].Rows.Count;

            if (count==1)
            {
                SqlDataReader response = data.ExecuteReader();


                string Balance = "SELECT * FROM Balance WHERE CustomerId='" + CustId + "'";
                SqlDataAdapter bal = new SqlDataAdapter(Balance, con);
                DataTable dt = new DataTable();
                bal.Fill(dt);


                while (response.Read())
                {

                    string CustomerId = response["CustomerId"].ToString();
                    string CustomerName = response["CustomerName"].ToString();
                    string DOB = response["DOB"].ToString();
                    string BankName = response["BankName"].ToString();
                    string IfscCode = response["IfscCode"].ToString();
                    string Branch = response["Branch"].ToString();
                    string AccountNumber = response["AccountNumber"].ToString();
                    DateTime dateAndTime = DateTime.Now;

                    Console.WriteLine("Welcome {0} in Our HDFC Bank.", CustomerName);

                    Console.WriteLine("Do you want to Proceed to the Banking Area (Yes / No ) ? ");
                    string Answer = Console.ReadLine();

                    if (Answer == Y)
                    {

                        switch (Answer)
                        {
                            case "Yes":
                                Console.WriteLine("Press 1 for Account Information.");

                                Console.WriteLine("Press 2 for Balance Enquery.");

                                Console.WriteLine("Press 3 for Balance Wthdrwal.");

                                Console.WriteLine("Press 4 for Balance Deposit.");

                                string yAnswer = Console.ReadLine();

                                switch (yAnswer)
                                {
                                    case "1":
                                        Console.WriteLine("Bank Name: {0}", BankName);
                                        Console.WriteLine("IFSC Code: {0}", IfscCode);
                                        Console.WriteLine("Branch Name: {0}", Branch);
                                        Console.WriteLine("Account Number: {0}", AccountNumber);
                                        Console.WriteLine("Account Holder: {0}", CustomerName);
                                        Console.WriteLine("Date of Birth: {0}", DOB);
                                        break;

                                    case "2":
                                        //sql statement for case 2.
                                            Console.WriteLine("Your Available Balance is {0}", dt.Rows[0][3].ToString());
                                        break;

                                    case "3":
                                        //sql statement for case 3.
                                        Console.WriteLine("How Much Amount you want to Withdrwal (Enter the Amount)? ");
                                        double wAmt = Convert.ToInt32(Console.ReadLine());
                                        Console.WriteLine("Confirm the Amount Again !");
                                        double wcAmt = Convert.ToInt32(Console.ReadLine());

                                        if (wAmt != wcAmt)
                                        {
                                            Console.WriteLine("Confirm Amount Does Not Match.Please Ckeck again.");
                                        }
                                        if (Convert.ToDouble(dt.Rows[0][3].ToString()) < Convert.ToDouble(wcAmt))
                                        {
                                            Console.WriteLine("Insufficiant Fund.");
                                        }
                                        if((Convert.ToDouble(dt.Rows[0][3].ToString()) > Convert.ToDouble(wcAmt)) && ((Convert.ToDouble(dt.Rows[0][3].ToString())-Convert.ToDouble(wcAmt))>100))
                                        {
                                            Random r = new Random();
                                            int randNum = r.Next(1000000);
                                            string sixDigitNumber = randNum.ToString("D6");

                                            string TransType1 = "D";

                                            double AccountBalance = Convert.ToDouble(dt.Rows[0][3].ToString()) - wcAmt;
                                            string ResualtantDTransId = wTransId + wcAmt + 'N' + sixDigitNumber;

                                            string InsertTransaction = "INSERT INTO AcTransaction ( CustomerId, TransactionId, TransactionType, TransactionDate, Debit ) VALUES ('" + CustomerId + "','" + ResualtantDTransId + "','" + TransType1 + "','" + dateAndTime + "','" + wcAmt + "')"; 
                                            SqlCommand Ins_Trs = new SqlCommand(InsertTransaction, con);
                                            Ins_Trs.ExecuteNonQuery();

                                            string UpdateBalance = "UPDATE Balance SET AvailableBalance=" + AccountBalance + "WHERE CustomerId='" + CustomerId + "'";
                                            SqlCommand Up_Trs = new SqlCommand(UpdateBalance, con);
                                            Up_Trs.ExecuteNonQuery();

                                            Console.WriteLine("Withdrwal Successfull with Rs: {0}", wcAmt);
                                            Console.WriteLine("Transaction ID: {0}", ResualtantDTransId);
                                            Console.WriteLine("Currently Available Balance Rs: {0}", AccountBalance);
                                        }
                                        if(((Convert.ToDouble(dt.Rows[0][3].ToString()) - Convert.ToDouble(wcAmt)) < 100) || Convert.ToDouble(dt.Rows[0][3].ToString()) < 100)
                                        {
                                            Console.WriteLine("You have to Maintain The Minimum Account Balance Rs: {0}", Convert.ToDouble(response["MaintainanceBalance"].ToString()));
                                        }

                                        break;

                                    case "4":
                                        Console.WriteLine("How Much Amount you want to Depodit (Enter the Amount) ? ");
                                        double dAmt = Convert.ToInt32(Console.ReadLine());
                                        Console.WriteLine("Confirm the Amount Again !");
                                        double dcAmt = Convert.ToInt32(Console.ReadLine());

                                        string TransType = "C";

                                        if (dAmt != dcAmt)
                                        {
                                            Console.WriteLine("Confirm Amount Does Not Match.Please Check again.");
                                        }
                                        if (dAmt == dcAmt)
                                        {
                                            Random r = new Random();
                                            int randNum = r.Next(1000000);
                                            string sixDigitNumber = randNum.ToString("D6");

                                            string ResualtantCTransId = dTransId + dcAmt + 'N' + sixDigitNumber;
                                            double AccountBalance = Convert.ToDouble(dt.Rows[0][3].ToString()) + dcAmt;

                                            string InsertTransaction = "INSERT INTO AcTransaction ( CustomerId, TransactionId, TransactionType, TransactionDate, Credit ) VALUES ('" + CustomerId + "','" + ResualtantCTransId + "','" + TransType + "','" + dateAndTime + "','" + dcAmt + "')";
                                            SqlCommand Ins_Trs = new SqlCommand(InsertTransaction, con);
                                            Ins_Trs.ExecuteNonQuery();

                                            string UpdateBalance = "UPDATE Balance SET AvailableBalance=" + AccountBalance + "WHERE CustomerId='" + CustomerId + "'";
                                            SqlCommand Up_Trs = new SqlCommand(UpdateBalance, con);
                                            Up_Trs.ExecuteNonQuery();

                                            Console.WriteLine("Depodit Successfull with Rs: {0}", dcAmt);
                                            Console.WriteLine("Transaction ID: {0}", ResualtantCTransId);
                                            Console.WriteLine("Currently Available Balance Rs: {0}", AccountBalance);
                                        }
                                        break;
                                    default:
                                        Console.WriteLine("Invalid Entry. Please Check Again.");
                                        break;
                                }
                                break;

                            case "No":
                                Console.WriteLine("Thank You for Using banking Servise.");
                                break;
                        }

                    }
                    else if (Answer == N)
                    {
                        Console.WriteLine("Give appropiate Input, Its case Sensitive, Put as written as Statement ( Yes / No )");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid Customer Details, Please chek and Try again Later.");
            }

            con.Close();
            Console.ReadLine();
        }
    }
}
