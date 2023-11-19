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
           
            string connection = "Server=ABC\\SQLEXPRESS;Database=HDFC_BANK;Trusted_Connection=True;MultipleActiveResultSets=True;";
            SqlConnection con = new SqlConnection(connection);
            con.Open();

            string CustId = "HDF";
            string BankName = "HDFC BANK";
            string IFSC = "HDFC01235";
            string Branch = "Asist Park Sec V Kolkata";
            string AccNum = "01235";
            DateTime AcOpeningTime = DateTime.Now;
            string IsActive = "1";
            //string MaintainBalanceId;


            Console.WriteLine("Welcome To HDFC Bank. Wec are Here to Asist You.");

            Console.WriteLine("Do You have an Account in our HDFC Bank ( Y / N) ?");
            string AccountAnswer = Console.ReadLine();

            if(AccountAnswer == "N")
            {
                Console.WriteLine("Do You Want to Create an Account in HDFC Bank ( 1 / 0 ) ?");
                string AccountCreateAnswer = Console.ReadLine();

                switch (AccountCreateAnswer)
                {
                    case "1":
                        Console.WriteLine("Enter Customer Name : ");
                        string CustomerName = Console.ReadLine();

                        Console.WriteLine("Enter Date Of Birth in YYYYMMDD Format : ");
                        string Dob = Console.ReadLine();

                        Console.WriteLine("Enter PAN Card Number : ");
                        string Pan = Console.ReadLine();

                        Console.WriteLine("Enter Aadhar Card Number : ");
                        string Aadhar = Console.ReadLine();

                        Console.WriteLine("Enter Address in Detail : ");
                        string Address = Console.ReadLine();

                        Console.WriteLine("Enter PIN Code : ");
                        string Pin = Console.ReadLine();

                        Console.WriteLine("Enter Nominee Name : ");
                        string Nominee = Console.ReadLine();

                        Console.WriteLine("Enter Mobile Number : ");
                        string Mobile = Console.ReadLine();

                        Console.WriteLine("Enter Email Id : ");
                        string Email = Console.ReadLine();

                        Console.WriteLine("Enter Alternate Mobile Number : ");
                        string AltMobile = Console.ReadLine();

                        Console.WriteLine("Enter Alternate Email Id : ");
                        string AltEmail = Console.ReadLine();


                        Random r = new Random();
                        int randNum = r.Next(1000000);
                        string CustRandom = randNum.ToString("D5");
                        string timeString = AcOpeningTime.ToString("hhmmss");
                        string CustomerId = CustId + timeString + CustRandom;                  // New CustomerID

                        string AccountNumber = AccNum + Dob + timeString;       //  New Account Number


                        Console.WriteLine("Choose Account Type ( Type:  Savings as Sav / Salary as Sal  / Coporate as Cor ) : ");
                        string AccType = Console.ReadLine();

                        switch (AccType)
                        {
                            case "Sav":
                                string type1 = "Savings";
                                string MinBalanceQuery1 = "SELECT * FROM Maintainance_Balance WHERE AccountType='" + type1 + "'";
                                SqlDataAdapter MinBalanceId1 = new SqlDataAdapter(MinBalanceQuery1, con);
                                DataTable dt1 = new DataTable();
                                MinBalanceId1.Fill(dt1);

                                Console.WriteLine("You Have Choosen Savings Account");

                                Console.WriteLine("Confirm the above details ? If Yes type Y else N for No. (If Yes account will be created!)");
                                string confirm1 = Console.ReadLine();

                                switch(confirm1)
                                {
                                    case "Y":
                                        Console.WriteLine("(Optional) Please Enter a minimum balance of Rs. 100. If not possible right now please enter 0 as balance.");
                                        double enterSavingsBalance = Convert.ToDouble(Console.ReadLine());

                                        if(enterSavingsBalance >= 100)
                                        {
                                            string InsertCustomer1 = "INSERT INTO Customer ( CustomerId, CustomerName, DOB, BankName, IfscCode, Branch, AccountNumber, PAN, Aadhar, AccountType, Address, PIN,Nominee, MobileNumber, EmailId, AlternateMobile, AlternateEmail, AcIssueDate, IsActive, MaintainanceBalanceId ) " +
                                                " VALUES ('" + CustomerId + "','" + CustomerName + "','" + Dob + "','" + BankName + "','" + IFSC + "','" + Branch + "','" + AccountNumber + "','" + Pan + "','" + Aadhar + "','" + dt1.Rows[0][1].ToString() + "','" + Address + "','" + Pin + "','" + Nominee + "','"
                                                + Mobile + "','" + Email + "','" + AltMobile + "','" + AltEmail + "','" + AcOpeningTime + "','" + IsActive + "','" + dt1.Rows[0][0].ToString() + "')";

                                            SqlCommand Ins_Cust1 = new SqlCommand(InsertCustomer1, con);
                                            Ins_Cust1.ExecuteNonQuery();


                                            string balanceTableQry1 = "INSERT INTO Balance (CustomerId,CustomerName,AccountNumber,AvailableBalance) VALUES('" + CustomerId + "','" + CustomerName + "','" + AccountNumber + "','" + enterSavingsBalance + "')";
                                            SqlCommand Ins_Bal_Min1 = new SqlCommand(balanceTableQry1, con);
                                            Ins_Bal_Min1.ExecuteNonQuery();


                                            Random r1 = new Random();
                                            int randNum1 = r1.Next(1000000);
                                            string sixDigitNumber1 = randNum1.ToString("D6");
                                            string SMTransType1 = "SavM";
                                            string ResualtantMTransId = AcOpeningTime.ToString("yyMMdd-hhmmss") + 'N' + sixDigitNumber1;

                                            string TrancTableQry2 = "INSERT INTO AcTransaction (CustomerId,TransactionId,TransactionType,TransactionDate,Credit) VALUES('" + CustomerId + "','" + ResualtantMTransId + "','" + SMTransType1 + "','" + AcOpeningTime + "','" + enterSavingsBalance + "')";
                                            SqlCommand Transaction2 = new SqlCommand(TrancTableQry2, con);
                                            Transaction2.ExecuteNonQuery();
                                        }
                                        else if(enterSavingsBalance < 100)
                                        {
                                            string InsertCustomer1 = "INSERT INTO Customer ( CustomerId, CustomerName, DOB, BankName, IfscCode, Branch, AccountNumber, PAN, Aadhar, AccountType, Address, PIN,Nominee, MobileNumber, EmailId, AlternateMobile, AlternateEmail, AcIssueDate, IsActive, MaintainanceBalanceId ) " +
                                                " VALUES ('" + CustomerId + "','" + CustomerName + "','" + Dob + "','" + BankName + "','" + IFSC + "','" + Branch + "','" + AccountNumber + "','" + Pan + "','" + Aadhar + "','" + dt1.Rows[0][1].ToString() + "','" + Address + "','" + Pin + "','" + Nominee + "','"
                                                + Mobile + "','" + Email + "','" + AltMobile + "','" + AltEmail + "','" + AcOpeningTime + "','" + 0 + "','" + dt1.Rows[0][0].ToString() + "')";

                                            SqlCommand Ins_Cust1 = new SqlCommand(InsertCustomer1, con);
                                            Ins_Cust1.ExecuteNonQuery();


                                            string balanceTableQry1 = "INSERT INTO Balance (CustomerId,CustomerName,AccountNumber,AvailableBalance) VALUES('" + CustomerId + "','" + CustomerName + "','" + AccountNumber + "','" + enterSavingsBalance + "')";
                                            SqlCommand Ins_Bal_Min1 = new SqlCommand(balanceTableQry1, con);
                                            Ins_Bal_Min1.ExecuteNonQuery();


                                            Random r1 = new Random();
                                            int randNum1 = r1.Next(1000000);
                                            string sixDigitNumber1 = randNum1.ToString("D6");
                                            string SMTransType1 = "SavM";
                                            string ResualtantMTransId = AcOpeningTime.ToString("yyMMdd-hhmmss") + 'N' + sixDigitNumber1;

                                            string TrancTableQry2 = "INSERT INTO AcTransaction (CustomerId,TransactionId,TransactionType,TransactionDate,Credit) VALUES('" + CustomerId + "','" + ResualtantMTransId + "','" + SMTransType1 + "','" + AcOpeningTime + "','" + enterSavingsBalance + "')";
                                            SqlCommand Transaction2 = new SqlCommand(TrancTableQry2, con);
                                            Transaction2.ExecuteNonQuery();
                                        }
                                        break;
                                    case "N":
                                        Console.WriteLine("Account Creation Aborted!");
                                        break;
                                }
                                
                                break;
                            case "Sal":
                                string type2 = "Salary";
                                string MinBalanceQuery2 = "SELECT * FROM Maintainance_Balance WHERE AccountType='" + type2 + "'";
                                SqlDataAdapter MinBalanceId2 = new SqlDataAdapter(MinBalanceQuery2, con);
                                DataTable dt2 = new DataTable();
                                MinBalanceId2.Fill(dt2);

                                Console.WriteLine("You Have Choosen Salary Account");

                                Console.WriteLine("Confirm the above details ? If Yes type Y else N for No. (If Yes account will be created!)");
                                string confirm2 = Console.ReadLine();

                                switch (confirm2)
                                {
                                    case "Y":
                                    getAmount:
                                        Console.WriteLine("Please Enter a minimum balance of Rs. 1000: ");
                                        double enterSalaryBalance = Convert.ToDouble(Console.ReadLine());

                                        if (enterSalaryBalance >= 1000)
                                        {
                                            string InsertCustomer2 = "INSERT INTO Customer ( CustomerId, CustomerName, DOB, BankName, IfscCode, Branch, AccountNumber, PAN, Aadhar, AccountType, Address, PIN,Nominee, MobileNumber, EmailId, AlternateMobile, AlternateEmail, AcIssueDate, IsActive, MaintainanceBalanceId ) " +
                                                " VALUES ('" + CustomerId + "','" + CustomerName + "','" + Dob + "','" + BankName + "','" + IFSC + "','" + Branch + "','" + AccountNumber + "','" + Pan + "','" + Aadhar + "','" + dt2.Rows[0][1].ToString() + "','" + Address + "','" + Pin + "','" + Nominee + "','"
                                                + Mobile + "','" + Email + "','" + AltMobile + "','" + AltEmail + "','" + AcOpeningTime + "','" + IsActive + "','" + dt2.Rows[0][0].ToString() + "')";

                                            SqlCommand Ins_Cust2 = new SqlCommand(InsertCustomer2, con);
                                            Ins_Cust2.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            goto getAmount;
                                        }

                                        string balanceTableQry2 = "INSERT INTO Balance (CustomerId,CustomerName,AccountNumber,AvailableBalance) VALUES('" + CustomerId + "','" + CustomerName + "','" + AccountNumber + "','" + enterSalaryBalance + "')";
                                        SqlCommand Ins_Bal_Min2 = new SqlCommand(balanceTableQry2, con);
                                        Ins_Bal_Min2.ExecuteNonQuery();


                                        Random r2 = new Random();
                                        int randNum2 = r2.Next(1000000);
                                        string sixDigitNumber2 = randNum2.ToString("D6");
                                        string SMTransType2 = "SM";
                                        string ResualtantMTransId = AcOpeningTime.ToString("yyMMdd-hhmmss") + 'N' + sixDigitNumber2;

                                        string TrancTableQry2 = "INSERT INTO AcTransaction (CustomerId,TransactionId,TransactionType,TransactionDate,Credit) VALUES('" + CustomerId + "','" + ResualtantMTransId + "','" + SMTransType2 + "','" + AcOpeningTime + "','" + enterSalaryBalance + "')";
                                        SqlCommand Transaction2 = new SqlCommand(TrancTableQry2, con);
                                        Transaction2.ExecuteNonQuery();

                                        break;
                                    case "N":
                                        Console.WriteLine("Account Creation Aborted!");
                                        break;
                                }

                                break;
                            case "Cor":
                                string type3 = "Corporate";
                                string MinBalanceQuery3 = "SELECT * FROM Maintainance_Balance WHERE AccountType='" + type3 + "'";
                                SqlDataAdapter MinBalanceId3 = new SqlDataAdapter(MinBalanceQuery3, con);
                                DataTable dt3 = new DataTable();
                                MinBalanceId3.Fill(dt3);

                                Console.WriteLine("You Have Choosen Corporate Account");

                                Console.WriteLine("Confirm the above details ? If Yes type Y else N for No. (If Yes account will be created!)");
                                string confirm3 = Console.ReadLine();

                                switch (confirm3)
                                {
                                    case "Y":
                                        getAmount:
                                            Console.WriteLine("Please Enter a minimum balance of Rs. 50000: ");
                                            double enterCorporateBalance = Convert.ToDouble(Console.ReadLine());

                                        if(enterCorporateBalance >= 50000)
                                        {
                                            string InsertCustomer3 = "INSERT INTO Customer ( CustomerId, CustomerName, DOB, BankName, IfscCode, Branch, AccountNumber, PAN, Aadhar, AccountType, Address, PIN,Nominee, MobileNumber, EmailId, AlternateMobile, AlternateEmail, AcIssueDate, IsActive, MaintainanceBalanceId ) " +
                                                " VALUES ('" + CustomerId + "','" + CustomerName + "','" + Dob + "','" + BankName + "','" + IFSC + "','" + Branch + "','" + AccountNumber + "','" + Pan + "','" + Aadhar + "','" + dt3.Rows[0][1].ToString() + "','" + Address + "','" + Pin + "','" + Nominee + "','"
                                                + Mobile + "','" + Email + "','" + AltMobile + "','" + AltEmail + "','" + AcOpeningTime + "','" + IsActive + "','" + dt3.Rows[0][0].ToString() + "')";

                                            SqlCommand Ins_Cust3 = new SqlCommand(InsertCustomer3, con);
                                            Ins_Cust3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            goto getAmount;
                                        }

                                        string balanceTableQry3 = "INSERT INTO Balance (CustomerId,CustomerName,AccountNumber,AvailableBalance) VALUES('" + CustomerId + "','" + CustomerName + "','" + AccountNumber + "','" + enterCorporateBalance + "')";
                                        SqlCommand Ins_Bal_Min3 = new SqlCommand(balanceTableQry3, con);
                                        Ins_Bal_Min3.ExecuteNonQuery();


                                        Random r3 = new Random();
                                        int randNum3 = r3.Next(1000000);
                                        string sixDigitNumber3 = randNum3.ToString("D6");
                                        string CMTransType3 = "CM";
                                        string ResualtantMTransId = AcOpeningTime.ToString("yyMMdd-hhmmss") + 'N' + sixDigitNumber3;

                                        string TrancTableQry3 = "INSERT INTO AcTransaction (CustomerId,TransactionId,TransactionType,TransactionDate,Credit) VALUES('" + CustomerId + "','" + ResualtantMTransId + "','" + CMTransType3 + "','" + AcOpeningTime + "','" + enterCorporateBalance + "')";
                                        SqlCommand Transaction3 = new SqlCommand(TrancTableQry3, con);
                                        Transaction3.ExecuteNonQuery();

                                        break;
                                    case "N":
                                        Console.WriteLine("Account Creation Aborted!");
                                        break;
                                }

                               break;
                        }

                        Console.WriteLine("Accout Created Successfully.");
                        Console.WriteLine("Account Number: {0}", AccountNumber);
                        Console.WriteLine("Account Holder: {0}", CustomerName);
                        Console.WriteLine("IFSC Code: {0}", IFSC);
                        Console.WriteLine("Branch Name: {0}", Branch);
                        Console.WriteLine("Login Password: {0}", Dob);

                        //Console.WriteLine(CustomerId);
                        //Console.WriteLine(CustomerName);
                        //Console.WriteLine(Dob);
                        //Console.WriteLine(BankName);
                        //Console.WriteLine(IFSC);
                        //Console.WriteLine(Branch);
                        //Console.WriteLine(AccountNumber);
                        //Console.WriteLine(Pan);
                        //Console.WriteLine(Aadhar);
                        //Console.WriteLine(AccType);
                        //Console.WriteLine(Address);
                        //Console.WriteLine(Pin);
                        //Console.WriteLine(Nominee);
                        //Console.WriteLine(Mobile);
                        //Console.WriteLine(Email);
                        //Console.WriteLine(AltMobile);
                        //Console.WriteLine(AltEmail);
                        //Console.WriteLine(AcOpeningTime);
                        //Console.WriteLine(IsActive);
                        //Console.WriteLine(MaintainBalance);

                        break;
                    case "0":
                        Console.WriteLine("We are ThankFul to you for Borwsing our HDFC Bank.");
                        break;
                }
            }

            if (AccountAnswer == "Y")
            {
                string dTransId = "HDFC001D";
                string wTransId = "HDFC002W";

                string CustmId;
                double dob;

                string Y = "Yes";
                string N = "No";

                Console.WriteLine("Please Enter Customer Id  : ");
                CustmId = Convert.ToString(Console.ReadLine());

                Console.WriteLine("Please Enter Date Of Birth in YYYYMMDD Format : ");
                dob = Convert.ToDouble(Console.ReadLine());


                string SelectString = "SELECT * FROM Customer WHERE CustomerId='" + CustmId + "'AND DOB='" + dob + "'";
                SqlCommand data = new SqlCommand(SelectString, con);

                //for Login
                SqlDataAdapter adapt = new SqlDataAdapter(data);
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                int count = ds.Tables[0].Rows.Count;

                if (count == 1)
                {
                    SqlDataReader response = data.ExecuteReader();

                    string Balance = "SELECT * FROM Balance WHERE CustomerId='" + CustmId + "'";
                    SqlDataAdapter bal = new SqlDataAdapter(Balance, con);
                    DataTable dt = new DataTable();
                    bal.Fill(dt);

                    double restAmt = Convert.ToDouble(100) - Convert.ToDouble(dt.Rows[0][3].ToString());

                    if (Convert.ToInt32(ds.Tables[0].Rows[0][18].ToString()) == 0)
                    {
                        Console.WriteLine("You have no sufficient balance! You still need to pay: " + Convert.ToString(restAmt));

                        //Pay logic
                    }
                    else
                    {
                        while (response.Read())
                        {

                            string CustomerId = response["CustomerId"].ToString();
                            string CustomerName = response["CustomerName"].ToString();
                            string DOB = response["DOB"].ToString();
                            string Bank = response["BankName"].ToString();
                            string IfscCode = response["IfscCode"].ToString();
                            string BranchName = response["Branch"].ToString();
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
                                                Console.WriteLine("Bank Name: {0}", Bank);
                                                Console.WriteLine("IFSC Code: {0}", IfscCode);
                                                Console.WriteLine("Branch Name: {0}", BranchName);
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
                                                if ((Convert.ToDouble(dt.Rows[0][3].ToString()) > Convert.ToDouble(wcAmt)) && ((Convert.ToDouble(dt.Rows[0][3].ToString()) - Convert.ToDouble(wcAmt)) > 100))
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
                                                if (((Convert.ToDouble(dt.Rows[0][3].ToString()) - Convert.ToDouble(wcAmt)) < 100) || Convert.ToDouble(dt.Rows[0][3].ToString()) < 100)
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
                            else //if (Answer = N)
                            {
                                Console.WriteLine("Give appropiate Input, Its case Sensitive, Put as written as Statement ( Yes / No )");
                            }
                        }
                    }
                          
                }
                else
                {
                    Console.WriteLine("Invalid Customer Details, Please chek and Try again Later.");
                }
            }
            
            con.Close();
            Console.ReadLine();
        }
    }
}
